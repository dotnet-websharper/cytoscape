namespace WebSharper.Cytoscape.Tests

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI.Next

[<JavaScript>]
module Client =
    open WebSharper.JavaScript
    open WebSharper.Testing

    let Tests =
        TestCategory "General" {

            Test "Sanity check" {
                equalMsg (1 + 1) 2 "1 + 1 = 2"
            }

            Test "Construct an element" {
                let cy = Cytoscape.Cytoscape()
                notEqualMsg (cy) (JS.Undefined) "Constructor"
            }

            Test "Get Width" {
                let cy = Cytoscape.Cytoscape()
                notEqualMsg (cy.Width()) (JS.Undefined) "Constructor"
            }

            Test "Get Height" {
                let cy = Cytoscape.Cytoscape()
                notEqualMsg (cy.Height()) (JS.Undefined) "Constructor"
            }

            Test "Add element" {
                let cy = Cytoscape.Cytoscape()
                notEqualMsg (cy.Add
                    (
                        Cytoscape.ElementObject(
                            Group="nodes",
                            Data=Cytoscape.ElementData(Id="n1"),
                            Position=Cytoscape.Position(
                                X=200.,
                                Y=200.
                            )
                        )
                    )
                ) (JS.Undefined) "Constructor"
            }

        }

#if ZAFIR
    let RunTests() =
        Runner.RunTests [
            Tests
        ]
#endif

module Site =
    open WebSharper.UI.Next.Server
    open WebSharper.UI.Next.Html

    [<Website>]
    let Main =
        Application.SinglePage (fun ctx ->
            Content.Page(
                Title = "WebSharper.CytoscapeJS Tests",
                Body = [
#if ZAFIR
                    client <@ Client.RunTests() @>
#else
                    WebSharper.Testing.Runner.Run [
                        System.Reflection.Assembly.GetExecutingAssembly()
                    ]
                    |> Doc.WebControl
#endif
                ]
            )
        )
