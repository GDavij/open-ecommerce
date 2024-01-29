[Return To Index](../../../../README.md)

#### Comments

Should transform the definitions into RFCs(Rule Formalization Checklist) in future

# Human Resources Module

The Human Resources Module is considerable important especially for the bootstrap of the system, since it generates the administrator of the system, and manages all collaborators.

The human resources will to be able to receive applications for jobs analyze then and add to the job applications database

## If error

If a downtime occur in this module, The creation and managing of Collaborators, Contracts and the reception of applications for job would not work, but the rest of the system would work with good integrity

## Future mitigation features

- Work with us endpoint

This endpoint can have malicious file injections by crackers

TODO: Need to write more mitigation features because of module importance

## Module Use Cases (Generic Description)

TODO

For more details about the use cases [see this Document](./human-resouces-use-cases.md)

## Definition of Business Rules

- A Administrator or Human Resources Collaborator Must be able to create, manage, and break contracts

- A Administrator or Human Resources Collaborator Must be able to Receive and manage Job Applications

- A Administrator or Human Resources Collaborator Must be able to control work hours of other collaborators(if admin, anyone, if HR collaborator only other sectors that not is HR)

- Administrators or Human Resources Collaborators must be able to Add Collaborators and manage their data that the business need

- Administrator and Human Resources must be able to Add, Manage and Delete States into the system like (Illinois, New York or São Paulo)

## Definition of Application Rules

- Human Resources Collaborators must have a Stratified control of access in Human Resources Module like position -> (Future Improvement)

- Only Administrators can Modify or Delete Human Resources Collaborators(Collaborators that does have a contract for the Human Resources Sector)

- One Collaborator Cannot have more than one active contract(not broken nor deleted) for a same sector at the same time

- Every Collaborator must have a valid Email Address that is Unique for the collaborator

- All DateTime Data that a contract have must be valid, it is not out of the time Scope

- Every Collaborator must have a valid Phone Number

- A Address must have a valid State

- Human Resources Collaborators can Update their own Data

- To Create a Human Resources Collaborator the collaborator must have at least one contract attached to it
