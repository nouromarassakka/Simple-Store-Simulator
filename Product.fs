
module Product

type Product = {
    Name: string
    Price: decimal
    Description: string
}

// Product Catalog
let products: Product list = [
    {
        Name = "Laptop"
        Price = 25000M
        Description = "HP Laptop with 8GB RAM"
    }
    {
        Name = "Mouse"
        Price = 350M
        Description = "Wireless optical mouse"
    }
    {
        Name = "Keyboard"
        Price = 750M
        Description = "Mechanical gaming keyboard"
    }
    {
        Name = "Headphones"
        Price = 1200M
        Description = "Wireless headphones"
    }
]

// Function 1: Retrieve all products
let getAllProducts () : Product list =
    products

// Function 2: Search for a product by name
let searchProduct (name: string) : Product option =
    if System.String.IsNullOrWhiteSpace(name) then
        None
    else
        products
        |> List.tryFind (fun p ->
            p.Name.Equals(
                name.Trim(),
                System.StringComparison.OrdinalIgnoreCase
            ))

// Function 3: Check if a product exists
let productExists (name: string) : bool =
    searchProduct name |> Option.isSome

// Function 4: Get product price by name
let getProductPrice (name: string) : decimal option =
    searchProduct name
    |> Option.map (fun p -> p.Price)

// Function 5: Get number of products
let getProductsCount () : int =
    List.length products
