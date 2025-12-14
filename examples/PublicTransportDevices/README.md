# PublicTransportDevices

[English](README.md) | [Русский](README.ru.md)

This project involves designing the architecture for a real-time analytics system to handle a high volume of device data, and implementing a prototype server application in C# for data collection and processing.

## Technical requirements

1. Develop the architecture of a real-time analytics system.
Input data:
     - Data is collected from 20 thousand devices every second;
     - Type of data collected: operation parameters of sensors and devices, geolocation;
     - The data is used for subsequent display on dashboards.

Propose a service architecture: what components should it consist of?
Why are certain solutions chosen?
What technologies are used and why?

2. Implement a prototype of the analytics system in C#.
The prototype is a server application.
This application is required primarily to collect data from devices installed on vehicles.

The server receives requests from the client and processes them:
- Registration of the device with entering it into the database;
- Registration of a new event for a specific device with its entry into the database;
- Processing the event list request.

An event can be considered a JSON received by a service that contains key-value pairs, where the value is the data types `int`, `float` and `str`.
How communication between devices and the event collection system will be implemented, as well as whether you will implement device authorization, a queue for storing the event stream, or otherwise select events from this device is also your choice.

## Architecture 

The current implementation of the application uses the following technologies:

- WebAPI;
- Database: PostgreSQL;
- Message Broker: RabbitMQ.

![MessageQueueArchitecture](../../docs/img/examples/PublicTransportDevices/MessageQueueArchitecture.png)

An explanation of why this type of architecture was chosen, and what alternatives to this approach can be, can be found at [this link](docs/architecture.md).

## Implementation  

There are two controllers, each with GET and POST methods:
- The GET method returns an object of the `System.Data.DataTable` type (made on the assumption that the client application can also display data in a tabular form);
- The POST method does not return any parameter.

### How to run 

1. Database configuring

PostgreSQL is used as the database.

You can specify ConnectionString in `appsettings.json`:

```JSON
"AppSettings": {
    "PostgresConnectionString": "Server=127.0.0.1;Port=5432;Userid=postgres;Password=postgres;Database=postgres"
}
```

The `init` folder contains files with scripts for setting up the database (`initpg.sql` - for initializing tables).

2. Setting up a message broker

RabbitMQ is used as a message broker:

- Exchange name: `ptd_exchange`;
- Queue name: `ptd_queue`.

### Registering the device with entering it into the database

Device registration in C#:

```C#
var httpClient = new HttpClient(); 
var url = "https://localhost:7010/RegisterDevice"; 
var device = new Device 
    {
        Uid = "3eb20d9f-3350-4bd3-b343-d903d2e51cfb", 
        DeviceType = DeviceType.DriverConsole
    };
await httpClient.PostAsJsonAsync(url, device); 
```

Registering a device using JSON (Request URL: `https://localhost:7010/RegisterDevice`):

```JSON
{
  "uid": "3eb20d9f-3350-4bd3-b343-d903d2e51cfb",
  "deviceType": 2
}
```

### Registering a new event for a specific device and entering it into the database

[This link](insertptdi.md) provides information on how a new event is registered for a specific device and entered into the database.
