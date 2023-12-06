# User Access Use Cases

This Documentation aims to document all Use Cases for User Access Module

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

- **_Create Client Session Domain Event_**

Represents a Client session that has been created

- **Consumers**

- AuditModule
  > Probably replace this with a diagram showing domain events coupling (connections)

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

- **_Create Collaborator Session Domain Event_**

Represents a Collaborator session that has been created

## Messaging Calls

### Authenticate Client Session

#### Domain Events

### Authenticate Collaborator Session For Module XYZ..

#### Domain Events
