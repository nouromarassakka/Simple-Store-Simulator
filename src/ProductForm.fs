module ProductForm

open System
open System.Drawing
open System.Windows.Forms
open Product // Member 1's module (Product.fs)

// ---------------------------------------------------------------
// Shared state / hooks for other members
// ---------------------------------------------------------------

/// The product currently selected in the catalog (None if nothing selected).
/// Member 3 (Add to Cart) can read this.
let mutable selectedProduct : Product option = None

/// Callback fired whenever the user selects a product.
/// Member 3 / Member 7 can replace it, e.g.:
///   ProductForm.onProductSelected <- fun p -> printfn "%s" p.Name
let mutable onProductSelected : Product -> unit = ignore

// ---------------------------------------------------------------
// Theme
// ---------------------------------------------------------------

let private rgb r g b = Color.FromArgb(r, g, b)

let private headerColor   = rgb 62 39 35      // dark chocolate brown
let private accentColor   = rgb 121 85 61     // medium coffee brown
let private accentHover   = rgb 93 64 45      // darker brown (hover)
let private pageColor     = rgb 245 237 224   // soft beige
let private cardColor     = rgb 255 251 245   // cream
let private textDark      = rgb 62 39 35      // dark brown text
let private textMuted     = rgb 120 98 84     // taupe
let private priceColor    = rgb 166 94 27     // caramel
let private rowAlt        = rgb 250 244 234   // light beige
let private rowSelected   = rgb 232 213 190   // sand
let private gridLine      = rgb 230 219 203   // light tan

// ---------------------------------------------------------------
// Form
// ---------------------------------------------------------------

