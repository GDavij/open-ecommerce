# Stock Module

The Stock Module is important since it is responsible for expose and control the number of units of products that gonna apear in the online store website.

This Module will to be responsible for making restock requests that will be needed to be authorized by the Buying Manager from the Finances Module.

This module will be responsible for managing Brands that this E-commerce will offer and to see and manage suppliers that has some product.

The control of sales will to be saved on this database module.

## If error

If this module goes down, the products will not be restocked since no request can be created, the management of brands, suppliers and products won't be able to occur to.

Still the sales of products, control of unit would still work normally, but the number of products on the stock can be distanced between one module and another, since this is important to implement a retry police on endpoints that uses messaging to manage to have a eventual consistency when the module return to work normally and healthy.

## Future mitigation features

TODO: Need to write some mitigation features.

## Module Use Cases (Generic Description)

TODO
