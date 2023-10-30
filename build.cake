// ========================================
// INSTALL ADDINS & TOOLS
// ========================================

// Eg: #addin nuget:?package=PackageName&version=1.1.x

// ========================================
// CONSTANTS
// ========================================

const string ApplicationName = "dbml.NET";
readonly DirectoryPath assetsDirectory = Directory("./assets");
readonly DirectoryPath artifactsDirectory = Directory("./artifacts");
readonly DirectoryPath packageArtifactsDirectory = Directory($"{artifactsDirectory}/package");
readonly DirectoryPath testsArtifactsDirectory = Directory($"{artifactsDirectory}/tests");
readonly DirectoryPath unitTestsArtifactsDirectory = Directory($"{testsArtifactsDirectory}/unit-tests");
readonly DirectoryPath integrationTestsArtifactsDirectory = Directory($"{testsArtifactsDirectory}/integration-tests");
readonly DirectoryPath acceptanceTestsArtifactsDirectory = Directory($"{testsArtifactsDirectory}/acceptance-tests");
readonly DirectoryPath coverageArtifactsDirectory = Directory($"{artifactsDirectory}/coverage");
readonly DirectoryPath publishArtifactsDirectory = Directory($"{artifactsDirectory}/publish");

// ========================================
// ARGUMENTS
// ========================================

#nullable enable // Enable C# nullability

// Flag indicating if the test and code coverage reports should be opened automatically.
// Default value `false`.
readonly bool OPEN_REPORTS = HasArgument("open-reports");

readonly bool OPEN_COVERAGE_REPORTS = OPEN_REPORTS;
readonly bool OPEN_TEST_REPORTS = OPEN_REPORTS;

// Configuration can have a value of "Release" or "Debug".
// Default configuration `Release`.
readonly string CONFIGURATION = Argument<string>("configuration", "Release");

// Environment can have a value of "Production", "Development", "Test" or "Local".
// Default environment `(string)null`.
readonly string? ENVIRONMENT = Argument<string?>("environment", null);

// Flag indicating if the code coverage artifacts should be removed or not.
// Default value `false`.
readonly bool REMOVE_COVERAGE_ARTIFACTS = HasArgument("remove-coverage-artifacts");

// The maximum number of iterations to run the tests.
// Default value `1`.
readonly int TestUntil_MaxRuns = Argument<int>("maxRuns", 1);

// The root directory where the tests are located.
// Default value `(string)null`.
readonly string? TestUntil_Directory = Argument<string?>("directory", null);

#nullable disable // Disable C# nullability

// ========================================
// SETUP / TEARDOWN
// ========================================

Setup(ctx =>
{
    // Executed BEFORE the first task.
    Information("Running tasks...");
});

Teardown(ctx =>
{
    // Executed AFTER the last task.
    Information("Finished running tasks.");
});

// ========================================
// TASKS
// ========================================

void DeleteDirectoriesByPattern(string pattern)
{
    foreach (DirectoryPath directory in GetDirectories(pattern))
        DeleteDirectory(directory);
}

void DeleteDirectory(DirectoryPath directory)
{
    if (!DirectoryExists(directory))
        return;

    Information($"Removing directory '{directory}'");

    DeleteDirectories(
        directories: new DirectoryPath[] { directory },
        settings: new DeleteDirectorySettings() { Force = true, Recursive = true }
    );
}

Task("clean")
    .Description("Cleans existing artifacts.")
    .Does(() =>
    {
        Information($"Clean existing artifacts...");

        DeleteDirectory(testsArtifactsDirectory);

        if (REMOVE_COVERAGE_ARTIFACTS)
            DeleteDirectory(coverageArtifactsDirectory);

        DeleteDirectoriesByPattern($"./**/bin/{CONFIGURATION}");
        DeleteDirectoriesByPattern($"./**/obj/{CONFIGURATION}");
    });

Task("restore")
    .Description("Restores NuGet packages.")
    .Does(() =>
    {
        Information($"Restore solution packages...");

        DotNetRestore();
    });

Task("build")
    .Description("Builds the solution.")
    .IsDependentOn("restore")
    .Does(() =>
    {
        Information($"Building solution...");

        DotNetBuild(
            project: ".",
            settings: new DotNetBuildSettings
            {
                Configuration = CONFIGURATION,
                NoLogo = true,
                NoRestore = true
            }
        );
    });

Task("package")
    .Description($"Package the solution.")
    .Does(() =>
    {
        DeleteDirectory(packageArtifactsDirectory);
    })
    .DoesForEach(GetFiles("./src/**/*.csproj"), project =>
    {
        Information($"Packaging project '{project}'...");

        DotNetPack(
            project: project.ToString(),
            settings: new DotNetPackSettings
            {
                Configuration = CONFIGURATION,
                NoLogo = true,
                NoRestore = false, // perform restore
                NoBuild = false,   // perform build
                OutputDirectory = packageArtifactsDirectory
            }
        );
    })
    .DeferOnError();

