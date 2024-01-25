# Open E-commerce

This Project is a simple project that aims to create a basic E-commerce Domain that is structured as a modular monolith, but with focus on concepts like

- Messaging
- System Communication
- Integration Events
- Bounded Contexts
- CQRS (Command Query responsibility segregation)
- Project documentation (more for study and keep a good documentation)
- Diagram architecture (more for study)

## Documentation Index

You can already see some documentation made and in progress by navigating in the docs pages, but for a more structured way here is a index i think is good to go.

1. [Communication Architecture of the project](docs/core-api/communication-architeture.md)

2. User Access Module

   1. [User Access Definition](docs/core-api/modules/user-access/user-access-definition.md)
   2. [User Access Domain Entities](docs/core-api/modules/user-access/user-access-domain-entities.md)
   3. [User Access Use Cases](docs/core-api/modules/user-access/user-access-use-cases.md)

3. Stock Module

   1. [Stock Definition](docs/core-api/modules/stock/stock-definition.md)
   2. [Stock Domain Entities](docs/core-api/modules/stock/stock-domain-entities.md)
   3. [Stock Use Cases](docs/core-api/modules/stock/stock-use-cases.md)

4. Human Resources Module

   1. [Human Resources Definition](docs/core-api/modules/human-resources/human-resources-definition.md)
   2. [Human Resources Domain Entities](docs/core-api/modules/human-resources/human-resources-domain-entities.md)
   3. [Human Resources Use Cases](docs/core-api/modules/human-resources/human-resouces-use-cases.md)

## Technologies to be used

### Backend

1. [.NET 6](#) (LTS)
2. [ASPNET 6](#)

#### Some Libraries

- [MediatR](#)
  Used to implement the mediator pattern and decouple dependencies between modules for handling requests

- [MassTransit](#)
  Used To Implement a communication between modules using messaging, this reduces coupling between modules and helps to migrate a module from the modular monolith to a microservices since all internal system communication gonna happen using a thirty service like RabbitMq for example

- [Fluent Validation](#)
  Used to automatically validate requests till it don't depends of external services or database connection, it helps to create a more secure system and facilitate the development of the system + code complexity since most validations will be handled by the library

- [Entity Framework](#)
  Used to Map Entities to database, query then, manage database connections, and related database communication, it also provides a protection for sql injection and more features expected for a ORM

- [NSubstitute](#)
  Used to Mock Data used in Tests, it make the development of tests more fast since is easy to abstract code dependencies that depends on virtual methods or interfaces.

- [XUnit](#)
  Used to Define Test Cases and test then.

- [Fluent Assertion](#)
  Used to Validate Data using the Fluent Design it is used to validate the tests expected results

- [MockQueryable NSubstitute](#)
  Used to Mock the database Context abstraction of efCore, since it simplifies the process of mocking the database this library specific with EfCore, but it works with other mocking libraries to.
  It Mocks the Database using a InMemory Alternative for tests.

### Frontend

It is expected to have two frontend one for administration supports that is a SPA, since SEO is not required for a internal system and other that supports SSR and SSG because it will be where sales happen and SSO, Scalability and cache is very necessary in those cases.

Since this consideration the technologies proposed are the following

- SPA -> [Angular](#) or [ReactJS](#)
- SSR and SSG -> [NextJS](#)

### Infrastructure

For the time this readme have been wrote, this are the following technologies

- [Docker](#)
  Used to make development dependencies like the Database.

Is good to say that is needed to implement a docker-compose.yml file for dependencies building and as infrastructure documentation, and a DOCKERFILE for building the project and as Infrastructure Documentation to.
