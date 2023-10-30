# Commands

## Running tests multiple times

The `test-until` task enable running tests until the process stop or the maximum runs are achieved.

Usage:

```shell
dotnet cake --task test-until [options]
```

Options:

- `--maxRuns`    The maximum number of iterations to run the tests.
- `--directory`  The root directory where the tests are located.

Examples:

```shell
dotnet cake --task test-until --maxRuns 10 --directory ./tests/
```

## Push commits one by one automatically

- Windows (PowerShell):

```powershell
foreach ($rev in $(git rev-list --reverse origin/branch-name..branch-name))
{
    git push origin ($rev + ":branch-name") -f
}
```

- Linux (Shell):

```shell
for rev in $(git rev-list --reverse origin/branch-name..branch-name); do
    git push origin $rev:branch-name -f;
done
```