Task("test-until")
    .Description($"Runs tests until the process stop or the maximum runs are achieved.")
    .IsDependentOn("build")
    .Does(() =>
    {
        DirectoryPath projectsDirectory = TestUntil_Directory ?? "./tests";
        string projectRegexLookup = $"{projectsDirectory}/**/*.Tests.*.csproj";
        FilePathCollection projects = GetFiles(projectRegexLookup);
        const string DelimiterText = "========================================";

        // Print usage information
        Information(string.Empty);
        Information(DelimiterText);
        Information("Usage: dotnet cake --task test-until [options]");
        Information(string.Empty);
        Information("test-until enable running tests until the process stop or the maximum runs are achieved.");
        Information(string.Empty);
        Information("Options:");
        Information("  `--maxRuns`    The maximum number of iterations to run the tests.");
        Information("  `--directory`  The root directory where the tests are located.");
        Information(string.Empty);
        Information("Examples:");
        Information("   dotnet cake --task test-until");
        Information("   dotnet cake --task test-until --maxRuns 10 --directory ./tests/");
        Information(DelimiterText);

        // Print configuration information
        Information(string.Empty);
        Information(DelimiterText);
        Information($"  - Max number of runs: {TestUntil_MaxRuns}");
        Information($"  - Root directory: '{projectsDirectory}'");
        Information($"  - Test lookup: '{projectRegexLookup}'");
        Information($"  - Number of projects: {projects.Count}");
        foreach (FilePath project in projects)
            Information($"    - {project.ToString()}");
        Information(DelimiterText);

        // Run tests
        int currentRunCount = 1;

    TestLoop:
        if (currentRunCount > TestUntil_MaxRuns)
            goto TaskEnd;

        for (int projectNr = 1; projectNr <= projects.Count; projectNr++)
        {
            FilePath project = projects.ElementAt(projectNr - 1);
            string projectName = System.IO.Path.GetFileName(project.ToString());

            // Print test run information
            Information(string.Empty);
            Information(DelimiterText);
            Information($"(Run #{currentRunCount}) - #{projectNr}-project: '{projectName}'...");
            Information(DelimiterText);
            Information(string.Empty);

            try
            {
                DotNetTest(
                    project: project.ToString(),
                    settings: new DotNetTestSettings
                    {
                        Configuration = CONFIGURATION,
                        NoLogo = true,
                        NoRestore = true,
                        NoBuild = false,
                        ToolTimeout = TimeSpan.FromMinutes(10),
                        Blame = true,
                        Loggers = new string[] { "trx" },
                        Collectors = new string[] { "XPlat Code Coverage" },
                        ResultsDirectory = unitTestsArtifactsDirectory
                    }
                );
            }
            catch
            {
                // Do nothing, errors are expected
            }
        }

        // Go to next run...
        currentRunCount++;
        goto TestLoop;

    TaskEnd:
        // Print configuration information
        Information(string.Empty);
        Information(DelimiterText);
        Information($"  - Max number of runs: {TestUntil_MaxRuns}");
        Information($"  - Number of runs: {currentRunCount - 1}");
        Information(DelimiterText);
    })
    .DeferOnError();

Task("unit-tests")
    .Description($"Runs unit tests.")
    .IsDependentOn("build")
    .DoesForEach(GetFiles("./tests/**/*.Tests.Unit.csproj"), project =>
    {
        Information($"Testing project '{project}'...");

        DotNetTest(
            project: project.ToString(),
            settings: new DotNetTestSettings
            {
                Configuration = CONFIGURATION,
                NoLogo = true,
                NoRestore = true,
                NoBuild = false,
                ToolTimeout = TimeSpan.FromMinutes(10),
                Blame = true,
                Loggers = new string[] { "trx" },
                Collectors = new string[] { "XPlat Code Coverage" },
                ResultsDirectory = unitTestsArtifactsDirectory
            }
        );
    })
    .DeferOnError();

Task("integration-tests")
    .Description($"Runs integration tests.")
    .IsDependentOn("build")
    .DoesForEach(GetFiles("./tests/**/*.Tests.Integration.csproj"), project =>
    {
        Information($"Testing project '{project}'...");

        DotNetTest(
            project: project.ToString(),
            settings: new DotNetTestSettings
            {
                EnvironmentVariables = new Dictionary<string, string> {
                    { "ASPNETCORE_ENVIRONMENT", ENVIRONMENT ?? "Test" }
                },
                Configuration = CONFIGURATION,
                NoLogo = true,
                NoRestore = true,
                NoBuild = false,
                ToolTimeout = TimeSpan.FromMinutes(10),
                Blame = true,
                Loggers = new string[] { "trx" },
                Collectors = new string[] { "XPlat Code Coverage" },
                ResultsDirectory = integrationTestsArtifactsDirectory
            }
        );
    })
    .DeferOnError();

