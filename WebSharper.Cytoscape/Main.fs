// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2025 IntelliFactory
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
                    "directed", T<bool>
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
                    "renderedPosition", Position.Type
                    "selected", T<bool>
                    "selectable", T<bool>
                    "locked", T<bool>
                    "grabbable", T<bool>
                    "pannable", T<bool>
                    "classes", T<string> + !| T<string>
                    "style", T<obj>
                    "css", T<obj>
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
                    "container", T<Dom.Element>
                    "elements", !| ElementObject + ElementObject + T<Promise<_>>[ElementObject] + T<Promise<_>>[!|ElementObject]
                    "style", !| StyleConfig
                    "layout", T<obj>
                    "data", T<obj>
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
                    "pixelRatio", T<string> + T<float>
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
                    "zoom", ZoomOptions.Type
                    "pan", Position.Type
                    "panBy", Position.Type
                    "fit", FitOptions.Type
                    "center", !| ElementObject.Type + T<string>
                    "duration", T<int>
                    "queue", T<bool>
                    "complete", T<Function>
                    "step", T<Function>
                    "easing", T<string>
                ]
        }

    let LayoutOptions =
        Pattern.Config "LayoutOptions" {
            Required = []
            Optional = 
                [
                    "name", T<string>
                    "ready", T<Function>
                    "stop", T<Function>
                    "fit", T<bool>
                    "padding", T<int>
                    "boundingBox", T<obj>
                    "animate", T<bool>
                    "animationDuration", T<int>
                    "animationEasing", T<obj>
                    "animateFilter", T<Function>
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
                    "sort", T<Function>
                    "radius", T<float>
                    "startAngle", T<float>
                    "sweep", T<float>
                    "clockwise", T<bool>
                    "equidistant", T<bool>
                    "minNodeSpacing", T<int>
                    "height", T<int>
                    "width", T<int>
                    "concentric", T<Function>
                    "levelWidth", T<Function>
                    "directed", T<bool>
                    "roots", !| T<string>
                    "maximalAdjustments", T<int>
                    "maximal", T<bool>
                    "grid", T<bool>
                    "depthSort", T<Function>
                    "refresh", T<int>
                    "randomize", T<bool>
                    "componentSpacing", T<int>
                    "nodeRepulsion", T<Function>
                    "nodeOverlap", T<int>
                    "idealEdgeLength", T<Function>
                    "edgeElasticity", T<Function>
                    "nestingFactor", T<int>
                    "gravity", T<int>
                    "numIter", T<int>
                    "initialTemp", T<int>
                    "coolingFactor", T<float>
                    "minTemp", T<float>
                    "weaver", T<bool>
                    "animationThreshold", T<float>
                ]
        }

    let LayoutClass =
        Class "Layout"
        |+> Instance [
            "run" => T<unit> ^-> TSelf
            "start" => T<unit> ^-> TSelf
            "stop" => T<unit> ^-> TSelf

            //events
            "on" => T<string> * T<Function> ^-> TSelf
            "on" => T<string> * T<obj> * T<Function> ^-> TSelf

            "bind" => T<string> * T<Function> ^-> TSelf
            "bind" => T<string> * T<obj> * T<Function> ^-> TSelf

            "listen" => T<string> * T<Function> ^-> TSelf
            "listen" => T<string> * T<obj> * T<Function> ^-> TSelf

            "addListener" => T<string> * T<Function> ^-> TSelf
            "addListener" => T<string> * T<obj> * T<Function> ^-> TSelf

            "promiseOn" => T<string> ^-> TSelf
            "pon" => T<string> ^-> TSelf

            "one" => T<string> * T<Function> ^-> TSelf
            "one" => T<string> * T<obj> * T<Function> ^-> TSelf

            "off" => T<string> * !? T<Function> ^-> TSelf
            "unbind" => T<string> * !? T<Function> ^-> TSelf
            "unlisten" => T<string> * !? T<Function> ^-> TSelf
            "removeListener" => T<string> * !? T<Function> ^-> TSelf

            "removeAllListeners" => T<unit> ^-> TSelf

            "trigger" => T<string> * !? (!| T<obj>) ^-> TSelf
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
                    "weight", T<Function>
                    "heuristic", T<Function>
                    "visit", T<Function>
                    "directed", T<bool>
                    "dampingFactor", T<float>
                    "precision", T<float>
                    "iterations", T<float>
                    "harmonic", T<bool>

                    "attributes", !| T<Function>
                    "distance", T<string> + T<Function>
                    "linkage", T<string>
                    "mode", T<string>
                    "threshold", T<float>
                    "dendrogramDepth", T<float>
                    "addDendrogram", T<bool>                    
                    "expandFactor", T<float>
                    "inflateFactor", T<float>
                    "multFactor", T<float>
                    "maxIterations", T<int>
                    "k", T<int>
                    "sensitivityThreshold", T<float>
                    "preference", T<string> + T<float>
                    "damping", T<float>
                    "minIterations", T<int>
                    "degreeOfMembership", !| !| T<float>
                    "found", T<bool>
                    "trail", T<obj>
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
            "negativeWeightCycles" =@ TSelf
        ]

    let KragerSteinClass =
        Class "KragerStein"
        |+> Instance [
            "cut" =? EdgesClass
            "components" =? EdgesClass
            "partitionFirst" =? NodesClass
            "partitionSecond" =? NodesClass
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
            "betweennessNormalised" =? NodeClass + T<string> ^-> T<float>
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

    let ViewportOptions = 
        Pattern.Config "ViewportOptions" {
            Required = []
            Optional =
                [
                    "zoom", T<float>
                    "pan", Position.Type
                ]
        }

    let SelectionType = 
        Pattern.EnumStrings "SelectionType" ["additive"; "single"]

    let HierarchicalClusteringClass =
        Class "HierarchicalClusteringClass"
        |+> Instance [
            "clusters" =? NodesClass
            "dendrogram" =? ElesClass
        ]

    let FuzzyCMeansClass =
        Class "FuzzyCMeansClass"
        |+> Instance [
            "clusters" =? !| NodesClass
            "dendrogram" =? !| !| T<float>
        ]

    let HierholzerClass =
        Class "HierholzerClass"
        |+> Instance [
            "found" =? T<bool>
            "trail" =? ElesClass
        ]

    let HTBClass =
        Class "HTBClass"
        |+> Instance [
            "cut" =? ElesClass
            "components" =? ElesClass
        ]

    let TSCClass = 
        Class "TSCClass"
        |=> Inherits HTBClass

    let MarkovClusteringResult = !| NodesClass
    let KMeansResult = !| NodesClass
    let KMedoidsResult = !| NodesClass
    let AffinityPropagationResult = !| NodesClass

    let ClosenessCentralityNClass = 
        Class "ClosenessCentralityNormalizedResult"
        |+> Instance [
            "closeness" => NodeClass ^-> T<obj>
        ]

    let AnimationManipulation =
        Class "AnimationManipulation"
        |+> Instance [
            "play" => T<unit> ^-> TSelf
            "run" => T<unit> ^-> TSelf
            "playing" => T<unit> ^-> T<bool>
            "running" => T<unit> ^-> T<bool>

            "progress" => T<unit> ^-> T<float>
            "progress" => T<float> ^-> TSelf

            "time" => T<unit> ^-> T<float>
            "time" => T<float> ^-> TSelf

            "rewind" => T<unit> ^-> TSelf
            "fastforward" => T<unit> ^-> TSelf

            "pause" => T<unit> ^-> TSelf
            "stop" => T<unit> ^-> TSelf

            "completed" => T<unit> ^-> TSelf
            "complete" => T<unit> ^-> TSelf

            "apply" => T<unit> ^-> TSelf
            "applying" => T<unit> ^-> TSelf

            "reverse" => T<unit> ^-> TSelf

            "promise" => !? T<string> ^-> T<Promise<obj>>
        ]

    let Style =
        Class "Style"
        |+> Instance [
            "append" => (T<string> + T<obj> + !|T<obj>) ^-> TSelf

            "clear" => T<unit> ^-> TSelf

            "fromJson" => T<obj> ^-> TSelf

            "fromString" => T<string> ^-> TSelf

            "resetToDefault" => T<unit> ^-> TSelf

            "selector" => T<string> ^-> TSelf

            "style" => T<string> * T<string> ^-> TSelf

            "style" => T<obj> ^-> TSelf

            "update" => T<unit> ^-> T<unit>
        ]

    EleClass
        |+> Instance [
            //Graph manipulation
            "cy" => T<unit> ^-> CytoscapeClass
            "removed" => T<unit> ^-> T<bool>
            "inside" => T<unit> ^-> T<bool>

            //Data
            "scratch" => !?(T<string>) ^-> T<obj>
            "scratch" => T<string> * T<obj> ^-> TSelf
            "removeScratch" => T<string> ^-> TSelf
            "id" => T<unit> ^-> T<string>
            "json" => T<unit> ^-> T<obj>
            "group" => T<unit> ^-> T<string>
            "isNode" => T<unit> ^-> T<bool>
            "isEdge" => T<unit> ^-> T<bool>
            "component" => T<unit> ^-> T<obj>

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

    let ClassNames = T<string> + !| T<string>

    ElesClass
        |=> Inherits EleClass
        |+> Instance [
            //Graph manipulation
            "remove" => T<unit> ^-> TSelf
            "restore" => T<unit> ^-> TSelf
            "clone" => T<unit> ^-> TSelf
            "copy" => T<unit> ^-> TSelf
            "move" => Location.Type ^-> TSelf

            //Events
            "on" => T<string> * T<Function> ^-> TSelf
            "on" => T<string> * T<string> * T<Function> ^-> TSelf

            "bind" => T<string> * T<Function> ^-> TSelf
            "bind" => T<string> * T<string> * T<Function> ^-> TSelf

            "listen" => T<string> * T<Function> ^-> TSelf
            "listen" => T<string> * T<string> * T<Function> ^-> TSelf

            "addListener" => T<string> * T<Function> ^-> TSelf
            "addListener" => T<string> * T<string> * T<Function> ^-> TSelf
    
            "promiseOn" => T<string> * !? T<string> ^-> TSelf
            "pon" => T<string> * !? T<string> ^-> TSelf

            "one" => T<string> * T<Function> ^-> TSelf
            "one" => T<string> * T<string> * T<Function> ^-> TSelf
            "one" => T<string> * T<string> * T<obj> * T<Function> ^-> TSelf

            "once" => T<string> * T<Function> ^-> TSelf
            "once" => T<string> * T<string> * T<Function> ^-> TSelf
            "once" => T<string> * T<string> * T<obj> * T<Function> ^-> TSelf

            "off" => T<string> * !? T<string> * !? T<Function> ^-> TSelf

            "unbind" => T<string> * !? T<string> * !? T<Function> ^-> TSelf

            "unlisten" => T<string> * !? T<string> * !? T<Function> ^-> TSelf

            "removeListener" => T<string> * !? T<string> * !? T<Function> ^-> TSelf

            "removeAllListeners" => T<unit> ^-> TSelf

            "trigger" => T<string> * !? (!|T<obj>) ^-> TSelf
            "emit" => T<string> * !? (!|T<obj>) ^-> TSelf
            
            //Data
            "data" => !?T<string> ^-> T<obj>
            "data" => T<string> * T<obj> ^-> TSelf
            "data" => T<obj> ^-> TSelf

            "attr" => T<string> ^-> T<obj>
            "attr" => T<string> * T<obj> ^-> TSelf
            "attr" => T<obj> ^-> TSelf

            "removeData" => !? T<string> ^-> TSelf
            "removeAttr" => !? T<string> ^-> TSelf

            "jsons" => T<unit> ^-> !|T<string>

            //Positions and dimensions    
            "boundingBox" => BoundingBoxOptions.Type ^-> Box.Type
            "renderedBoundingBox" => BoundingBoxOptions.Type ^-> Box.Type
            
            //Layout
            "layout" => LayoutOptions.Type ^-> LayoutClass

            //Selection
            "select" => T<unit> ^-> TSelf
            "unselect" => T<unit> ^-> TSelf
            "deselect" => T<unit> ^-> TSelf
            "selectify" => T<unit> ^-> TSelf
            "unselectify" => T<unit> ^-> TSelf
            
            //Stlye
            "addClass" => ClassNames ^-> TSelf
            "removeClass" => ClassNames ^-> TSelf
            "toggleClass" => ClassNames * !? T<bool> ^-> TSelf

            "classes" => ClassNames ^-> TSelf
            "classes" => ClassNames ^-> TSelf + !| T<string>
            "classes" => T<unit> ^-> !| T<string>

            "fleshClass" => ClassNames * !? T<int> ^-> TSelf
            "style" => T<string> * T<obj> ^-> TSelf
            "style" => T<string> ^-> T<obj>
            "style" => T<obj> ^-> TSelf
            "style" => T<unit> ^-> T<obj>

            "css" => T<string> * T<obj> ^-> TSelf
            "css" => T<string> ^-> T<obj>
            "css" => T<obj> ^-> TSelf
            "css" => T<unit> ^-> T<obj>

            "removeStyle" => !? T<string> ^-> TSelf

            //Animation
            "animate" => AnimateOptions.Type ^-> TSelf
            "delay" => T<int> * !?T<Function> ^-> TSelf
            "stop" => T<bool> * !?T<bool> ^-> TSelf
            "clearQueue" => T<unit> ^-> TSelf

            //Comparison
            "same" => TSelf ^-> T<bool>
            "anySame" => TSelf ^-> T<bool>
            "contains" => TSelf ^-> T<bool>
            "has" => TSelf ^-> T<bool>
            "allAreNeighbors" => TSelf ^-> T<bool>
            "allAreNeighbours" => TSelf ^-> T<bool>
            "is" => TSelf ^-> T<bool>
            "allAre" => TSelf ^-> T<bool>
            "some" => T<Function> * !? TSelf ^-> T<bool>
            "every" => T<Function> * !? TSelf ^-> T<bool>

            //Iteration
            "size" => T<unit> ^-> T<int>
            "empty" => T<unit> ^-> T<bool>
            "nonempty" => T<unit> ^-> T<bool>
            "each" => (T<Function> + T<bool>) * !? TSelf ^-> TSelf
            "forEach" => (T<Function> + T<bool>) * !? TSelf ^-> TSelf
            "eq" => T<int> ^-> TSelf
            "first" => T<unit> ^-> TSelf
            "last" => T<unit> ^-> TSelf
            "slice" => !? T<int> * !? T<int> ^-> TSelf
            "toArray" => T<unit> ^-> !|TSelf

            //Building and filtering
            "getElementById" => T<string> ^-> TSelf
            "of" => T<string>?str ^-> ElesClass
                |> WithInline "$($str)"

            "union" => TSelf + T<string> ^-> TSelf
            "u" => TSelf + T<string> ^-> TSelf
            "add" => TSelf + T<string> ^-> TSelf
            "or" => TSelf + T<string> ^-> TSelf

            "difference" => TSelf + T<string> ^-> TSelf
            "subtract" => TSelf + T<string> ^-> TSelf
            "not" => TSelf + T<string> ^-> TSelf
            "relativeComplement" => TSelf + T<string> ^-> TSelf

            "absoluteComplement" => T<unit> ^-> TSelf
            "abscomp" => T<unit> ^-> TSelf
            "complement" => T<unit> ^-> TSelf

            "intersection" => TSelf + T<string> ^-> TSelf
            "intersect" => TSelf + T<string> ^-> TSelf
            "and" => TSelf + T<string> ^-> TSelf
            "n" => TSelf + T<string> ^-> TSelf

            "symmetricDifference" => TSelf + T<string> ^-> TSelf
            "symdiff" => TSelf + T<string> ^-> TSelf
            "xor" => TSelf + T<string> ^-> TSelf

            "diff" => T<string> ^-> TSelf

            "merge" => TSelf + T<string> ^-> TSelf
            "unmerge" => TSelf + T<string> ^-> TSelf

            "filter" => T<string> + T<Function> ^-> TSelf
            "nodes" => !? T<string> ^-> TSelf
            "edges" => !? T<string> ^-> TSelf

            "sort" => T<Function> ^-> TSelf
            "map" => T<Function> ^-> TSelf
            "reduce" => T<Function> ^-> TSelf

            "min" => T<Function> ^-> T<obj>
            "max" => T<Function> ^-> T<obj>

            //Traversing
            "neighborhood" => !? T<string> ^-> TSelf
            "openNeighborhood" => !? T<string> ^-> TSelf
            "closedNeighborhood" => !? T<string> ^-> TSelf
            "componenets" => T<unit> ^-> !|TSelf
            "componenets" => T<string> ^-> !|TSelf
            
            //Algorithms
            "breadthFirstSearch" => AlgorithmOptions.Type ^-> BFSClass.Type
            "bfs" => AlgorithmOptions.Type ^-> BFSClass
            "depthFirstSearch" => AlgorithmOptions.Type ^-> DFSClass.Type
            "dfs" => AlgorithmOptions.Type ^-> DFSClass.Type
            "dijkstra" => AlgorithmOptions.Type ^-> DijkstraClass.Type
            "aStar" => AlgorithmOptions.Type ^-> AStarClass.Type
            "floydWarshall" => AlgorithmOptions.Type ^-> FWClass.Type
            "bellmanFord" => AlgorithmOptions.Type ^-> BFClass.Type
            "hierholzer" => AlgorithmOptions ^-> HierholzerClass

            "kruskal" => !? T<Function> ^-> ElesClass
            "kragerStein" => T<unit> ^-> KragerSteinClass.Type

            "hopcroftTarjanBiconnected" => T<unit> ^-> HTBClass.Type
            "hopcroftTarjanBiconnectedComponents" => T<unit> ^-> HTBClass.Type
            "htb" => T<unit> ^-> HTBClass.Type
            "htbc" => T<unit> ^-> HTBClass.Type
            "tarjanStronglyConnected" => T<unit> ^-> TSCClass.Type
            "tarjanStronglyConnectedComponents" => T<unit> ^-> TSCClass.Type
            "tsc" => T<unit> ^-> TSCClass.Type
            "tscc" => T<unit> ^-> TSCClass.Type
            
            "degreeCentrality" => AlgorithmOptions.Type ^-> DegreeCentralityClass.Type
            "degreeCentralityNormalized" => AlgorithmOptions.Type ^-> DegreeCentralityNClass.Type
            "closenessCentrality" => AlgorithmOptions.Type ^-> T<float>
            "closenessCentralityNormalized" => AlgorithmOptions.Type ^-> ClosenessCentralityNClass
            "betweennessCentrality" => AlgorithmOptions.Type ^-> BetweennessCentralityClass.Type
            "pageRank" => AlgorithmOptions.Type ^-> PageRankClass.Type

            "markovClustering" => AlgorithmOptions ^-> MarkovClusteringResult
            "kMeans" => AlgorithmOptions ^-> KMeansResult
            "hierarchicalClustering" => AlgorithmOptions ^-> HierarchicalClusteringClass.Type
            "kMedoids" => AlgorithmOptions ^-> KMedoidsResult
            "fuzzyCMeans" => AlgorithmOptions ^-> FuzzyCMeansClass.Type
            "affinityPropagation" => AlgorithmOptions ^-> AffinityPropagationResult
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
            "position" => T<unit> ^-> Position
            "position" => Position ^-> T<float>
            "position" => Position * T<float> ^-> TSelf
            "position" => Position ^-> TSelf

            "modelPosition" => T<unit> ^-> Position
            "modelPosition" => Position ^-> T<float>
            "modelPosition" => Position * T<float> ^-> TSelf
            "modelPosition" => Position ^-> TSelf

            "point" => T<unit> ^-> Position
            "point" => Position ^-> T<float>
            "point" => Position * T<float> ^-> TSelf
            "point" => Position ^-> TSelf

            "renderedPosition" => !? Position ^-> Position
            "renderedPosition" => Position * Position ^-> TSelf
            "renderedPosition" => T<obj> ^-> TSelf

            "renderedPoint" => !? Position ^-> Position
            "renderedPoint" => Position * Position ^-> TSelf
            "renderedPoint" => T<obj> ^-> TSelf

            "relativePosition" => !? Position ^-> Position
            "relativePosition" => Position * Position ^-> TSelf
            "relativePosition" => T<obj> ^-> TSelf

            "relativePoint" => !? Position ^-> Position
            "relativePoint" => Position * Position ^-> TSelf
            "relativePoint" => T<obj> ^-> TSelf

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
            "positions" => (T<Function> + Position) ^-> TSelf
            "modelPositions" => (T<Function> + Position) ^-> TSelf
            "points" => (T<Function> + Position) ^-> TSelf

            "grabify" => T<unit> ^-> TSelf
            "ungrabify" => T<unit> ^-> TSelf

            "lock" => T<unit> ^-> TSelf
            "unlock" => T<unit> ^-> TSelf

            "pannable" => T<unit> ^-> T<bool>
            "panify" => T<unit> ^-> TSelf
            "unpanify" => T<unit> ^-> TSelf

            //Layout
            "layoutPositions" => LayoutClass * LayoutOptions.Type * T<Function> ^-> !| Position.Type
        
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
            //Points
            "controlPoints" => T<unit> ^-> !| Position
            "renderedControlPoints" => T<unit> ^-> !| Position
            "segmentPoints" => T<unit> ^-> !| Position
            "renderedSegmentPoints" => T<unit> ^-> !| Position
            "sourceEndpoint" => T<unit> ^-> Position
            "renderedSourceEndpoint" => T<unit> ^-> Position
            "targetEndpoint" => T<unit> ^-> Position
            "renderedTargetEndpoint" => T<unit> ^-> Position
            "midpoint" => T<unit> ^-> Position
            "renderedMidpoint" => T<unit> ^-> Position

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
            "add" => ElementObject.Type + !| ElementObject.Type + ElesClass ^-> TSelf
            "remove" => ElementObject.Type + T<string> ^-> TSelf
            "collection" => T<unit> + T<string> + !| ElementObject.Type ^-> TSelf
            "hasElementWithId" => T<string> ^-> T<bool>
            "getElementById" => T<string> ^-> TSelf
            "of" => T<string>?str ^-> ElesClass
                |> WithInline "$($str)"
            "of" => T<unit> ^-> ElesClass
                |> WithInline "$()"
            "elements" => T<unit> + T<string> ^-> ElesClass
            "nodes" => T<unit> + T<string> ^-> NodesClass
            "edges" => T<unit> + T<string> ^-> EdgesClass
            "filter" => (T<string> + T<Function>) ^-> ElesClass
            "batch" => T<Function> ^-> TSelf
            "startBatch" => T<unit> ^-> TSelf
            "endBatch" => T<unit> ^-> TSelf
            "mount" => T<Dom.Element> ^-> T<unit>
            "unmount" => T<unit> ^-> T<unit>
            "destroy" => T<unit> ^-> TSelf
            "destroyed" => T<unit> ^-> T<bool>
            "scratch" => T<unit> + T<string> + (T<string> * T<obj>) ^-> TSelf
            "removeScratch" => T<string> ^-> TSelf

            //Data
            "data" => !? T<string> ^-> T<obj>
            "data" => T<string> * T<obj> ^-> TSelf
            "data" => T<obj> ^-> TSelf

            "attr" => !? T<string> ^-> T<obj>
            "attr" => T<string> * T<obj> ^-> TSelf
            "attr" => T<obj> ^-> TSelf

            "removeData" => !? T<string> ^-> TSelf

            "removeAttr" => !? T<string> ^-> TSelf

            //Events
            "on" => T<string> * T<Function> ^-> TSelf
            "on" => T<string> * T<string> * T<Function> ^-> TSelf
            "on" => T<string> * T<string> * T<obj> * T<Function> ^-> TSelf
            "on" => T<obj> * !? T<string> * !? T<obj> ^-> TSelf

            "bind" => T<string> * T<Function> ^-> TSelf
            "bind" => T<string> * T<string> * T<Function> ^-> TSelf
            "bind" => T<string> * T<string> * T<obj> * T<Function> ^-> TSelf
            "bind" => T<obj> * !? T<string> * !? T<obj> ^-> TSelf

            "listen" => T<string> * T<Function> ^-> TSelf
            "listen" => T<string> * T<string> * T<Function> ^-> TSelf
            "listen" => T<string> * T<string> * T<obj> * T<Function> ^-> TSelf
            "listen" => T<obj> * !? T<string> * !? T<obj> ^-> TSelf

            "addListener" => T<string> * T<Function> ^-> TSelf
            "addListener" => T<string> * T<string> * T<Function> ^-> TSelf
            "addListener" => T<string> * T<string> * T<obj> * T<Function> ^-> TSelf
            "addListener" => T<obj> * !? T<string> * !? T<obj> ^-> TSelf
    
            "promiseOn" => T<string> * !? T<string> ^-> TSelf
            "pon" => T<string> * !? T<string> ^-> TSelf

            "one" => T<string> * T<Function> ^-> TSelf
            "one" => T<string> * T<string> * T<Function> ^-> TSelf
            "one" => T<string> * T<string> * T<obj> * T<Function> ^-> TSelf
            "one" => T<obj> * !? T<string> * !? T<obj> ^-> TSelf

            "off" => T<string> * !? T<Function> ^-> TSelf
            "off" => T<string> * T<string> * !? T<Function> ^-> TSelf
            "off" => T<obj> * !? T<string> ^-> TSelf

            "unbind" => T<string> * !? T<Function> ^-> TSelf
            "unbind" => T<string> * T<string> * !? T<Function> ^-> TSelf
            "unbind" => T<obj> * !? T<string> ^-> TSelf

            "unlisten" => T<string> * !? T<Function> ^-> TSelf
            "unlisten" => T<string> * T<string> * !? T<Function> ^-> TSelf
            "unlisten" => T<obj> * !? T<string> ^-> TSelf

            "removeListener" => T<string> * !? T<Function> ^-> TSelf
            "removeListener" => T<string> * T<string> * !? T<Function> ^-> TSelf
            "removeListener" => T<obj> * !? T<string> ^-> TSelf

            "removeAllListeners" => T<unit> ^-> TSelf

            "trigger" => T<string> * !? (!|T<obj>) ^-> TSelf
            "emit" => T<string> * !? (!|T<obj>) ^-> TSelf

            "ready" => T<Function> ^-> TSelf

            //Viewport manipulation
            "container" => T<unit> ^-> T<Dom.Element>
            "center" => T<unit> + !| ElementObject.Type ^-> TSelf
            "centre" => T<unit> + !| ElementObject.Type ^-> TSelf
            "fit" => !? !| ElementObject.Type * !? T<string> ^-> TSelf
            "reset" => T<unit> ^-> TSelf
            "pan" => T<unit> + Position.Type ^-> TSelf
            "panBy" => Position.Type ^-> TSelf
            "panningEnabled" => !?T<bool> ^-> TSelf
            "userPanningEnabled" => !?T<bool> ^-> TSelf
            "zoom" => T<unit> + T<string> + ZoomOptions.Type ^-> TSelf
            "zoomingEnabled" => !?T<bool> ^-> TSelf
            "userZoomingEnabled" => !?T<bool> ^-> TSelf
            "minZoom" => (T<unit> ^-> T<bool>) + (T<float> ^-> TSelf)
            "maxZoom" => (T<unit> ^-> T<bool>) + (T<float> ^-> TSelf)
            "viewport" => ViewportOptions ^-> TSelf
            "selectionType" => T<unit> ^-> SelectionType
            "selectionType" => SelectionType ^-> TSelf
            "boxSelectionEnabled" => !?T<bool> ^-> TSelf
            "width" => T<unit> ^-> T<int>
            "height" => T<unit> ^-> T<int>
            "extent" => T<unit> ^-> Box
            "renderedExtent" => T<unit> ^-> Box
            "autolock" => !?T<bool> ^-> TSelf
            "autoungrabify" => !?T<bool> ^-> TSelf
            "autounselectify" => !?T<bool> ^-> TSelf
            "forcerender" => T<unit> ^-> TSelf
            "resize" => T<unit> ^-> TSelf
            "invalidateDimensions" => T<unit> ^-> TSelf

            //Animation
            "animated" => T<unit> ^-> T<bool>
            "animate" => AnimateOptions.Type ^-> TSelf
            "animation" => AnimateOptions.Type ^-> AnimationManipulation
            "delay" => T<int> * T<Function> ^-> TSelf
            "delayAnimation" => T<int> ^-> AnimationManipulation
            "stop" => T<bool> * T<bool> ^-> TSelf
            "clearQueue" => T<unit> ^-> TSelf

            //Layout
            "layout" => LayoutOptions.Type ^-> LayoutClass
            "makeLayout" => LayoutOptions.Type ^-> LayoutClass
            "createLayout" => LayoutOptions.Type ^-> LayoutClass

            //Style
            "style" => !? (StyleConfig + !| StyleConfig + T<string>) ^-> Style

            //Export
            "png" => !? ImageOptions.Type ^-> T<string>
            "png" => !? ImageOptions.Type ^-> T<Blob>
            "png" => !? ImageOptions.Type ^-> T<Promise<Blob>>

            "jpg" => !? ImageOptions.Type ^-> T<string>
            "jpg" => !? ImageOptions.Type ^-> T<Blob>
            "jpg" => !? ImageOptions.Type ^-> T<Promise<Blob>>

            "jpeg" => !? ImageOptions.Type ^-> T<string>
            "jpeg" => !? ImageOptions.Type ^-> T<Blob>
            "jpeg" => !? ImageOptions.Type ^-> T<Promise<Blob>>

            "json" => T<unit> ^-> T<obj>
            "json" => T<obj> ^-> TSelf
        ]
        |> ImportDefault "cytoscape"
        |> ignore

    let Assembly =
        Assembly [
            // Version 3.32.0
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
                Style
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
                HierarchicalClusteringClass
                FuzzyCMeansClass
                ClosenessCentralityNClass
                TSCClass
                HTBClass
                HierholzerClass
                Box
                EleClass
                ElesClass
                NodeClass
                NodesClass
                EdgeClass
                EdgesClass
                SelectionType
                ViewportOptions
                AnimationManipulation
            ]
        ]

[<Sealed>]
type Extension() =
    interface IExtension with
        member x.Assembly = Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()
