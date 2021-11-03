# DistributedWarehouses
You need to create an Asp.net core web app that provides API of distributed warehouses.

Main functions:
1) Return list of all SKUs (GET /v1​/items)
2) Return info about one SKU (GET /v1​/items/{sku})
				how many items left in each warehouse
                how many items are reserved
                TODO: how many items are planned to be delivered soon
3) Reserve SKU (POST /v1​/reservations)
                reservation should have an expiration time
                a reservation could be done within a few warehouses, but this should be done only in case no one warehouse has the required amount of SKUs
4) Remove reservation of SKU (DELETE /v1​/reservations/{reservationId}/items/{itemSku}/warehouses/{warehouseId})
                not abstract, but one of the previous
5) SKU is sold (POST /v1​/warehouses/sell-item)
                not only reserved SKUs can be sold
                sold SKUs are removed from a warehouse
                an invoice should be saved
                invoice number is returned
6) Return list of all warehouses (GET /v1​/warehouses)
7) Return info of one warehouse (GET /v1​/warehouses/{id})
                how many goods are stored
                how many goods are reserved
                how much free space is available
8) Add goods to the warehouse (POST /v1​/warehouses)
9) Return list of all invoices (GET /v1/invoices)
10) Return info about one invoice (GET /v1/invoices/{id})
11) TODO: Return all goods within an invoice
                goods can be returned to different warehouses
				

DB information:
Server=tcp:distributed-warehouses.database.windows.net,1433;Database=DistributedWarehouses;
User ID=cepas;Password=Augustas!