Task("acceptance-tests")
    .Description($"Runs acceptance tests.")
    .IsDependentOn("build")
    .DoesForEach(GetFiles("./tests/**/*.Tests.Acceptance.csproj"), project =>
    {
        Information($"Testing project '{project}'...");

        DotNetTest(
            project: project.ToString(),
            settings: new DotNetTestSettings
            {
                EnvironmentVariables = new Dictionary<string, string> {
                    { "ASPNETCORE_ENVIRONMENT", ENVIRONMENT ?? "Test" }
                },
                Configuration = CONFIGURATION,
                NoLogo = true,
                NoRestore = true,
                NoBuild = false,
                ToolTimeout = TimeSpan.FromMinutes(10),
                Blame = true,
                Loggers = new string[] { "trx" },
                Collectors = new string[] { "XPlat Code Coverage" },
                ResultsDirectory = acceptanceTestsArtifactsDirectory
            }
        );
    })
    .DeferOnError();

Task("test-reports")
    .Description($"Generate test reports.")
    .Does(() =>
    {
        Information($"Starting generating test reports...");

        List<string> inputFiles = new();

        // Unit tests
        inputFiles.AddRange(
            GetFiles($"{unitTestsArtifactsDirectory}/**/*.trx")
                .Select(path => $"\"File={path};Format=Trx;GroupTitle=Unit Tests\"")
                .ToArray()
        );

        // Integration tests
        inputFiles.AddRange(
            GetFiles($"{integrationTestsArtifactsDirectory}/**/*.trx")
                .Select(path => $"\"File={path};Format=Trx;GroupTitle=Integration Tests\"")
                .ToArray()
        );

        // Acceptance tests
        inputFiles.AddRange(
            GetFiles($"{acceptanceTestsArtifactsDirectory}/**/*.trx")
                .Select(path => $"\"File={path};Format=Trx;GroupTitle=Acceptance Tests\"")
                .ToArray()
        );

        if (!inputFiles.Any())
        {
            Warning($"No *.trx test results found. Test reports cannot be generated.");
            return;
        }

        string testReportOutputFilePath = $"{testsArtifactsDirectory}/{ApplicationName}.test_report.md";

        string cmdCommand =
            $"liquid" +
            $" --inputs {string.Join(' ', inputFiles)}" +
            $" --template {assetsDirectory}/test-report-template.md" +
            $" --title \"{ApplicationName} test report - {DateTime.Now:yyyy-MM-dd}\"" +
            $" --output-file \"{testReportOutputFilePath}\"";

        // Generate human readable test reports
        DotNetTool(cmdCommand);

        Information($"Generated test reports under '{testReportOutputFilePath}'.");

        bool canShowReports = IsRunningOnWindows() && BuildSystem.IsLocalBuild;
        bool shouldShowReports = canShowReports && OPEN_TEST_REPORTS;
        if (shouldShowReports)
        {
            string testReportFilePath = testReportOutputFilePath;
            if (!FileExists(testReportFilePath))
            {
                Warning($"Cannot find '{testReportFilePath}'.");
            }
            else
            {
                Information($"Opening file '{testReportFilePath}'...");

                StartProcess("cmd", new ProcessSettings
                {
                    Arguments = $"/C start \"\" {testReportFilePath}"
                });
            }
        }
        else if (canShowReports)
        {
            Information("Using '--open-reports' option the test reports will open automatically in your default browser or editor.");
        }
    })
    .DeferOnError();