let createProductForm () : Form =
    let form =
        new Form(
            Text = "Simple Store - Product Catalog",
            Size = Size(820, 620),
            MinimumSize = Size(640, 520),
            StartPosition = FormStartPosition.CenterScreen,
            BackColor = pageColor,
            Font = new Font("Segoe UI", 10.0f)
        )

    // ---------- Header banner ----------
    let header = new Panel(Dock = DockStyle.Top, Height = 85, BackColor = headerColor)

    let title =
        new Label(
            Text = "Product Catalog",
            Font = new Font("Segoe UI Semibold", 20.0f),
            ForeColor = Color.White,
            AutoSize = true,
            Location = Point(25, 12)
        )
    let subtitle =
        new Label(
            Text = "Browse our products and select one to see its details",
            Font = new Font("Segoe UI", 10.0f),
            ForeColor = rgb 214 196 176,
            AutoSize = true,
            Location = Point(28, 54)
        )
    header.Controls.AddRange([| title :> Control; subtitle :> Control |])

    // ---------- Product table ----------
    let grid =
        new DataGridView(
            Dock = DockStyle.Fill,
            BackgroundColor = cardColor,
            BorderStyle = BorderStyle.None,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            RowHeadersVisible = false,
            MultiSelect = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
            GridColor = gridLine,
            EnableHeadersVisualStyles = false,
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
            ColumnHeadersHeight = 44,
            Cursor = Cursors.Hand
        )

    // header style
    grid.ColumnHeadersDefaultCellStyle.BackColor <- accentColor
    grid.ColumnHeadersDefaultCellStyle.ForeColor <- Color.White
    grid.ColumnHeadersDefaultCellStyle.Font <- new Font("Segoe UI Semibold", 10.5f)
    grid.ColumnHeadersDefaultCellStyle.SelectionBackColor <- accentColor
    grid.ColumnHeadersDefaultCellStyle.Padding <- Padding(12, 0, 0, 0)

    // row style
    grid.RowTemplate.Height <- 42
    grid.DefaultCellStyle.BackColor <- cardColor
    grid.DefaultCellStyle.ForeColor <- textDark
    grid.DefaultCellStyle.SelectionBackColor <- rowSelected
    grid.DefaultCellStyle.SelectionForeColor <- textDark
    grid.DefaultCellStyle.Padding <- Padding(12, 0, 0, 0)
    grid.AlternatingRowsDefaultCellStyle.BackColor <- rowAlt
    grid.AlternatingRowsDefaultCellStyle.SelectionBackColor <- rowSelected
    grid.AlternatingRowsDefaultCellStyle.SelectionForeColor <- textDark

    // columns
    grid.Columns.Add("Name", "Product") |> ignore
    grid.Columns.Add("Price", "Price") |> ignore
    grid.Columns.Add("Description", "Description") |> ignore
    grid.Columns.[0].FillWeight <- 25.0f
    grid.Columns.[1].FillWeight <- 20.0f
    grid.Columns.[2].FillWeight <- 55.0f
    grid.Columns.[0].DefaultCellStyle.Font <- new Font("Segoe UI Semibold", 10.0f)
    grid.Columns.[1].DefaultCellStyle.ForeColor <- priceColor
    grid.Columns.[1].DefaultCellStyle.Font <- new Font("Segoe UI Semibold", 10.0f)
    for c in grid.Columns do
        c.SortMode <- DataGridViewColumnSortMode.NotSortable

    // fill from Member 1's catalog
    for p in getAllProducts () do
        let idx = grid.Rows.Add(p.Name, p.Price.ToString("N2"), p.Description)
        grid.Rows.[idx].Tag <- box p

    // ---------- Details card ----------
    let spacer = new Panel(Dock = DockStyle.Bottom, Height = 16, BackColor = pageColor)

    let card = new Panel(Dock = DockStyle.Bottom, Height = 130, BackColor = cardColor)
    let accentStrip = new Panel(Dock = DockStyle.Left, Width = 6, BackColor = accentColor)

    let nameLabel =
        new Label(
            Text = "Select a product",
            Font = new Font("Segoe UI Semibold", 15.0f),
            ForeColor = textDark,
            AutoSize = true,
            Location = Point(26, 14)
        )
    let descLabel =
        new Label(
            Text = "Click any row in the table to see its details here.",
            Font = new Font("Segoe UI", 10.5f),
            ForeColor = textMuted,
            AutoSize = true,
            Location = Point(28, 54)
        )
    let priceLabel =
        new Label(
            Text = "",
            Font = new Font("Segoe UI Semibold", 14.0f),
            ForeColor = priceColor,
            AutoSize = true,
            Location = Point(28, 86)
        )
    card.Controls.AddRange(
        [| nameLabel :> Control; descLabel :> Control; priceLabel :> Control; accentStrip :> Control |]
    )

    // ---------- Body (grid + details) ----------
    let body = new Panel(Dock = DockStyle.Fill, Padding = Padding(20, 20, 20, 10), BackColor = pageColor)
    body.Controls.Add(grid)     // Fill control first
    body.Controls.Add(spacer)
    body.Controls.Add(card)

    // ---------- Footer ----------
    let footer = new Panel(Dock = DockStyle.Bottom, Height = 64, BackColor = cardColor, Padding = Padding(20, 12, 20, 12))

    let countLabel =
        new Label(
            Text = sprintf "%d products available" (getProductsCount ()),
            Dock = DockStyle.Left,
            Width = 280,
            TextAlign = ContentAlignment.MiddleLeft,
            ForeColor = textMuted,
            Font = new Font("Segoe UI", 10.0f)
        )

    let closeButton =
        new Button(
            Text = "Close",
            Dock = DockStyle.Right,
            Width = 120,
            FlatStyle = FlatStyle.Flat,
            BackColor = accentColor,
            ForeColor = Color.White,
            Font = new Font("Segoe UI Semibold", 10.0f),
            Cursor = Cursors.Hand
        )
    closeButton.FlatAppearance.BorderSize <- 0
    closeButton.FlatAppearance.MouseOverBackColor <- accentHover
    closeButton.Click.Add(fun _ -> form.Close())

    footer.Controls.Add(countLabel)
    footer.Controls.Add(closeButton)

    // ---------- Product selection ----------
    grid.SelectionChanged.Add(fun _ ->
        if grid.SelectedRows.Count > 0 then
            let p = grid.SelectedRows.[0].Tag :?> Product
            selectedProduct <- Some p
            nameLabel.Text <- p.Name
            descLabel.Text <- p.Description
            priceLabel.Text <- "Price: " + p.Price.ToString("N2")
            onProductSelected p
        else
            selectedProduct <- None
            nameLabel.Text <- "Select a product"
            descLabel.Text <- "Click any row in the table to see its details here."
            priceLabel.Text <- "")

    // Form.Controls: Fill first, then Bottom, then Top (docking order)
    form.Controls.Add(body)
    form.Controls.Add(footer)
    form.Controls.Add(header)

    form

/// Opens the catalog form (non-blocking). Member 7 can call this from the main menu.
let showProductForm () =
    let form = createProductForm ()
    form.Show()