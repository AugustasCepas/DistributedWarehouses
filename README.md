# DistributedWarehouses
Solution provides API of distributed warehouses.

Main functions:
1) Return list of all SKUs
2) Return info about one SKU
				how many items left in each warehouse
                how many items are reserved
                TODO: how many items are planned to be delivered soon
3) Reserve SKU
                reservation should have an expiration time
                a reservation could be done within a few warehouses, but this should be done only in case no one warehouse has the required amount of SKUs
4) Remove reservation of SKU
                not abstract, but one of the previous
5) SKU is sold
                not only reserved SKUs can be sold
                sold SKUs are removed from a warehouse
                an invoice should be saved
                invoice number is returned
6) Return list of all warehouses
7) Return info of one warehouse
                how many goods are stored
                how many goods are reserved
                how much free space is available
8) Add goods to the warehouse
9) Return list of all invoices
10) Return info about one invoice 
11) Return all goods within an invoice goods can be returned to different warehouses
