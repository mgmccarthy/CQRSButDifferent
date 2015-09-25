A sample solution inspired by Udi Dahan's presentation "CRQS-But Different" at the NDC conference in Oslo, Norway: https://vimeo.com/131199089

How to run the project:
	- you need a Sql Server LocalDb instance
	- press enter in the ResupplyVendorConsoleClient. This inserts a quantity of 100 for product 1 into the ProductQuantity entity.
	- press enter in the ConsoleClient to start placing orders for product 1. This will send a PlaceOrder command every second with a random quantity between 1-6 for for product 1
	- when there is 10 left of product 1 in stock, the OrderEndpoint will start logging WARN messages to the screen to resupply the vendor. 
	- To resupply the vendor, hit enter again in the ResupplyVendorConsoleClient. This will put 100 more quantity of product 1 in supply.
	- if you're not quick enough, you'll run out of stock, and the OrderEndpoint will log ERROR messages saying that you need to resupply the vendor.

- the ProductQuantity entity is a running delta of resupplies and orders. 
- when an order is placed, the quantity on the order is turned into a negative number and inserted into this entity. 
- when the vendor is resupplied, a positive number (in this case, a hard-coded quantity of 100) is inserted into the databse
- PlaceOrderHandler sums the delta off of Product Quantity to get the available quantity of product 1. 
	- Based off the sum operation, 
		- the NSB console shows a WARN message if there are less than 10 product 1's available.
		- if we're out of product 1
			- the NSB console shows the WARN message and publishes InsuffcientProductQuantityForOrder
			ELSE
			- writes the negated order's quantity to ProductQuantity