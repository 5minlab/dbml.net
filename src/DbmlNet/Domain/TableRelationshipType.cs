namespace DbmlNet.Domain;

/// <summary>
/// Represents a relationship type.
/// </summary>
public enum TableRelationshipType
{
    /// <summary>
    /// One-to-many, <![CDATA[`<`]]> token.
    /// E.g: users.id <![CDATA[<]]> posts.user_id.
    /// </summary>
    OneToMany,

    /// <summary>
    /// Many-to-one, `>` token.
    /// E.g: posts.user_id > users.id.
    /// </summary>
    ManyToOne,

    /// <summary>
    /// One-to-one, `-` token.
    /// E.g: users.id - user_infos.user_id.
    /// </summary>
    OneToOne,

    /// <summary>
    /// Many-to-many, <![CDATA[`<>`]]> token.
    /// E.g: authors.id <![CDATA[<>]]> books.id.
    /// </summary>
    ManyToMany,
}
