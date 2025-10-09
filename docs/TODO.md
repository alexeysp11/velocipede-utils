# TODO 

## Documentation 

- [ ] Add documents (taxation, employment, termination) for the following countries: Russia, Georgia, Armenia, Kazakhstan, Serbia, Montenegro, Italy, Spain, Turkey, Cyprus, Argentina, Mexico, Brazil, Uruguay, USA, Canada.
- [ ] Classes for onboarding.
- [ ] Add XML and JSON extensions.

## Database connections 

- [ ] Explain what is the differnece between `DataSource` and `ConnString` in database classes and to use them properly. 
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
- [ ] Info about columns, foreign keys in different databases is not unique.

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
