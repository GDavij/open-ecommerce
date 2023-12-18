[Return To Index](../../README.md)

# Module Communication

This Section gonna be separated by two aspects `Direct Endpoint Calls` and `Messaging Calls`.

The modules will have two types of communication (APIs), one being by HTTP Protocol which request will be received explicitly by API Project.

### Endpoint Calls

All HTTP requests(or commands) will be received and forwarded to it's respective modules command handlers, this forwarding will be made by the external library [MediatR](https://github.com/jbogard/MediatR) that acts as a mediator for the `commands`.

### Messaging Calls

In the other side, all communication between modules will be made using messaging, the objective is to allow messaging using [RabbitMq](https://www.rabbitmq.com), even if the project is too simple to need to use it, it will allow a more easy migration to microservices besides it's greater complexity.

To start basic first will be implemented `In Memory Messaging` with [MassTransit Library](https://masstransit.io), later on the project changes gonna be made to allow [RabbitMq](https://www.rabbitmq.com).

Just to clarify more the messaging communication will be of two types

- Pub/Sub
- Request/Response

the first one will be implemented for notify about events in the system for modules like `Audit Module`(TODO Add Link for audit service).

for example when a client or collaborator login into the system via [User Access Module](./modules/user-access/user-access-definition.md) the authentication service gonna publish a message (notification) for everyone that need the `ClientLoginNotification` or `CollaboratorLoginNotification` for example.

This in the context of the [Audit Module](./modules/audit/audit-definition.md) is useful for tracking access to site and detecting anomalies, since everyone that create a session on the site will have their login sessions stored for analysis.

### Example of Commmunication

![Example Communication](./build-assets/example-system-communication.drawio.svg)
