namespace WebSharper.Cytoscape.Sample

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open WebSharper.UI.Templating
open WebSharper.Cytoscape

[<JavaScript>]
module Client =

    // The template is the whole document, loaded from the DOM, so editing
    // index.html and refreshing the browser is enough.
    type IndexTemplate = Template<"index.html", ClientLoad.FromDocument>

    // ---------------------------------------------------------------
    // State bound into the template
    // ---------------------------------------------------------------

    let status = Var.Create "Ready — the graph is initialised on load."
    let notice = Var.Create "This sample doubles as the binding's test case: it exercises the core, collection, element, style, layout, animation, event, export and algorithm APIs."
    let stats = Var.Create "-"
    let viewport = Var.Create "-"
    let selection = Var.Create "-"
    let bounds = Var.Create "-"
    let algorithm = Var.Create "-"
    let events = Var.Create "no events yet"

    // Theme: light is the default; the choice is persisted in localStorage.
    // The storage key is shared with the bootstrap script in index.html.
    let themeStorageKey = "websharper-cytoscape-theme"
    let theme = Var.Create "light"
    let themeLabel = Var.Create "🌙 Dark mode"

    // The graph container lives inside the template, so its DOM element only
    // exists after the template has been bound.
    let graph = Elt.div [ attr.id "cy"; Attr.Class "graph" ] []

    let mutable cy : option<Cytoscape> = None

    // ---------------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------------

    let withCy (f: Cytoscape -> unit) =
        match cy with
        | Some c -> f c
        | None -> status.Value <- "The graph is not ready yet."

    let fmt (x: float) = sprintf "%.1f" x

    [<Inline "$0.click()">]
    let private click (el: Dom.Element) = X<unit>

    let applyTheme () =
        JS.Document.DocumentElement.SetAttribute("data-theme", theme.Value)
        themeLabel.Value <-
            if theme.Value = "dark" then "☀️ Light mode" else "🌙 Dark mode"
        // Keep the graph's label colour legible on the current background.
        match cy with
        | None -> ()
        | Some c ->
            let labelColor = if theme.Value = "dark" then "#e2e8f0" else "#0f172a"
            let mutedColor = if theme.Value = "dark" then "#94a3b8" else "#64748b"
            c.Style().Selector("node").Style("color", labelColor).Update() |> ignore
            c.Style().Selector("edge").Style("color", mutedColor).Update() |> ignore

    let toggleTheme () =
        theme.Value <- (if theme.Value = "dark" then "light" else "dark")
        try JS.Window.LocalStorage.SetItem(themeStorageKey, theme.Value) with _ -> ()
        applyTheme ()

    let nodeOf (eles: Eles) : option<WebSharper.Cytoscape.Node> =
        if eles.Empty() then None else Some (As<WebSharper.Cytoscape.Node> eles)

    let nodeById (c: Cytoscape) (id: string) =
        nodeOf (c.GetElementById(id))

    let eleOf (eles: Eles) : option<Ele> =
        if eles.Empty() then None else Some (As<Ele> eles)

    // ---------------------------------------------------------------
    // Readout
    // ---------------------------------------------------------------

    let refresh () =
        withCy (fun c ->
            let nodes = c.Nodes().Size()
            let edges = c.Edges().Size()
            let selected = c.Elements().Filter(":selected").Size()
            stats.Value <-
                sprintf "%d nodes · %d edges · %d selected · %d components"
                    nodes edges selected (c.Elements().Components().Length)

            let p = c.Pan()
            viewport.Value <-
                sprintf "zoom %.2f · pan (%s, %s) · %d×%d px"
                    (c.Zoom()) (fmt p.X) (fmt p.Y) (int (c.Width())) (int (c.Height()))

            let bb = c.Extent()
            bounds.Value <-
                sprintf "x %s…%s · y %s…%s"
                    (fmt bb.X1) (fmt bb.X2) (fmt bb.Y1) (fmt bb.Y2)

            match eleOf (c.Elements().Filter(":selected")) with
            | None -> selection.Value <- "none"
            | Some ele ->
                let parts =
                    [ sprintf "%s" (ele.Id()) ]
                let parts =
                    if ele.IsNode() then
                        let node = As<WebSharper.Cytoscape.Node> ele
                        let pos = node.Position()
                        parts @
                            [
                                sprintf "degree %d" (node.Degree())
                                sprintf "pos (%s, %s)" (fmt pos.X) (fmt pos.Y)
                            ]
                    else
                        let edge = As<Edge> ele
                        let mid = edge.Midpoint()
                        let src = edge.Source()
                        let tgt = edge.Target()
                        parts @
                            [
                                sprintf "%s → %s" (src.Id()) (tgt.Id())
                                sprintf "midpoint (%s, %s)" (fmt mid.X) (fmt mid.Y)
                            ]
                let classes = ele.Classes()
                selection.Value <-
                    String.concat " · " (parts @ [ sprintf "classes [%s]" (String.concat ", " classes) ]))

    // ---------------------------------------------------------------
    // Viewport and selection
    // ---------------------------------------------------------------

    let fit () =
        withCy (fun c ->
            c.Fit() |> ignore
            refresh ()
            status.Value <- "Fitted the viewport to all elements.")

    let center () =
        withCy (fun c ->
            c.Center() |> ignore
            refresh ()
            status.Value <- "Centred the viewport.")

    let reset () =
        withCy (fun c ->
            c.Reset() |> ignore
            refresh ()
            status.Value <- "Reset zoom and pan.")

    let zoomIn () =
        withCy (fun c ->
            c.Zoom(c.Zoom() * 1.25) |> ignore
            refresh ()
            status.Value <- "Zoomed in.")

    let zoomOut () =
        withCy (fun c ->
            c.Zoom(c.Zoom() / 1.25) |> ignore
            refresh ()
            status.Value <- "Zoomed out.")

    let panRight () =
        withCy (fun c ->
            c.PanBy(Position(x = 80., y = 0.)) |> ignore
            refresh ()
            status.Value <- "Panned the viewport by (80, 0).")

    let selectAll () =
        withCy (fun c ->
            c.Elements().Select() |> ignore
            refresh ()
            status.Value <- "Selected every element.")

    let clearSelection () =
        withCy (fun c ->
            c.Elements().Unselect() |> ignore
            refresh ()
            status.Value <- "Cleared the selection.")

    // ---------------------------------------------------------------
    // Graph manipulation
    // ---------------------------------------------------------------

    let mutable nextNode = 1

    let bindElementEvents (c: Cytoscape) =
        let log (e: EventObject) =
            let target =
                let ele = As<Ele> e.Target
                if ele.IsNode() then sprintf "node %s" (ele.Id()) else sprintf "edge %s" (ele.Id())
            events.Value <- sprintf "%s on %s" e.Type target
            refresh ()
        c.Elements().On("tap", log) |> ignore
        c.Elements().On("select", log) |> ignore
        c.Elements().On("unselect", log) |> ignore
        c.On("layoutstop", fun (e: EventObject) ->
            events.Value <- sprintf "%s (layout finished)" e.Type
            refresh ())
        |> ignore

    let addNode () =
        withCy (fun c ->
            let id = sprintf "n%d" nextNode
            nextNode <- nextNode + 1
            let added =
                c.Add(
                    ElementObject(
                        Data = ElementData(Id = id, Name = id.ToUpper(), Weight = 1.),
                        Position = Position(x = 40. + 35. * float nextNode, y = 60. + 25. * float nextNode)
                    )
                )
            // Connect the new node to the previous one (or to "a" the first time).
            let previous = sprintf "n%d" (nextNode - 2)
            let anchor = if c.HasElementWithId(previous) then previous else "a"
            if c.HasElementWithId(anchor) then
                c.Add(ElementObject(Data = ElementData(Id = id + "e", Source = anchor, Target = id)))
                |> ignore
            added.AddClass("fresh") |> ignore
            bindElementEvents c
            refresh ()
            status.Value <- sprintf "Added node %s." id)

    let removeSelected () =
        withCy (fun c ->
            let selected = c.Elements().Filter(":selected")
            if selected.Empty() then
                status.Value <- "Nothing is selected."
            else
                let removed = c.Remove(selected).Size()
                bindElementEvents c
                refresh ()
                status.Value <- sprintf "Removed %d element(s)." removed)

    let highlight () =
        withCy (fun c ->
            let nodes = c.Nodes()
            nodes.ToggleClass("highlight") |> ignore
            c.Edges().AddClass("highlight") |> ignore
            refresh ()
            status.Value <- "Toggled the `highlight` class on the nodes.")

    let flashEdges () =
        withCy (fun c ->
            c.Edges().FlashClass("flash", 400) |> ignore
            status.Value <- "Flashed the edges via FlashClass(name, 400).")

    // ---------------------------------------------------------------
    // Layouts
    // ---------------------------------------------------------------

    let runLayout (options: LayoutOptions) =
        withCy (fun c ->
            c.Layout(options).Run() |> ignore
            refresh ()
            status.Value <- sprintf "Started the `%s` layout." options.Name)

    let layoutGrid () =
        runLayout (LayoutOptions(Name = "grid", Animate = true, AnimationDuration = 500, Fit = true, Padding = 40., Rows = 3))

    let layoutCircle () =
        runLayout (LayoutOptions(Name = "circle", Animate = true, AnimationDuration = 500, Fit = true, Padding = 40., Radius = 160.))

    let layoutConcentric () =
        runLayout (
            LayoutOptions(
                Name = "concentric",
                Animate = true,
                AnimationDuration = 500,
                Fit = true,
                Padding = 40.,
                Equidistant = true,
                MinNodeSpacing = 30.,
                Concentric = System.Func<WebSharper.Cytoscape.Node, float>(fun n -> float (n.Degree()))
            )
        )

    let layoutBreadthFirst () =
        runLayout (
            LayoutOptions(
                Name = "breadthfirst",
                Animate = true,
                AnimationDuration = 500,
                Fit = true,
                Padding = 40.,
                Directed = true,
                Circle = false,
                Grid = true
            )
        )

    let layoutCose () =
        runLayout (
            LayoutOptions(
                Name = "cose",
                Animate = true,
                AnimationDuration = 800,
                Fit = true,
                Padding = 40.,
                Randomize = false,
                ComponentSpacing = 80.,
                NodeRepulsion = 8000.,
                IdealEdgeLength = 90.,
                Gravity = 60.
            )
        )

    let layoutRandom () =
        runLayout (LayoutOptions(Name = "random", Animate = true, AnimationDuration = 500, Fit = true, Padding = 40.))

    // ---------------------------------------------------------------
    // Style
    // ---------------------------------------------------------------

    let restyle () =
        withCy (fun c ->
            c.Style()
                .Selector("node")
                .Style("background-color", "#f59e0b")
                .Style("border-width", 2.)
                .Update()
            |> ignore
            status.Value <- "Restyled nodes through the `Style` API.")

    // ---------------------------------------------------------------
    // Animations
    // ---------------------------------------------------------------

    let animateElements () =
        withCy (fun c ->
            let nodes = c.Nodes()
            nodes.Animate(
                AnimateOptions(
                    Style = New [ ("opacity", box 0.45) ],
                    Duration = 600,
                    Easing = "ease-in-out",
                    Complete = System.Action(fun () -> nodes.Animate(AnimateOptions(Style = New [ ("opacity", box 1.) ], Duration = 600)) |> ignore)
                )
            )
            |> ignore
            let someNode = nodeById c "a"
            match someNode with
            | None -> ()
            | Some node ->
                let animation =
                    node.Animation(
                        AnimateOptions(
                            Position = Position(x = node.Position().X + 60., y = node.Position().Y),
                            Duration = 700,
                            Easing = "ease-in-out"
                        )
                    )
                animation.Play() |> ignore
                animation.Pause() |> ignore
                animation.Rewind() |> ignore
            refresh ()
            status.Value <- "Animated the element style and a node position.")

    let animateViewport () =
        withCy (fun c ->
            c.Animate(
                AnimateOptions(
                    Zoom = ZoomOptions(Level = c.Zoom() * 1.4, RenderedPosition = Position(x = c.Width() / 2., y = c.Height() / 2.)),
                    Duration = 700,
                    Easing = "ease-in-out"
                )
            )
            |> ignore
            refresh ()
            status.Value <- "Animated the viewport to a new zoom level.")

    // ---------------------------------------------------------------
    // Export
    // ---------------------------------------------------------------

    let exportPng () =
        withCy (fun c ->
            let dataUrl = c.Png()
            let a = JS.Document.CreateElement("a")
            a.SetAttribute("href", dataUrl)
            a.SetAttribute("download", "cytoscape-graph.png")
            click a
            status.Value <- "Exported the graph as a PNG data URI.")

    // ---------------------------------------------------------------
    // Algorithms
    // ---------------------------------------------------------------

    let rootNode (c: Cytoscape) =
        match nodeById c "a" with
        | Some n -> n
        | None ->
            match nodeOf (c.Nodes()) with
            | Some n -> n
            | None -> As<WebSharper.Cytoscape.Node> (c.Collection())

    let runBfs () =
        withCy (fun c ->
            let r = c.Elements().BreadthFirstSearch(AlgorithmOptions(Root = rootNode c))
            algorithm.Value <-
                sprintf "BFS: visited %d elements, found %d node(s)"
                    (r.Path.Size()) (r.Found.Size()))

    let runDijkstra () =
        withCy (fun c ->
            let root = rootNode c
            let target = As<WebSharper.Cytoscape.Node> (c.Nodes().Last())
            let r = c.Elements().Dijkstra(AlgorithmOptions(Root = root))
            let path = r.PathTo(target)
            algorithm.Value <-
                sprintf "Dijkstra: distance %s, path of %d element(s)"
                    (fmt (r.DistanceTo(target))) (path.Size()))

    let runPageRank () =
        withCy (fun c ->
            let r = c.Elements().PageRank(AlgorithmOptions())
            let node = rootNode c
            algorithm.Value <- sprintf "PageRank(a) = %.4f" (r.Rank(node)))

    let runDegree () =
        withCy (fun c ->
            let r = c.Elements().DegreeCentrality(AlgorithmOptions(Root = rootNode c))
            algorithm.Value <- sprintf "Degree centrality(a) = %s" (fmt r.Degree))

    let runCloseness () =
        withCy (fun c ->
            let v = c.Elements().ClosenessCentrality(AlgorithmOptions(Root = rootNode c))
            algorithm.Value <- sprintf "Closeness centrality(a) = %.4f" v)

    let runBetweenness () =
        withCy (fun c ->
            let r = c.Elements().BetweennessCentrality(AlgorithmOptions())
            let node = rootNode c
            algorithm.Value <-
                sprintf "Betweenness(a) = %s, normalised = %s"
                    (fmt (r.Betweenness(node))) (fmt (r.BetweennessNormalized(node))))

    let runComponents () =
        withCy (fun c ->
            let parts = c.Elements().Components()
            algorithm.Value <- sprintf "%d connected component(s)" parts.Length)

    let runKruskal () =
        withCy (fun c ->
            let tree = c.Elements().Kruskal(fun (e: Edges) -> 1.)
            algorithm.Value <-
                sprintf "Kruskal: spanning tree of %d edge(s) from %d"
                    (tree.Filter("edge").Size()) (c.Edges().Size()))

    let runKargerStein () =
        withCy (fun c ->
            let r = c.Elements().KargerStein()
            algorithm.Value <-
                sprintf "Karger-Stein: cut of %d edge(s), partitions %d / %d"
                    (r.Cut.Size()) (r.Partition1.Size()) (r.Partition2.Size()))

    let runKMeans () =
        withCy (fun c ->
            let clusters =
                c.Nodes().KMeans(
                    ClusteringOptions(
                        Attributes = [| System.Func<WebSharper.Cytoscape.Node, float>(fun n -> As<float> (n.Data("weight"))) |],
                        K = 2,
                        MaxIterations = 20
                    )
                )
            algorithm.Value <- sprintf "k-means: %d cluster(s)" clusters.Length)

    let runHierarchical () =
        withCy (fun c ->
            let clusters =
                c.Nodes().HierarchicalClustering(
                    ClusteringOptions(
                        Attributes = [| System.Func<WebSharper.Cytoscape.Node, float>(fun n -> float (n.Degree())) |],
                        Linkage = "mean",
                        Mode = "threshold"
                    )
                )
            algorithm.Value <- sprintf "hierarchical clustering: %d cluster(s)" clusters.Length)

    // ---------------------------------------------------------------
    // The graph
    // ---------------------------------------------------------------

    let elements =
        [|
            ElementObject(Data = ElementData(Id = "p", Name = "Parent", Weight = 4.))
            ElementObject(Data = ElementData(Id = "a", Parent = "p", Name = "A", Weight = 2.), Position = Position(x = 120., y = 120.))
            ElementObject(Data = ElementData(Id = "b", Parent = "p", Name = "B", Weight = 3.), Position = Position(x = 220., y = 110.))
            ElementObject(Data = ElementData(Id = "c", Name = "C", Weight = 1.), Position = Position(x = 330., y = 180.))
            ElementObject(Data = ElementData(Id = "d", Name = "D", Weight = 5.), Position = Position(x = 180., y = 260.))
            ElementObject(Data = ElementData(Id = "e", Name = "E", Weight = 2.), Position = Position(x = 320., y = 300.))
            ElementObject(Data = ElementData(Id = "f", Name = "F", Weight = 4.), Position = Position(x = 430., y = 240.))
            ElementObject(Data = ElementData(Id = "g", Name = "G", Weight = 3.), Position = Position(x = 420., y = 350.))
            ElementObject(Data = ElementData(Id = "h", Name = "H", Weight = 1.), Position = Position(x = 250., y = 380.))
            ElementObject(Data = ElementData(Id = "ab", Source = "a", Target = "b", Weight = 3.))
            ElementObject(Data = ElementData(Id = "bc", Source = "b", Target = "c", Weight = 1.))
            ElementObject(Data = ElementData(Id = "cd", Source = "c", Target = "d", Weight = 2.))
            ElementObject(Data = ElementData(Id = "de", Source = "d", Target = "e", Weight = 4.))
            ElementObject(Data = ElementData(Id = "ef", Source = "e", Target = "f", Weight = 2.))
            ElementObject(Data = ElementData(Id = "fa", Source = "f", Target = "a", Weight = 5.))
            ElementObject(Data = ElementData(Id = "ad", Source = "a", Target = "d", Weight = 1.))
            ElementObject(Data = ElementData(Id = "cg", Source = "c", Target = "g", Weight = 2.))
            ElementObject(Data = ElementData(Id = "gh", Source = "g", Target = "h", Weight = 3.))
            ElementObject(Data = ElementData(Id = "he", Source = "h", Target = "e", Weight = 1.))
        |]

    let styles =
        [|
            StyleConfig(
                selector = "node",
                style =
                    New [
                        "background-color", box "#6366f1"
                        "border-color", box "#ffffff"
                        "border-width", box 2
                        "label", box "data(name)"
                        "color", box "#0f172a"
                        "font-size", box 11
                        "text-valign", box "bottom"
                        "text-margin-y", box 6
                        "width", box 26
                        "height", box 26
                    ]
            )
            StyleConfig(
                selector = "node:parent",
                style =
                    New [
                        "background-color", box "#e0e7ff"
                        "background-opacity", box 0.6
                        "border-color", box "#6366f1"
                        "border-style", box "dashed"
                        "label", box "data(name)"
                        "text-valign", box "top"
                    ]
            )
            StyleConfig(
                selector = "edge",
                style =
                    New [
                        "width", box 2
                        "line-color", box "#cbd5e1"
                        "target-arrow-color", box "#cbd5e1"
                        "target-arrow-shape", box "triangle"
                        "curve-style", box "bezier"
                        "label", box "data(weight)"
                        "font-size", box 9
                        "color", box "#64748b"
                    ]
            )
            StyleConfig(
                selector = "node.highlight",
                style =
                    New [
                        "background-color", box "#f59e0b"
                        "border-color", box "#b45309"
                    ]
            )
            StyleConfig(
                selector = ":selected",
                style =
                    New [
                        "background-color", box "#0e7490"
                        "line-color", box "#0e7490"
                        "target-arrow-color", box "#0e7490"
                    ]
            )
            StyleConfig(
                selector = ".fresh",
                style = New [ "border-color", box "#16a34a"; "border-width", box 4 ]
            )
        |]

    let createGraph () =
        let c =
            Cytoscape(
                CytoscapeOptions(
                    Container = graph.Dom,
                    Elements = elements,
                    Style = styles,
                    Layout = LayoutOptions(Name = "cose", Fit = true, Padding = 40.),
                    WheelSensitivity = 0.2,
                    BoxSelectionEnabled = true,
                    SelectionType = SelectionType.Additive
                )
            )
        cy <- Some c
        bindElementEvents c
        c.Fit() |> ignore
        applyTheme ()

        // Show a few read-only APIs that are otherwise easy to miss.
        let bb = c.Elements().BoundingBox()
        bounds.Value <- sprintf "x %s…%s · y %s…%s" (fmt bb.X1) (fmt bb.X2) (fmt bb.Y1) (fmt bb.Y2)
        let view = c.GetFitViewport()
        viewport.Value <- sprintf "fit zoom %.2f" view.Zoom
        refresh ()

    // ---------------------------------------------------------------
    // Entry point
    // ---------------------------------------------------------------

    [<SPAEntryPoint>]
    let Main () =
        // Restore the stored theme before binding the template.
        try
            let stored = JS.Window.LocalStorage.GetItem(themeStorageKey)
            if stored ===. "dark" || stored ===. "light" then theme.Value <- stored
        with _ -> ()
        applyTheme ()

        IndexTemplate()
            .graphContainer(graph :> Doc)
            .status(status.V)
            .notice(notice.V)
            .stats(stats.V)
            .viewport(viewport.V)
            .selection(selection.V)
            .bounds(bounds.V)
            .algorithm(algorithm.V)
            .events(events.V)
            .themeLabel(themeLabel.V)
            .ToggleTheme(fun _ -> toggleTheme ())
            .Fit(fun _ -> fit ())
            .Center(fun _ -> center ())
            .Reset(fun _ -> reset ())
            .ZoomIn(fun _ -> zoomIn ())
            .ZoomOut(fun _ -> zoomOut ())
            .PanRight(fun _ -> panRight ())
            .SelectAll(fun _ -> selectAll ())
            .ClearSelection(fun _ -> clearSelection ())
            .AddNode(fun _ -> addNode ())
            .RemoveSelected(fun _ -> removeSelected ())
            .Highlight(fun _ -> highlight ())
            .FlashEdges(fun _ -> flashEdges ())
            .AnimateElements(fun _ -> animateElements ())
            .Restyle(fun _ -> restyle ())
            .AnimateViewport(fun _ -> animateViewport ())
            .ExportPng(fun _ -> exportPng ())
            .LayoutGrid(fun _ -> layoutGrid ())
            .LayoutCircle(fun _ -> layoutCircle ())
            .LayoutConcentric(fun _ -> layoutConcentric ())
            .LayoutBreadthFirst(fun _ -> layoutBreadthFirst ())
            .LayoutCose(fun _ -> layoutCose ())
            .LayoutRandom(fun _ -> layoutRandom ())
            .RunBfs(fun _ -> runBfs ())
            .RunDijkstra(fun _ -> runDijkstra ())
            .RunPageRank(fun _ -> runPageRank ())
            .RunDegree(fun _ -> runDegree ())
            .RunCloseness(fun _ -> runCloseness ())
            .RunBetweenness(fun _ -> runBetweenness ())
            .RunComponents(fun _ -> runComponents ())
            .RunKruskal(fun _ -> runKruskal ())
            .RunKargerStein(fun _ -> runKargerStein ())
            .RunKMeans(fun _ -> runKMeans ())
            .RunHierarchical(fun _ -> runHierarchical ())
            .Bind()

        // The DOM is ready now: create the graph in the hydrated container.
        createGraph ()