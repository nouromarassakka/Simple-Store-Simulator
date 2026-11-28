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


  
Mirna member3 cart operations
### CartOperations.fs
- **Cart Management:** Stores cart items as a list of product names and provides functions to retrieve, update, and clear the cart.
- **Cart Changed Event:** Defines an event that is triggered whenever the cart is updated or cleared.
- **Product Validation:** Validates product names against the existing product catalog and handles empty or invalid product names.
- **Quantity Counting:** Calculates how many times a specific product appears in the cart.
- **Cart Quantities:** Groups repeated products and calculates the quantity of each product, sorting the results alphabetically.
- **Add to Cart:** Adds a validated product to a given cart list and returns either the updated list or an error message.
- **Add to Current Cart:** Adds a product to the current cart, updates the cart state, triggers the cart-changed event, and returns the product name and its updated quantity.

Ganna member4 view cart
### CartForm.fs
- **Cart Display Rows:** Converts the cart (a list of product names) into display rows. Repeated products are grouped using `cartQuantities`, each name is looked up in the product catalog, and the subtotal (price × quantity) is calculated. Names not found in the catalog are ignored.
- **Shopping Cart Form:** Builds the Windows Forms cart window with a header, a product grid, a summary card, and a footer, using the same color theme as the product catalog form.
- **Cart Grid:** Displays each product in the cart with its name, unit price, quantity, and subtotal.
- **Empty Cart Message:** Hides the grid and shows a friendly message when the cart has no items.
- **Cart Summary:** Shows the total number of items, the number of different products, and the cart total.
- **Refresh Function:** Redraws the grid, summary labels, and status text from the current cart contents.
- **Auto-Update:** Subscribes to the `cartChanged` event from `CartOperations.fs`, so the form updates automatically whenever an item is added, removed, or the cart is cleared. The subscription is disposed when the form closes.
- **Show Cart Form:** Provides `createCartForm` to build the form and `showCartForm` to open it, so the Main Menu can call it.

