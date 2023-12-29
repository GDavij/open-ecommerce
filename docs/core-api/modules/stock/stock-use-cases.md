[Return To Index](../../../../README.md)

# Stock Use Cases

This Documentation aims to document all Use Cases for Stock Module

**ADVICE: None of Integration Events are implemented for these use cases yet**

## INDEX of use cases

TODO

## Direct Endpoint Call

This are the use cases that is received via direct endpoint call (HTTP/HTTPS - JSON)

### Create Product

#### Objective

This use case has the objective to receive data about the product entity and if valid, will create a product into the system

This includes the Product Details and tags, those being Lists, but not images(specific endpoint for blobs).

TODO: Should Validate Name Equality and Throw exception

#### Payloads

**Ideia of Request Payload** - JSON With Comments

```JSON with comments
{
    "brandId": "Guid id of the brand",
    "name": "Name of the product to create",
    "description": "Description of the product to create or null",
    "sku": "Sku or null",
    "ean": "ean",
    "upc": "upc or null",
    "price": 123456.99,
    "stockUnitCount": 2,
    "tagsIds": [
        "Tag1 id",
        "tag 2 id"
    ],
    "measurements": [
        {
            "name": "measurement name(description)",
            "value": "value of the measurement",
            "showOrder": 1,// Represents the order to show when in the Ui(for sort reasons)
            "MeasureUnitId": "Measure Unit Id or null"
        }
    ],
    "technicalDetails": [
        {
            "name": "technical detail name(description)",
            "value": "value of the technical detail",
            "showOrder": 1,// Represents the order to show when in the Ui(for sort reasons)
            "MeasureUnitId": "Measure Unit Id or null"
        }
    ]
    "otherDetails": [
        {
            "name": "other detail name(description)",
            "value": "value of the other detail",
            "showOrder": 1,// Represents the order to show when in the Ui(for sort reasons)
            "MeasureUnitId": "Measure Unit Id or null"
        }
    ]
}
```

**Ideia of Response Payload** - Json With Comments

```JSON with Comments
{
    "Resource": "Path for the frontend to navigate(redirect, etc...) and see the product"// probably like "https:// {domain}/{baseUrl}/products/{productId}.{topLevelDomain}"
}
```

#### Test Cases

[X] - Should create Product with valid information.  
[X] - Should not create Product with not existent brand.  
[] - Should not create product with same Name of a existent product. -> TODO: Test is now in business rule
[X] - Should not create Product with same EAN of a existent Product.  
[X] - Should not create Product with same UPC of a existent Product.  
[X] - Should not create Product with same SKU of a existent Product.  
[X] - Should not create Product with a any Tag that does not exist in the system.  
[X] - Should not create Product with any ProductDetails(Measure, Technical, Other) that has a not existent measure.  
[X] - Should not create product with any ProductDetails(Measure, Technical, Other) with repeated show order in product detail lists

##### Command Validation Test Cases

[x] - Should Accept valid command.  
[X] - Should Negate invalid command with null values (consider Lists as null)  
[X] - Should Negate invalid command with null values (consider inner List values as null)  
[X] - Should Negate invalid command with empty values  
[X] - Should Negate invalid command with less than min values  
[X] - Should Negate invalid command with more than max values  
[X] - Should Accept Command with Empty Lists for ProductDetails and Tags

#### Integration Events

[X] - Product Created Integration Event(Without Retry with Polly) -> (Important to Sales Module)

### Add Image to a Product

#### Objective

This use case has the objective to receive a `multipart-formdata` with a blob of a image and a product id, and if everything valid it will create the image in a cloud storage and link it to the product.

#### Payloads

**Idea of Request** - multipart-form-data(represented in docs as JSON)

```JSON
{
    "productId": "ProductId",//Comes from route parameters
    "description": "Image Description",
    "imageFile": BLOB
}
```

**Idea of Response** - JSON

```JSON
{
    "Resource": "Path for the frontend to navigate(redirect, etc...) and see the product"/* probably like "https:// {domain}/{baseUrl}/products/{productId}.{topLevelDomain}"*/
}
```

#### Test Cases

[X] - Should Add a Image into a already existent product  
[X] - Should Not Add a Image into a not existent product

##### Command Validator Test Cases

