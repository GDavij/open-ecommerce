[Return To Index](../../../../README.md)

# Human Resources Use Cases

This Documentation aims to document all Use Cases for Human Resources Module

**ADVICE: None of Integration Events are implemented for these use cases yet**

## Index of use cases

1. [Direct Endpoint Call](#direct-endpoint-call)  
   1.1 [`Collaborators`](#collaborators)  
    1.1.1 [Create Collaborator](#create-collaborator-with-contract)  
    1.1.2 [Update Collaborator](#update-collaborator)  
    1.1.3 [Delete Collaborator](#delete-collaborator-force-break-contracts)  
    1.1.4 [Get Collaborator](#get-collaborator)  
    1.1.5 [Search Collaborators](#search-collaborators)

   1.2 [`Contracts`](#contracts)  
    1.2.1 [Break Contract](#break-contract)  
    1.2.2 [Add Contracts](#add-contracts)  
    1.2.3 [Delete Contract](#delete-contract)  
    1.2.4 [Search Contracts](#search-contracts)  
    1.2.5 [Get Contract](#get-contract)  
   1.2.6 [Add Work Hour to Contract Contribution Year](#add-work-hour-to-contract-contribution-year)  
   1.2.7 [Remove Work Hour from Contract Contribution Year](#remove-work-hour-from-contract-contribution-year)

   1.3 [`Job Applications`](#job-applications)  
    1.3.1 [Send Job Application](#send-job-application)  
    1.3.2 [Search Job Application](#search-job-applications)  
    1.3.3 [Get Job Application](#get-job-application)  
    1.3.4 [Update Job Application Status](#update-job-application-status)

   1.4 [`States`](#states)  
    1.4.1 [Create State](#create-state)  
    1.4.2 [List States](#list-states)

## Direct Endpoint Call

This are the use cases that is received via direct endpoint call (HTTP/HTTPS - JSON)

### `Collaborators`

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

---

### Update Collaborator

#### Objective

This use Case has the objective to receive new data about the collaborator and if valid updates it.

#### Payload

TODO: Request Response

#### Integration Events

- Updated Collaborator

---

### Delete Collaborator (Force Break Contracts)

#### Objective

This use case has the objective to receive a Collaborator Id and if Valid it should Delete the collaborator

#### Payload

TODO: Create Request Response

#### Integration Events

- Deleted Collaborator

---

### Get Collaborator

#### Objective

This use case has the objective to receive a collaborator id and if exists return collaborator data

#### Payload

TODO: Create Request and Response

#### Integration Events

No Integration Events

---

### Search Collaborators

#### Objective

This use case has the objective to receive a search term and some filters and it must return the founded collaborators for it.

#### Payload

TODO: Create Request and Response

#### Integration Events

No Integration Events

---

### `Contracts`

### Break Contract

#### Objective

This use case has the objective to receive a contract id and if valid it should break the contract

---

### Add Contracts

#### Objective

This use case has the objective to receive contracts and a collaborator Id and if valid it must add the contracts received to the collaborator

---

### Delete Contract

#### Objective

This use case the objective to receive contracts and a collaborator Id and if valid it must delete the contract

---

### Search Contracts

#### Objective

This use case has the objective to receive a search term, a sector to filter contracts, is broken and is deleted filters and then return the contracts that it has found

#### Payload

TODO: Create Request and Response

#### Integration Events

No Integration Events

---

### Get Contract

#### Objective

This use case has the objective to receive a contract id and if valid it must return the found contract

#### Payload

TODO: Create Request and Response

#### Integration Events

No Integration Events

---

### Add Work Hour to contract contribution year

#### Objective

This use case has the objective to receive a contract id and work hour data (day, start, end) and if valid it must append this new work hour registration to the collaboration year of a contract

#### Payload

TODO: Create Request and Response

#### Integration Events

No Integration Events

---

### Remove Work Hour from contract Contribution Year

#### Objective

This use case has the objective that if a mistake occurs a HR collaborator or Admin can sent the Id of the work hour where is wrong and if valid(Exists) it must remove from the Contract Contribution Years

#### Payload

TODO: Create Request and Response

#### Integration Events

No Integration Events

---

### `Job Applications`

### Send Job application

#### Objective

This use case has the objective to receive a job application add it to the specified job topic and save in the database

#### Payload

TODO: Create Request and Response

#### Integration Events

No Integration Events

---

### Search Job Applications

#### Objective

This use case has objective to search and list job applications

#### Payload

TODO: Create Request and Response

#### Integration Events

No Integrations Events

---

### Get Job Application

#### Objective

This use case has the object to receive a job application id and if valid it must return the job application

#### Payload

TODO: Request and Response

#### Integration Events

No Integration Events

---

### Update Job Application Status

#### Objective

This use case has the objective to receive a job application id and a status of the process step, and if valid it must update the process step to the new one sent

#### Payload

TODO: Request and Response

#### Integration Events

No Integration Events

---

### `States`

### Create State

#### Objective

This use case has the objective to create a State for later be attached to an Address

#### Payload

TODO: Request and Response

#### Integration Events

No Integration Events

---

### List States

#### Objective

This use case has the objective to list all states in the module

#### Payload

TODO: Request Response

#### Integration Events

No Integration Events

---

## Messaging Calls

TODO: Define Messaging Integration Events(Subjects) if any
