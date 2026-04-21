# TODO

- [ ] DDL: Operations used to build and modify the database structure itself:
    - creating databases,
    - creating tables,
    - add/delete triggers,
    - altering table structure (e.g. add/delete columns, add/delete indexes),
    - or dropping tables.
- [ ] Data Control Language (DCL): Commands for managing user access and permissions, such as GRANT and REVOKE.
- [ ] Database maintenance: Tasks like creating backups, restoring data, replication, and optimizing the database.

---

DbOperations | unifying metadata, columns, extend datatypes

- [ ] Refactoring:
    - [ ] Determine whether a column is unique or autoincremented.
- [ ] Column sorting:
    - [ ] Add sorting by a specified column to retrieve all the data.
    - [ ] Check whether the column order will be preserved when retrieving metadata if columns are changed or removed and then added.
- [ ] Add the datatypes the library can parse.
        - [ ] Попробовать подобрать такой `DbType`, чтобы было выброшено исключение о том, что тип не поддерживается. После этого исправить данное исключение.
    - [ ] Native datatypes:
        - [ ] SQLite
            - https://sqlite.org/datatype3.html
        - [ ] PostgreSQL
            - [ ] https://www.postgresql.org/docs/current/datatype.html
        - [ ] SQL Server
            - [ ] **Other datatypes**: `cursor`, `geography`, `geometry`, `hierarchyid`, `json`, `vector`, `rowversion`, `sql_variant` `table`, `uniqueidentifier`, `xml`.
                - https://learn.microsoft.com/en-us/sql/t-sql/data-types/data-types-transact-sql?view=sql-server-ver17#other-data-types
- [ ] Testing:
    - [ ] SQLite
    - [ ] PostgreSQL
    - [ ] SQL Server

---

DbOperations | QueryBuilder, alter table, columns

- [ ] Refactoring.
- [ ] Development:
    - [ ] `AlterTableQueryBuilder`
        - [ ] Initializing QueryBuilder to altering tables.
            - [ ] Query construction for databases.
                - [ ] SQLite:
                    - https://www.sqlite.org/lang_altertable.html
                - [ ] PostgreSQL:
                    - https://www.postgresql.org/docs/current/sql-altertable.html
- [ ] Integration tests.
    - [ ] `AlterTableQueryBuilder`
- [ ] Unit tests.
    - [ ] `AlterTableQueryBuilder`

---

- [ ] Добавить тесты для случаев, когда таблица создавалась с `[]` (актуально для MS SQL).
