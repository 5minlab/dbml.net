# Database design: Application Identity

## Project Application Identity

**Notes**: Identity database schema.

## Table of contents

- [Database design: Application Identity](#database-design-application-identity)
  - [Project Application Identity](#project-application-identity)
  - [Table of contents](#table-of-contents)
  - [Legend](#legend)
  - [Tables](#tables)
    - [Table: Users](#table-users)
      - [Indexes: Users](#indexes-users)
    - [Table: UserClaims](#table-userclaims)
      - [Relationships: UserClaims](#relationships-userclaims)
    - [Table: UserLogins](#table-userlogins)
      - [Relationships: UserLogins](#relationships-userlogins)
    - [Table: UserTokens](#table-usertokens)
      - [Relationships: UserTokens](#relationships-usertokens)
    - [Table: UserRoles](#table-userroles)
      - [Relationships: UserRoles](#relationships-userroles)
    - [Table: Roles](#table-roles)
      - [Indexes: Roles](#indexes-roles)
    - [Table: RoleClaims](#table-roleclaims)
      - [Relationships: RoleClaims](#relationships-roleclaims)

## Legend

- `DBML`: Database Markup Language. DBML is a simple, readable DSL language designed to define database structures.
For more information, please check out [DBML homepage](https://dbml.dbdiagram.io/home/).

There are 3 types of index definitions:

- Index with single field (with index name): `CREATE INDEX created_at_index on users (created_at)`
- Index with multiple fields (composite index): `CREATE INDEX on users (created_at, country)`
- Index with an expression: `CREATE INDEX ON films ( first_name + last_name )`
- (bonus) Composite index with expression: `CREATE INDEX ON users ( country, (lower(name)) )`

There are 4 types of relationships: one-to-one, one-to-many, many-to-one and many-to-many:

- `<`: one-to-many. E.g: `users.id < posts.user_id`
- `>`: many-to-one. E.g: `posts.user_id > users.id`
- `-`: one-to-one. E.g: `users.id - user_infos.user_id`
- `<>`: many-to-many. E.g: `authors.id <> books.id`

## Tables

### Table: Users

[☝️ To table of contents](#table-of-contents)

**Notes**: Represents a user in the identity system.

| Column Name | Data Type | Max Length | Required | Default | Note |
| :---------- | :-------- | :--------: | :------: | :-----: | :--- |
| 🔑 `Id` | nvarchar(450) | 450 | ✔️ true | N/A | The primary key for this user. |
| UserName | nvarchar(256) | 256 | ❌ false | null | The user name for this user. |
| NormalizedUserName | nvarchar(256) | 256 | ❌ false | null | The normalized user name for this user. |
| Email | nvarchar(256) | 256 | ❌ false | null | The email address for this user. |
| NormalizedEmail | nvarchar(256) | 256 | ❌ false | null | The normalized email address for this user. |
| EmailConfirmed | bit | 1 | ✔️ true | False | A flag indicating if a user has confirmed their email address. |
| PasswordHash | nvarchar(MAX) | MAX | ❌ false | null | A salted and hashed representation of the password for this user. |
| SecurityStamp | nvarchar(MAX) | MAX | ❌ false | null | A random value that must change whenever a users credentials change (password changed, login removed). |
| ConcurrencyStamp | nvarchar(MAX) | MAX | ❌ false | null | A random value that must change whenever a user is persisted to the store. |
| PhoneNumber | nvarchar(MAX) | MAX | ❌ false | null | A telephone number for the user. |
| PhoneNumberConfirmed | bit | 1 | ✔️ true | False | A flag indicating if a user has confirmed their telephone address. |
| TwoFactorEnabled | bit | 1 | ✔️ true | False | A flag indicating if two factor authentication is enabled for this user. |
| LockoutEnd | datetimeoffset(7) | 3.155378976E+18 | ❌ false | null | The date and time, in UTC, when any user lockout ends. |
| LockoutEnabled | bit | 1 | ✔️ true | False | A flag indicating if the user could be locked out. |
| AccessFailedCount | int | 2147483647 | ✔️ true | 0 | The number of failed login attempts for the current user. |

#### Indexes: Users

[☝️ To table of contents](#table-of-contents)

| Name | Table | Column | Primary Key | Unique | Note | Settings |
| :--- | :---- | :----- | :---------- | :----- | :--- | :------- |
| UserNameIndex | Users | NormalizedUserName | ❌ no | ✔️ yes | N/A | N/A |
| EmailIndex | Users | NormalizedEmail | ❌ no | ✔️ yes | N/A | N/A |

### Table: UserClaims

[☝️ To table of contents](#table-of-contents)

**Notes**: Represents a claim that a user possesses.

| Column Name | Data Type | Max Length | Required | Default | Note |
| :---------- | :-------- | :--------: | :------: | :-----: | :--- |
| 🔑 `Id` | nvarchar(450) | 450 | ✔️ true | N/A | The identifier for this user claim. |
| UserId | nvarchar(450) | 450 | ✔️ true | N/A | The primary key of the user associated with this claim. |
| ClaimType | nvarchar(450) | 450 | ❌ false | null | The claim type for this claim. |
| ClaimValue | nvarchar(450) | 450 | ❌ false | null | The claim value for this claim. |

#### Relationships: UserClaims

[☝️ To table of contents](#table-of-contents)

| Primary Table | Primary Columns | Relation Type | Foreign Key Table | Foreign Key Columns |
| :-----------: | :-------------: | :-----------: | :---------------- | :-----------------: |
| UserClaims | UserId | > (**ManyToOne**) | Users | Id |

### Table: UserLogins

[☝️ To table of contents](#table-of-contents)

**Notes**: Represents a login and its associated provider for a user.

| Column Name | Data Type | Max Length | Required | Default | Note |
| :---------- | :-------- | :--------: | :------: | :-----: | :--- |
| UserId | nvarchar(450) | 450 | ✔️ true | N/A | The primary key of the user associated with this login. |
| 🔑 `LoginProvider` | nvarchar(450) | 450 | ✔️ true | N/A | The login provider for the login (e.g. facebook, google). |
| 🔑 `ProviderKey` | nvarchar(450) | 450 | ✔️ true | N/A | The unique provider identifier for this login. |
| ProviderDisplayName | nvarchar(450) | 450 | ❌ false | null | The friendly name used in a UI for this login. |

#### Relationships: UserLogins

[☝️ To table of contents](#table-of-contents)

| Primary Table | Primary Columns | Relation Type | Foreign Key Table | Foreign Key Columns |
| :-----------: | :-------------: | :-----------: | :---------------- | :-----------------: |
| UserLogins | UserId | > (**ManyToOne**) | Users | Id |

### Table: UserTokens

[☝️ To table of contents](#table-of-contents)

**Notes**: Represents an authentication token for a user.

| Column Name | Data Type | Max Length | Required | Default | Note |
| :---------- | :-------- | :--------: | :------: | :-----: | :--- |
| 🔑 `UserId` | nvarchar(450) | 450 | ✔️ true | N/A | The primary key of the user that the token belongs to. |
| 🔑 `LoginProvider` | nvarchar(128) | 128 | ✔️ true | N/A | The LoginProvider this token is from. |
| 🔑 `Name` | nvarchar(128) | 128 | ✔️ true | N/A | The name of the token. |
| Value | nvarchar(MAX) | MAX | ✔️ true | N/A | The token value. |

#### Relationships: UserTokens

[☝️ To table of contents](#table-of-contents)

| Primary Table | Primary Columns | Relation Type | Foreign Key Table | Foreign Key Columns |
| :-----------: | :-------------: | :-----------: | :---------------- | :-----------------: |
| UserTokens | UserId | > (**ManyToOne**) | Users | Id |

### Table: UserRoles

[☝️ To table of contents](#table-of-contents)

**Notes**: Represents the link between a user and a role.

| Column Name | Data Type | Max Length | Required | Default | Note |
| :---------- | :-------- | :--------: | :------: | :-----: | :--- |
| 🔑 `UserId` | nvarchar(450) | 450 | ✔️ true | N/A | The primary key of the user that is linked to a role. |
| 🔑 `RoleId` | nvarchar(450) | 450 | ✔️ true | N/A | The primary key of the role that is linked to the user. |

#### Relationships: UserRoles

[☝️ To table of contents](#table-of-contents)

| Primary Table | Primary Columns | Relation Type | Foreign Key Table | Foreign Key Columns |
| :-----------: | :-------------: | :-----------: | :---------------- | :-----------------: |
| UserRoles | UserId | > (**ManyToOne**) | Users | Id |
| UserRoles | RoleId | > (**ManyToOne**) | Roles | Id |

### Table: Roles

[☝️ To table of contents](#table-of-contents)

**Notes**: Represents a role in the identity system.

| Column Name | Data Type | Max Length | Required | Default | Note |
| :---------- | :-------- | :--------: | :------: | :-----: | :--- |
| 🔑 `Id` | nvarchar(450) | 450 | ✔️ true | N/A | The primary key for this role. |
| Name | nvarchar(256) | 256 | ❌ false | null | The name for this role. |
| NormalizedName | nvarchar(256) | 256 | ❌ false | null | The normalized name for this role. |
| ConcurrencyStamp | nvarchar(MAX) | MAX | ❌ false | null | A random value that should change whenever a role is persisted to the store. |

#### Indexes: Roles

[☝️ To table of contents](#table-of-contents)

| Name | Table | Column | Primary Key | Unique | Note | Settings |
| :--- | :---- | :----- | :---------- | :----- | :--- | :------- |
| RoleNameIndex | Roles | NormalizedName | ❌ no | ✔️ yes | N/A | N/A |

### Table: RoleClaims

[☝️ To table of contents](#table-of-contents)

**Notes**: Represents a claim that is granted to all users within a role.

| Column Name | Data Type | Max Length | Required | Default | Note |
| :---------- | :-------- | :--------: | :------: | :-----: | :--- |
| 🔑 `Id` | int | 2147483647 | ✔️ true | 0 | The identifier for this role claim. |
| RoleId | nvarchar(450) | 450 | ✔️ true | N/A | The primary key of the role associated with this claim. |
| ClaimType | nvarchar(450) | 450 | ❌ false | null | The claim type for this claim. |
| ClaimValue | nvarchar(450) | 450 | ❌ false | null | The claim value for this claim. |

#### Relationships: RoleClaims

[☝️ To table of contents](#table-of-contents)

| Primary Table | Primary Columns | Relation Type | Foreign Key Table | Foreign Key Columns |
| :-----------: | :-------------: | :-----------: | :---------------- | :-----------------: |
| RoleClaims | RoleId | > (**ManyToOne**) | Roles | Id |
