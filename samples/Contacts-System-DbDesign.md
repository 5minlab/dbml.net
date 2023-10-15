# Database design: Application Contacts

## Project Application Contacts

**Notes**: Contacts database schema.

## Table of contents

- [Database design: Application Contacts](#database-design-application-contacts)
  - [Project Application Contacts](#project-application-contacts)
  - [Table of contents](#table-of-contents)
  - [Legend](#legend)
  - [Tables](#tables)
    - [Table: Users](#table-users)
      - [Indexes: Users](#indexes-users)
    - [Table: UserContacts](#table-usercontacts)
      - [Relationships: UserContacts](#relationships-usercontacts)
    - [Table: Contacts](#table-contacts)
      - [Indexes: Contacts](#indexes-contacts)
    - [Table: ContactEmails](#table-contactemails)
      - [Relationships: ContactEmails](#relationships-contactemails)
    - [Table: ContactPhoneNumbers](#table-contactphonenumbers)
      - [Relationships: ContactPhoneNumbers](#relationships-contactphonenumbers)
    - [Table: ContactResidenceAddresses](#table-contactresidenceaddresses)
      - [Relationships: ContactResidenceAddresses](#relationships-contactresidenceaddresses)
    - [Table: ContactRelations](#table-contactrelations)
      - [Relationships: ContactRelations](#relationships-contactrelations)

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

**Notes**: Represents a user in the contacts system.

| Column Name | Data Type | Max Length | Required | Default | Note |
| :---------- | :-------- | :--------: | :------: | :-----: | :--- |
| 🔑 `Id` | nvarchar(450) | 450 | ✔️ true | N/A | The primary key for this user. |
| UserName | nvarchar(250) | 250 | ✔️ true | N/A | The user name for this user. |
| NormalizedUserName | nvarchar(250) | 250 | ✔️ true | N/A | The normalized user name for this user. |
| Email | nvarchar(250) | 250 | ✔️ true | N/A | The email address for this user. |
| NormalizedEmail | nvarchar(250) | 250 | ✔️ true | N/A | The normalized email address for this user. |
| ConcurrencyStamp | nvarchar(MAX) | MAX | ❌ false | null | A random value that must change whenever a user is persisted to the store. |

#### Indexes: Users

[☝️ To table of contents](#table-of-contents)

| Name | Table | Column | Primary Key | Unique | Note | Settings |
| :--- | :---- | :----- | :---------- | :----- | :--- | :------- |
| UserNameIndex | Users | NormalizedUserName | ❌ no | ✔️ yes | N/A | N/A |
| EmailIndex | Users | NormalizedEmail | ❌ no | ✔️ yes | N/A | N/A |

### Table: UserContacts

[☝️ To table of contents](#table-of-contents)

**Notes**: Represents the link between a user and a contact.

| Column Name | Data Type | Max Length | Required | Default | Note |
| :---------- | :-------- | :--------: | :------: | :-----: | :--- |
| 🔑 `UserId` | nvarchar(450) | 450 | ✔️ true | N/A | The primary key of the user that is linked to a contact. |
| 🔑 `ContactId` | nvarchar(450) | 450 | ✔️ true | N/A | The primary key of the contact that is linked to a user. |

#### Relationships: UserContacts

[☝️ To table of contents](#table-of-contents)

| Primary Table | Primary Columns | Relation Type | Foreign Key Table | Foreign Key Columns |
| :-----------: | :-------------: | :-----------: | :---------------- | :-----------------: |
| UserContacts | UserId | > (**ManyToOne**) | Users | Id |
| UserContacts | ContactId | > (**ManyToOne**) | Contacts | Id |

### Table: Contacts

[☝️ To table of contents](#table-of-contents)

**Notes**: Represents an contact in the contacts system.

| Column Name | Data Type | Max Length | Required | Default | Note |
| :---------- | :-------- | :--------: | :------: | :-----: | :--- |
| 🔑 `Id` | nvarchar(450) | 450 | ✔️ true | N/A | The primary key of this contact. |
| GivenName | nvarchar(50) | 50 | ✔️ true | N/A | The given name of this contact. |
| MiddleName | nvarchar(50) | 50 | ❌ false | null | The middle name of this contact. |
| FamilyName | nvarchar(50) | 50 | ❌ false | null | The family name of this contact. |
| NormalizedFullName | nvarchar(250) | 250 | ✔️ true | N/A | The normalized full name for this contact. |
| DateOfBirth | datetimeoffset(7) | 3.155378976E+18 | ❌ false | null | The date and time, in UTC, when the contact was born. |
| Remarks | nvarchar(2000) | 2000 | ❌ false | null | The remarks of this contact. |
| ConcurrencyStamp | nvarchar(MAX) | MAX | ❌ false | null | A random value that should change whenever a contact is persisted to the store. |

#### Indexes: Contacts

[☝️ To table of contents](#table-of-contents)

| Name | Table | Column | Primary Key | Unique | Note | Settings |
| :--- | :---- | :----- | :---------- | :----- | :--- | :------- |
| FullNameIndex | Contacts | NormalizedFullName | ❌ no | ❌ no | N/A | N/A |

### Table: ContactEmails

[☝️ To table of contents](#table-of-contents)

**Notes**: Represents an email-address that a contact possesses.

| Column Name | Data Type | Max Length | Required | Default | Note |
| :---------- | :-------- | :--------: | :------: | :-----: | :--- |
| 🔑 `ContactId` | nvarchar(450) | 450 | ✔️ true | N/A | The primary key of the contact that this email belongs to. |
| Email | nvarchar(250) | 250 | ✔️ true | N/A | The email address for this contact. |
| 🔑 `NormalizedEmail` | nvarchar(250) | 250 | ✔️ true | N/A | The normalized email address for this contact. |
| EmailConfirmed | bit | 1 | ✔️ true | false | A flag indicating if a contact has confirmed their email address. |
| InactiveSince | datetimeoffset(7) | 3.155378976E+18 | ❌ false | null | The date and time, in UTC, since the email is inactive. For NULL values the email is active. |
| Label | nvarchar(50) | 50 | ❌ false | null | The label assigned to this address residence. |
| ConcurrencyStamp | nvarchar(MAX) | MAX | ❌ false | null | A random value that should change whenever a email is persisted to the store. |

#### Relationships: ContactEmails

[☝️ To table of contents](#table-of-contents)

| Primary Table | Primary Columns | Relation Type | Foreign Key Table | Foreign Key Columns |
| :-----------: | :-------------: | :-----------: | :---------------- | :-----------------: |
| ContactEmails | ContactId | > (**ManyToOne**) | Contacts | Id |

### Table: ContactPhoneNumbers

[☝️ To table of contents](#table-of-contents)

**Notes**: Represents an phone-number that a contact possesses.

| Column Name | Data Type | Max Length | Required | Default | Note |
| :---------- | :-------- | :--------: | :------: | :-----: | :--- |
| 🔑 `ContactId` | nvarchar(450) | 450 | ✔️ true | N/A | The primary key of the contact that this phone number belongs to. |
| 🔑 `Prefix` | nvarchar(3) | 3 | ✔️ true | N/A | The ITU E.164 prefix for this phone number. |
| 🔑 `Value` | nvarchar(15) | 15 | ✔️ true | N/A | The ITU E.164 value for this phone number. |
| PhoneNumberConfirmed | bit | 1 | ✔️ true | false | A flag indicating if a contact has confirmed their phone number. |
| InactiveSince | datetimeoffset(7) | 3.155378976E+18 | ❌ false | null | The date and time, in UTC, since the phone number is inactive. Phone number is active for `NULL` values. |
| Label | nvarchar(50) | 50 | ❌ false | null | The label assigned to this address residence. |
| ConcurrencyStamp | nvarchar(MAX) | MAX | ❌ false | null | A random value that should change whenever a phone number is persisted to the store. |

#### Relationships: ContactPhoneNumbers

[☝️ To table of contents](#table-of-contents)

| Primary Table | Primary Columns | Relation Type | Foreign Key Table | Foreign Key Columns |
| :-----------: | :-------------: | :-----------: | :---------------- | :-----------------: |
| ContactPhoneNumbers | ContactId | > (**ManyToOne**) | Contacts | Id |

### Table: ContactResidenceAddresses

[☝️ To table of contents](#table-of-contents)

**Notes**: Represents an address that a contact possesses.

| Column Name | Data Type | Max Length | Required | Default | Note |
| :---------- | :-------- | :--------: | :------: | :-----: | :--- |
| 🔑 `ContactId` | nvarchar(450) | 450 | ✔️ true | N/A | The primary key of the contact that this residence address belongs to. |
| 🔑 `NormalizedAddress` | nvarchar(250) | 250 | ✔️ true | N/A | The normalized address for this residence (ex: Str. {StreetName}, {Postcode} {Town} {County} {CountryCode}, PO box. {PostOfficeBox}.). |
| CountryCode | nvarchar(2) | 2 | ❌ false | null | The ISO 3166 two-letter country code for this residence address. |
| StreetName1 | nvarchar(250) | 250 | ❌ false | null | The line first of the street name for this residence address. |
| StreetName2 | nvarchar(250) | 250 | ❌ false | null | The line second of the street name for this residence address. |
| Postcode | nvarchar(250) | 250 | ❌ false | null | The post code for this residence address. |
| Town | nvarchar(250) | 250 | ❌ false | null | The town for this residence address. |
| County | nvarchar(250) | 250 | ❌ false | null | The county/sector for this residence address. |
| PostOfficeBox | nvarchar(40) | 40 | ❌ false | null | The post office number of a contact for this residence address. |
| Label | nvarchar(50) | 50 | ❌ false | null | The label for this residence address. |
| ConcurrencyStamp | nvarchar(MAX) | MAX | ❌ false | null | A random value that should change whenever a residence address is persisted to the store. |

#### Relationships: ContactResidenceAddresses

[☝️ To table of contents](#table-of-contents)

| Primary Table | Primary Columns | Relation Type | Foreign Key Table | Foreign Key Columns |
| :-----------: | :-------------: | :-----------: | :---------------- | :-----------------: |
| ContactResidenceAddresses | ContactId | > (**ManyToOne**) | Contacts | Id |

### Table: ContactRelations

[☝️ To table of contents](#table-of-contents)

| Column Name | Data Type | Max Length | Required | Default | Note |
| :---------- | :-------- | :--------: | :------: | :-----: | :--- |
| 🔑 `InitiatorContactId` | nvarchar(450) | 450 | ✔️ true | N/A | The primary key of the initiator contact that is linked to the target contact by relation-type. |
| 🔑 `TargetContactId` | nvarchar(450) | 450 | ✔️ true | N/A | The primary key of the target contact that is linked to the initiator contact by relation-type. |
| RelationType | nvarchar(450) | 450 | ✔️ true | N/A | The type for this relation (ex: mother of,father of,friend of). |
| 🔑 `NormalizedRelationType` | nvarchar(450) | 450 | ✔️ true | N/A | The normalized type for this relation. |
| StartDate | datetimeoffset(7) | 3.155378976E+18 | ❌ false | null | The date and time, in UTC, since the relation started. |
| EndDate | datetimeoffset(7) | 3.155378976E+18 | ❌ false | null | The date and time, in UTC, since the relation ended. |
| ConcurrencyStamp | nvarchar(MAX) | MAX | ❌ false | null | A random value that should change whenever a relation is persisted to the store. |

#### Relationships: ContactRelations

[☝️ To table of contents](#table-of-contents)

| Primary Table | Primary Columns | Relation Type | Foreign Key Table | Foreign Key Columns |
| :-----------: | :-------------: | :-----------: | :---------------- | :-----------------: |
| ContactRelations | InitiatorContactId | > (**ManyToOne**) | Contacts | Id |
| ContactRelations | TargetContactId | > (**ManyToOne**) | Contacts | Id |
