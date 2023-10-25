using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;

using DbmlNet.Domain;

namespace DbmlNet.Extensions;

/// <summary>
/// Represents a markdown writer.
/// </summary>
public sealed class DbmlMarkdownWriter
{
    private const string UnknownValue = "N/A";
    private readonly DbmlDatabase _database;

    private DbmlMarkdownWriter(DbmlDatabase database)
    {
        _database = database;
    }

    /// <summary>
    /// Writes the <see cref="DbmlDatabase"/> to a file.
    /// </summary>
    /// <param name="dbmlDatabase">The <see cref="DbmlDatabase"/> to write.</param>
    /// <param name="outputFilePath">The path of the output file.</param>
    public static void WriterToFile(DbmlDatabase dbmlDatabase, string outputFilePath)
    {
        string filePath = Path.GetFullPath(outputFilePath);
        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

        FileStream outputStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite);
        using StreamWriter writer = new StreamWriter(outputStream);

        DbmlMarkdownWriter dbmlWriter = new DbmlMarkdownWriter(dbmlDatabase);
        dbmlWriter.WriteTo(writer);
    }

    /// <summary>
    /// Writes the content to the specified <see cref="TextWriter"/>.
    /// </summary>
    /// <param name="writer">The <see cref="TextWriter"/> to write to.</param>
    public void WriteTo(TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(writer);

        using IndentedTextWriter indentedWriter =
            new IndentedTextWriter(writer, "  ");

        Write(indentedWriter, _database);
    }

    private static void Write(IndentedTextWriter writer, DbmlDatabase database)
    {
        writer.Write("# Database design");
        if (!string.IsNullOrWhiteSpace($"{database.Project}"))
            writer.Write($": {database.Project}");

        writer.WriteLine();

        if (database.Project is not null)
        {
            writer.WriteLine();
            WriteProjectDeclaration(writer, database.Project);
        }

        writer.WriteLine();
        WriteTableOfContents(writer, database);

        writer.WriteLine();
        WriteLegend(writer, database);

        if (database.Tables.Any())
        {
            writer.WriteLine();
            writer.WriteLine("## Tables");
            writer.WriteLine();
            DbmlTable? lastChild = database.Tables.LastOrDefault();
            foreach (DbmlTable table in database.Tables)
            {
                WriteTableDeclaration(writer, table);

                if (table != lastChild)
                    writer.WriteLine();
            }
        }
    }

#pragma warning disable MA0051 // Method is too long (maximum allowed: 60)

    private static void WriteTableOfContents(IndentedTextWriter writer, DbmlDatabase database)
    {
#pragma warning disable CA1308 // Normalize strings to uppercase

        writer.WriteLine("## Table of contents");

        string projectNameLink =
            !string.IsNullOrWhiteSpace($"{database.Project}")
                ? $"{database.Project}"
                    .Replace(" ", "-", StringComparison.CurrentCulture)
                    .ToLowerInvariant()
                : string.Empty;

        bool shouldRenderProjectInfo =
            !string.IsNullOrWhiteSpace(projectNameLink);

        string databaseDesignLink =
            !string.IsNullOrWhiteSpace(projectNameLink)
                ? $"- [Database design: {database.Project}](#database-design-{projectNameLink})"
                : "- [Database design](#database-design)";

        writer.WriteLine();
        writer.WriteLine(databaseDesignLink);
        writer.Indent++;

        if (shouldRenderProjectInfo)
            writer.WriteLine($"- [Project {database.Project}](#project-{projectNameLink})");

        writer.WriteLine($"- [Table of contents](#table-of-contents)");
        writer.WriteLine($"- [Legend](#legend)");
        writer.WriteLine($"- [Tables](#tables)");
        foreach (DbmlTable table in database.Tables)
        {
            writer.Indent++;

            string tableNameLink =
                table.Name
                .Replace(" ", "-", StringComparison.CurrentCulture)
                .ToLowerInvariant();

            writer.WriteLine($"- [Table: {table.Name}](#table-{tableNameLink})");

            if (table.Indexes.Any())
            {
                writer.Indent++;
                writer.WriteLine($"- [Indexes: {table.Name}](#indexes-{tableNameLink})");
                writer.Indent--;
            }

            if (table.Relationships.Any())
            {
                writer.Indent++;
                writer.WriteLine($"- [Relationships: {table.Name}](#relationships-{tableNameLink})");
                writer.Indent--;
            }

            writer.Indent--;
        }

        if (shouldRenderProjectInfo)
            writer.Indent--;

        writer.Indent--;
#pragma warning restore CA1308 // Normalize strings to uppercase
    }