[X] - Should Accept Valid Command  
[X] - Should Negate Command with empty values  
[X] - Should Negate Command with values lower than minimal limit  
[X] - Should Negate Command with values higher than max limit  
[X] - Should Negate Command With invalid MIMETYPE  

#### Integration Events

[X] - Add Image To Product Integration Event(Without Retry with Polly) -> (Important to Sales, since it add the image to then to)

### Remove Image from product

#### Objective

This use case has the objective to receive a Id of a image and if valid(exists, is appended to a product) it will remove the link to that image from the product and notify sales module that this image need to be removed from storage(Azure blob storage) and sales Product entity.

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
    "success": true // shows true if remove link from product and notify module otherwise false.
}
```

#### Test Cases

[X] - Should Remove Image reference and notify other modules successfully  
[X] - Should Not Remove Image when image is not existent

##### Command Validator Test Cases

[X] - Should Accept Valid Command  
[X] - Should Negate Invalid Command with empty values

#### Integration Events

[X] - Removed Image From Product Integration Event(Without Retry with Polly) -> (Important to Sales, since it will delete the image globally and remove it's reference to their module)

### Create Brand

#### Objective

This use case has the objective to receive data about a brand and if valid, will create the brand into the system

#### Payloads

**Ideia of Request Payload** - JSON

```JSON
{
    "name": "brand-name",
    "description": "brand-description"// Or null
}
```

**Ideia of Response Payload** - JSON with comments

```JSON with comments
{
    "Resource": "Path for the frontend to navigate(redirect, etc...) and see the brand"/* probably like "https:// {domain}/{baseUrl}/brands/{productId}.{topLevelDomain}"*/
}
```

#### Test Cases

[X] - Should Create a Brand for valid command  
[X] - Should not Create Brand for already existent brand(brand that has same name)  

##### Command Validator Test Cases

[X] - Should Accept Valid Command  
[X] - Should Negate Command with Empty Values  
[X] - Should Negate Command with values higher than maximum limit

#### Integration Events

[X] - Brand Created Integration Event(Without Retry with Polly) -> (Important to Sales Module)

### Create Measure Unit

#### Objective

This use case has the objective to receive data about a measure and if valid it will add it globally to use in the system

#### Payloads

TODO: Request and Response

#### Test Cases

[X] - Should Create Measure Unit For Valid Command
[X] - Should Not Create Measure Unit For Invalid Command With Same Name Or Shortname As Existent Measure Unit

##### Command Validator Test Cases

[X] - Should Accept Valid Command
[X] - Should Negate Invalid Command With Empty Values
[X] - Should Negate Invalid Command with Values Higher Than Maximum Limit

#### Integration Events

[X] - Measure Unit Created(Without Retry With Polly)

### Update Product

#### Objective

This use case has the objective to receive a product id, and their updated data, it will validate, and if valid it will update the product data into the system.

#### Payloads

TODO: Request and Response

#### Test Cases

[X] - Should Update Product for Valid Command with Different Values  
[X] - Should Update Product for valid command with same Values as before Update  
[X] - Should Not Update Product For Invalid Command with not existent Product Id  
[X] - Should Not Update Product For Invalid Command with not existent Brand Id  
[X] - Should Not Update Product For Invalid Command with Name same as existent product name that is not the one to update  
[X] - Should Not Update Product For Invalid Command with Ean same as existent product EAN that is not the one to update  
[X] - Should Not Update Product For Invalid Command with UPC same as existent product UPC that is not the one to update  
[X] - Should Not Update Product For Invalid Command with SKU same as existent product SKU that is not the one to update  
[X] - Should Not Update Product For Invalid Command with Repeated Order on Any Product Details(Measurements, Technical Details, Other Details)  
[X] - Should Not Update Product For Invalid Command with Any Invalid MeasureUnit in Any Product Details (Measurements, Technical Details, Other Details)  


##### Command Validator Test Cases

TODO

#### Integration Events

- Product Updated Integration Event -> (Important to Sales Module)

### Delete Product

#### Objective

This use case has the objective to receive a product id, and if valid it will notify Sales module to delete this product from their database and delete all related data(images, etc...)

#### Payloads

TODO: Requests and Responses

#### Test Cases

TODO

##### Command Validator Test Cases

TODO

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