Task("code-coverage-reports")
    .Description($"Generate code coverage reports.")
    .Does(() =>
    {
        Information($"Generate code coverage reports...");

        string[] reportTypes = new string[]
        {
            "Badges",
            // "Clover",
            "Cobertura",
            // "CsvSummary",
            // "Html",
            // "Html_Dark",
            // "Html_Light",
            // "HtmlChart",
            // "HtmlInline",
            // "HtmlInline_AzurePipelines",
            "HtmlInline_AzurePipelines_Dark",
            // "HtmlInline_AzurePipelines_Light",
            "HtmlSummary",
            "JsonSummary",
            // "Latex",
            // "LatexSummary",
            // "lcov",
            "MarkdownSummary",
            "MarkdownSummaryGithub",
            // "MHtml",
            // "PngChart",
            // "SonarQube",
            // "TeamCitySummary",
            "TextSummary",
            "Xml",
            "XmlSummary"
        };

        string reportsPath = $"{testsArtifactsDirectory}/**/coverage.cobertura.xml";

        if (GetFiles(reportsPath).Count <= 0)
        {
            Warning($"The report file pattern '{reportsPath}' found no matching files.");
            return;
        }

        string cmdCommand =
            $"reportgenerator" +
            $" -verbosity:Warning" +
            $" -title:{ApplicationName}" +
            $" -reports:{reportsPath}" +
            $" -targetdir:{coverageArtifactsDirectory}" +
            $" -reporttypes:{string.Join(';', reportTypes)}";

        // Generate nice human readable coverage report
        DotNetTool(cmdCommand);

        Information($"Generated code coverage reports under '{coverageArtifactsDirectory}'.");

        bool canShowReports = IsRunningOnWindows() && BuildSystem.IsLocalBuild;
        bool shouldShowReports = canShowReports && OPEN_COVERAGE_REPORTS;
        if (shouldShowReports)
        {
            string coverageIndexFilePath = $"{coverageArtifactsDirectory}/index.htm";
            if (!FileExists(coverageIndexFilePath))
            {
                Warning($"Cannot find '{coverageIndexFilePath}'.");
            }
            else
            {
                Information($"Opening file '{coverageIndexFilePath}'...");

                StartProcess("cmd", new ProcessSettings
                {
                    Arguments = $"/C start \"\" {coverageIndexFilePath}"
                });
            }
        }
        else if (canShowReports)
        {
            Information("Using '--open-reports' option the code coverage reports will open automatically in your default browser.");
        }
    })
    .DeferOnError();

Task("upload-test-reports")
    .WithCriteria(() =>
        BuildSystem.AzurePipelines.IsRunningOnAzurePipelines ||
        BuildSystem.GitHubActions.IsRunningOnGitHubActions
    )
    .Description("Upload test reports to CI.")
    .Does(() =>
    {
        Information($"Upload test reports to CI...");

        FilePath[] testResultsFiles =
            GetFiles($"{testsArtifactsDirectory}/**/*.trx").ToArray();

        if (!testResultsFiles.Any())
        {
            Warning($"No test reports was found, skipping upload to CI.");
            return;
        }

        if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        {
            Information($"Starting uploading test reports to Azure...");

            BuildSystem.AzurePipelines.Commands.PublishTestResults(
                new AzurePipelinesPublishTestResultsData
                {
                    Configuration = CONFIGURATION,
                    TestResultsFiles = testResultsFiles,
                    MergeTestResults = true,
                    TestRunner = AzurePipelinesTestRunnerType.VSTest
                }
            );
        }
        else if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        {
            Information($"Starting uploading test reports to Github...");

            BuildSystem.GitHubActions.Commands.UploadArtifact(
                path: testsArtifactsDirectory,
                artifactName: $"{ApplicationName} Test Reports"
            );
        }
    });

Task("upload-code-coverage-reports")
    .WithCriteria(() =>
        BuildSystem.AzurePipelines.IsRunningOnAzurePipelines ||
        BuildSystem.GitHubActions.IsRunningOnGitHubActions
    )
    .Description("Upload code coverage reports to CI.")
    .Does(() =>
    {
        Information($"Upload code coverage reports to CI...");

        if (!FileExists($"{coverageArtifactsDirectory}/Cobertura.xml"))
        {
            Warning($"No code coverage reports was found, skipping upload to CI.");
            return;
        }

        if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        {
            Information($"Starting uploading code coverage reports to Azure...");

            BuildSystem.AzurePipelines.Commands.PublishCodeCoverage(
                new AzurePipelinesPublishCodeCoverageData
                {
                    CodeCoverageTool = AzurePipelinesCodeCoverageToolType.Cobertura,
                    ReportDirectory = $"{coverageArtifactsDirectory}",
                    SummaryFileLocation = $"{coverageArtifactsDirectory}/Cobertura.xml"
                }
            );
        }
        else if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        {
            Information($"Starting uploading code coverage reports to Github...");

            BuildSystem.GitHubActions.Commands.UploadArtifact(
                path: coverageArtifactsDirectory,
                artifactName: $"{ApplicationName} Code Coverage Reports"
            );
        }
    });

Task("test")
    .IsDependentOn("clean")
    .IsDependentOn("unit-tests")
    .IsDependentOn("integration-tests")
    .IsDependentOn("acceptance-tests")
    .IsDependentOn("test-reports")
    .IsDependentOn("code-coverage-reports");

Task("app-release")
    .IsDependentOn("package");

Task("default")
    .IsDependentOn("clean")
    .IsDependentOn("unit-tests")
    .IsDependentOn("integration-tests")
    .IsDependentOn("acceptance-tests")
    .IsDependentOn("test-reports")
    .IsDependentOn("upload-test-reports")
    .IsDependentOn("code-coverage-reports")
    .IsDependentOn("upload-code-coverage-reports");

// ========================================
// EXECUTION
// ========================================

RunTarget(Argument("task", "default"));
