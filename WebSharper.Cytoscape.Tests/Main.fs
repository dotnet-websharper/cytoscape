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
                notEqualMsg (cy.Width()) (JS.Undefined) "Get Width"
            }

            Test "Get Height" {
                let cy = Cytoscape.Cytoscape()
                notEqualMsg (cy.Height()) (JS.Undefined) "Get Height"
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
                ) (JS.Undefined) "Add element"
            }

        }

    let RunTests() =
        Runner.RunTests [
            Tests
        ]

module Site =
    open WebSharper.UI.Next.Server
    open WebSharper.UI.Next.Html

    [<Website>]
    let Main =
        Application.SinglePage (fun ctx ->
            Content.Page(
                Title = "WebSharper.CytoscapeJS Tests",
                Body = [
                    client <@ Client.RunTests() @>
                ]
            )
        )
