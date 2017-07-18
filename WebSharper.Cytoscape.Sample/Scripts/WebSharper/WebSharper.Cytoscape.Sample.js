(function()
{
 "use strict";
 var Global,WebSharper,Cytoscape,Sample,Client,UI,Next,AttrProxy,Doc;
 Global=window;
 WebSharper=Global.WebSharper=Global.WebSharper||{};
 Cytoscape=WebSharper.Cytoscape=WebSharper.Cytoscape||{};
 Sample=Cytoscape.Sample=Cytoscape.Sample||{};
 Client=Sample.Client=Sample.Client||{};
 UI=WebSharper&&WebSharper.UI;
 Next=UI&&UI.Next;
 AttrProxy=Next&&Next.AttrProxy;
 Doc=Next&&Next.Doc;
 Client.Main=function()
 {
  var graph,a,cy,a$1,r,r$1,r$2,r$3,r$4,r$5,r$6,r$7;
  graph=(a=[AttrProxy.Create("style","height:100vh;")],Doc.Element("div",a,[]));
  Doc.RunById("main",graph);
  cy=(a$1=(r={},r.container=graph.elt,r.elements=[(r$1={},r$1.data=(r$2={},r$2.id="a",r$2),r$1),(r$3={},r$3.data=(r$4={},r$4.id="b",r$4),r$3),(r$5={},r$5.data=(r$6={},r$6.id="ab",r$6.source="a",r$6.target="b",r$6),r$5)],r.style=[{
   selector:"node",
   style:{
    "background-color":"#666",
    label:"data(id)"
   }
  },{
   selector:"edge",
   style:{
    width:"3",
    "line-color":"#ccc",
    "target-arrow-color":"#ccc",
    "target-arrow-shape":"triangle"
   }
  }],r.layout=(r$7={},r$7.name="grid",r$7.rows=1,r$7),r),new Global.cytoscape(a$1));
 };
}());
