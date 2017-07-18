namespace WebSharper.Cytoscape.Extension

open WebSharper
open WebSharper.InterfaceGenerator

module Definition =
    let CytoscapeClass = Class "cytoscape"

    let Position =
        Pattern.Config "Position" {
            Required = []
            Optional =
                [
                    "x", T<float>
                    "y", T<float>
                ]
        }

    let ElementData =
        Pattern.Config "ElementData" {
            Required = []
            Optional =
                [
                    "id", T<string>
                    "parent", T<string> 
                    "source", T<string>
                    "target", T<string>
                ]
        }

    let ElementObject =
        Pattern.Config "ElementObject" {
            Required = []
            Optional =
                [
                    "group", T<string>
                    "data", ElementData.Type
                    "scratch", T<obj>
                    "position", Position.Type
                    "renderPosition", Position.Type
                    "selected", T<bool>
                    "selectable", T<bool>
                    "locked", T<bool>
                    "grabbable", T<bool>
                    "classes", T<string>
                ]
        }

    let StyleConfig =
        Pattern.Config "StyleConfig" {
            Required =
                [
                    "selector", T<string>
                    "style", T<obj>
                ]
            Optional = []
        }

    let CytoscapeOptions =
        Pattern.Config "CytoscapeOptions" {
            Required = []
            Optional = 
                [
                    "container", T<JavaScript.Dom.Element>
                    "elements", !| ElementObject.Type
                    "style", !| StyleConfig.Type
                    "layout", T<obj>

                    "zoom", T<float>
                    "pan", Position.Type

                    "minZoom", T<float>
                    "maxZoom", T<float>
                    "zoomingEnabled", T<bool>
                    "userZoomingEnabled", T<bool>
                    "panningEnabled", T<bool>
                    "userPanningEnabled", T<bool>
                    "boxSelectionEnabled", T<bool>
                    "selectionType", T<string>
                    "touchTapThreshold", T<int>
                    "desktopTapThreshold", T<int>
                    "autolock", T<bool>
                    "autoungrabify", T<bool>
                    "autounselectify", T<bool>
                    
                    "headless", T<bool>
                    "styleEnabled", T<bool>
                    "hideEdgesOnViewport", T<bool>
                    "hideLabelsOnViewport", T<bool>
                    "textureOnViewport", T<bool>
                    "motionBlur", T<bool>
                    "motionBlurOpacity", T<float>
                    "wheelSensitivity", T<float>
                    "pixelRatio", T<string>
                ]
        }

    let ZoomOptions =
        Pattern.Config "ZoomOptions" {
            Required = []
            Optional =
                [
                    "level", T<float>
                    "position", Position.Type
                    "renderedPosition", Position.Type
                ]
        }

    let FitOptions =
        Pattern.Config "FitOptions" {
            Required = []
            Optional =
                [
                    "eles", !| ElementObject.Type + T<string>
                    "padding", T<string>
                ]
        }

    let AnimateOptions =
        Pattern.Config "AnimateOptions" {
            Required = []
            Optional =
                [
                    "zoom", T<float>
                    "pan", Position.Type
                    "panBy", Position.Type
                    "fit", FitOptions.Type
                    "center", !| ElementObject.Type + T<string>
                    "duration", T<int>
                    "queue", T<bool>
                    "complete", T<JavaScript.Function>
                    "step", T<JavaScript.Function>
                    "easing", T<string>
                ]
        }

    let LayoutOptions =
        Pattern.Config "LayoutOptions" {
            Required = []
            Optional = 
                [
                    "name", T<string>
                    "ready", T<JavaScript.Function>
                    "stop", T<JavaScript.Function>
                    "fit", T<bool>
                    "padding", T<int>
                    "boundingBox", T<obj>
                    "animate", T<bool>
                    "animationDuration", T<int>
                    "animationEasing", T<obj>
                    "positions", T<obj>
                    "zoom", ZoomOptions.Type
                    "pan", Position.Type
                    "avoidOverlap", T<bool>
                    "avoidOverlapPadding", T<int>
                    "nodeDimensionsIncludeLabels", T<bool>
                    "spacingFactor", T<float>
                    "condense", T<bool>
                    "rows", T<int>
                    "cols", T<int>
                    "position", Position.Type
                    "sort", T<JavaScript.Function>
                    "radius", T<float>
                    "startAngle", T<float>
                    "sweep", T<float>
                    "clockwise", T<bool>
                    "equidistant", T<bool>
                    "minNodeSpacing", T<int>
                    "height", T<int>
                    "width", T<int>
                    "concentric", T<JavaScript.Function>
                    "levelWidth", T<JavaScript.Function>
                    "directed", T<bool>
                    "roots", T<obj>
                    "maximalAdjustments", T<int>
                    "refresh", T<int>
                    "randomize", T<bool>
                    "componentSpacing", T<int>
                    "nodeRepulsion", T<JavaScript.Function>
                    "nodeOverlap", T<int>
                    "idealEdgeLength", T<JavaScript.Function>
                    "edgeElasticity", T<JavaScript.Function>
                    "nestingFactor", T<int>
                    "gravity", T<int>
                    "numIter", T<int>
                    "initialTemp", T<int>
                    "coolingFactor", T<float>
                    "minTemp", T<float>
                    "weaver", T<bool>
                ]
        }

    let LayoutClass =
        Class "Layout"
        |+> Instance [
            "start" => T<unit> ^-> TSelf
            "stop" => T<unit> ^-> TSelf

            //events
            "on" => (T<string> * T<JavaScript.Function> ^-> TSelf) + (T<string> * T<string> * T<JavaScript.Function> ^-> TSelf)
            "promiseOn" => T<string> * !? T<string> ^-> TSelf
            "one" => (T<string> * T<JavaScript.Function> ^-> TSelf) + (T<string> * T<string> * T<JavaScript.Function> ^-> TSelf)
            "off" => (T<string> * T<JavaScript.Function> ^-> TSelf) + (T<string> * T<string> * T<JavaScript.Function> ^-> TSelf)
            "trigger" => T<string> *+ T<obj> ^-> TSelf
        ]

    let ImageOptions =
        Pattern.Config "ImageOptions" {
            Required = []
            Optional =
                [
                    "output", T<string>
                    "bg", T<string>
                    "full", T<bool>
                    "scale", T<float>
                    "maxWidth", T<int>
                    "maxHeight", T<int>
                    "quality", T<float>
                ]
        }

    let Location =
        Pattern.Config "Location" {
            Required = []
            Optional =
                [
                    "source", T<string>
                    "target", T<string>
                    "parent", T<string>
                ]
        }

    let AlgorithmOptions =
        Pattern.Config "AlgorithmOptions" {
            Required = []
            Optional =
                [
                    "root", CytoscapeClass.Type
                    "goal", CytoscapeClass.Type
                    "weight", T<JavaScript.Function>
                    "heuristic", T<JavaScript.Function>
                    "visit", T<JavaScript.Function>
                    "directed", T<bool>
                    "dampingFactor", T<float>
                    "precision", T<float>
                    "iterations", T<float>
                    "harmonic", T<bool>
                ]
        }

    CytoscapeClass
        |+> Static [
            Constructor (T<unit> + CytoscapeOptions.Type)
        ]
        |+> Instance [
            //Graph manipulation
            "add" => ElementObject.Type + !| ElementObject.Type ^-> TSelf
            "remove" => ElementObject.Type + T<string> ^-> TSelf
            "collection" => T<unit> + T<string> + !| ElementObject.Type ^-> TSelf
            "getElementById" => T<string> ^-> TSelf
            "$" => T<string> ^-> TSelf
            "elements" => T<string> ^-> TSelf
            "nodes" => T<string> ^-> TSelf
            "edges" => T<string> ^-> TSelf
            "filter" => (T<string> + T<JavaScript.Function>) ^-> TSelf
            "batch" => T<JavaScript.Function> ^-> TSelf
            "startBatch" => T<unit> ^-> TSelf
            "endBatch" => T<unit> ^-> TSelf
            "destroy" => T<unit> ^-> TSelf
            "scratch" => T<unit> + T<string> + (T<string> * T<string>) ^-> TSelf
            "removeScratch" => T<string> ^-> TSelf

            //Events
            "on" => (T<string> * T<JavaScript.Function> ^-> TSelf) + (T<string> * T<string> * T<JavaScript.Function> ^-> TSelf)
            "promiseOn" => T<string> * !? T<string> ^-> TSelf
            "one" => (T<string> * T<JavaScript.Function> ^-> TSelf) + (T<string> * T<string> * T<JavaScript.Function> ^-> TSelf)
            "off" => (T<string> * T<JavaScript.Function> ^-> TSelf) + (T<string> * T<string> * T<JavaScript.Function> ^-> TSelf)
            "trigger" => T<string> *+ T<obj> ^-> TSelf
            "ready" => T<JavaScript.Function> ^-> TSelf

            //Viewport manipulation
            "container" => T<unit> ^-> T<JavaScript.Dom.Element>
            "center" => T<unit> + !| ElementObject.Type ^-> TSelf
            "fit" => !? !| ElementObject.Type * !? T<string> ^-> TSelf
            "reset" => T<unit> ^-> TSelf
            "pan" => T<unit> + Position.Type ^-> TSelf
            "panBy" => Position.Type ^-> TSelf
            "panningEnabled" => (T<unit> ^-> T<bool>) + (T<bool> ^-> TSelf)
            "userPanningEnabled" => (T<unit> ^-> T<bool>) + (T<bool> ^-> TSelf)
            "zoom" => T<unit> + T<string> + ZoomOptions.Type ^-> TSelf
            "zoomingEnabled" => (T<unit> ^-> T<bool>) + (T<bool> ^-> TSelf)
            "userZoomingEnabled" => (T<unit> ^-> T<bool>) + (T<bool> ^-> TSelf)
            "minZoom" => (T<unit> ^-> T<bool>) + (T<float> ^-> TSelf)
            "maxZoom" => (T<unit> ^-> T<bool>) + (T<float> ^-> TSelf)
            "viewport" => T<float> * Position.Type ^-> TSelf
            "boxSelectionEnabled" => (T<unit> ^-> T<bool>) + (T<bool> ^-> TSelf)
            "extent" => T<unit> ^-> TSelf
            "autolock" => (T<unit> ^-> T<bool>) + (T<bool> ^-> TSelf)
            "autoungrabify" => (T<unit> ^-> T<bool>) + (T<bool> ^-> TSelf)
            "autounselectify" => (T<unit> ^-> T<bool>) + (T<bool> ^-> TSelf)
            "forcerender" => T<unit> ^-> TSelf
            "resize" => T<unit> ^-> TSelf

            //Animation
            "animated" => T<unit> ^-> T<bool>
            "animate" => AnimateOptions.Type ^-> TSelf
            "animation" => AnimateOptions.Type ^-> TSelf
            "delay" => T<int> * T<JavaScript.Function> ^-> TSelf
            "delayAnimation" => T<int> ^-> TSelf
            "stop" => T<bool> * T<bool> ^-> TSelf
            "clearQueue" => T<unit> ^-> TSelf

            //Layout
            "layout" => LayoutOptions.Type ^-> LayoutClass

            //Style
            "style" => T<unit> ^-> T<JavaScript.CSSStyleDeclaration> + T<JavaScript.CSSStyleDeclaration> ^-> TSelf

            //Export
            "png" => ImageOptions.Type ^-> T<JavaScript.ImageData>
            "jpg" => ImageOptions.Type ^-> T<JavaScript.ImageData>
            "json" => T<unit> ^-> T<obj> + T<obj> ^-> TSelf

            //Collections
            //Graph manipulation
            "remove" => T<unit> ^-> TSelf
            "removed" => T<unit> ^-> T<bool>
            "inside" => T<unit> ^-> T<bool>
            "restore" => T<unit> ^-> TSelf
            "clone" => T<unit> ^-> TSelf
            "move" => Location.Type ^-> TSelf

            //Data
            "data" => T<string> + (T<string> + T<string>) + T<obj> ^-> TSelf
            "removeData" => T<unit> + T<string> ^-> TSelf
            "id" => T<unit> ^-> T<string>
            "jsons" => T<unit> ^-> !| T<obj>
            "group" => T<unit> ^-> !| T<string>
            "isNode" => T<unit> ^-> T<bool>
            "isEdge" => T<unit> ^-> T<bool>
            "isSimple" => T<unit> ^-> T<bool>

            //Metadata
            "degree" => T<bool> ^-> T<int>
            "indegree" => T<bool> ^-> T<int>
            "outdegree" => T<bool> ^-> T<int>
            "totalDegree" => T<bool> ^-> T<int>
            "minDegree" => T<bool> ^-> T<int>
            "maxDegree" => T<bool> ^-> T<int>
            "minIndegree" => T<bool> ^-> T<int>
            "maxIndegree" => T<bool> ^-> T<int>
            "minOutdegree" => T<bool> ^-> T<int>
            "maxOutdegree" => T<bool> ^-> T<int>

            //Positions and dimensions
            "position" => (T<unit> ^-> Position.Type) + (T<string> ^-> T<int>) + (T<string> * T<int> ^-> TSelf) + (Position.Type ^-> TSelf)
            "positions" => T<JavaScript.Function> ^-> TSelf + Position.Type ^-> TSelf
            "renderedPosition" => (T<unit> ^-> Position.Type) + (T<string> ^-> T<int>) + (T<string> * T<int> ^-> TSelf) + (Position.Type ^-> TSelf)
            "relativePosition" => (T<unit> ^-> Position.Type) + (T<string> ^-> T<int>) + (T<string> * T<int> ^-> TSelf) + (Position.Type ^-> TSelf)
            "width" => T<unit> ^-> T<int>
            "outerWidth" => T<unit> ^-> T<int>
            "renderedWidth" => T<unit> ^-> T<int>
            "renderedOuterWidth" => T<unit> ^-> T<int>
            "height" => T<unit> ^-> T<int>
            "outerHeight" => T<unit> ^-> T<int>
            "renderedHeight" => T<unit> ^-> T<int>
            "renderedOuterHeight" => T<unit> ^-> T<int>
            "grabbed" => T<unit> ^-> T<bool>
            "grabbable" => T<unit> ^-> T<bool>
            "grabify" => T<unit> ^-> TSelf
            "ungrabify" => T<unit> ^-> TSelf
            "locked" => T<unit> ^-> T<bool>
            "lock" => T<unit> ^-> TSelf
            "unlock" => T<unit> ^-> TSelf
            "active" => T<unit> ^-> T<bool>

            //Layout
            "layoutPositions" => LayoutClass * LayoutOptions.Type * T<JavaScript.Function> ^-> !| Position.Type
            "layoutDimensions" => LayoutOptions.Type ^-> !| Position.Type

            //Selection
            "selected" => T<unit> ^-> T<bool>
            "select" => T<unit> ^-> TSelf
            "unselect" => T<unit> ^-> TSelf
            "selectable" => T<unit> ^-> T<bool>
            "selectify" => T<unit> ^-> TSelf
            "unselectify" => T<unit> ^-> TSelf

            //Stlye
            "addClass" => T<string> ^-> TSelf
            "removeClass" => T<string> ^-> TSelf
            "toggleClass" => T<string> * !? T<bool> ^-> TSelf
            "classes" => !? T<string> ^-> TSelf
            "fleshClass" => T<string> * !? T<int> ^-> TSelf
            "hasClass" => T<string> ^-> T<bool>
            "style" => T<unit> ^-> T<JavaScript.CSSStyleDeclaration> + T<string> ^-> T<string> + (T<string> * T<string> ^-> TSelf) + T<obj> ^-> TSelf
            "removeStyle" => T<unit> ^-> TSelf + T<string> ^-> TSelf
            "renderedStyle" => T<unit> ^-> T<JavaScript.CSSStyleDeclaration> ^-> TSelf + T<string> ^-> T<string>
            "numericStyle" => T<string> ^-> T<string>
            "numericStyleUnits" => T<string> ^-> T<string>
            "visible" => T<unit> ^-> T<bool>
            "hidden" => T<unit> ^-> T<bool>
            "effectiveOpacity" => T<unit> ^-> T<float>
            "transparent" => T<unit> ^-> T<bool>

            //Comparison
            "same" => TSelf ^-> T<bool>
            "anySame" => TSelf ^-> T<bool>
            "contains" => TSelf ^-> T<bool>
            "allAreNeighbors" => TSelf ^-> T<bool>
            "is" => TSelf ^-> T<bool>
            "allAre" => TSelf ^-> T<bool>
            "some" => T<JavaScript.Function> ^-> T<bool>
            "every" => T<JavaScript.Function> ^-> T<bool>

            //Iteration
            "size" => T<unit> ^-> T<int>
            "empty" => T<unit> ^-> T<bool>
            "nonempty" => T<unit> ^-> T<bool>
            "forEach" => T<JavaScript.Function> * !? TSelf ^-> TSelf
            "eq" => T<int> ^-> TSelf
            "first" => T<unit> ^-> TSelf
            "last" => T<unit> ^-> TSelf
            "slice" => !? T<int> * !? T<int> ^-> TSelf

            //Building and filtering
            "union" => TSelf + T<string> ^-> TSelf
            "difference" => TSelf + T<string> ^-> TSelf
            "absoluteComplement" => T<unit> ^-> TSelf
            "intersection" => TSelf + T<string> ^-> TSelf
            "symmetricDifference" => TSelf + T<string> ^-> TSelf
            "diff" => TSelf + T<string> ^-> TSelf
            "sort" => T<JavaScript.Function> ^-> TSelf
            "map" => T<JavaScript.Function> ^-> TSelf
            "reduce" => T<JavaScript.Function> ^-> TSelf
            "min" => T<JavaScript.Function> ^-> TSelf
            "max" => T<JavaScript.Function> ^-> TSelf

            //Traversing
            "neighborhood" => !? T<string> ^-> TSelf
            "openNeighborhood" => !? T<string> ^-> TSelf
            "closedNeighborhood" => !? T<string> ^-> TSelf
            "componenets" => T<unit> ^-> TSelf
            "edgesWith" => TSelf + T<string> ^-> TSelf
            "edgesTo" => TSelf + T<string> ^-> TSelf
            "connectedNodes" => !? T<string> ^-> TSelf
            "connectedEdges" => !? T<string> ^-> TSelf
            "source" => !? T<string> ^-> TSelf
            "sources" => !? T<string> ^-> TSelf
            "target" => !? T<string> ^-> TSelf
            "targets" => !? T<string> ^-> TSelf
            "parallelEdges" => !? T<string> ^-> TSelf
            "codirectedEdges" => !? T<string> ^-> TSelf
            "roots" => !? T<string> ^-> TSelf
            "leaves" => !? T<string> ^-> TSelf
            "outgoers" => !? T<string> ^-> TSelf
            "successors" => !? T<string> ^-> TSelf
            "incomers" => !? T<string> ^-> TSelf
            "predecessors" => !? T<string> ^-> TSelf

            //Algorithms
            "breadthFirstSearch" => AlgorithmOptions.Type ^-> TSelf
            "depthFirstSearch" => AlgorithmOptions.Type ^-> TSelf
            "dijkstra" => AlgorithmOptions.Type ^-> TSelf
            "aStar" => AlgorithmOptions.Type ^-> TSelf
            "floydWarshall" => AlgorithmOptions.Type ^-> TSelf
            "bellmanFord" => AlgorithmOptions.Type ^-> TSelf
            "kruskal" => !? T<JavaScript.Function> ^-> TSelf
            "kragerStein" => T<unit> ^-> TSelf
            "pageRank" => AlgorithmOptions.Type ^-> TSelf
            "degreeCentrality" => T<unit> ^-> TSelf
            "degreeCentralityNormalized" => T<unit> ^-> TSelf
            "closenessCentrality" => AlgorithmOptions.Type ^-> TSelf
            "closenessCentralityNormalized" => AlgorithmOptions.Type ^-> TSelf
            "betweennessCentrality" => AlgorithmOptions.Type ^-> TSelf

            //Compound nodes
            "isParent" => T<unit> ^-> T<bool>
            "isChildless" => T<unit> ^-> T<bool>
            "isChild" => T<unit> ^-> T<bool>
            "isOrphan" => T<unit> ^-> T<bool>
            "parent" => !? T<string> ^-> TSelf
            "ancestors" => !? T<string> ^-> TSelf
            "commonAncestors" => !? T<string> ^-> TSelf
            "orphans" => !? T<string> ^-> TSelf
            "nonorphans" => !? T<string> ^-> TSelf
            "children" => !? T<string> ^-> TSelf
            "descendants" => !? T<string> ^-> TSelf
            "siblings" => !? T<string> ^-> TSelf
        ]
        |> ignore

    let Assembly =
        Assembly [
            Namespace "WebSharper.Cytoscape.Resources" [
                Resource "Js" "https://cdnjs.cloudflare.com/ajax/libs/cytoscape/3.1.3/cytoscape.min.js"
                |> AssemblyWide
            ]
            Namespace "WebSharper.Cytoscape" [
                CytoscapeClass
                Position
                ElementData
                ElementObject
                CytoscapeOptions
                ZoomOptions
                FitOptions
                AnimateOptions
                LayoutOptions
                LayoutClass
                ImageOptions
                Location
                AlgorithmOptions
                StyleConfig
            ]
        ]


[<Sealed>]
type Extension() =
    interface IExtension with
        member x.Assembly = Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()
