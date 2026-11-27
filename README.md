# Simple Store Simulator

An F# project that simulates a simple store and shopping cart.

## Team Modules

| Member | Responsibility | Main File(s) |
|---|---|---|
| Member 1 | Product Catalog & Data Model | `Product.fs` |
| Member 2 | Browse Products & GUI | `ProductForm.fs` |
| Member 3 | Add to Cart | `CartOperations.fs` |
| Member 4 | View Cart | `CartForm.fs` |
| Member 5 | Remove from Cart | `CartRemoval.fs` |
| Member 6 | Checkout & Total Calculation | `Checkout.fs`, `CheckoutForm.fs` |
| Member 7 | Main Application, Integration & Testing | `MainForm.fs`, `Program.fs` |

## Technologies

- F#
- .NET
- Windows Forms

### CartOperations.fs

This file handles the shopping cart operations, including:

Mirna member3 cart operations
### CartOperations.fs

- **Cart Management:** Stores cart items as a list of product names and provides functions to retrieve, update, and clear the cart.
- **Cart Changed Event:** Defines an event that is triggered whenever the cart is updated or cleared.
- **Product Validation:** Validates product names against the existing product catalog and handles empty or invalid product names.
- **Quantity Counting:** Calculates how many times a specific product appears in the cart.
- **Cart Quantities:** Groups repeated products and calculates the quantity of each product, sorting the results alphabetically.
- **Add to Cart:** Adds a validated product to a given cart list and returns either the updated list or an error message.
- **Add to Current Cart:** Adds a product to the current cart, updates the cart state, triggers the cart-changed event, and returns the product name and its updated quantity.

