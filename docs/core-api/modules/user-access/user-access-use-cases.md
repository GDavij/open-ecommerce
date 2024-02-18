[Return To Index](../../../../README.md)

# User Access Use Cases

This Documentation aims to document all Use Cases for User Access Module

# Index of use cases

1. [Direct Endpoint Call](#direct-endpoint-call)  
   1.1 [`Clients`](#clients)  
    1.1.1 [Create Client Session](#create-client-session)

   1.2 [`Collaborators`](#collaborators)  
    1.2.1 [Create Collaborator Session](#create-collaborator-session)

   1.3 [`Administrators`](#administrators)  
    1.3.1 [Create Administrator]

2. [Messaging Calls](#messaging-calls)  
   2.1 [`Auth`](#auth)  
   2.1.1 [Authenticate Client](#authenticate-client)  
    2.1.2 [Authenticate Collaborator For Sector](#authenticate-collaborator-for-sector)

   2.2 [`Clients`](#clients)  
    2.2.1 [Create Client](#create-client-deprecated---gonna-change)

   2.3 [`Administrators`](#administrators-1)  
    2.3.1 [Get Administrators Ids](#get-administrator-ids-maybe-gonna-to-be-deprecated-in-future)

   2.4 [`Collaborators`](#collaborators-1)  
    2.4.1 [Get Collaborator Is Admin](#get-collaborator-is-admin)  
    2.4.2 [Get Collaborator Is Deleted](#get-collaborator-is-deleted)  
    2.4.3 [Get Collaborators Ids From Sector](#get-collaborator-ids-from-sector-query-handler)  
    2.4.4 [Get Deleted Collaborator Ids](#get-deleted-collaborators-ids)

## Direct Endpoint Call

This are the use cases that is received via direct endpoint call (HTTP/HTTPS - JSON)

### `Clients`

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
    "token": "JWT Token"//It Is a Encoded JWT Token
}
```

#### Integration Events

- Create Client Session Integration Event

---

### `Collaborators`

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

#### Integration Events

- Create Collaborator Session Integration Event

---

### `Administrators`

### Create Administrator

#### Objective

This use case has the objective to create a administrator if none exists or if other administrator is the one making the request.

#### Payload

TODO: Request and Response

---

## Messaging Calls

### `Auth`

### Authenticate Client

#### Objective

This use case has the objective to receive the client Token and return a validation result representing if a user is authenticated or not

#### Payload

TODO: Create Request and Response

---

### Authenticate Collaborator For Sector

#### Objective

This use case has the objective to receive a collaborator Token and the sector to validate auth.

it must return a Authentication Result representing if the user is authenticated or not

#### Payload

TODO: Create Payload

---

### `Clients`

### Create Client (DEPRECATED) -> GONNA CHANGE

#### Objective

This use case has the objective to receive about a recently created client and must create the client into the Database(of the User Access Module)

#### Payload

TODO: DEPRECATED, GONNA NOT DO IT

---

### `Administrators`

### Get Administrator Ids (Maybe gonna to be deprecated in future)

#### Objective

This use case has the objective to return all ids from the Administrators of the system stored in the Database

#### Payload

TODO: Create Request and Response

---

### `Collaborators`

### Get Collaborator Is Admin

#### Objective

This use case has the objective to receive a collaborator Id and return a Evaluation Result that Wrap a boolean value representing if the Collaborator is or not a Admin

#### Payload

TODO: Create Request and Response

---

### Get Collaborator Is Deleted

#### Objective

This use case has the objective to receive a collaborator Id and return a Evaluation Result that wraps a boolean value that represents if the collaborator is Deleted or not

#### Payload

TODO: Request and Response

---

### Get Collaborator Ids from Sector

#### Objective

This use case has the objective to receive a Sector and it must return a Evaluation Result containing a HashSet with all Collaborators Ids from the Sector received as a Parameter

#### Payload

TODO: Request and Response

---

### Get Deleted Collaborators Ids

#### Objective

This use case has the objective to return a Evaluation Result containing a HashSet with all Collaborators Ids that are Deleted

#### Payload

TODO: Request and Response

---
