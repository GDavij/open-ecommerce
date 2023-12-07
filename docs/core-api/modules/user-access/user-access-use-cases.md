# User Access Use Cases

This Documentation aims to document all Use Cases for User Access Module

**ADVICE: None of Domain Events are implemented for these use cases yet**

## Direct Endpoint Call

This are the use cases that is received via direct endpoint call (HTTP/HTTPS - JSON)

### Create Client Session

#### Objective

This use case has the objective to Receive a Client `Email` and `Password` process data, validate and if valid return a Client `Session Token` that is used to authenticate the Client Again

#### Payloads

**Ideia of Request Payload** - JSON

```JSON
{
    "email": "docs.email@email.com",
    "password": "my-secret-password"
}
```

**Ideia of Response Payload** - JSON with Comments

```JSON with Comments
{
    "token": "JWT Token"// It Is a Encoded JWT Token
}
```

#### Domain Events

- Create Client Session Domain Event

### Create Collaborator Session

#### Objective

This use case has the objective to receive a collaborator `E-mail` and `Password` process data, validate and if valid return a Collaborator `Session Token`, that is used to authenticate the collaborator again

#### Payloads

**Ideia of Request Payload** - JSON

```JSON
{
    "email": "docs.email@email.com",
    "password": "my-secret-password"
}
```

**Ideia of Response Payload** - JSON with Comments

```JSON with Comments
{
    "token": "JWT Token"// It Is a Encoded JWT Token
}
```

#### Domain Events

- Create Collaborator Session Domain Event

## Messaging Calls

### Authenticate Client Session

#### Objective

The Objective is to receive a token, validate and return a authentication result, if authenticated, return an identity that is simple a Record with an `ModuleId` Attribute

#### Payloads

**Ideia of Request Payload** - JSON

```JSON With Comments
{
    "EncodedToken": "jwtToken",
}
```

**Ideia of Response Payload** - JSON with Comments

```JSON With Comments
{
    "IsAuthenticated": true, // or false
    "Identity": {
        "Id": "moduleId"
}
```

#### Domain Events

- Client Authenticated Domain Event

### Authenticate Collaborator Session For Module X

#### Objective

The Objective is to receive a token and the module to authenticate to. then validate and return a authentication result, if authenticated, return an identity that is simple a Record with an `ModuleId` Attribute

#### Payloads

**Ideia of Request Payload** - JSON

```JSON With Comments
{
    "EncodedToken": "jwtToken",
    "Sector": 0 // This is a ENUM
}
```

**Ideia of Response Payload** - JSON with Comments

```JSON With Comments
{
    "IsAuthenticated": true, // or false
    "Identity": {
        "Id": "moduleId"
}
```

#### Domain Events

- Collaborator Authenticated Domain Event

### Create Client

#### Objective

The Objective for this module is to receive minimal information about the client send by other module(probably sales module), this use case has the responsibility to create a Client entity in the `User Access Context` and store it in the database.

#### Future Implementations

Since This Use Case is an Only **Pub Consumer** and don't implement the `Request/Response` Pattern it will be needed to implement `Retry Policies` to mitigate data loss and rely on **Eventual Consistency**

#### Payloads

**Ideia of Request Payload** - JSON

```JSON With Comments
{
    "ClientModuleId": "guid-from-probably-sales-module",
    "Email": "client-email",
    "Password": "client-password"
}
```

**This module will have no response(Pub Consumer Only)**

#### Domain Events

**No Domain Events till the moment**

### Create Collaborator

#### Objective

The Objective for this module is to receive minimal information about a collaborator send by other module(probably Human Resources module), this use case has the responsibility to create a Collaborator entity in the `User Access Context` and store it in the database.

#### Future Implementations

Since This Use Case is an Only **Pub Consumer** and don't implement the `Request/Response` Pattern it will be needed to implement `Retry Policies` to mitigate data loss and rely on **Eventual Consistency**

#### Payloads

**Ideia of Request Payload** - JSON

```JSON With Comments
{
    "CollaboratorModuleId": "guid-from-probably-human-resources-module",
    "CollaboratorSector": 1,// This is a ENUM
    "Email": "client-email",
    "Password": "client-password",
}
```

**This module will have no response(Pub Consumer Only)**

#### Domain Events

**No Domain Events till the moment**
