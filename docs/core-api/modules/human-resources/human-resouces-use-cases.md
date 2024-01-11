[Return To Index](../../../../README.md)

# Human Resources Use Cases

This Documentation aims to document all Use Cases for Human Resources Module

**ADVICE: None of Integration Events are implemented for these use cases yet**

## INDEX of use cases

TODO

## Direct Endpoint Call

This are the use cases that is received via direct endpoint call (HTTP/HTTPS - JSON)

### Create Collaborator (with contract)

#### Objective

This use case has the objective to receive data needed for create a collaborator, this includes their `module` or `administrator`` being then   
- Stock  
- RH  
- Finance  
and if the data is valid it must create a collaborator

#### Payload

TODO: Create Request Response

#### Integration Events

One of these
- Administrator Created
- Stock Collaborator Created
- Human Resources Collaborator Created
- Finance Collaborator Created

### Update Collaborator

#### Objective 

This use Case has the objective to receive new data about the collaborator and if valid updates it.

#### Payload

TODO: Request Response

#### Integration Events

One of these
- Administrator Updated
- Stock Collaborator Updated
- Human Resources Collaborator Updated
- Finance Collaborator Updated

## Messaging Calls

TODO: Define Messaging Integration Events(Subjects)

### Contract Renewal

#### Objective

This use case has the objective to receive a data about the contract and if valid it gonna update the contract for a specific collaborator.

#### Payload

TODO: Create Request Response

#### Integration Events

No Integration Events

### Delete Collaborator (Force Break Contract)

#### Objective

This use case has the objective to receive a Collaborator Id and if Valid it should Delete the collaborator

#### Payload

TODO: Create Request Response

#### Integration Events

- Administrator Deleted
- Stock Collaborator Deleted
- Human Resources Collaborator Deleted
- Finance Collaborator Deleted

### Send Job application

#### Objective

This use case has the objective to receive a job application add it to the specified job topic and save in the database

#### Payload

TODO: Create Request and Response

#### Integration Events

No Integration Events

### Search Job Applications 

#### Objective 

This use case has objective to search and list job applications

#### Payload

TODO: Create Request and Response

#### Integration Events

No Integrations Events