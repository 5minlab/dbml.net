namespace DbmlNet.Domain;

/// <summary>
/// Represents a table relationship.
/// </summary>
public sealed class DbmlTableRelationship
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DbmlTableRelationship"/>.
    /// </summary>
    /// <param name="fromIdentifier">The identifier of the source column.</param>
    /// <param name="relationshipType">The type of relationship between the tables.</param>
    /// <param name="toIdentifier">The identifier of the target column.</param>
    public DbmlTableRelationship(
        DbmlColumnIdentifier fromIdentifier,
        TableRelationshipType relationshipType,
        DbmlColumnIdentifier toIdentifier)
    {
        FromIdentifier = fromIdentifier;
        RelationshipType = relationshipType;
        ToIdentifier = toIdentifier;
    }

    /// <summary>
    /// Gets the from identifier.
    /// </summary>
    public DbmlColumnIdentifier FromIdentifier { get; }

    /// <summary>
    /// Gets the relationship type.
    /// </summary>
    public TableRelationshipType RelationshipType { get; }

    /// <summary>
    /// Gets the to identifier.
    /// </summary>
    public DbmlColumnIdentifier ToIdentifier { get; }
}
