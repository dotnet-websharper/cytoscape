namespace WebSharper.Cytoscape.Sample

open WebSharper
open WebSharper.Cytoscape
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

[<JavaScript>]
module Client =

    [<SPAEntryPoint>]
    let Main () =
        let graph = Elt.div [attr.style "height:100vh;"] []

        graph
        |> Doc.RunById "main"

        let cy = 
            Cytoscape(
                CytoscapeOptions(
                    Container = graph.Dom,
                    Elements = 
                        [|
                            ElementObject(Data = ElementData(Id = "a"))
                            ElementObject(Data = ElementData(Id = "b"))
                            ElementObject(Data = ElementData(Id = "c"))
                            ElementObject(Data = ElementData(Id = "d"))
                            ElementObject(Data = ElementData(Id = "e"))
                            ElementObject(Data = ElementData(Id = "f"))
                            ElementObject(Data = ElementData(Id = "ab", Source = "a", Target = "b"))
                            ElementObject(Data = ElementData(Id = "ba", Source = "b", Target = "a"))
                            ElementObject(Data = ElementData(Id = "bc", Source = "b", Target = "c"))
                            ElementObject(Data = ElementData(Id = "ad", Source = "a", Target = "d"))
                            ElementObject(Data = ElementData(Id = "cf", Source = "c", Target = "f"))
                            ElementObject(Data = ElementData(Id = "de", Source = "d", Target = "e"))
                            ElementObject(Data = ElementData(Id = "bf", Source = "b", Target = "f"))
                        |],
                    Style = 
                        [|
                            StyleConfig(
                                selector = "node",
                                style = New [
                                    "background-color", "#666" :> obj
                                    "label", "data(id)" :> obj
                                ]
                            )
                            StyleConfig(
                                selector = "edge",
                                style = New [
                                    "width", "3" :> obj
                                    "line-color", "#ccc" :> obj
                                    "target-arrow-color", "#ccc" :> obj
                                    "target-arrow-shape", "triangle" :> obj
                                    "curve-style", "bezier" :> obj
                                ]
                            )
                            StyleConfig(
                                selector = ":selected",
                                style = New [
                                    "background-color", "#070" :> obj
                                    "line-color", "#070" :> obj
                                    "target-arrow-color", "#070" :> obj
                                    "text-outline-color", "#000" :> obj
                                ]
                            )
                        |],
                    Layout = LayoutOptions(
                        Name = "grid",
                        Rows = 1
                    )
                )
            )
        ()
        //cy.Elements().Kruskal().Select()
