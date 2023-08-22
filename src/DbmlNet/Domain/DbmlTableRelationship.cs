namespace DbmlNet.Domain;

/// <summary>
/// </summary>
public sealed class DbmlTableRelationship
{
    /// <summary>
    /// </summary>
    /// <param name="fromIdentifier"></param>
    /// <param name="relationshipType"></param>
    /// <param name="toIdentifier"></param>
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
    /// </summary>
    public DbmlColumnIdentifier FromIdentifier { get; }

    /// <summary>
    /// </summary>
    public TableRelationshipType RelationshipType { get; }

    /// <summary>
    /// </summary>
    public DbmlColumnIdentifier ToIdentifier { get; }
}
