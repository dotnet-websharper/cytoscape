namespace WebSharper.Cytoscape.Extension

open WebSharper
open WebSharper.InterfaceGenerator

module Definition =
    let CytoscapeClass = Class "cytoscape"
    let ElesClass = Class "eles"
    let EleClass = Class "ele"
    let NodesClass = Class "nodes"
    let NodeClass = Class "node"
    let EdgesClass = Class "edges"
    let EdgeClass = Class "edge"

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
                    "weight", T<float>
                    "faveColor", T<string>
                    "faveShape", T<string>
                    "strength", T<float>
                    "name", T<string>
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
                    "root", NodeClass + T<string>
                    "goal", NodeClass + T<string>
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

    let BoundingBoxOptions =
        Pattern.Config "BoundingBoxOptions" {
            Required = []
            Optional =
                [
                    "includeNodes", T<bool>
                    "includeEdges", T<bool>
                    "includeLabels", T<bool>
                ]
        }

    let BFSClass =
        Class "BFS"
        |+> Instance [
            "path" =? ElesClass
            "found" =? NodeClass
        ]

    let DFSClass =
        Class "DFS"
        |+> Instance [
            "path" =? ElesClass
            "found" =? NodeClass
        ]

    let DijkstraClass =
        Class "Dijkstra"
        |+> Instance [
            "distanceTo" =? NodeClass + T<string> ^-> T<float>
            "pathTo" =? NodeClass + T<string> ^-> !| NodeClass
        ]

    let AStarClass =
        Class "AStar"
        |+> Instance [
            "found" =? T<bool>
            "distance" =? T<float>
            "path" =? ElesClass
        ]

    let FWClass =
        Class "FW"
        |+> Instance [
            "distanceTo" =? (NodeClass + T<string>) * (NodeClass + T<string>) ^-> T<float>
            "pathTo" =? (NodeClass + T<string>) * (NodeClass + T<string>) ^-> !| NodeClass
        ]

    let BFClass =
        Class "BF"
        |+> Instance [
            "distanceTo" =? NodeClass + T<string> ^-> T<float>
            "pathTo" =? NodeClass + T<string> ^-> !| NodeClass
            "hasNegativeWeightCycle" =? T<bool>
        ]

    let KragerSteinClass =
        Class "KragerStein"
        |+> Instance [
            "cut" =? EdgesClass
            "partition1" =? NodesClass
            "partition2" =? NodesClass
        ]

    let PageRankClass =
        Class "PageRank"
        |+> Instance [
            "rank" =? NodeClass + T<string> ^-> T<float>
        ]
       
    let DegreeCentralityClass =
        Class "DegreeCentrality"
        |+> Instance [
            "degree" =? T<float>
            "indegree" =? T<float>
            "outdegree" =? T<float>
        ]

    let DegreeCentralityNClass =
        Class "DegreeCentralityN"
        |+> Instance [
            "degree" =? NodeClass + T<string> ^-> T<float>
            "indegree" =? NodeClass + T<string> ^-> T<float>
            "outdegree" =? NodeClass + T<string> ^-> T<float>
        ]

    let BetweennessCentralityClass =
        Class "BetweennessCentrality"
        |+> Instance [
            "betweenness" =? NodeClass + T<string> ^-> T<float>
            "betweennessNormalized" =? NodeClass + T<string> ^-> T<float>
        ]

    let Box =
        Pattern.Config "Box" {
            Required = []
            Optional =
                [
                    "x1", T<float>
                    "y1", T<float>
                    "x2", T<float>
                    "y2", T<float>
                    "w", T<float>
                    "h", T<float>
                ]
        }

    EleClass
        |+> Instance [
            //Graph manipulation
            "removed" => T<unit> ^-> T<bool>
            "inside" => T<unit> ^-> T<bool>

            //Data
            "scratch" => T<string> + (T<string> * T<obj>) ^-> TSelf
            "removeScratch" => T<string> ^-> TSelf
            "id" => T<unit> ^-> T<string>
            "json" => T<unit> ^-> T<obj>
            "group" => T<unit> ^-> !| T<string>
            "isNode" => T<unit> ^-> T<bool>
            "isEdge" => T<unit> ^-> T<bool>

            //Positions and dimensions    
            "width" => T<unit> ^-> T<int>
            "outerWidth" => T<unit> ^-> T<int>
            "renderedWidth" => T<unit> ^-> T<int>
            "renderedOuterWidth" => T<unit> ^-> T<int>
            "height" => T<unit> ^-> T<int>
            "outerHeight" => T<unit> ^-> T<int>
            "renderedHeight" => T<unit> ^-> T<int>
            "renderedOuterHeight" => T<unit> ^-> T<int>
            "active" => T<unit> ^-> T<bool>
            
            //Selection
            "selected" => T<unit> ^-> T<bool>
            "selectable" => T<unit> ^-> T<bool>
            
            //Stlye
            "hasClass" => T<string> ^-> T<bool>
            "renderedStyle" => T<unit> ^-> T<obj> ^-> TSelf + T<string> ^-> T<string>
            "numericStyle" => T<string> ^-> T<string>
            "numericStyleUnits" => T<string> ^-> T<string>
            "visible" => T<unit> ^-> T<bool>
            "hidden" => T<unit> ^-> T<bool>
            "effectiveOpacity" => T<unit> ^-> T<float>
            "transparent" => T<unit> ^-> T<bool>
            
            //Animation
            "animated" => T<unit> ^-> T<bool>
            "animation" => AnimateOptions.Type ^-> TSelf
            "delayAnimation" => T<int> ^-> TSelf

        ]
        |> ignore

    ElesClass
        |=> Inherits EleClass
        |+> Instance [
            //Graph manipulation
            "remove" => T<unit> ^-> TSelf
            "restore" => T<unit> ^-> TSelf
            "clone" => T<unit> ^-> TSelf
            "move" => Location.Type ^-> TSelf

            //Events
            "on" => (T<string> * T<JavaScript.Function> ^-> TSelf) + (T<string> * T<string> * T<JavaScript.Function> ^-> TSelf)
            "promiseOn" => T<string> * !? T<string> ^-> TSelf
            "one" => (T<string> * T<JavaScript.Function> ^-> TSelf) + (T<string> * T<string> * T<JavaScript.Function> ^-> TSelf)
            "off" => (T<string> * T<JavaScript.Function> ^-> TSelf) + (T<string> * T<string> * T<JavaScript.Function> ^-> TSelf)
            "trigger" => T<string> *+ T<obj> ^-> TSelf
            "ready" => T<JavaScript.Function> ^-> TSelf
            
            //Data
            "data" => T<string> + (T<string> + T<string>) + T<obj> ^-> TSelf
            "removeData" => T<unit> + T<string> ^-> TSelf
            "jsons" => T<unit> ^-> !| T<obj>
            "isLoop" => T<unit> ^-> T<bool>
            "isSimple" => T<unit> ^-> T<bool>

            //Positions and dimensions    
            "boundingBox" => BoundingBoxOptions.Type ^-> Box.Type
            "renderedBoundingBox" => BoundingBoxOptions.Type ^-> Box.Type
            
            //Layout
            "layout" => LayoutOptions.Type ^-> LayoutClass

            //Selection
            "select" => T<unit> ^-> TSelf
            "unselect" => T<unit> ^-> TSelf
            "selectify" => T<unit> ^-> TSelf
            "unselectify" => T<unit> ^-> TSelf
            
            //Stlye
            "addClass" => T<string> ^-> TSelf
            "removeClass" => T<string> ^-> TSelf
            "toggleClass" => T<string> * !? T<bool> ^-> TSelf
            "classes" => !? T<string> ^-> TSelf
            "fleshClass" => T<string> * !? T<int> ^-> TSelf
            "style" => T<unit> ^-> T<obj> + T<string> ^-> T<string> + (T<string> * T<string> ^-> TSelf) + T<obj> ^-> TSelf        
            "removeStyle" => T<unit> ^-> TSelf + T<string> ^-> TSelf

            //Animation
            "animate" => AnimateOptions.Type ^-> TSelf
            "delay" => T<int> * T<JavaScript.Function> ^-> TSelf
            "stop" => T<bool> * T<bool> ^-> TSelf
            "clearQueue" => T<unit> ^-> TSelf

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
            
            //Algorithms
            "breadthFirstSearch" => AlgorithmOptions.Type ^-> BFSClass.Type
            "depthFirstSearch" => AlgorithmOptions.Type ^-> DFSClass.Type
            "dijkstra" => AlgorithmOptions.Type ^-> DijkstraClass.Type
            "aStar" => AlgorithmOptions.Type ^-> AStarClass.Type
            "floydWarshall" => AlgorithmOptions.Type ^-> FWClass.Type
            "bellmanFord" => AlgorithmOptions.Type ^-> BFClass.Type
            "kruskal" => !? T<JavaScript.Function> ^-> ElesClass
            "kragerStein" => T<unit> ^-> KragerSteinClass.Type
            "pageRank" => AlgorithmOptions.Type ^-> PageRankClass.Type
            "degreeCentrality" => AlgorithmOptions.Type ^-> DegreeCentralityClass.Type
            "degreeCentralityNormalized" => AlgorithmOptions.Type ^-> DegreeCentralityNClass.Type
            "closenessCentrality" => AlgorithmOptions.Type ^-> T<float>
            "closenessCentralityNormalized" => AlgorithmOptions.Type ^-> T<float>
            "betweennessCentrality" => AlgorithmOptions.Type ^-> BetweennessCentralityClass.Type
        ]
        |> ignore

    NodeClass
        |+> Instance [
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
            "renderedPosition" => (T<unit> ^-> Position.Type) + (T<string> ^-> T<int>) + (T<string> * T<int> ^-> TSelf) + (Position.Type ^-> TSelf)
            "relativePosition" => (T<unit> ^-> Position.Type) + (T<string> ^-> T<int>) + (T<string> * T<int> ^-> TSelf) + (Position.Type ^-> TSelf)
            "grabbed" => T<unit> ^-> T<bool>
            "grabbable" => T<unit> ^-> T<bool>
            "locked" => T<unit> ^-> T<bool>

            //Layout
            "layoutDimensions" => LayoutOptions.Type ^-> !| Position.Type

            //Compound nodes
            "isParent" => T<unit> ^-> T<bool>
            "isChildless" => T<unit> ^-> T<bool>
            "isChild" => T<unit> ^-> T<bool>
            "isOrphan" => T<unit> ^-> T<bool>
        ]
        |> ignore

    NodesClass
        |=> Inherits NodeClass
        |+> Instance [
            //Positions and dimensions
            "positions" => T<JavaScript.Function> ^-> TSelf + Position.Type ^-> TSelf
            "grabify" => T<unit> ^-> TSelf
            "ungrabify" => T<unit> ^-> TSelf
            "lock" => T<unit> ^-> TSelf
            "unlock" => T<unit> ^-> TSelf

            //Layout
            "layoutPositions" => LayoutClass * LayoutOptions.Type * T<JavaScript.Function> ^-> !| Position.Type
        
            //Traversing
            "edgesWith" => TSelf + T<string> ^-> TSelf
            "edgesTo" => TSelf + T<string> ^-> TSelf
            "connectedEdges" => !? T<string> ^-> TSelf
            "roots" => !? T<string> ^-> TSelf
            "leaves" => !? T<string> ^-> TSelf
            "outgoers" => !? T<string> ^-> TSelf
            "successors" => !? T<string> ^-> TSelf
            "incomers" => !? T<string> ^-> TSelf
            "predecessors" => !? T<string> ^-> TSelf

            //Compound nodes
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

    EdgeClass
        |+> Instance [
            //Traversing
            "source" => !? T<string> ^-> TSelf
            "target" => !? T<string> ^-> TSelf
        ]  
        |> ignore 

    EdgesClass
        |=> Inherits EdgeClass
        |+> Instance [
            //Traversing
            "connectedNodes" => !? T<string> ^-> TSelf
            "sources" => !? T<string> ^-> TSelf
            "targets" => !? T<string> ^-> TSelf
            "parallelEdges" => !? T<string> ^-> TSelf
            "codirectedEdges" => !? T<string> ^-> TSelf
        ] 
        |> ignore

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
            "of" => T<string>?str ^-> ElesClass
                |> WithInline "$($str)"
            "of" => T<unit> ^-> ElesClass
                |> WithInline "$()"
            "elements" => T<unit> + T<string> ^-> ElesClass
            "nodes" => T<unit> + T<string> ^-> NodesClass
            "edges" => T<unit> + T<string> ^-> EdgesClass
            "filter" => (T<string> + T<JavaScript.Function>) ^-> ElesClass
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
            "width" => T<unit> ^-> T<int>
            "height" => T<unit> ^-> T<int>
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
            "style" => T<unit> ^-> T<obj> + T<obj> ^-> TSelf

            //Export
            "png" => ImageOptions.Type ^-> T<JavaScript.ImageData>
            "jpg" => ImageOptions.Type ^-> T<JavaScript.ImageData>
            "json" => T<unit> ^-> T<obj> + T<obj> ^-> TSelf
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
                BoundingBoxOptions
                BFSClass
                DFSClass
                DijkstraClass
                AStarClass
                FWClass
                BFClass
                KragerSteinClass
                PageRankClass
                DegreeCentralityClass
                DegreeCentralityNClass
                BetweennessCentralityClass
                Box
                EleClass
                ElesClass
                NodeClass
                NodesClass
                EdgeClass
                EdgesClass
            ]
        ]


[<Sealed>]
type Extension() =
    interface IExtension with
        member x.Assembly = Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()
