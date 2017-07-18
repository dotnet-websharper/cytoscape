# Cytoscape

With [Cytoscape]() you're able to render big graphs with labeled nodes, weighted edges on the screen. This extension even allows you to do higher level graph theory calculations on your website. The bind is close to the original JavaScript implementation with some changes.

## Configuration

In JavaScript to configure Cytoscape we just pass an options object to the constructor. We have a new type for this options object in F#: `CytoscapeOptions`.

Configuring a simple test graph in JavaScript (from the original documentation):

```javascript
let cy = cytoscape({
    container: document.getElementById('cy'),
    elements: [
        {
            data: { id: 'a' }
        },
        {
            data: { id: 'b' }
        },
        {
            data: { id: 'ab', source: 'a', target: 'b' }
        }
    ],
    style: [
        {
            selector: 'node',
            style: {
                'background-color': '#666',
                'label': 'data(id)'
            }
        },
        {
            selector: 'edge',
            style: {
                'width': 3,
                'line-color': '#ccc',
                'target-arrow-color': '#ccc',
                'target-arrow-shape': 'triangle'
            }
        }
    ],
    layout: {
        name: 'grid',
        rows: 1
    }
});
```

The same code in F#:

```fsharp
let cy = 
    Cytoscape(
        CytoscapeOptions(
            Container = Document.GetElementById('cy'),
            Elements = 
                [|
                    ElementObject(Data = ElementData(Id = "a"))
                    ElementObject(Data = ElementData(Id = "b"))
                    ElementObject(Data = ElementData(Id = "ab", Source = "a", Target = "b"))
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
                        ]
                    )
                |],
            Layout = LayoutOptions(
                Name = "grid",
                Rows = 2
            )
        )
    )
```

As you can see every JavaScript object have its own F# implementation. These usualy have the name either FieldnameConfig or FieldnameOptions. The properties are the same as the JavaScript object properties.

## Container

The container can must the a DOM element. In WebSharper we can create DOM elements and pass those in as a container. For example:

```fsharp
let myDiv = div []

...

let cy =
    Cytoscape(CytoscapeOptions(
        Container = myDiv.Dom
    ))
```

But this will only work if the element is already in our DOM. We can solve this simply by assigning an element to our div like this:

```fsharp
let myDiv = div []

myDiv
|> Doc.RunById "main"

let cy =
    Cytoscape(CytoscapeOptions(
        Container = myDiv.Dom
    ))
```

And having a `<div id="main"></div>` in our html file.