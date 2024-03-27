# TODO 

## Documentation 

- Add docs for all the `Monetary` classes.
- Add documents (taxation, employment, termination) for the following countries: Russia, Georgia, Armenia, Kazakhstan, Serbia, Montenegro, Italy, Spain, Turkey, Cyprus, Argentina, Mexico, Brazil, Uruguay, USA, Canada.
- Classes for onboarding.
- Add XML and JSON extensions.

## Database connections 

- Explain what is the differnece between `DataSource` and `ConnString` in database classes and to use them properly. 
- Create template for using multiple database connections. 
- How to deal with `blob` objects in databases connection classes?
    - Add the functionality to get byte arrays from the DB connection.
- Method `Cims.WorkflowLib.DbConnections.BaseDbConnection.GetSqlFromDataTable()` does not correct SQL statement, so you need to explain how to use this method properly. 
- How to transfer data from one database to another? 

## MS Excel converter 

- Add new worksheet using OpenXML. 

## Network communication 

- Potentially, [Enterprise service bus](https://en.wikipedia.org/wiki/Enterprise_service_bus) could be implemented using this library.

![ESB-components](https://upload.wikimedia.org/wikipedia/commons/thumb/1/1d/ESB_Component_Hive.png/330px-ESB_Component_Hive.png)

- Additional fields for internetworking:
    - Fields for authentication and authorization when interacting between microservices (for example, access token).
    - Fields for monitoring (for example, response time, number of requests).

## Models

- In the recipe object, you need to set a status that would show the relevance of the recipe - actually, there is already a status of the entity, which can be equal to "deleted" or "active", but in a real situation it may be important to set deadlines for relevance.
