# TODO 

## Documentation 

- [ ] Add documents (taxation, employment, termination) for the following countries: Russia, Georgia, Armenia, Kazakhstan, Serbia, Montenegro, Italy, Spain, Turkey, Cyprus, Argentina, Mexico, Brazil, Uruguay, USA, Canada.
- [ ] Classes for onboarding.
- [ ] Add XML and JSON extensions.

## Database connections 

- [ ] Create template for using multiple database connections.
- [ ] How to deal with `blob` objects in databases connection classes?
    - [ ] Add the functionality to get byte arrays from the DB connection.
- [ ] Method `VelocipedeUtils.Shared.DbConnections.BaseDbConnection.GetSqlFromDataTable()` does not correct SQL statement, so you need to explain how to use this method properly. Maybe, this method should be implemented based on metadata about the tables.
- [ ] How to transfer data from one database to another?
- [ ] Execute requests with parameters.
- [ ] Extend methods for getting connection string.
- [ ] Remove code duplication for database operations using Dapper.
- [ ] Connect to another DB within an existing connection if it's active. This can reduce the number of connection initialization and increase performance of the library.
- [ ] Execute `SELECT` from another database:
    - [ ] MS SQL:
        - To select a table from another database within the same SQL Server instance, you can use a fully qualified name in your `SELECT` statement: `SELECT * FROM [DatabaseName].[SchemaName].[TableName];`.
    - [ ] PostgreSQL:
        - Directly selecting a table from another database within a single `SELECT` statement is not a native feature in PostgreSQL. However, you can achieve this functionality using the `postgres_fdw` (Foreign Data Wrapper) extension. This extension allows you to treat a table in a remote PostgreSQL database as if it were a local table.
- [ ] Test info about columns, foreign keys in different databases:
    - [ ] Check not only quantity but the content of the response as well.
    - [ ] Adapt tests for all tables.
    - [ ] Check how the methods for with and without the quotes: `'`, `"`, `` ` `` (in MS SQL it could be `[]` as well).
- [ ] Test how the library can deal with multithreaded apps.
    - [ ] Note that SQLite offers three threading modes:
        - **Single-thread mode**: This mode disables all mutexes and is unsafe for use by multiple threads.
        - **Multi-thread mode**: In this mode, SQLite can be safely used by multiple threads, provided that each thread uses its own database connection. No single database connection or any object derived from it (like a prepared statement) should be used by two or more threads simultaneously. 
        - **Serialized mode**: This is the default and safest mode for multithreaded applications. It enables mutexes to protect all shared data structures, including individual database connections. This means that even if you attempt to use a single database connection from multiple threads, SQLite will internally serialize access to ensure thread safety.

## MS Excel converter 

- [ ] Add new worksheet using OpenXML. 

## Network communication 

- [ ] Potentially, [Enterprise service bus](https://en.wikipedia.org/wiki/Enterprise_service_bus) could be implemented using this library.

![ESB-components](https://upload.wikimedia.org/wikipedia/commons/thumb/1/1d/ESB_Component_Hive.png/330px-ESB_Component_Hive.png)

- [ ] Additional fields for internetworking:
    - [ ] Fields for authentication and authorization when interacting between microservices (for example, access token).
    - [ ] Fields for monitoring (for example, response time, number of requests).

## Models

- [ ] In the recipe object, you need to set a status that would show the relevance of the recipe - [ ] actually, there is already a status of the entity, which can be equal to "deleted" or "active", but in a real situation it may be important to set deadlines for relevance.

## Data representation

- [ ] Data visualization: Line chart, Bar chart, Histogram, Scatter plot, Box plot, Pareto chart, Pie chart, Area chart, Tree map, Bubble chart, Stripe graphic, Control chart, Run chart, Stem-and-leaf display, Cartogram, Small multiple, Sparkline, Table, Marimekko chart. 

## Architectural 

- [ ] Add XML/JSON wrapper.
