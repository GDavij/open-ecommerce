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

This use case has the objective to receive data needed for create a collaborator, this includes their `module` being those

- Stock
- RH
- Finance

and if the data is valid it must create a collaborator

#### Payload

TODO: Create Request Response

#### Integration Events

- Created Collaborator

### Update Collaborator

#### Objective

This use Case has the objective to receive new data about the collaborator and if valid updates it.

#### Payload

TODO: Request Response

#### Integration Events

- Updated Collaborator

### Delete Collaborator (Force Break Contracts)

#### Objective

This use case has the objective to receive a Collaborator Id and if Valid it should Delete the collaborator

#### Payload

TODO: Create Request Response

#### Integration Events

- Deleted Collaborator

### Break Contract

#### Objective

This use case has the objective to receive a contract id and if valid it should break the contract

### Add Contracts

#### Objective

This use case has the objective to receive contracts and a collaborator Id and if valid it must add the contracts received to the collaborator

### Delete Contract

#### Objective

This use case the objective to receive contracts and a collaborator Id and if valid it must delete the contract

### Get Collaborator

#### Objective

This use case has the objective to receive a collaborator id and if exists return collaborator data

#### Payload

TODO: Create Request and Response

#### Integration Events

No Integration Events

### Search Collaborators (Don't Search Administrators, and don't search human resources collaborators if not admin)

#### Objective

This use case has the objective to receive a search term and some filters and it must return the founded collaborators for it.

#### Payload

TODO: Create Request and Response

#### Integration Events

No Integration Events

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

## Messaging Calls

TODO: Define Messaging Integration Events(Subjects)