#pragma warning restore MA0051 // Method is too long (maximum allowed: 60)

    private static void WriteLegend(IndentedTextWriter writer, DbmlDatabase database)
    {
        writer.WriteLine("## Legend");
        writer.WriteLine();
        writer.WriteLine("- `DBML`: Database Markup Language. DBML is a simple, readable DSL language designed to define database structures.");
        writer.WriteLine("For more information, please check out [DBML homepage](https://dbml.dbdiagram.io/home/).");

        bool hasIndexes =
            database
                .Tables
                .SelectMany(p => p.Indexes)
                .Any();

        if (hasIndexes)
        {
            writer.WriteLine();
            writer.WriteLine("There are 3 types of index definitions:");
            writer.WriteLine();
            writer.WriteLine("- Index with single field (with index name): `CREATE INDEX created_at_index on users (created_at)`");
            writer.WriteLine("- Index with multiple fields (composite index): `CREATE INDEX on users (created_at, country)`");
            writer.WriteLine("- Index with an expression: `CREATE INDEX ON films ( first_name + last_name )`");
            writer.WriteLine("- (bonus) Composite index with expression: `CREATE INDEX ON users ( country, (lower(name)) )`");
        }

        bool hasRelations =
            database
                .Tables
                .SelectMany(p => p.Relationships)
                .Any();

        if (hasRelations)
        {
            writer.WriteLine();
            writer.WriteLine("There are 4 types of relationships: one-to-one, one-to-many, many-to-one and many-to-many:");
            writer.WriteLine();
            writer.WriteLine("- `<`: one-to-many. E.g: `users.id < posts.user_id`");
            writer.WriteLine("- `>`: many-to-one. E.g: `posts.user_id > users.id`");
            writer.WriteLine("- `-`: one-to-one. E.g: `users.id - user_infos.user_id`");
            writer.WriteLine("- `<>`: many-to-many. E.g: `authors.id <> books.id`");
        }
    }

    private static void WriteProjectDeclaration(IndentedTextWriter writer, DbmlProject project)
    {
        ArgumentNullException.ThrowIfNull(project);

        writer.WriteLine($"## Project {project.Name}");

        if (!string.IsNullOrEmpty(project.Note))
        {
            writer.WriteLine();
            writer.WriteLine($"**Notes**: {project.Note}");
        }
    }

    private static void WriteTableDeclaration(IndentedTextWriter writer, DbmlTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        writer.WriteLine($"### Table: {table.Name}");

        writer.WriteLine();
        WriteGoToLinks(writer);

        if (!string.IsNullOrEmpty(table.Note))
        {
            writer.WriteLine();
            writer.WriteLine($"**Notes**: {table.Note}");
        }

        if (table.Columns.Any())
        {
            writer.WriteLine();
            writer.WriteLine("| Column Name | Data Type | Max Length | Required | Default | Note |");
            writer.WriteLine("| :---------- | :-------- | :--------: | :------: | :-----: | :--- |");

            foreach (DbmlTableColumn column in table.Columns)
                WriteColumnDeclaration(writer, column);
        }

        if (table.Indexes.Any())
        {
            writer.WriteLine();
            writer.WriteLine($"#### Indexes: {table.Name}");

            writer.WriteLine();
            WriteGoToLinks(writer);

            writer.WriteLine();
            writer.WriteLine($"| Name | Table | Column | Primary Key | Unique | Note | Settings |");
            writer.WriteLine($"| :--- | :---- | :----- | :---------- | :----- | :--- | :------- |");

            foreach (DbmlTableIndex index in table.Indexes)
                WriteIndexDeclaration(writer, index);
        }

        if (table.Relationships.Any())
        {
            writer.WriteLine();
            writer.WriteLine($"#### Relationships: {table.Name}");

            writer.WriteLine();
            WriteGoToLinks(writer);

            writer.WriteLine();
            writer.WriteLine("| Primary Table | Primary Columns | Relation Type | Foreign Key Table | Foreign Key Columns |");
            writer.WriteLine("| :-----------: | :-------------: | :-----------: | :---------------- | :-----------------: |");

            foreach (DbmlTableRelationship relationship in table.Relationships)
                WriteRelationshipDeclaration(writer, relationship);
        }
    }

    private static void WriteGoToLinks(IndentedTextWriter writer)
    {
        writer.WriteLine($"[☝️ To table of contents](#table-of-contents)");
    }

    private static void WriteColumnDeclaration(IndentedTextWriter writer, DbmlTableColumn column)
    {
        ArgumentNullException.ThrowIfNull(column);

        string name = column.IsPrimaryKey
            ? $"🔑 `{column.Name}`"
            : column.Name;

        string type = column.Type ?? UnknownValue;
        string maxLength = column switch
        {
            { HasMaxLength: true } => "MAX",
            { MaxLength: { } } => $"{column.MaxLength}",
            _ => UnknownValue
        };

        string isRequired = column.IsRequired ? "✔️ true" : "❌ false";
        string defaultValue = column switch
        {
            { IsRequired: false } => "null",
            { HasDefaultValue: true } => $"{column.DefaultValue}",
            _ => UnknownValue
        };

        string note = column.Note ?? UnknownValue;
        writer.WriteLine($"| {name} | {type} | {maxLength} | {isRequired} | {defaultValue} | {note} |");
    }

    private static void WriteIndexDeclaration(IndentedTextWriter writer, DbmlTableIndex index)
    {
        ArgumentNullException.ThrowIfNull(index);

        string indexName = index.IsPrimaryKey ? $"🔑 {index.Name}" : index.Name;
        string indexTable = index.Table?.Name ?? UnknownValue;
        string indexColumn = index.ColumnName;
        string primaryKeyColumn = index.IsPrimaryKey ? "✔️ yes" : "❌ no";
        string uniqueColumn = index.IsUnique ? "✔️ yes" : "❌ no";
        string indexNote = index.Note ?? UnknownValue;
        const string indexSettings = UnknownValue;

        writer.WriteLine($"| {indexName} | {indexTable} | {indexColumn} | {primaryKeyColumn} | {uniqueColumn} | {indexNote} | {indexSettings} |");
    }

    private static void WriteRelationshipDeclaration(IndentedTextWriter writer, DbmlTableRelationship relationship)
    {
        string primaryTable = relationship.FromIdentifier.TableName ?? UnknownValue;
        string primaryColumn = relationship.FromIdentifier.ColumnName ?? UnknownValue;
        string relationType = relationship.RelationshipType switch
        {
            TableRelationshipType.OneToMany => $"< (**{relationship.RelationshipType}**)",
            TableRelationshipType.ManyToOne => $"> (**{relationship.RelationshipType}**)",
            TableRelationshipType.OneToOne => $"- (**{relationship.RelationshipType}**)",
            TableRelationshipType.ManyToMany => $"<> (**{relationship.RelationshipType}**)",
            _ => $"{relationship.RelationshipType}"
        };

        string foreignKeyTable = relationship.ToIdentifier.TableName ?? UnknownValue;
        string foreignKeyColumn = relationship.ToIdentifier.ColumnName ?? UnknownValue;

        writer.WriteLine($"| {primaryTable} | {primaryColumn} | {relationType} | {foreignKeyTable} | {foreignKeyColumn} |");
    }
}
