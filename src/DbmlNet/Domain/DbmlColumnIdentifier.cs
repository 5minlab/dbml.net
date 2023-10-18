namespace DbmlNet.Domain;

/// <summary>
/// Represents a column identifier.
/// </summary>
public sealed class DbmlColumnIdentifier
{

    /// <summary>
    /// Initializes a new instance of the <see cref="DbmlColumnIdentifier"/> with the specified schema name, table name, and column name.
    /// </summary>
    /// <param name="schemaName">The schema name.</param>
    /// <param name="tableName">The table name.</param>
    /// <param name="columnName">The column name.</param>
    public DbmlColumnIdentifier(
        string? schemaName,
        string? tableName,
        string columnName)
    {
        SchemaName = schemaName;
        TableName = tableName;
        ColumnName = columnName;
    }

    /// <summary>
    /// Gets the schema name.
    /// </summary>
    public string? SchemaName { get; }

    /// <summary>
    /// Gets the table name.
    /// </summary>
    public string? TableName { get; }

    /// <summary>
    /// Gets the column name.
    /// </summary>
    public string ColumnName { get; }

    /// <summary>
    /// Returns the full text for this column identifier using format {schema}.{table}.{column}.
    /// </summary>
    /// <returns>The full text for this column identifier.</returns>
    public override string ToString()
    {
        if (!string.IsNullOrEmpty(SchemaName))
            return $"{SchemaName}.{TableName}.{ColumnName}";
        else if (!string.IsNullOrEmpty(TableName))
            return $"{TableName}.{ColumnName}";
        else
            return ColumnName;
    }
}
