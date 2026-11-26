open System
open System.Windows.Forms

[<STAThread>]
[<EntryPoint>]
let main _ =
    Application.EnableVisualStyles()
    Application.Run(ProductForm.createProductForm ())
    0