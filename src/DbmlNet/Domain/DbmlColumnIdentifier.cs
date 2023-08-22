using System;
using System.Security.Cryptography;

namespace DbmlNet.Domain;

/// <summary>
/// </summary>
public sealed class DbmlColumnIdentifier
{
    /// <summary>
    /// </summary>
    /// <param name="schemaName"></param>
    /// <param name="tableName"></param>
    /// <param name="columnName"></param>
    public DbmlColumnIdentifier(
        string schemaName,
        string tableName,
        string columnName)
    {
        SchemaName = schemaName;
        TableName = tableName;
        ColumnName = columnName;
    }

    /// <summary>
    /// </summary>
    public string SchemaName { get; }

    /// <summary>
    /// </summary>
    public string TableName { get; }

    /// <summary>
    /// </summary>
    public string ColumnName { get; }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        if (!string.IsNullOrEmpty(SchemaName))
            return $"{SchemaName}.{TableName}.{ColumnName}";
        else if (!string.IsNullOrEmpty(TableName))
            return $"{TableName}.{ColumnName}";
        else
            return $"{TableName}.{ColumnName}";
    }
}
