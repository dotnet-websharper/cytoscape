// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2018 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}
namespace WebSharper.Cytoscape.Extension

open WebSharper
open WebSharper.JavaScript
open WebSharper.InterfaceGenerator

/// Binding for cytoscape.js, tracked against the declarations shipped with
/// the npm package (cytoscape@3.35.0, index.d.ts).
module Definition =

    // The library is imported from its npm package; there is no CDN <script>.
    let CytoscapeClass = Class "cytoscape" |> ImportDefault "cytoscape"
    let ElesClass = Class "eles"
    let EleClass = Class "ele"
    let NodesClass = Class "nodes"
    let NodeClass = Class "node"
    let EdgesClass = Class "edges"
    let EdgeClass = Class "edge"
    let LayoutClass = Class "Layout"
    let StyleClass = Class "Style"
    let AnimationClass = Class "Animation"
    let EventObjectClass = Class "EventObject"

    let ElementGroup =
        Pattern.EnumStrings "ElementGroup" [
            "nodes"
            "edges"
        ]

    let SelectionType =
        Pattern.EnumStrings "SelectionType" [
            "single"
            "additive"
        ]

    let PositionDimension =
        Pattern.EnumStrings "PositionDimension" [
            "x"
            "y"
        ]

    let LayoutDirection =
        Pattern.EnumStrings "LayoutDirection" [
            "downward"
            "upward"
            "rightward"
            "leftward"
        ]

    let ExportOutput =
        Pattern.EnumStrings "ExportOutput" [
            "base64uri"
            "base64"
            "blob"
            "blob-promise"
        ]

    let Position =
        Pattern.Config "Position" {
            Required =
                [
                    "x", T<float>
                    "y", T<float>
                ]
            Optional = []
        }

    /// A bounding box in model or rendered coordinates.
    let Box =
        Pattern.Config "Box" {
            Required =
                [
                    "x1", T<float>
                    "y1", T<float>
                    "x2", T<float>
                    "y2", T<float>
                    "w", T<float>
                    "h", T<float>
                ]
            Optional = []
        }

    /// The row and column assigned by the `grid` layout's `position` callback.
    let GridCell =
        Pattern.Config "GridCell" {
            Required =
                [
                    "row", T<int>
                    "col", T<int>
                ]
            Optional = []
        }

    /// The width and height of a node as computed for layout purposes.
    let Size =
        Pattern.Config "Size" {
            Required =
                [
                    "w", T<float>
                    "h", T<float>
                ]
            Optional = []
        }

    /// Developer-defined data attached to an element. The well-known fields are
    /// typed; any further JSON-serializable fields can be set dynamically on
    /// the F# value (it compiles to a plain JavaScript object).
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
                    "directed", T<bool>
                ]
        }

    /// An element definition (a node or an edge) as accepted by the
    /// constructor, `Cy.Add` and `ElementsDefinition`.
    let ElementObject =
        Pattern.Config "ElementObject" {
            Required = []
            Optional =
                [
                    "group", ElementGroup.Type
                    "data", ElementData.Type
                    "scratch", T<obj>
                    "position", Position.Type
                    "renderedPosition", Position.Type
                    "selected", T<bool>
                    "selectable", T<bool>
                    "locked", T<bool>
                    "grabbable", T<bool>
                    "pannable", T<bool>
                    "classes", !| T<string> + T<string>
                    "style", T<obj>
                    "css", T<obj>
                    "removed", T<bool>
                ]
        }

    /// The nodes and edges of a graph, as accepted by the constructor.
    let ElementsDefinition =
        Pattern.Config "ElementsDefinition" {
            Required =
                [
                    "nodes", !| ElementObject.Type
                    "edges", !| ElementObject.Type
                ]
            Optional = []
        }

    /// A stylesheet rule using the `style` key.
    let StyleConfig =
        Pattern.Config "StyleConfig" {
            Required =
                [
                    "selector", T<string>
                    "style", T<obj>
                ]
            Optional = []
        }

    /// A stylesheet rule using the `css` key (an alias of `style`).
    let CssConfig =
        Pattern.Config "CssConfig" {
            Required =
                [
                    "selector", T<string>
                    "css", T<obj>
                ]
            Optional = []
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
                    "eles", ElesClass + T<string>
                    "padding", T<float>
                ]
        }

    let CenterOptions =
        Pattern.Config "CenterOptions" {
            Required = []
            Optional =
                [
                    ("eles", ElesClass + T<string>)
                ]
        }

    /// The options of a viewport or element animation.
    let AnimateOptions =
        Pattern.Config "AnimateOptions" {
            Required = []
            Optional =
                [
                    "zoom", ZoomOptions.Type
                    "pan", Position.Type
                    "panBy", Position.Type
                    "fit", FitOptions.Type
                    "center", CenterOptions.Type
                    "style", T<obj>
                    "duration", T<int>
                    "queue", T<bool>
                    "complete", T<unit> ^-> T<unit>
                    "step", T<unit> ^-> T<unit>
                    "easing", T<string>
                    "position", Position.Type
                    "renderedPosition", Position.Type
                ]
        }

    /// Options shared by every built-in layout, plus the specific options of
    /// the `random`, `preset`, `grid`, `circle`, `concentric`, `breadthfirst`
    /// and `cose` layouts. Which fields apply is selected by `Name`.
    let LayoutOptions =
        Pattern.Config "LayoutOptions" {
            Required = []
            Optional =
                [
                    "name", T<string>
                    "ready", EventObjectClass.Type ^-> T<unit>
                    "stop", EventObjectClass.Type ^-> T<unit>
                    "transform", NodeClass * Position.Type ^-> Position.Type

                    // Common layout options
                    "fit", T<bool>
                    "padding", T<float>
                    "boundingBox", Box.Type
                    "animate", T<bool>
                    "animationDuration", T<int>
                    "animationEasing", T<string>
                    "animateFilter", NodeClass * T<int> ^-> T<bool>
                    "nodeDimensionsIncludeLabels", T<bool>
                    "spacingFactor", T<float>
                    "avoidOverlap", T<bool>
                    "avoidOverlapPadding", T<float>
                    "sort", NodeClass * NodeClass ^-> T<float>

                    // Random / preset
                    "positions", T<obj> + (T<string> ^-> Position.Type)
                    "zoom", T<float>
                    "pan", Position.Type

                    // Grid
                    "condense", T<bool>
                    "rows", T<int>
                    "cols", T<int>
                    "position", NodeClass ^-> GridCell.Type

                    // Circle / concentric
                    "radius", T<float>
                    "startAngle", T<float>
                    "sweep", T<float>
                    "clockwise", T<bool>
                    "equidistant", T<bool>
                    "minNodeSpacing", T<float>
                    "height", T<float>
                    "width", T<float>
                    "concentric", NodeClass ^-> T<float>
                    "levelWidth", NodesClass ^-> T<float>

                    // Breadthfirst
                    "directed", T<bool>
                    "direction", LayoutDirection.Type
                    "circle", T<bool>
                    "roots", !| T<string>
                    "maximalAdjustments", T<int>
                    "maximal", T<bool>
                    "grid", T<bool>
                    "depthSort", NodeClass * NodeClass ^-> T<float>

                    // Cose
                    "refresh", T<int>
                    "randomize", T<bool>
                    "componentSpacing", T<float>
                    "nodeRepulsion", T<float>
                    "nodeOverlap", T<float>
                    "idealEdgeLength", T<float>
                    "edgeElasticity", T<float>
                    "nestingFactor", T<float>
                    "gravity", T<float>
                    "numIter", T<int>
                    "initialTemp", T<float>
                    "coolingFactor", T<float>
                    "minTemp", T<float>
                    "weaver", T<bool>
                    "animationThreshold", T<int>
                ]
        }

    /// The options accepted by the `Cytoscape` constructor.
    let CytoscapeOptions =
        Pattern.Config "CytoscapeOptions" {
            Required = []
            Optional =
                [
                    // Commonly used options
                    "container", T<Dom.Element>
                    "elements", !| ElementObject.Type
                    "style", !| StyleConfig.Type
                    "layout", LayoutOptions.Type
                    "data", T<obj>

                    // Initial viewport state
                    "zoom", T<float>
                    "pan", Position.Type

                    // Interaction
                    "minZoom", T<float>
                    "maxZoom", T<float>
                    "zoomingEnabled", T<bool>
                    "userZoomingEnabled", T<bool>
                    "panningEnabled", T<bool>
                    "userPanningEnabled", T<bool>
                    "boxSelectionEnabled", T<bool>
                    "selectionType", SelectionType.Type
                    "touchTapThreshold", T<int>
                    "desktopTapThreshold", T<int>
                    "autolock", T<bool>
                    "autoungrabify", T<bool>
                    "autounselectify", T<bool>
                    "multiClickDebounceTime", T<int>

                    // Rendering
                    "headless", T<bool>
                    "styleEnabled", T<bool>
                    "hideEdgesOnViewport", T<bool>
                    "hideLabelsOnViewport", T<bool>
                    "textureOnViewport", T<bool>
                    "motionBlur", T<bool>
                    "motionBlurOpacity", T<float>
                    "wheelSensitivity", T<float>
                    "pixelRatio", T<float> + T<string>
                    "webgl", T<bool>
                    "webglDebug", T<bool>
                    "webglTexSize", T<int>
                    "webglTexRows", T<int>
                    "webglBatchSize", T<int>
                    "webglTexPerBatch", T<int>
                ]
        }

    /// The options of `Cy.Png`, `Cy.Jpg` and `Cy.Jpeg`.
    let ImageOptions =
        Pattern.Config "ImageOptions" {
            Required = []
            Optional =
                [
                    "bg", T<string>
                    "full", T<bool>
                    "scale", T<float>
                    "maxWidth", T<int>
                    "maxHeight", T<int>
                    "quality", T<float>
                    "output", ExportOutput.Type
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
                    "roots", NodesClass + T<string>
                    "goal", NodeClass + T<string>
                    "weight", EdgesClass ^-> T<float>
                    "heuristic", NodesClass ^-> T<float>
                    "visit", NodeClass * EdgeClass * NodeClass * T<int> * T<int> ^-> T<bool>
                    "directed", T<bool>
                    "findNegativeWeightCycles", T<bool>
                    "alpha", T<float>
                    "dampingFactor", T<float>
                    "precision", T<float>
                    "iterations", T<int>
                    "harmonic", T<bool>
                ]
        }

    /// Options for the clustering algorithms (`MarkovClustering`, `KMeans`,
    /// `KMedoids`, `FuzzyCMeans`, `HierarchicalClustering`,
    /// `AffinityPropagation`).
    let ClusteringOptions =
        Pattern.Config "ClusteringOptions" {
            Required = []
            Optional =
                [
                    "attributes", !| (NodeClass ^-> T<float>)
                    "k", T<int>
                    "distance", T<string> + (NodeClass * NodeClass ^-> T<float>)
                    "linkage", T<string>
                    "mode", T<string>
                    "threshold", T<float>
                    "dendrogramDepth", T<int>
                    "addDendrogram", T<bool>
                    "maxIterations", T<int>
                    "sensitivityThreshold", T<float>
                    "expandFactor", T<float>
                    "inflateFactor", T<float>
                    "multFactor", T<float>
                    "preference", T<string> + T<float>
                    "damping", T<float>
                    "minIterations", T<int>
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
                    "includeMainLabels", T<bool>
                    "includeSourceLabels", T<bool>
                    "includeTargetLabels", T<bool>
                    "includeOverlays", T<bool>
                    "includeUnderlays", T<bool>
                ]
        }

    let HitTestOptions =
        Pattern.Config "HitTestOptions" {
            Required = []
            Optional =
                [
                    "includeBody", T<bool>
                    "includeMainLabels", T<bool>
                    "includeSourceLabels", T<bool>
                    "includeTargetLabels", T<bool>
                    "isTouch", T<bool>
                ]
        }

    let LayoutPositionOptions =
        Pattern.Config "LayoutPositionOptions" {
            Required = []
            Optional =
                [
                    "eles", ElesClass.Type
                    "animate", T<bool>
                    "animationDuration", T<int>
                    "animationEasing", T<string>
                    "fit", T<bool>
                    "padding", T<float>
                    "pan", Position.Type
                    "spacingFactor", T<float>
                    "zoom", T<float>
                ]
        }

    let LayoutDimensionOptions =
        Pattern.Config "LayoutDimensionOptions" {
            Required = []
            Optional =
                [
                    ("nodeDimensionsIncludeLabels", T<bool>)
                ]
        }

    /// The model position of a node, keyed by node id (for the `preset` layout).
    let NodePositionMap =
        Class "NodePositionMap"
        |+> Instance [
            "" =? Position.Type |> Indexed T<string>
        ]

    let ViewportOptions =
        Pattern.Config "ViewportOptions" {
            Required =
                [
                    "zoom", T<float>
                    "pan", Position.Type
                ]
            Optional = []
        }

    let DiffResult =
        Pattern.Config "DiffResult" {
            Required =
                [
                    "left", ElesClass.Type
                    "right", ElesClass.Type
                    "both", ElesClass.Type
                ]
            Optional = []
        }

    let MinMaxResult =
        Pattern.Config "MinMaxResult" {
            Required =
                [
                    "value", T<float>
                    "ele", EleClass.Type
                ]
            Optional = []
        }

    // ------------------------------------------------------------------
    // Event objects, layouts, styles and animations
    // ------------------------------------------------------------------

    // The event object handed to event handlers (`event.position`,
    // `event.renderedPosition`, `event.target`, …).
    EventObjectClass
        |+> Instance [
            "cy" =? CytoscapeClass
            "target" =? T<obj>
            "type" =? T<string>
            "namespace" =? T<string>
            "timeStamp" =? T<float>
            "position" =? Position.Type
            "renderedPosition" =? Position.Type
            "originalEvent" =? T<Dom.Event>
            "layout" =? LayoutClass

            "preventDefault" => T<unit> ^-> T<unit>
                |> WithComment "Prevents the default action of the event."
            "stopPropagation" => T<unit> ^-> T<unit>
                |> WithComment "Stops the propagation of the event."
            "stopImmediatePropagation" => T<unit> ^-> T<unit>
            "isDefaultPrevented" => T<unit> ^-> T<bool>
            "isPropagationStopped" => T<unit> ^-> T<bool>
            "isImmediatePropagationStopped" => T<unit> ^-> T<bool>
        ]
        |> ignore

    // A layout object, as returned by `Cy.Layout`, `Eles.Layout` and
    // `Cy.MakeLayout`.
    LayoutClass
        |+> Instance [
            "run" => T<unit> ^-> TSelf
                |> WithComment "Runs the layout."
            "start" => T<unit> ^-> TSelf
                |> WithComment "Alias of `run`."
            "stop" => T<unit> ^-> TSelf
                |> WithComment "Stops a running (discrete/asynchronous) layout."

            // Events
            "on" => T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "on" => T<string> * T<obj> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "bind" => T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "bind" => T<string> * T<obj> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "listen" => T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "listen" => T<string> * T<obj> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "addListener" => T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "addListener" => T<string> * T<obj> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "promiseOn" => T<string> ^-> T<Promise<_>>[EventObjectClass]
            "pon" => T<string> ^-> T<Promise<_>>[EventObjectClass]
            "one" => T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "one" => T<string> * T<obj> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "off" => !?T<string>?events * !?(EventObjectClass.Type ^-> T<unit>)?handler ^-> TSelf
            "unbind" => !?T<string>?events * !?(EventObjectClass.Type ^-> T<unit>)?handler ^-> TSelf
            "unlisten" => !?T<string>?events * !?(EventObjectClass.Type ^-> T<unit>)?handler ^-> TSelf
            "removeListener" => !?T<string>?events * !?(EventObjectClass.Type ^-> T<unit>)?handler ^-> TSelf
            "removeAllListeners" => T<unit> ^-> TSelf
            "trigger" => T<string>?events *+ T<obj> ^-> TSelf
        ]
        |> ignore

    // The style object returned by `Cy.Style`.
    StyleClass
        |+> Instance [
            "append" => (StyleConfig.Type + !| StyleConfig.Type + T<string>) ^-> TSelf
                |> WithComment "Adds a rule to the stylesheet."
            "clear" => T<unit> ^-> TSelf
                |> WithComment "Removes all styles, including the default styles."
            "fromJson" => T<obj> ^-> TSelf
                |> WithComment "Sets the style from a JSON array of rules."
            "fromString" => T<string> ^-> TSelf
                |> WithComment "Sets the style from a CSS-like string."
            "resetToDefault" => T<unit> ^-> TSelf
                |> WithComment "Resets the style to the default stylesheet."
            "selector" => T<string>?s ^-> TSelf
                |> WithComment "Sets the selector context for the following style calls."
            "style" => T<string>?key * (T<string> + T<float>)?value ^-> TSelf
            "style" => T<obj>?css ^-> TSelf
            "update" => T<unit> ^-> TSelf
                |> WithComment "Makes the changes active."
        ]
        |> ignore

    // A single-element animation handle, as returned by `Cy.Animation`,
    // `Cy.DelayAnimation`, `Ele.Animation` and `Ele.DelayAnimation`.
    AnimationClass
        |+> Instance [
            "play" => T<unit> ^-> TSelf
            "run" => T<unit> ^-> TSelf
            "playing" => T<unit> ^-> T<bool>
            "running" => T<unit> ^-> T<bool>
            "progress" => T<unit> ^-> T<float>
            "progress" => T<float>?progress ^-> TSelf
            "time" => T<unit> ^-> T<float>
            "time" => T<float>?time ^-> TSelf
            "rewind" => T<unit> ^-> TSelf
            "fastforward" => T<unit> ^-> TSelf
            "pause" => T<unit> ^-> TSelf
            "stop" => T<unit> ^-> TSelf
            "completed" => T<unit> ^-> TSelf
            "complete" => T<unit> ^-> TSelf
            "apply" => T<unit> ^-> TSelf
            "applying" => T<unit> ^-> TSelf
            "reverse" => T<unit> ^-> TSelf
            "promise" => !?T<string>?animationEvent ^-> T<Promise<_>>[EventObjectClass]
                |> WithComment "The event name is one of `completed`, `complete` or `frame`."
        ]
        |> ignore

    // ------------------------------------------------------------------
    // Algorithm result types
    // ------------------------------------------------------------------

    let BFSClass =
        Class "BFS"
        |+> Instance [
            "path" =? ElesClass
            "found" =? NodesClass
        ]

    let DFSClass =
        Class "DFS"
        |+> Instance [
            "path" =? ElesClass
            "found" =? NodesClass
        ]

    let DijkstraClass =
        Class "Dijkstra"
        |+> Instance [
            "distanceTo" => NodeClass ^-> T<float>
            "pathTo" => NodeClass ^-> ElesClass
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
            "distance" => NodeClass * NodeClass ^-> T<float>
            "path" => NodeClass * NodeClass ^-> ElesClass
        ]

    let BFClass =
        Class "BF"
        |+> Instance [
            "distanceTo" => NodeClass ^-> T<float>
            "pathTo" => NodeClass ^-> ElesClass
            "hasNegativeWeightCycle" =? T<bool>
            "negativeWeightCycles" =? !| ElesClass
        ]

    let HierholzerClass =
        Class "Hierholzer"
        |+> Instance [
            "found" =? T<bool>
            "trail" =? ElesClass
        ]

    let KargerSteinClass =
        Class "KargerStein"
        |+> Instance [
            "cut" =? EdgesClass
            "components" =? ElesClass
            "partition1" =? NodesClass
            "partition2" =? NodesClass
        ]

    let HopcroftTarjanBiconnectedClass =
        Class "HopcroftTarjanBiconnected"
        |+> Instance [
            "cut" =? NodesClass
            "components" =? ElesClass
        ]

    let TarjanStronglyConnectedClass =
        Class "TarjanStronglyConnected"
        |+> Instance [
            "cut" =? EdgesClass
            "components" =? ElesClass
        ]

    let PageRankClass =
        Class "PageRank"
        |+> Instance [
            "rank" => NodeClass ^-> T<float>
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
            "degree" => NodeClass ^-> T<float>
            "indegree" => NodeClass ^-> T<float>
            "outdegree" => NodeClass ^-> T<float>
        ]

    let ClosenessCentralityNClass =
        Class "ClosenessCentralityN"
        |+> Instance [
            "closeness" => NodeClass ^-> T<float>
        ]

    let BetweennessCentralityClass =
        Class "BetweennessCentrality"
        |+> Instance [
            "betweenness" => NodeClass ^-> T<float>
            "betweennessNormalized" => NodeClass ^-> T<float>
            "betweennessNormalised" => NodeClass ^-> T<float>
        ]

    let FuzzyCMeansResult =
        Class "FuzzyCMeansResult"
        |+> Instance [
            "clusters" =? !| NodesClass
            "degreeOfMembership" =? !| !| T<float>
        ]

    // ------------------------------------------------------------------
    // Collections (eles / ele / nodes / node / edges / edge)
    // ------------------------------------------------------------------

    ElesClass
        |+> Instance [
            // Graph manipulation
            "remove" => T<unit> ^-> TSelf
            "restore" => T<unit> ^-> TSelf
            "clone" => T<unit> ^-> TSelf
            "copy" => T<unit> ^-> TSelf
            "move" => Location.Type ^-> TSelf

            // Data
            "data" => !?T<string>?name ^-> T<obj>
            "data" => T<string>?name * T<obj>?value ^-> TSelf
            "data" => T<obj>?obj ^-> TSelf
            "attr" => !?T<string>?name ^-> T<obj>
            "attr" => T<string>?name * T<obj>?value ^-> TSelf
            "attr" => T<obj>?obj ^-> TSelf
            "removeData" => T<unit> ^-> TSelf
            "removeData" => T<string>?names *+ T<string> ^-> TSelf
            "removeAttr" => T<unit> ^-> TSelf
            "removeAttr" => T<string>?names *+ T<string> ^-> TSelf
            "jsons" => T<unit> ^-> !| T<obj>

            // Events
            "on" => T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "on" => T<string> * T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "on" => T<string> * T<string> * T<obj> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "bind" => T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "bind" => T<string> * T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "bind" => T<string> * T<string> * T<obj> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "listen" => T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "listen" => T<string> * T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "listen" => T<string> * T<string> * T<obj> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "addListener" => T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "addListener" => T<string> * T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "addListener" => T<string> * T<string> * T<obj> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "promiseOn" => T<string> * !?T<string>?selector ^-> T<Promise<_>>[EventObjectClass]
            "pon" => T<string> * !?T<string>?selector ^-> T<Promise<_>>[EventObjectClass]
            "one" => T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "one" => T<string> * T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "one" => T<string> * T<string> * T<obj> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "once" => T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "once" => T<string> * T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "once" => T<string> * T<string> * T<obj> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "off" => !?T<string>?events * !?T<string>?selector * !?(EventObjectClass.Type ^-> T<unit>)?handler ^-> TSelf
            "unbind" => !?T<string>?events * !?T<string>?selector * !?(EventObjectClass.Type ^-> T<unit>)?handler ^-> TSelf
            "unlisten" => !?T<string>?events * !?T<string>?selector * !?(EventObjectClass.Type ^-> T<unit>)?handler ^-> TSelf
            "removeListener" => !?T<string>?events * !?T<string>?selector * !?(EventObjectClass.Type ^-> T<unit>)?handler ^-> TSelf
            "removeAllListeners" => T<unit> ^-> TSelf
            "trigger" => T<string>?events *+ T<obj> ^-> TSelf
            "emit" => T<string>?events *+ T<obj> ^-> TSelf

            // Positions and dimensions
            "boundingBox" => !?BoundingBoxOptions?options ^-> Box.Type
            "boundingbox" => !?BoundingBoxOptions?options ^-> Box.Type
            "renderedBoundingBox" => !?BoundingBoxOptions?options ^-> Box.Type
            "renderedBoundingbox" => !?BoundingBoxOptions?options ^-> Box.Type

            // Layout
            "layout" => LayoutOptions.Type ^-> LayoutClass
            "makeLayout" => LayoutOptions.Type ^-> LayoutClass
            "createLayout" => LayoutOptions.Type ^-> LayoutClass

            // Selection
            "select" => T<unit> ^-> TSelf
            "unselect" => T<unit> ^-> TSelf
            "deselect" => T<unit> ^-> TSelf
            "selectify" => T<unit> ^-> TSelf
            "unselectify" => T<unit> ^-> TSelf

            // Style
            "addClass" => T<string>?classes ^-> TSelf
            "removeClass" => T<string>?classes ^-> TSelf
            "toggleClass" => T<string>?classes * !?T<bool>?toggle ^-> TSelf
            "classes" => T<unit> ^-> !| T<string>
            "classes" => T<string>?classes ^-> TSelf
            "flashClass" => T<string>?classes * !?T<int>?duration ^-> TSelf
            "style" => T<unit> ^-> T<obj>
            "style" => T<string>?name ^-> T<obj>
            "style" => T<string>?name * T<obj>?value ^-> TSelf
            "style" => T<obj>?obj ^-> TSelf
            "css" => T<unit> ^-> T<obj>
            "css" => T<string>?name ^-> T<obj>
            "css" => T<string>?name * T<obj>?value ^-> TSelf
            "css" => T<obj>?obj ^-> TSelf
            "removeStyle" => !?T<string>?names ^-> TSelf

            // Animation
            "animate" => AnimateOptions.Type * !?AnimateOptions?options ^-> TSelf
            "delay" => T<int>?duration * !?(T<unit> ^-> T<unit>)?complete ^-> TSelf
            "stop" => !?T<bool>?clearQueue * !?T<bool>?jumpToEnd ^-> TSelf
            "clearQueue" => T<unit> ^-> TSelf

            // Comparison
            "same" => ElesClass ^-> T<bool>
            "anySame" => ElesClass ^-> T<bool>
            "contains" => ElesClass ^-> T<bool>
            "has" => ElesClass ^-> T<bool>
            "allAreNeighbors" => ElesClass ^-> T<bool>
            "allAreNeighbours" => ElesClass ^-> T<bool>
            "is" => T<string> ^-> T<bool>
            "allAre" => T<string> ^-> T<bool>
            "some" => (EleClass * T<int> * ElesClass ^-> T<bool>) * !?T<obj>?thisArg ^-> T<bool>
            "every" => (EleClass * T<int> * ElesClass ^-> T<bool>) * !?T<obj>?thisArg ^-> T<bool>

            // Iteration
            "size" => T<unit> ^-> T<int>
            "empty" => T<unit> ^-> T<bool>
            "nonempty" => T<unit> ^-> T<bool>
            "each" => (EleClass * T<int> * ElesClass ^-> T<unit>) * !?T<obj>?thisArg ^-> TSelf
            "forEach" => (EleClass * T<int> * ElesClass ^-> T<unit>) * !?T<obj>?thisArg ^-> TSelf
            "eq" => T<int>?index ^-> EleClass
            "first" => T<unit> ^-> EleClass
            "last" => T<unit> ^-> EleClass
            "slice" => !?T<int>?start * !?T<int>?``end`` ^-> TSelf
            "toArray" => T<unit> ^-> !| EleClass

            // Building and filtering
            "getElementById" => T<string>?id ^-> EleClass
            "union" => ElesClass + T<string> ^-> TSelf
            "difference" => ElesClass + T<string> ^-> TSelf
            "absoluteComplement" => T<unit> ^-> TSelf
            "abscomp" => T<unit> ^-> TSelf
            "complement" => T<unit> ^-> TSelf
            "intersection" => ElesClass + T<string> ^-> TSelf
            "symmetricDifference" => ElesClass + T<string> ^-> TSelf
            "diff" => ElesClass + T<string> ^-> DiffResult.Type
            "merge" => ElesClass + T<string> ^-> TSelf
            "unmerge" => ElesClass + T<string> ^-> TSelf
            "filter" => (T<string> + (EleClass * T<int> * ElesClass ^-> T<bool>)) ^-> TSelf
            "nodes" => !?T<string>?selector ^-> NodesClass
            "edges" => !?T<string>?selector ^-> EdgesClass
            "sort" => (EleClass * EleClass ^-> T<float>) ^-> TSelf
            // `map`/`reduce` are generic in the JavaScript API; the untyped value
            // type is the best WebSharper can express here.
            "map" => (EleClass * T<int> * ElesClass ^-> T<obj>) * !?T<obj>?thisArg ^-> !| T<obj>
            "reduce" => (T<obj> * EleClass * T<int> * ElesClass ^-> T<obj>) * T<obj>?initialValue ^-> T<obj>
            "min" => (EleClass * T<int> * ElesClass ^-> T<float>) * !?T<obj>?thisArg ^-> MinMaxResult.Type
            "max" => (EleClass * T<int> * ElesClass ^-> T<float>) * !?T<obj>?thisArg ^-> MinMaxResult.Type
            "withinBox" => Box.Type ^-> TSelf
            "polygonIntersection" => !| Position.Type ^-> TSelf
            "hit" => Position.Type * !?HitTestOptions?options ^-> TSelf

            // Traversing
            "neighborhood" => !?T<string>?selector ^-> TSelf
            "openNeighborhood" => !?T<string>?selector ^-> TSelf
            "closedNeighborhood" => !?T<string>?selector ^-> TSelf
            "components" => T<unit> ^-> !| ElesClass
            "componentsOf" => T<string>?root ^-> !| ElesClass

            // Algorithms
            "breadthFirstSearch" => AlgorithmOptions.Type ^-> BFSClass
            "bfs" => AlgorithmOptions.Type ^-> BFSClass
            "depthFirstSearch" => AlgorithmOptions.Type ^-> DFSClass
            "dfs" => AlgorithmOptions.Type ^-> DFSClass
            "dijkstra" => AlgorithmOptions.Type ^-> DijkstraClass
            "aStar" => AlgorithmOptions.Type ^-> AStarClass
            "floydWarshall" => AlgorithmOptions.Type ^-> FWClass
            "bellmanFord" => AlgorithmOptions.Type ^-> BFClass
            "hierholzer" => AlgorithmOptions.Type ^-> HierholzerClass
            "kruskal" => (EdgesClass ^-> T<float>) ^-> TSelf
            "kargerStein" => T<unit> ^-> KargerSteinClass
            "hopcroftTarjanBiconnected" => T<unit> ^-> HopcroftTarjanBiconnectedClass
            "hopcroftTarjanBiconnectedComponents" => T<unit> ^-> HopcroftTarjanBiconnectedClass
            "htb" => T<unit> ^-> HopcroftTarjanBiconnectedClass
            "htbc" => T<unit> ^-> HopcroftTarjanBiconnectedClass
            "tarjanStronglyConnected" => T<unit> ^-> TarjanStronglyConnectedClass
            "tarjanStronglyConnectedComponents" => T<unit> ^-> TarjanStronglyConnectedClass
            "tsc" => T<unit> ^-> TarjanStronglyConnectedClass
            "tscc" => T<unit> ^-> TarjanStronglyConnectedClass
            "degreeCentrality" => AlgorithmOptions.Type ^-> DegreeCentralityClass
            "degreeCentralityNormalized" => AlgorithmOptions.Type ^-> DegreeCentralityNClass
            "closenessCentrality" => AlgorithmOptions.Type ^-> T<float>
            "closenessCentralityNormalized" => AlgorithmOptions.Type ^-> ClosenessCentralityNClass
            "betweennessCentrality" => AlgorithmOptions.Type ^-> BetweennessCentralityClass
            "pageRank" => AlgorithmOptions.Type ^-> PageRankClass

            // Clustering
            "markovClustering" => ClusteringOptions.Type ^-> !| NodesClass
            "kMeans" => ClusteringOptions.Type ^-> !| NodesClass
            "kMedoids" => ClusteringOptions.Type ^-> !| NodesClass
            "affinityPropagation" => ClusteringOptions.Type ^-> !| NodesClass
            "hierarchicalClustering" => ClusteringOptions.Type ^-> !| NodesClass
            "fuzzyCMeans" => ClusteringOptions.Type ^-> FuzzyCMeansResult
        ]
        |> ignore

    EleClass
        |=> Inherits ElesClass
        |+> Instance [
            // Graph manipulation
            "cy" => T<unit> ^-> CytoscapeClass
            "removed" => T<unit> ^-> T<bool>
            "inside" => T<unit> ^-> T<bool>

            // Data
            "scratch" => !?T<string>?ns ^-> T<obj>
            "scratch" => T<string>?ns * T<obj>?value ^-> TSelf
            "removeScratch" => T<string>?ns ^-> TSelf
            "id" => T<unit> ^-> T<string>
            "json" => T<unit> ^-> T<obj>
            "json" => T<obj>?obj ^-> T<obj>
            "group" => T<unit> ^-> ElementGroup.Type
            "isNode" => T<unit> ^-> T<bool>
            "isEdge" => T<unit> ^-> T<bool>
            "component" => T<unit> ^-> ElesClass

            // Positions and dimensions
            "width" => T<unit> ^-> T<float>
            "outerWidth" => T<unit> ^-> T<float>
            "renderedWidth" => T<unit> ^-> T<float>
            "renderedOuterWidth" => T<unit> ^-> T<float>
            "height" => T<unit> ^-> T<float>
            "outerHeight" => T<unit> ^-> T<float>
            "renderedHeight" => T<unit> ^-> T<float>
            "renderedOuterHeight" => T<unit> ^-> T<float>
            "active" => T<unit> ^-> T<bool>

            // Selection
            "selected" => T<unit> ^-> T<bool>
            "selectable" => T<unit> ^-> T<bool>

            // Style
            "hasClass" => T<string>?className ^-> T<bool>
            "renderedStyle" => T<unit> ^-> T<obj>
            "renderedStyle" => T<string>?name ^-> T<obj>
            "renderedCss" => T<unit> ^-> T<obj>
            "renderedCss" => T<string>?name ^-> T<obj>
            "numericStyle" => T<string>?name ^-> T<obj>
            "numericStyleUnits" => T<string>?name ^-> T<obj>
            "visible" => T<unit> ^-> T<bool>
            "hidden" => T<unit> ^-> T<bool>
            "effectiveOpacity" => T<unit> ^-> T<float>
            "transparent" => T<unit> ^-> T<bool>

            // Animation
            "animated" => T<unit> ^-> T<bool>
            "animation" => AnimateOptions.Type ^-> AnimationClass
            "delayAnimation" => T<int>?duration ^-> AnimationClass
        ]
        |> ignore

    NodeClass
        |=> Inherits EleClass
        |+> Instance [
            // Metadata
            "degree" => !?T<bool>?includeLoops ^-> T<int>
            "indegree" => !?T<bool>?includeLoops ^-> T<int>
            "outdegree" => !?T<bool>?includeLoops ^-> T<int>

            // Positions and dimensions
            "position" => T<unit> ^-> Position.Type
            "position" => PositionDimension.Type ^-> T<float>
            "position" => PositionDimension.Type * T<float>?value ^-> TSelf
            "position" => Position.Type ^-> TSelf
            "modelPosition" => T<unit> ^-> Position.Type
            "modelPosition" => PositionDimension.Type ^-> T<float>
            "modelPosition" => PositionDimension.Type * T<float>?value ^-> TSelf
            "modelPosition" => Position.Type ^-> TSelf
            "point" => T<unit> ^-> Position.Type
            "point" => PositionDimension.Type ^-> T<float>
            "point" => PositionDimension.Type * T<float>?value ^-> TSelf
            "point" => Position.Type ^-> TSelf
            "renderedPosition" => T<unit> ^-> Position.Type
            "renderedPosition" => PositionDimension.Type * T<float>?value ^-> TSelf
            "renderedPosition" => Position.Type ^-> TSelf
            "renderedPoint" => T<unit> ^-> Position.Type
            "renderedPoint" => PositionDimension.Type * T<float>?value ^-> TSelf
            "renderedPoint" => Position.Type ^-> TSelf
            "relativePosition" => T<unit> ^-> Position.Type
            "relativePosition" => PositionDimension.Type * T<float>?value ^-> TSelf
            "relativePosition" => Position.Type ^-> TSelf
            "relativePoint" => T<unit> ^-> Position.Type
            "relativePoint" => PositionDimension.Type * T<float>?value ^-> TSelf
            "relativePoint" => Position.Type ^-> TSelf
            "grabbed" => T<unit> ^-> T<bool>
            "grabbable" => T<unit> ^-> T<bool>
            "locked" => T<unit> ^-> T<bool>

            // Layout
            "layoutDimensions" => LayoutDimensionOptions.Type ^-> Size.Type

            // Compound nodes
            "isParent" => T<unit> ^-> T<bool>
            "isChildless" => T<unit> ^-> T<bool>
            "isChild" => T<unit> ^-> T<bool>
            "isOrphan" => T<unit> ^-> T<bool>
        ]
        |> ignore

    NodesClass
        |=> Inherits NodeClass
        |+> Instance [
            // Metadata
            "totalDegree" => !?T<bool>?includeLoops ^-> T<int>
            "minDegree" => !?T<bool>?includeLoops ^-> T<int>
            "maxDegree" => !?T<bool>?includeLoops ^-> T<int>
            "minIndegree" => !?T<bool>?includeLoops ^-> T<int>
            "maxIndegree" => !?T<bool>?includeLoops ^-> T<int>
            "minOutdegree" => !?T<bool>?includeLoops ^-> T<int>
            "maxOutdegree" => !?T<bool>?includeLoops ^-> T<int>

            // Positions and dimensions
            "shift" => PositionDimension.Type * !?T<float>?value ^-> TSelf
            "shift" => Position.Type ^-> TSelf
            "positions" => (NodeClass * T<int> ^-> Position.Type) + Position.Type ^-> TSelf
            "modelPositions" => (NodeClass * T<int> ^-> Position.Type) + Position.Type ^-> TSelf
            "points" => (NodeClass * T<int> ^-> Position.Type) + Position.Type ^-> TSelf
            "grabify" => T<unit> ^-> TSelf
            "ungrabify" => T<unit> ^-> TSelf
            "lock" => T<unit> ^-> TSelf
            "unlock" => T<unit> ^-> TSelf
            "pannable" => T<unit> ^-> T<bool>
            "panify" => T<unit> ^-> TSelf
            "unpanify" => T<unit> ^-> TSelf

            // Layout
            "layoutPositions" => LayoutClass?layout * LayoutPositionOptions.Type * (NodeClass * T<int> ^-> Position.Type)?handler ^-> T<unit>

            // Traversing
            "edgesWith" => ElesClass + T<string> ^-> EdgesClass
            "edgesTo" => ElesClass + T<string> ^-> EdgesClass
            "connectedEdges" => !?T<string>?selector ^-> EdgesClass
            "roots" => !?T<string>?selector ^-> TSelf
            "leaves" => !?T<string>?selector ^-> TSelf
            "outgoers" => !?T<string>?selector ^-> TSelf
            "successors" => !?T<string>?selector ^-> TSelf
            "incomers" => !?T<string>?selector ^-> TSelf
            "predecessors" => !?T<string>?selector ^-> TSelf

            // Compound nodes
            "parent" => !?T<string>?selector ^-> TSelf
            "ancestors" => !?T<string>?selector ^-> TSelf
            "parents" => !?T<string>?selector ^-> TSelf
            "commonAncestors" => !?T<string>?selector ^-> TSelf
            "orphans" => !?T<string>?selector ^-> TSelf
            "nonorphans" => !?T<string>?selector ^-> TSelf
            "children" => !?T<string>?selector ^-> TSelf
            "descendants" => !?T<string>?selector ^-> TSelf
            "siblings" => !?T<string>?selector ^-> TSelf
        ]
        |> ignore

    EdgeClass
        |=> Inherits EleClass
        |+> Instance [
            // Data
            "isLoop" => T<unit> ^-> T<bool>
            "isSimple" => T<unit> ^-> T<bool>

            // Traversing
            "source" => !?T<string>?selector ^-> NodesClass
            "target" => !?T<string>?selector ^-> NodesClass

            // Edge points
            "controlPoints" => T<unit> ^-> !| Position.Type
            "renderedControlPoints" => T<unit> ^-> !| Position.Type
            "segmentPoints" => T<unit> ^-> !| Position.Type
            "renderedSegmentPoints" => T<unit> ^-> !| Position.Type
            "sourceEndpoint" => T<unit> ^-> Position.Type
            "renderedSourceEndpoint" => T<unit> ^-> Position.Type
            "targetEndpoint" => T<unit> ^-> Position.Type
            "renderedTargetEndpoint" => T<unit> ^-> Position.Type
            "midpoint" => T<unit> ^-> Position.Type
            "renderedMidpoint" => T<unit> ^-> Position.Type
        ]
        |> ignore

    EdgesClass
        |=> Inherits EdgeClass
        |+> Instance [
            // Traversing
            "connectedNodes" => !?T<string>?selector ^-> NodesClass
            "sources" => !?T<string>?selector ^-> NodesClass
            "targets" => !?T<string>?selector ^-> NodesClass
            "parallelEdges" => !?T<string>?selector ^-> TSelf
            "codirectedEdges" => !?T<string>?selector ^-> TSelf
        ]
        |> ignore

    // ------------------------------------------------------------------
    // The core (cytoscape)
    // ------------------------------------------------------------------

    CytoscapeClass
        |+> Static [
            Constructor (T<unit> + CytoscapeOptions.Type)
            "use" => (T<obj> ^-> T<unit>) ^-> T<unit>
                |> WithComment "Registers a cytoscape.js extension."
            "warnings" => T<bool>?condition ^-> T<unit>
                |> WithComment "Enable or disable the library's console warnings."
        ]
        |+> Instance [
            // Graph manipulation
            "add" => ElementObject.Type + !| ElementObject.Type + ElementsDefinition.Type + ElesClass ^-> ElesClass
            "remove" => ElesClass + T<string> ^-> ElesClass
            "collection" => T<unit> ^-> ElesClass
            "collection" => !| ElementObject.Type ^-> ElesClass
            "collection" => T<string> ^-> ElesClass
            "hasElementWithId" => T<string>?id ^-> T<bool>
            "getElementById" => T<string>?id ^-> ElesClass
            "elements" => !?T<string>?selector ^-> ElesClass
            "nodes" => !?T<string>?selector ^-> NodesClass
            "edges" => !?T<string>?selector ^-> EdgesClass
            "filter" => (T<string> + (EleClass * T<int> * ElesClass ^-> T<bool>)) ^-> ElesClass
            "batch" => (T<unit> ^-> T<unit>) ^-> T<unit>
            "startBatch" => T<unit> ^-> T<unit>
            "endBatch" => T<unit> ^-> T<unit>
            "mount" => T<Dom.Element>?element ^-> T<unit>
            "unmount" => T<unit> ^-> T<unit>
            "destroy" => T<unit> ^-> T<unit>
            "destroyed" => T<unit> ^-> T<bool>

            // Data
            "data" => !?T<string>?name ^-> T<obj>
            "data" => T<string>?name * T<obj>?value ^-> TSelf
            "data" => T<obj>?obj ^-> TSelf
            "attr" => !?T<string>?name ^-> T<obj>
            "attr" => T<string>?name * T<obj>?value ^-> TSelf
            "attr" => T<obj>?obj ^-> TSelf
            "removeData" => !?T<string>?names ^-> TSelf
            "removeAttr" => !?T<string>?names ^-> TSelf

            // Scratchpad
            "scratch" => !?T<string>?ns ^-> T<obj>
            "scratch" => T<string>?ns * T<obj>?value ^-> TSelf
            "removeScratch" => T<string>?ns ^-> TSelf

            // Events
            "on" => T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "on" => T<string> * T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "on" => T<string> * T<string> * T<obj> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "bind" => T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "bind" => T<string> * T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "bind" => T<string> * T<string> * T<obj> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "listen" => T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "listen" => T<string> * T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "listen" => T<string> * T<string> * T<obj> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "addListener" => T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "addListener" => T<string> * T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "addListener" => T<string> * T<string> * T<obj> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "promiseOn" => T<string> * !?T<string>?selector ^-> T<Promise<_>>[EventObjectClass]
            "pon" => T<string> * !?T<string>?selector ^-> T<Promise<_>>[EventObjectClass]
            "one" => T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "one" => T<string> * T<string> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "one" => T<string> * T<string> * T<obj> * (EventObjectClass.Type ^-> T<unit>) ^-> TSelf
            "off" => !?T<string>?events * !?T<string>?selector * !?(EventObjectClass.Type ^-> T<unit>)?handler ^-> TSelf
            "unbind" => !?T<string>?events * !?T<string>?selector * !?(EventObjectClass.Type ^-> T<unit>)?handler ^-> TSelf
            "unlisten" => !?T<string>?events * !?T<string>?selector * !?(EventObjectClass.Type ^-> T<unit>)?handler ^-> TSelf
            "removeListener" => !?T<string>?events * !?T<string>?selector * !?(EventObjectClass.Type ^-> T<unit>)?handler ^-> TSelf
            "removeAllListeners" => T<unit> ^-> TSelf
            "trigger" => T<string>?events *+ T<obj> ^-> TSelf
            "emit" => T<string>?events *+ T<obj> ^-> TSelf
            "ready" => (EventObjectClass.Type ^-> T<unit>)?fn ^-> TSelf

            // Viewport manipulation
            "container" => T<unit> ^-> T<Dom.Element>
            "container" => !?T<Dom.Element>?element ^-> TSelf
            "center" => T<unit> ^-> TSelf
            "center" => ElesClass ^-> TSelf
            "centre" => T<unit> ^-> TSelf
            "centre" => ElesClass ^-> TSelf
            "fit" => T<unit> ^-> TSelf
            "fit" => (ElesClass + T<string>)?eles * !?T<float>?padding ^-> TSelf
            "reset" => T<unit> ^-> TSelf
            "pan" => T<unit> ^-> Position.Type
            "pan" => Position.Type ^-> TSelf
            "panBy" => Position.Type ^-> TSelf
            "panningEnabled" => T<unit> ^-> T<bool>
            "panningEnabled" => T<bool>?bool ^-> TSelf
            "userPanningEnabled" => T<unit> ^-> T<bool>
            "userPanningEnabled" => T<bool>?bool ^-> TSelf
            "zoom" => T<unit> ^-> T<float>
            "zoom" => T<float>?level ^-> TSelf
            "zoom" => ZoomOptions.Type ^-> TSelf
            "zoomingEnabled" => T<unit> ^-> T<bool>
            "zoomingEnabled" => T<bool>?bool ^-> TSelf
            "userZoomingEnabled" => T<unit> ^-> T<bool>
            "userZoomingEnabled" => T<bool>?bool ^-> TSelf
            "minZoom" => T<unit> ^-> T<float>
            "minZoom" => T<float>?zoom ^-> TSelf
            "maxZoom" => T<unit> ^-> T<float>
            "maxZoom" => T<float>?zoom ^-> TSelf
            "viewport" => T<float>?zoom * Position.Type?pan ^-> TSelf
            "getFitViewport" => T<unit> ^-> ViewportOptions.Type
            "boxSelectionEnabled" => T<unit> ^-> T<bool>
            "boxSelectionEnabled" => T<bool>?bool ^-> TSelf
            "selectionType" => T<unit> ^-> SelectionType.Type
            "selectionType" => SelectionType.Type ^-> TSelf
            "width" => T<unit> ^-> T<float>
            "height" => T<unit> ^-> T<float>
            "extent" => T<unit> ^-> Box.Type
            "renderedExtent" => T<unit> ^-> Box.Type
            "autolock" => T<unit> ^-> T<bool>
            "autolock" => T<bool>?bool ^-> TSelf
            "autoungrabify" => T<unit> ^-> T<bool>
            "autoungrabify" => T<bool>?bool ^-> TSelf
            "autounselectify" => T<unit> ^-> T<bool>
            "autounselectify" => T<bool>?bool ^-> TSelf
            "forceRender" => T<unit> ^-> TSelf
            "resize" => T<unit> ^-> TSelf
            "invalidateDimensions" => T<unit> ^-> TSelf

            // Animation
            "animated" => T<unit> ^-> T<bool>
            "animate" => AnimateOptions.Type * !?AnimateOptions?options ^-> TSelf
            "animation" => AnimateOptions.Type ^-> AnimationClass
            "delay" => T<int>?duration * !?(T<unit> ^-> T<unit>)?complete ^-> TSelf
            "delayAnimation" => T<int>?duration ^-> AnimationClass
            "stop" => !?T<bool>?clearQueue * !?T<bool>?jumpToEnd ^-> TSelf
            "clearQueue" => T<unit> ^-> TSelf

            // Layout
            "layout" => LayoutOptions.Type ^-> LayoutClass
            "makeLayout" => LayoutOptions.Type ^-> LayoutClass
            "createLayout" => LayoutOptions.Type ^-> LayoutClass

            // Style
            "style" => T<unit> ^-> StyleClass
            "style" => StyleConfig.Type + !| StyleConfig.Type + T<string> ^-> StyleClass

            // Export
            "png" => T<unit> ^-> T<string>
            "png" => ImageOptions.Type ^-> (T<string> + T<Blob> + T<Promise<_>>[T<Blob>])
            "jpg" => T<unit> ^-> T<string>
            "jpg" => ImageOptions.Type ^-> (T<string> + T<Blob> + T<Promise<_>>[T<Blob>])
            "jpeg" => T<unit> ^-> T<string>
            "jpeg" => ImageOptions.Type ^-> (T<string> + T<Blob> + T<Promise<_>>[T<Blob>])
            "json" => T<unit> ^-> T<obj>
            "json" => T<obj>?json ^-> TSelf
        ]
        |> ignore

    let Assembly =
        Assembly [
            Namespace "WebSharper.Cytoscape" [
                CytoscapeClass
                Position
                Box
                Size
                GridCell
                ElementGroup
                SelectionType
                PositionDimension
                LayoutDirection
                ExportOutput
                ElementData
                ElementObject
                ElementsDefinition
                StyleConfig
                CssConfig
                CytoscapeOptions
                ZoomOptions
                FitOptions
                CenterOptions
                AnimateOptions
                LayoutOptions
                ImageOptions
                Location
                AlgorithmOptions
                ClusteringOptions
                BoundingBoxOptions
                HitTestOptions
                LayoutPositionOptions
                LayoutDimensionOptions
                NodePositionMap
                ViewportOptions
                DiffResult
                MinMaxResult
                EventObjectClass
                LayoutClass
                StyleClass
                AnimationClass
                BFSClass
                DFSClass
                DijkstraClass
                AStarClass
                FWClass
                BFClass
                HierholzerClass
                KargerSteinClass
                HopcroftTarjanBiconnectedClass
                TarjanStronglyConnectedClass
                PageRankClass
                DegreeCentralityClass
                DegreeCentralityNClass
                ClosenessCentralityNClass
                BetweennessCentralityClass
                FuzzyCMeansResult
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