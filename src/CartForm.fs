module CartForm

open System
open System.Drawing
open System.Windows.Forms
open Product
open CartOperations

let private rgb r g b = Color.FromArgb(r, g, b)

let private headerColor = rgb 62 39 35
let private accentColor = rgb 121 85 61
let private accentHover = rgb 93 64 45
let private pageColor   = rgb 245 237 224
let private cardColor   = rgb 255 251 245
let private textDark    = rgb 62 39 35
let private textMuted   = rgb 120 98 84
let private priceColor  = rgb 166 94 27
let private rowAlt      = rgb 250 244 234
let private rowSelected = rgb 232 213 190
let private gridLine    = rgb 230 219 203

let private buildCartRows (items: string list) : (Product * int * decimal) list =
    cartQuantities items
    |> List.choose (fun (name, qty) ->
        match searchProduct name with
        | Some p -> Some (p, qty, p.Price * decimal qty)
        | None -> None)

let createCartForm () : Form =
    let form =
        new Form(
            Text = "Simple Store - Shopping Cart",
            Size = Size(820, 620),
            MinimumSize = Size(640, 520),
            StartPosition = FormStartPosition.CenterScreen,
            BackColor = pageColor,
            Font = new Font("Segoe UI", 10.0f)
        )

    let header = new Panel(Dock = DockStyle.Top, Height = 85, BackColor = headerColor)

    let title =
        new Label(
            Text = "Shopping Cart",
            Font = new Font("Segoe UI Semibold", 20.0f),
            ForeColor = Color.White,
            AutoSize = true,
            Location = Point(25, 12)
        )
    let subtitle =
        new Label(
            Text = "Review the products you added to your cart",
            Font = new Font("Segoe UI", 10.0f),
            ForeColor = rgb 214 196 176,
            AutoSize = true,
            Location = Point(28, 54)
        )
    header.Controls.AddRange([| title :> Control; subtitle :> Control |])

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
            ColumnHeadersHeight = 44
        )

    grid.ColumnHeadersDefaultCellStyle.BackColor <- accentColor
    grid.ColumnHeadersDefaultCellStyle.ForeColor <- Color.White
    grid.ColumnHeadersDefaultCellStyle.Font <- new Font("Segoe UI Semibold", 10.5f)
    grid.ColumnHeadersDefaultCellStyle.SelectionBackColor <- accentColor
    grid.ColumnHeadersDefaultCellStyle.Padding <- Padding(12, 0, 0, 0)

    grid.RowTemplate.Height <- 42
    grid.DefaultCellStyle.BackColor <- cardColor
    grid.DefaultCellStyle.ForeColor <- textDark
    grid.DefaultCellStyle.SelectionBackColor <- rowSelected
    grid.DefaultCellStyle.SelectionForeColor <- textDark
    grid.DefaultCellStyle.Padding <- Padding(12, 0, 0, 0)
    grid.AlternatingRowsDefaultCellStyle.BackColor <- rowAlt
    grid.AlternatingRowsDefaultCellStyle.SelectionBackColor <- rowSelected
    grid.AlternatingRowsDefaultCellStyle.SelectionForeColor <- textDark

    grid.Columns.Add("Name", "Product") |> ignore
    grid.Columns.Add("Price", "Unit Price") |> ignore
    grid.Columns.Add("Quantity", "Quantity") |> ignore
    grid.Columns.Add("Subtotal", "Subtotal") |> ignore
    grid.Columns.[0].FillWeight <- 40.0f
    grid.Columns.[1].FillWeight <- 20.0f
    grid.Columns.[2].FillWeight <- 15.0f
    grid.Columns.[3].FillWeight <- 25.0f
    grid.Columns.[0].DefaultCellStyle.Font <- new Font("Segoe UI Semibold", 10.0f)
    grid.Columns.[1].DefaultCellStyle.ForeColor <- priceColor
    grid.Columns.[3].DefaultCellStyle.ForeColor <- priceColor
    grid.Columns.[3].DefaultCellStyle.Font <- new Font("Segoe UI Semibold", 10.0f)
    for c in grid.Columns do
        c.SortMode <- DataGridViewColumnSortMode.NotSortable

    let emptyLabel =
        new Label(
            Text = "Your cart is empty.\nGo back to the catalog and add some products.",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 12.0f),
            ForeColor = textMuted,
            BackColor = cardColor,
            Visible = false
        )

    let spacer = new Panel(Dock = DockStyle.Bottom, Height = 16, BackColor = pageColor)

    let card = new Panel(Dock = DockStyle.Bottom, Height = 110, BackColor = cardColor)
    let accentStrip = new Panel(Dock = DockStyle.Left, Width = 6, BackColor = accentColor)

    let itemsLabel =
        new Label(
            Text = "Total items: 0",
            Font = new Font("Segoe UI Semibold", 13.0f),
            ForeColor = textDark,
            AutoSize = true,
            Location = Point(26, 14)
        )
    let distinctLabel =
        new Label(
            Text = "Different products: 0",
            Font = new Font("Segoe UI", 10.5f),
            ForeColor = textMuted,
            AutoSize = true,
            Location = Point(28, 48)
        )
    let totalLabel =
        new Label(
            Text = "Cart total: 0.00",
            Font = new Font("Segoe UI Semibold", 14.0f),
            ForeColor = priceColor,
            AutoSize = true,
            Location = Point(28, 72)
        )
    card.Controls.AddRange(
        [| itemsLabel :> Control; distinctLabel :> Control; totalLabel :> Control; accentStrip :> Control |]
    )

    let body = new Panel(Dock = DockStyle.Fill, Padding = Padding(20, 20, 20, 10), BackColor = pageColor)
    body.Controls.Add(grid)
    body.Controls.Add(emptyLabel)
    body.Controls.Add(spacer)
    body.Controls.Add(card)

    let footer = new Panel(Dock = DockStyle.Bottom, Height = 64, BackColor = cardColor, Padding = Padding(20, 12, 20, 12))

    let statusLabel =
        new Label(
            Text = "",
            Dock = DockStyle.Left,
            Width = 360,
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

    footer.Controls.Add(statusLabel)
    footer.Controls.Add(closeButton)

    let refresh () =
        let items = getCart ()
        let rows = buildCartRows items

        grid.Rows.Clear()
        for (p, qty, subtotal) in rows do
            let idx = grid.Rows.Add(p.Name, p.Price.ToString("N2"), string qty, subtotal.ToString("N2"))
            grid.Rows.[idx].Tag <- box p

        let isEmpty = List.isEmpty rows
        grid.Visible <- not isEmpty
        emptyLabel.Visible <- isEmpty

        let totalItems = rows |> List.sumBy (fun (_, q, _) -> q)
        let totalPrice = rows |> List.sumBy (fun (_, _, s) -> s)

        itemsLabel.Text <- sprintf "Total items: %d" totalItems
        distinctLabel.Text <- sprintf "Different products: %d" (List.length rows)
        totalLabel.Text <- "Cart total: " + totalPrice.ToString("N2")
        statusLabel.Text <-
            if isEmpty then "Cart is empty"
            else sprintf "%d item(s) in your cart" totalItems

    refresh ()

    let subscription = cartChanged |> Observable.subscribe (fun () -> refresh ())
    form.FormClosed.Add(fun _ -> subscription.Dispose())

    form.Controls.Add(body)
    form.Controls.Add(footer)
    form.Controls.Add(header)

    form

let showCartForm () =
    let form = createCartForm ()
    form.Show()
