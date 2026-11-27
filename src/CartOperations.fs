module CartOperations

open System
open Product 

let mutable private cart : string list = [] // This will hold JUST THE NAMES of products in the cart


//cartChangedEvent is private which means the other modules cannot control its trigger (encapsulation) 
let private cartChangedEvent = Event<unit>() // creating event which will be triggered whenever the cart changes
let cartChanged = cartChangedEvent.Publish

let getCart () : string list = cart // gets the current cart items

let setCart (newCart: string list) : unit = // sets the cart to a new list of items 
    cart <- newCart
    cartChangedEvent.Trigger()

let clearCart () : unit = // empty the cart and trigger the event
    cart <- []
    cartChangedEvent.Trigger()

let validateProduct (name: string) : Product option = // checks if the product exists in the store
    searchProduct name


//OrdinalIgnoreCase is used to compare strings without considering case sensitivity.
let countInCart (name: string) (items: string list) : int = // counts how many times a product appears in the cart
    items |> List.filter (fun n -> n.Equals(name, StringComparison.OrdinalIgnoreCase)) |> List.length

let cartQuantities (items: string list) : (string * int) list = // returns a list of tuples containing product names and their corresponding quantities
    items
    |> List.countBy id
    |> List.sortBy fst  // sorts the list of tuples by product name in ascending order


// addToCart vs addToCurrentCart:
// addToCart is a more general function that takes the current cart as an argument and returns a Result type indicating success or failure. It does not modify the global cart.
// addToCurrentCart is a specialized function that directly modifies the global cart and returns the updated cart along with the quantity of the added product.
// The reason for having both functions is to separate the logic of adding a product to a cart from the side effect of modifying the global cart state. (State management and side effects are separated from the core logic.)
//This allows for better testability and reusability of the addToCart function.


//returns Result<SuccessType, ErrorType> , if successful returns the  cart list, if not returns an error message
// we chose result instead of option because we want to provide a specific error message when the product is not found or when the input is invalid.
let addToCart (current: string list) (name: string) : Result<string list, string> =
    match validateProduct name with
    | None when String.IsNullOrWhiteSpace name -> // even the input is empty or whitespace, we want to return a specific error message
        Error "Please select or enter a product name."
    | None ->
        Error (sprintf "Product '%s' does not exist in the store." (name.Trim()))
    | Some product ->
        Ok (current @ [ product.Name ]) //@ concatenates the current cart with the new product name
        // product.Name is used instead of name.Trim() to ensure that the product name is consistent with the store's product list.


let addToCurrentCart (name: string) : Result<string * int, string> =
    match addToCart cart name with
    | Error msg -> Error msg
    | Ok newCart ->
        setCart newCart
        let added = List.last newCart
        Ok (added, countInCart added newCart)