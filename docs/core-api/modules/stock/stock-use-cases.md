[Return To Index](../../../../README.md)

# Stock Use Cases

This Documentation aims to document all Use Cases for Stock Module

**ADVICE: None of Integration Events are implemented for these use cases yet**

## Direct Endpoint Call

This are the use cases that is received via direct endpoint call (HTTP/HTTPS - JSON)

### Create Product

#### Objective

This use case has the objective to receive data about the product entity and if valid, will create a product into the system

This includes the Product Details and tags, those being Lists, but not images(specific endpoint for blobs).

#### Payloads

TODO: Request and Response.

#### Test Cases

[X] - Should create Product with valid information.  
[X] - Should not create Product with not existent brand.  
[X] - Should not create Product with same EAN of a existent Product.  
[X] - Should not create Product with same UPC of a existent Product.  
[X] - Should not create Product with same SKU of a existent Product.  
[X] - Should not create Product with a any Tag that does not exist in the system.  
[X] - Should not create Product with any ProductDetails(Measure, Technical, Other) that has a not existent measure.

#### Integration Events

[] - Product Created Integration Event -> (Important to Sales Module)

### Add Image to a Product

#### Objective

TODO: Confirm the correct data type used!!!!

This use case has the objective to receive a `multipart-formdata` with a blob of a image and a product id, and if everything valid it will create the image in a cloud storage and link it to the product.

#### Payloads

TODO: Request Response

#### Integration Events

- Add Image To Product Integration Event -> (Important to Sales, since it add the image to then to)

### Remove Image of a product

#### Objective

This use case has the objective to receive a Id of a image and if valid(exists, is appended to a product) it will remove the link to that image from the product and notify sales module that this image need to be removed from storage and sales product entity.

### Payloads

**Ideia of Request Payload** - JSON

```JSON
{
    "id": "image-id"
}
```

**Ideia of Response Payload** - JSON with comments

```JSON with comments
{
    "success": true // shows  true if remove link from product and notify module otherwise false.
}
```

#### Integration Events

- Remove Image From Product Integration Event -> (Important to Sales, since it will delete the image globally and remove it's reference to their module)

### Create Brand

#### Objective

This use case has the objective to receive data about a brand and if valid, will create the brand into the system

#### Payloads

Todo: Request and Response

#### Integration Events

- Brand Created Integration Event -> (Important to Sales Module)

### Create Supplier

#### Objective

This use case has the objective to receive data about a supplier and if valid it will create a supplier into the system

#### Payloads

TODO: Request and Response

#### Integration Events

- Supplier Created Integration Events -> (Important to Finance Module)

### Create Measure Unit

#### Objective

This use case has the objective to receive data about a measure and if valid it will add it globally to use in the system

#### Payloads

TODO: Request and Response

#### Integration Events

- Measure Unit Created

### Update Product

#### Objective

This use case has the objective to receive a product id, and their updated data, it will validate, and if valid it will update the product data into the system.

#### Payloads

TODO: Request and Response

#### Integration Events

- Product Updated Integration Event -> (Important to Sales Module)

### Delete Product

#### Objective

This use case has the objective to receive a product id, and if valid it will notify Sales module to delete this product from their database and delete all related data(images, etc...)

#### Payloads

TODO: Requests and Responses

#### Integration Events

- Delete Product Integration Event -> (Important to Sales module, notify about the delete procedure to execute(remove from sales module))

### Update Brand

#### Objective

The objective of this use case is to receive a brand id and the updated brand data, validate and if valid update the brand in the system.

#### Payloads

TODO: Requests and Responses

#### Integration Events

- Update Brand Integration Event -> (Important to Sales Module)

### Delete Brand

#### Objective

The objective of this use case is to receive a brand id and if valid, it will delete the brand from the system and notify other modules that "share" the entity to delete to.

#### Payloads

TODO: Request and Response

#### Integration Events

- Deleted Brand Integration Event

### Update Supplier

#### Objective

The objective of this use case is to receive a Supplier Id and it's updated data, validate and if valid it will update the supplier and notify other modules that "Share" this entity to update in their modules too.

#### Payloads

TODO: Request and Response

#### Integration Events

- Supplier Updated

### Delete Supplier

#### Objective

The objective of this use case is to receive a Supplier Id and if valid, it will delete the supplier and related data from the module and notify the other modules to delete it and remove all related data.

#### Payloads

TODO: Request and Response

#### Integration Events

- Delete Supplier

### Update Measure Unit

#### Objective

The objective of this use case is to receive a measure unit Id and it's updated data, validate and if valid it will update it's data in the database and notify other modules to update the too.

#### Payloads

TODO: Request and Response

#### Integration Events

- Measure Unit Updated

### Delete Measure Unit

#### Objective

The objective of this use case is to receive a measure unit Id and if valid it will remove the uses of the measure, delete the measure from the database, and notify other modules to remove and delete the measure.

#### Payloads

TODO: Request and Response

#### Integration Events

- Measure Unit Deleted

### Create Restock Order

#### Objective

This use case has the objective to receive data about a product that need to be restocked validate, and if valid, create a request that will needed to be validate by the buying's collaborator in
No Integration Events for this use case, no "shared" entities between contexts
the sales module.

#### Payloads

TODO: Request and Response

#### Integration Events

- RestockOrderCreated

### Delete Restock Order

#### Objective

This use case has the objective to receive a restock order Id, and if valid it will delete the Restock Order, and notify Finance module to delete the restock order on it's context.

#### Payloads

TODO: Request and Response

#### Integration Events

- RestockOrderDeleted

### Send Demand Message to Restock Order

#### Objective

This use case has the objective to receive a restock order id and a message data, and if valid will create a message attached to this use case.

This is used for internal communication to respond to possible doubts between module agents(collaborators).

#### Payloads

TODO: Request and Response

#### Integration Events

- Demand Message For Restock Order Sent

### Delete Demand Message from Restock Order

#### Objective

This use case has the objective to receive a restock order Id, and a message Id, and if valid it will delete the message from the database and will notify other modules to delete the message to.

#### Payloads

TODO: Request and Response

#### Integration Events

- Demand Message For Restock Order Deleted

## Messaging Calls

TODO: Define Messaging Integration Events(Subjects)

### Stock Collaborator Created (RH)

### Stock Collaborator Updated (RH)

### Stock Collaborator Deleted (RH)

### Create Supplier (Finance) (React event endpoint)

### Update Supplier (Finance) (React event endpoint)

### Delete Supplier (Finance) (React event endpoint)

### Denied Restock Order (Finance) (React Event Endpoint)

### Resolved Restock Order (Finance) (React Event Endpoint)
