// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2016 IntelliFactory
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

IntelliFactory = {
    Runtime: {
        Ctor: function (ctor, typeFunction) {
            ctor.prototype = typeFunction.prototype;
            return ctor;
        },

        Cctor: function (cctor) {
            var init = true;
            return function () {
                if (init) {
                    init = false;
                    cctor();
                }
            };
        },

        Class: function (members, base, statics) {
            var proto = base ? new base() : {};
            var typeFunction = function (copyFrom) {
                if (copyFrom) {
                    for (var f in copyFrom) { this[f] = copyFrom[f] }
                }
            }
            for (var m in members) { proto[m] = members[m] }
            typeFunction.prototype = proto;
            if (statics) {
                for (var f in statics) { typeFunction[f] = statics[f] }
            }
            return typeFunction;
        },

        Clone: function (obj) {
            var res = {};
            for (var p in obj) { res[p] = obj[p] }
            return res;
        },

        NewObject:
            function (kv) {
                var o = {};
                for (var i = 0; i < kv.length; i++) {
                    o[kv[i][0]] = kv[i][1];
                }
                return o;
            },

        DeleteEmptyFields:
            function (obj, fields) {
                for (var i = 0; i < fields.length; i++) {
                    var f = fields[i];
                    if (obj[f] === void (0)) { delete obj[f]; }
                }
                return obj;
            },

        GetOptional:
            function (value) {
                return (value === void (0)) ? null : { $: 1, $0: value };
            },

        SetOptional:
            function (obj, field, value) {
                if (value) {
                    obj[field] = value.$0;
                } else {
                    delete obj[field];
                }
            },

        SetOrDelete:
            function (obj, field, value) {
                if (value === void (0)) {
                    delete obj[field];
                } else {
                    obj[field] = value;
                }
            },

        Bind: function (f, obj) {
            return function () { return f.apply(this, arguments) };
        },

        CreateFuncWithArgs: function (f) {
            return function () { return f(Array.prototype.slice.call(arguments)) };
        },

        CreateFuncWithOnlyThis: function (f) {
            return function () { return f(this) };
        },

        CreateFuncWithThis: function (f) {
            return function () { return f(this).apply(null, arguments) };
        },

        CreateFuncWithThisArgs: function (f) {
            return function () { return f(this)(Array.prototype.slice.call(arguments)) };
        },

        CreateFuncWithRest: function (length, f) {
            return function () { return f(Array.prototype.slice.call(arguments, 0, length).concat([Array.prototype.slice.call(arguments, length)])) };
        },

        CreateFuncWithArgsRest: function (length, f) {
            return function () { return f([Array.prototype.slice.call(arguments, 0, length), Array.prototype.slice.call(arguments, length)]) };
        },

        BindDelegate: function (func, obj) {
            var res = func.bind(obj);
            res.$Func = func;
            res.$Target = obj;
            return res;
        },

        CreateDelegate: function (invokes) {
            if (invokes.length == 0) return null;
            if (invokes.length == 1) return invokes[0];
            var del = function () {
                var res;
                for (var i = 0; i < invokes.length; i++) {
                    res = invokes[i].apply(null, arguments);
                }
                return res;
            };
            del.$Invokes = invokes;
            return del;
        },

        CombineDelegates: function (dels) {
            var invokes = [];
            for (var i = 0; i < dels.length; i++) {
                var del = dels[i];
                if (del) {
                    if ("$Invokes" in del)
                        invokes = invokes.concat(del.$Invokes);
                    else
                        invokes.push(del);
                }
            }
            return IntelliFactory.Runtime.CreateDelegate(invokes);
        },

        DelegateEqual: function (d1, d2) {
            if (d1 === d2) return true;
            if (d1 == null || d2 == null) return false;
            var i1 = d1.$Invokes || [d1];
            var i2 = d2.$Invokes || [d2];
            if (i1.length != i2.length) return false;
            for (var i = 0; i < i1.length; i++) {
                var e1 = i1[i];
                var e2 = i2[i];
                if (!(e1 === e2 || ("$Func" in e1 && "$Func" in e2 && e1.$Func === e2.$Func && e1.$Target == e2.$Target)))
                    return false;
            }
            return true;
        },

        ThisFunc: function (d) {
            return function () {
                var args = Array.prototype.slice.call(arguments);
                args.unshift(this);
                return d.apply(null, args);
            };
        },

        ThisFuncOut: function (f) {
            return function () {
                var args = Array.prototype.slice.call(arguments);
                return f.apply(args.shift(), args);
            };
        },

        ParamsFunc: function (length, d) {
            return function () {
                var args = Array.prototype.slice.call(arguments);
                return d.apply(null, args.slice(0, length).concat([args.slice(length)]));
            };
        },

        ParamsFuncOut: function (length, f) {
            return function () {
                var args = Array.prototype.slice.call(arguments);
                return f.apply(null, args.slice(0, length).concat(args[length]));
            };
        },

        ThisParamsFunc: function (length, d) {
            return function () {
                var args = Array.prototype.slice.call(arguments);
                args.unshift(this);
                return d.apply(null, args.slice(0, length + 1).concat([args.slice(length + 1)]));
            };
        },

        ThisParamsFuncOut: function (length, f) {
            return function () {
                var args = Array.prototype.slice.call(arguments);
                return f.apply(args.shift(), args.slice(0, length).concat(args[length]));
            };
        },

        Curried: function (f, n, args) {
            args = args || [];
            return function (a) {
                var allArgs = args.concat([a === void (0) ? null : a]);
                if (n == 1)
                    return f.apply(null, allArgs);
                if (n == 2)
                    return function (a) { return f.apply(null, allArgs.concat([a === void (0) ? null : a])); }
                return IntelliFactory.Runtime.Curried(f, n - 1, allArgs);
            }
        },

        Curried2: function (f) {
            return function (a) { return function (b) { return f(a, b); } }
        },

        Curried3: function (f) {
            return function (a) { return function (b) { return function (c) { return f(a, b, c); } } }
        },

        UnionByType: function (types, value, optional) {
            var vt = typeof value;
            for (var i = 0; i < types.length; i++) {
                var t = types[i];
                if (typeof t == "number") {
                    if (Array.isArray(value) && (t == 0 || value.length == t)) {
                        return { $: i, $0: value };
                    }
                } else {
                    if (t == vt) {
                        return { $: i, $0: value };
                    }
                }
            }
            if (!optional) {
                throw new Error("Type not expected for creating Choice value.");
            }
        },

        OnLoad:
            function (f) {
                if (!("load" in this)) {
                    this.load = [];
                }
                this.load.push(f);
            },

        Start:
            function () {
                function run(c) {
                    for (var i = 0; i < c.length; i++) {
                        c[i]();
                    }
                }
                if ("init" in this) {
                    run(this.init);
                    this.init = [];
                }
                if ("load" in this) {
                    run(this.load);
                    this.load = [];
                }
            },
    }
}

IntelliFactory.Runtime.OnLoad(function () {
    if (window.WebSharper && WebSharper.Activator && WebSharper.Activator.Activate)
        WebSharper.Activator.Activate()
});

// Polyfill

if (!Date.now) {
    Date.now = function now() {
        return new Date().getTime();
    };
}

if (!Math.trunc) {
    Math.trunc = function (x) {
        return x < 0 ? Math.ceil(x) : Math.floor(x);
    }
}

function ignore() { };
function id(x) { return x };
function fst(x) { return x[0] };
function snd(x) { return x[1] };
function trd(x) { return x[2] };

if (!console) {
    console = {
        count: ignore,
        dir: ignore,
        error: ignore,
        group: ignore,
        groupEnd: ignore,
        info: ignore,
        log: ignore,
        profile: ignore,
        profileEnd: ignore,
        time: ignore,
        timeEnd: ignore,
        trace: ignore,
        warn: ignore
    }
};
(function () {
    var lastTime = 0;
    var vendors = ['webkit', 'moz'];
    for (var x = 0; x < vendors.length && !window.requestAnimationFrame; ++x) {
        window.requestAnimationFrame = window[vendors[x] + 'RequestAnimationFrame'];
        window.cancelAnimationFrame =
          window[vendors[x] + 'CancelAnimationFrame'] || window[vendors[x] + 'CancelRequestAnimationFrame'];
    }

    if (!window.requestAnimationFrame)
        window.requestAnimationFrame = function (callback, element) {
            var currTime = new Date().getTime();
            var timeToCall = Math.max(0, 16 - (currTime - lastTime));
            var id = window.setTimeout(function () { callback(currTime + timeToCall); },
              timeToCall);
            lastTime = currTime + timeToCall;
            return id;
        };

    if (!window.cancelAnimationFrame)
        window.cancelAnimationFrame = function (id) {
            clearTimeout(id);
        };
}());
;
(function()
{
 "use strict";
 var Global,WebSharper,Cytoscape,Sample,Client,UI,Next,Html,attr,Operators,JavaScript,Pervasives,Doc,AttrProxy,DomUtility,Attrs,Unchecked,Arrays,Client$1,DocNode,Array,Elt,SC$1,Docs,Abbrev,Mailbox,View,Enumerator,DocElemNode,List,T,Var,Docs$1,RunState,NodeSet,An,Snap,Async,Attrs$1,Dyn,SC$2,Fresh,Collections,HashSet,Concurrency,Anims,AppendList,T$1,Seq,SC$3,HashSet$1,SC$4,Scheduler,SC$5,Easing,DomNodes,OperationCanceledException,CancellationTokenSource,HashSetUtil,SC$6,Lazy,IntelliFactory,Runtime;
 Global=window;
 WebSharper=Global.WebSharper=Global.WebSharper||{};
 Cytoscape=WebSharper.Cytoscape=WebSharper.Cytoscape||{};
 Sample=Cytoscape.Sample=Cytoscape.Sample||{};
 Client=Sample.Client=Sample.Client||{};
 UI=WebSharper.UI=WebSharper.UI||{};
 Next=UI.Next=UI.Next||{};
 Html=Next.Html=Next.Html||{};
 attr=Html.attr=Html.attr||{};
 Operators=WebSharper.Operators=WebSharper.Operators||{};
 JavaScript=WebSharper.JavaScript=WebSharper.JavaScript||{};
 Pervasives=JavaScript.Pervasives=JavaScript.Pervasives||{};
 Doc=Next.Doc=Next.Doc||{};
 AttrProxy=Next.AttrProxy=Next.AttrProxy||{};
 DomUtility=Next.DomUtility=Next.DomUtility||{};
 Attrs=Next.Attrs=Next.Attrs||{};
 Unchecked=WebSharper.Unchecked=WebSharper.Unchecked||{};
 Arrays=WebSharper.Arrays=WebSharper.Arrays||{};
 Client$1=Next.Client=Next.Client||{};
 DocNode=Client$1.DocNode=Client$1.DocNode||{};
 Array=Next.Array=Next.Array||{};
 Elt=Next.Elt=Next.Elt||{};
 SC$1=Global.StartupCode$WebSharper_UI_Next$DomUtility=Global.StartupCode$WebSharper_UI_Next$DomUtility||{};
 Docs=Next.Docs=Next.Docs||{};
 Abbrev=Next.Abbrev=Next.Abbrev||{};
 Mailbox=Abbrev.Mailbox=Abbrev.Mailbox||{};
 View=Next.View=Next.View||{};
 Enumerator=WebSharper.Enumerator=WebSharper.Enumerator||{};
 DocElemNode=Next.DocElemNode=Next.DocElemNode||{};
 List=WebSharper.List=WebSharper.List||{};
 T=List.T=List.T||{};
 Var=Next.Var=Next.Var||{};
 Docs$1=Client$1.Docs=Client$1.Docs||{};
 RunState=Docs$1.RunState=Docs$1.RunState||{};
 NodeSet=Docs$1.NodeSet=Docs$1.NodeSet||{};
 An=Next.An=Next.An||{};
 Snap=Next.Snap=Next.Snap||{};
 Async=Abbrev.Async=Abbrev.Async||{};
 Attrs$1=Client$1.Attrs=Client$1.Attrs||{};
 Dyn=Attrs$1.Dyn=Attrs$1.Dyn||{};
 SC$2=Global.StartupCode$WebSharper_UI_Next$Attr_Client=Global.StartupCode$WebSharper_UI_Next$Attr_Client||{};
 Fresh=Abbrev.Fresh=Abbrev.Fresh||{};
 Collections=WebSharper.Collections=WebSharper.Collections||{};
 HashSet=Collections.HashSet=Collections.HashSet||{};
 Concurrency=WebSharper.Concurrency=WebSharper.Concurrency||{};
 Anims=Next.Anims=Next.Anims||{};
 AppendList=Next.AppendList=Next.AppendList||{};
 T$1=Enumerator.T=Enumerator.T||{};
 Seq=WebSharper.Seq=WebSharper.Seq||{};
 SC$3=Global.StartupCode$WebSharper_UI_Next$Animation=Global.StartupCode$WebSharper_UI_Next$Animation||{};
 HashSet$1=Abbrev.HashSet=Abbrev.HashSet||{};
 SC$4=Global.StartupCode$WebSharper_UI_Next$Abbrev=Global.StartupCode$WebSharper_UI_Next$Abbrev||{};
 Scheduler=Concurrency.Scheduler=Concurrency.Scheduler||{};
 SC$5=Global.StartupCode$WebSharper_Main$Concurrency=Global.StartupCode$WebSharper_Main$Concurrency||{};
 Easing=Next.Easing=Next.Easing||{};
 DomNodes=Docs$1.DomNodes=Docs$1.DomNodes||{};
 OperationCanceledException=WebSharper.OperationCanceledException=WebSharper.OperationCanceledException||{};
 CancellationTokenSource=WebSharper.CancellationTokenSource=WebSharper.CancellationTokenSource||{};
 HashSetUtil=Collections.HashSetUtil=Collections.HashSetUtil||{};
 SC$6=Global.StartupCode$WebSharper_UI_Next$AppendList=Global.StartupCode$WebSharper_UI_Next$AppendList||{};
 Lazy=WebSharper.Lazy=WebSharper.Lazy||{};
 IntelliFactory=Global.IntelliFactory;
 Runtime=IntelliFactory&&IntelliFactory.Runtime;
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
    "target-arrow-shape":"triangle",
    "curve-style":"bezier"
   }
  },{
   selector:":selected",
   style:{
    "background-color":"#070",
    "line-color":"#070",
    "target-arrow-color":"#070",
    "text-outline-color":"#000"
   }
  }],r.layout=(r$7={},r$7.name="grid",r$7.rows=1,r$7),r),new Global.cytoscape(a$1));
 };
 attr=Html.attr=Runtime.Class({},null,attr);
 Operators.FailWith=function(msg)
 {
  throw Global.Error(msg);
 };
 Operators.DefaultArg=function(x,d)
 {
  return x==null?d:x.$0;
 };
 Operators.Compare=function(a,b)
 {
  return Unchecked.Compare(a,b);
 };
 Pervasives.NewFromSeq=function(fields)
 {
  var r,e,f;
  r={};
  e=Enumerator.Get(fields);
  try
  {
   while(e.MoveNext())
    {
     f=e.Current();
     r[f[0]]=f[1];
    }
  }
  finally
  {
   if("Dispose"in e)
    e.Dispose();
  }
  return r;
 };
 Doc=Next.Doc=Runtime.Class({},null,Doc);
 Doc.Element=function(name,attr$1,children)
 {
  var attr$2,children$1,a;
  attr$2=AttrProxy.Concat(attr$1);
  children$1=Doc.Concat(children);
  a=DomUtility.CreateElement(name);
  return Elt.New(a,attr$2,children$1);
 };
 Doc.RunById=function(id,tr)
 {
  var m;
  m=DomUtility.Doc().getElementById(id);
  Unchecked.Equals(m,null)?Operators.FailWith("invalid id: "+id):Doc.Run(m,tr);
 };
 Doc.Concat=function(xs)
 {
  var x,d;
  x=Array.ofSeqNonCopying(xs);
  d=Doc.Empty();
  return Array.TreeReduce(d,Doc.Append,x);
 };
 Doc.Run=function(parent,doc)
 {
  var d,st,p,a;
  d=doc.docNode;
  Docs.LinkElement(parent,d);
  st=Docs.CreateRunState(parent,d);
  p=Mailbox.StartProcessor(Docs.PerformAnimatedUpdate(st,d));
  a=doc.updates;
  View.Sink(p,a);
 };
 Doc.Empty=function()
 {
  var a,a$1;
  a=DocNode.EmptyDoc;
  a$1=View.Const();
  return Doc.Mk(a,a$1);
 };
 Doc.Append=function(a,b)
 {
  var x,x$1,y,a$1;
  x=(x$1=a.updates,(y=b.updates,View.Map2Unit(x$1,y)));
  a$1={
   $:0,
   $0:a.docNode,
   $1:b.docNode
  };
  return Doc.Mk(a$1,x);
 };
 Doc.Mk=function(node,updates)
 {
  return new Doc.New(node,updates);
 };
 Doc.New=Runtime.Ctor(function(docNode,updates)
 {
  this.docNode=docNode;
  this.updates=updates;
 },Doc);
 AttrProxy=Next.AttrProxy=Runtime.Class({},null,AttrProxy);
 AttrProxy.Create=function(name,value)
 {
  return Attrs.Static(function(el)
  {
   DomUtility.SetAttr(el,name,value);
  });
 };
 AttrProxy.Concat=function(xs)
 {
  var x,d;
  x=Array.ofSeqNonCopying(xs);
  d=Attrs.EmptyAttr();
  return Array.TreeReduce(d,AttrProxy.Append,x);
 };
 AttrProxy.Append=function(a,b)
 {
  return Attrs.AppendTree(a,b);
 };
 DomUtility.CreateElement=function(name)
 {
  return DomUtility.Doc().createElement(name);
 };
 DomUtility.SetAttr=function(el,name,value)
 {
  el.setAttribute(name,value);
 };
 DomUtility.Doc=function()
 {
  SC$1.$cctor();
  return SC$1.Doc;
 };
 DomUtility.InsertAt=function(parent,pos,node)
 {
  var m,v;
  !(node.parentNode===parent?pos===(m=node.nextSibling,Unchecked.Equals(m,null)?null:m):false)?v=parent.insertBefore(node,pos):void 0;
 };
 DomUtility.RemoveNode=function(parent,el)
 {
  var v;
  if(el.parentNode===parent)
   {
    v=parent.removeChild(el);
   }
 };
 Attrs.Static=function(attr$1)
 {
  return new AttrProxy({
   $:3,
   $0:attr$1
  });
 };
 Attrs.EmptyAttr=function()
 {
  SC$2.$cctor();
  return SC$2.EmptyAttr;
 };
 Attrs.AppendTree=function(a,b)
 {
  var x;
  return a===null?b:b===null?a:(x=new AttrProxy({
   $:2,
   $0:a,
   $1:b
  }),(Attrs.SetFlags(x,Attrs.Flags(a)|Attrs.Flags(b)),x));
 };
 Attrs.Updates=function(dyn)
 {
  var x,d;
  x=dyn.DynNodes;
  d=View.Const();
  return Array.MapTreeReduce(function(x$1)
  {
   return x$1.NChanged();
  },d,View.Map2Unit,x);
 };
 Attrs.SetFlags=function(a,f)
 {
  a.flags=f;
 };
 Attrs.Flags=function(a)
 {
  return(a!==null?a.hasOwnProperty("flags"):false)?a.flags:0;
 };
 Attrs.Insert=function(elem,tree)
 {
  var nodes,oar,arr;
  function loop(node)
  {
   var b;
   if(!(node===null))
    if(node!=null?node.$==1:false)
     nodes.push(node.$0);
    else
     if(node!=null?node.$==2:false)
      {
       b=node.$1;
       loop(node.$0);
       loop(b);
      }
     else
      if(node!=null?node.$==3:false)
       node.$0(elem);
      else
       if(node!=null?node.$==4:false)
        oar.push(node.$0);
  }
  nodes=[];
  oar=[];
  loop(tree);
  arr=nodes.slice(0);
  return Dyn.New(elem,Attrs.Flags(tree),arr,oar.length===0?null:{
   $:1,
   $0:function(el)
   {
    Seq.iter(function(f)
    {
     f(el);
    },oar);
   }
  });
 };
 Attrs.HasChangeAnim=function(attr$1)
 {
  var flag;
  flag=4;
  return(attr$1.DynFlags&flag)===flag;
 };
 Attrs.GetChangeAnim=function(dyn)
 {
  return Attrs.GetAnim(dyn,function($1,$2)
  {
   return $1.NGetChangeAnim($2);
  });
 };
 Attrs.HasEnterAnim=function(attr$1)
 {
  var flag;
  flag=1;
  return(attr$1.DynFlags&flag)===flag;
 };
 Attrs.GetEnterAnim=function(dyn)
 {
  return Attrs.GetAnim(dyn,function($1,$2)
  {
   return $1.NGetEnterAnim($2);
  });
 };
 Attrs.HasExitAnim=function(attr$1)
 {
  var flag;
  flag=2;
  return(attr$1.DynFlags&flag)===flag;
 };
 Attrs.GetExitAnim=function(dyn)
 {
  return Attrs.GetAnim(dyn,function($1,$2)
  {
   return $1.NGetExitAnim($2);
  });
 };
 Attrs.GetAnim=function(dyn,f)
 {
  var m;
  return An.Concat((m=function(n)
  {
   return f(n,dyn.DynElem);
  },function(a)
  {
   return Arrays.map(m,a);
  }(dyn.DynNodes)));
 };
 Attrs.Sync=function(elem,dyn)
 {
  var a;
  a=function(d)
  {
   d.NSync(elem);
  };
  (function(a$1)
  {
   Arrays.iter(a,a$1);
  }(dyn.DynNodes));
 };
 Unchecked.Equals=function(a,b)
 {
  var m,eqR,k,k$1;
  if(a===b)
   return true;
  else
   {
    m=typeof a;
    if(m=="object")
    {
     if(((a===null?true:a===void 0)?true:b===null)?true:b===void 0)
      return false;
     else
      if("Equals"in a)
       return a.Equals(b);
      else
       if(a instanceof Global.Array?b instanceof Global.Array:false)
        return Unchecked.arrayEquals(a,b);
       else
        if(a instanceof Global.Date?b instanceof Global.Date:false)
         return Unchecked.dateEquals(a,b);
        else
         {
          eqR=[true];
          for(var k$2 in a)if(function(k$3)
          {
           eqR[0]=!a.hasOwnProperty(k$3)?true:b.hasOwnProperty(k$3)?Unchecked.Equals(a[k$3],b[k$3]):false;
           return!eqR[0];
          }(k$2))
           break;
          if(eqR[0])
           {
            for(var k$3 in b)if(function(k$4)
            {
             eqR[0]=!b.hasOwnProperty(k$4)?true:a.hasOwnProperty(k$4);
             return!eqR[0];
            }(k$3))
             break;
           }
          return eqR[0];
         }
    }
    else
     return m=="function"?"$Func"in a?a.$Func===b.$Func?a.$Target===b.$Target:false:("$Invokes"in a?"$Invokes"in b:false)?Unchecked.arrayEquals(a.$Invokes,b.$Invokes):false:false;
   }
 };
 Unchecked.arrayEquals=function(a,b)
 {
  var eq,i;
  if(Arrays.length(a)===Arrays.length(b))
   {
    eq=true;
    i=0;
    while(eq?i<Arrays.length(a):false)
     {
      !Unchecked.Equals(Arrays.get(a,i),Arrays.get(b,i))?eq=false:void 0;
      i=i+1;
     }
    return eq;
   }
  else
   return false;
 };
 Unchecked.dateEquals=function(a,b)
 {
  return a.getTime()===b.getTime();
 };
 Unchecked.Hash=function(o)
 {
  var m;
  m=typeof o;
  return m=="function"?0:m=="boolean"?o?1:0:m=="number"?o:m=="string"?Unchecked.hashString(o):m=="object"?o==null?0:o instanceof Global.Array?Unchecked.hashArray(o):Unchecked.hashObject(o):0;
 };
 Unchecked.hashString=function(s)
 {
  var hash,i,$1;
  if(s===null)
   return 0;
  else
   {
    hash=5381;
    for(i=0,$1=s.length-1;i<=$1;i++)hash=Unchecked.hashMix(hash,s.charCodeAt(i)<<0);
    return hash;
   }
 };
 Unchecked.hashArray=function(o)
 {
  var h,i,$1;
  h=-34948909;
  for(i=0,$1=Arrays.length(o)-1;i<=$1;i++)h=Unchecked.hashMix(h,Unchecked.Hash(Arrays.get(o,i)));
  return h;
 };
 Unchecked.hashObject=function(o)
 {
  var _,h,k;
  if("GetHashCode"in o)
   return o.GetHashCode();
  else
   {
    _=Unchecked.hashMix;
    h=[0];
    for(var k$1 in o)if(function(key)
    {
     h[0]=_(_(h[0],Unchecked.hashString(key)),Unchecked.Hash(o[key]));
     return false;
    }(k$1))
     break;
    return h[0];
   }
 };
 Unchecked.Compare=function(a,b)
 {
  var $1,m,$2,cmp,k,k$1;
  if(a===b)
   return 0;
  else
   {
    m=typeof a;
    switch(m=="function"?1:m=="boolean"?2:m=="number"?2:m=="string"?2:m=="object"?3:0)
    {
     case 0:
      return typeof b=="undefined"?0:-1;
      break;
     case 1:
      return Operators.FailWith("Cannot compare function values.");
      break;
     case 2:
      return a<b?-1:1;
      break;
     case 3:
      if(a===null)
       $2=-1;
      else
       if(b===null)
        $2=1;
       else
        if("CompareTo"in a)
         $2=a.CompareTo(b);
        else
         if("CompareTo0"in a)
          $2=a.CompareTo0(b);
         else
          if(a instanceof Global.Array?b instanceof Global.Array:false)
           $2=Unchecked.compareArrays(a,b);
          else
           if(a instanceof Global.Date?b instanceof Global.Date:false)
            $2=Unchecked.compareDates(a,b);
           else
            {
             cmp=[0];
             for(var k$2 in a)if(function(k$3)
             {
              return!a.hasOwnProperty(k$3)?false:!b.hasOwnProperty(k$3)?(cmp[0]=1,true):(cmp[0]=Unchecked.Compare(a[k$3],b[k$3]),cmp[0]!==0);
             }(k$2))
              break;
             if(cmp[0]===0)
              {
               for(var k$3 in b)if(function(k$4)
               {
                return!b.hasOwnProperty(k$4)?false:!a.hasOwnProperty(k$4)?(cmp[0]=-1,true):false;
               }(k$3))
                break;
              }
             $2=cmp[0];
            }
      return $2;
      break;
    }
   }
 };
 Unchecked.hashMix=function(x,y)
 {
  return(x<<5)+x+y;
 };
 Unchecked.compareArrays=function(a,b)
 {
  var cmp,i;
  if(Arrays.length(a)<Arrays.length(b))
   return -1;
  else
   if(Arrays.length(a)>Arrays.length(b))
    return 1;
   else
    {
     cmp=0;
     i=0;
     while(cmp===0?i<Arrays.length(a):false)
      {
       cmp=Unchecked.Compare(Arrays.get(a,i),Arrays.get(b,i));
       i=i+1;
      }
     return cmp;
    }
 };
 Unchecked.compareDates=function(a,b)
 {
  return Operators.Compare(a.getTime(),b.getTime());
 };
 Arrays.get=function(arr,n)
 {
  Arrays.checkBounds(arr,n);
  return arr[n];
 };
 Arrays.length=function(arr)
 {
  return arr.dims===2?arr.length*arr.length:arr.length;
 };
 Arrays.checkBounds=function(arr,n)
 {
  if(n<0?true:n>=arr.length)
   Operators.FailWith("Index was outside the bounds of the array.");
 };
 Arrays.set=function(arr,n,x)
 {
  Arrays.checkBounds(arr,n);
  arr[n]=x;
 };
 DocNode.EmptyDoc={
  $:3
 };
 Array.ofSeqNonCopying=function(xs)
 {
  var q,o,v;
  if(xs instanceof Global.Array)
   return xs;
  else
   if(xs instanceof T)
    return Arrays.ofList(xs);
   else
    if(xs===null)
     return[];
    else
     {
      q=[];
      o=Enumerator.Get(xs);
      try
      {
       while(o.MoveNext())
        {
         v=q.push(o.Current());
        }
       return q;
      }
      finally
      {
       if("Dispose"in o)
        o.Dispose();
      }
     }
 };
 Array.TreeReduce=function(defaultValue,reduction,array)
 {
  var l;
  function loop(off,len)
  {
   var $1,l2;
   return len<=0?defaultValue:(len===1?(off>=0?off<l:false)?true:false:false)?Arrays.get(array,off):(l2=len/2>>0,reduction(loop(off,l2),loop(off+l2,len-l2)));
  }
  l=Arrays.length(array);
  return loop(0,l);
 };
 Array.MapTreeReduce=function(mapping,defaultValue,reduction,array)
 {
  var l;
  function loop(off,len)
  {
   var $1,l2;
   return len<=0?defaultValue:(len===1?(off>=0?off<l:false)?true:false:false)?mapping(Arrays.get(array,off)):(l2=len/2>>0,reduction(loop(off,l2),loop(off+l2,len-l2)));
  }
  l=Arrays.length(array);
  return loop(0,l);
 };
 Elt=Next.Elt=Runtime.Class({},Doc,Elt);
 Elt.New=function(el,attr$1,children)
 {
  var node,rvUpdates,attrUpdates,updates,a;
  node=Docs.CreateElemNode(el,attr$1,children.docNode);
  rvUpdates=Var.Create$1(children.updates);
  attrUpdates=Attrs.Updates(node.Attr);
  updates=(a=rvUpdates.v,View.Bind(function(a$1)
  {
   return View.Map2Unit(attrUpdates,a$1);
  },a));
  return new Elt.New$1({
   $:1,
   $0:node
  },updates,el,rvUpdates);
 };
 Elt.New$1=Runtime.Ctor(function(docNode,updates,elt,rvUpdates)
 {
  Doc.New.call(this,docNode,updates);
  this.docNode$1=docNode;
  this.updates$1=updates;
  this.elt=elt;
  this.rvUpdates=rvUpdates;
 },Elt);
 SC$1.$cctor=Runtime.Cctor(function()
 {
  SC$1.Doc=Global.document;
  SC$1.$cctor=Global.ignore;
 });
 Docs.LinkElement=function(el,children)
 {
  var v;
  v=Docs.InsertDoc(el,children,null);
 };
 Docs.CreateRunState=function(parent,doc)
 {
  return RunState.New(NodeSet.get_Empty(),Docs.CreateElemNode(parent,Attrs.EmptyAttr(),doc));
 };
 Docs.PerformAnimatedUpdate=function(st,doc)
 {
  var a;
  return An.get_UseAnimations()?Concurrency.Delay(function()
  {
   var cur,change,enter,exit,x;
   cur=NodeSet.FindAll(doc);
   change=Docs.ComputeChangeAnim(st,cur);
   enter=Docs.ComputeEnterAnim(st,cur);
   exit=Docs.ComputeExitAnim(st,cur);
   x=An.Play(An.Append(change,exit));
   return Concurrency.Bind(x,function()
   {
    Docs.SyncElemNode(st.Top);
    return Concurrency.Bind(An.Play(enter),function()
    {
     st.PreviousNodes=cur;
     return Concurrency.Return(null);
    });
   });
  }):(a=function(ok)
  {
   var v;
   v=Global.requestAnimationFrame(function()
   {
    Docs.SyncElemNode(st.Top);
    ok();
   });
  },Concurrency.FromContinuations(function($1,$2,$3)
  {
   return a.apply(null,[$1,$2,$3]);
  }));
 };
 Docs.CreateElemNode=function(el,attr$1,children)
 {
  var attr$2;
  Docs.LinkElement(el,children);
  attr$2=Attrs.Insert(el,attr$1);
  return DocElemNode.New(attr$2,children,null,el,Fresh.Int(),Runtime.GetOptional(attr$2.OnAfterRender));
 };
 Docs.InsertDoc=function(parent,doc,pos)
 {
  var e,d,t,t$1,t$2,b,a;
  return doc.$==1?(e=doc.$0,Docs.InsertNode(parent,e.El,pos)):doc.$==2?(d=doc.$0,(d.Dirty=false,Docs.InsertDoc(parent,d.Current,pos))):doc.$==3?pos:doc.$==4?(t=doc.$0,Docs.InsertNode(parent,t.Text,pos)):doc.$==5?(t$1=doc.$0,Docs.InsertNode(parent,t$1,pos)):doc.$==6?(t$2=doc.$0,Arrays.foldBack(function($1,$2)
  {
   return $1.constructor===Global.Object?Docs.InsertDoc(parent,$1,$2):Docs.InsertNode(parent,$1,$2);
  },t$2.Els,pos)):(b=doc.$1,(a=doc.$0,Docs.InsertDoc(parent,a,Docs.InsertDoc(parent,b,pos))));
 };
 Docs.ComputeChangeAnim=function(st,cur)
 {
  var relevant,a,m,a$1,a$2;
  relevant=(a=function(n)
  {
   return Attrs.HasChangeAnim(n.Attr);
  },function(a$3)
  {
   return NodeSet.Filter(a,a$3);
  });
  return An.Concat((m=function(n)
  {
   return Attrs.GetChangeAnim(n.Attr);
  },function(a$3)
  {
   return Arrays.map(m,a$3);
  }(NodeSet.ToArray((a$1=relevant(st.PreviousNodes),(a$2=relevant(cur),NodeSet.Intersect(a$1,a$2)))))));
 };
 Docs.ComputeEnterAnim=function(st,cur)
 {
  var m,x,a,a$1;
  return An.Concat((m=function(n)
  {
   return Attrs.GetEnterAnim(n.Attr);
  },function(a$2)
  {
   return Arrays.map(m,a$2);
  }(NodeSet.ToArray((x=(a=function(n)
  {
   return Attrs.HasEnterAnim(n.Attr);
  },function(a$2)
  {
   return NodeSet.Filter(a,a$2);
  }(cur)),(a$1=st.PreviousNodes,NodeSet.Except(a$1,x)))))));
 };
 Docs.ComputeExitAnim=function(st,cur)
 {
  var m,a,a$1;
  return An.Concat((m=function(n)
  {
   return Attrs.GetExitAnim(n.Attr);
  },function(a$2)
  {
   return Arrays.map(m,a$2);
  }(NodeSet.ToArray((a=(a$1=function(n)
  {
   return Attrs.HasExitAnim(n.Attr);
  },function(a$2)
  {
   return NodeSet.Filter(a$1,a$2);
  }(st.PreviousNodes)),NodeSet.Except(cur,a))))));
 };
 Docs.SyncElemNode=function(el)
 {
  Docs.SyncElement(el);
  Docs.Sync(el.Children);
  Docs.AfterRender(el);
 };
 Docs.InsertNode=function(parent,node,pos)
 {
  DomUtility.InsertAt(parent,pos,node);
  return node;
 };
 Docs.SyncElement=function(el)
 {
  function hasDirtyChildren(el$1)
  {
   function dirty(doc)
   {
    var b,d,t;
    return doc.$==0?(b=doc.$1,dirty(doc.$0)?true:dirty(b)):doc.$==2?(d=doc.$0,d.Dirty?true:dirty(d.Current)):doc.$==6?(t=doc.$0,t.Dirty?true:Arrays.exists(hasDirtyChildren,t.Holes)):false;
   }
   return dirty(el$1.Children);
  }
  Attrs.Sync(el.El,el.Attr);
  hasDirtyChildren(el)?Docs.DoSyncElement(el):void 0;
 };
 Docs.Sync=function(doc)
 {
  var n,d,t,b,a;
  if(doc.$==1)
   Docs.SyncElemNode(doc.$0);
  else
   if(doc.$==2)
    {
     n=doc.$0;
     Docs.Sync(n.Current);
    }
   else
    if(doc.$==3)
     ;
    else
     if(doc.$==5)
      ;
     else
      if(doc.$==4)
       {
        d=doc.$0;
        d.Dirty?(d.Text.nodeValue=d.Value,d.Dirty=false):void 0;
       }
      else
       if(doc.$==6)
        {
         t=doc.$0;
         Arrays.iter(function(e)
         {
          Docs.SyncElemNode(e);
         },t.Holes);
         Arrays.iter(function(t$1)
         {
          var e,a$1;
          e=t$1[0];
          a$1=t$1[1];
          Attrs.Sync(e,a$1);
         },t.Attrs);
         Docs.AfterRender(t);
        }
       else
        {
         b=doc.$1;
         a=doc.$0;
         Docs.Sync(a);
         Docs.Sync(b);
        }
 };
 Docs.AfterRender=function(el)
 {
  var m,f;
  m=Runtime.GetOptional(el.Render);
  (m!=null?m.$==1:false)?(f=m.$0,f(el.El),Runtime.SetOptional(el,"Render",null)):void 0;
 };
 Docs.DoSyncElement=function(el)
 {
  var parent,ch,x,a,a$1,a$2,a$3,p,m,v;
  function ins(doc,pos)
  {
   var d,t;
   return doc.$==1?doc.$0.El:doc.$==2?(d=doc.$0,d.Dirty?(d.Dirty=false,Docs.InsertDoc(parent,d.Current,pos)):ins(d.Current,pos)):doc.$==3?pos:doc.$==4?doc.$0.Text:doc.$==5?doc.$0:doc.$==6?(t=doc.$0,(t.Dirty?t.Dirty=false:void 0,Arrays.foldBack(function($1,$2)
   {
    return $1.constructor===Global.Object?ins($1,$2):$1;
   },t.Els,pos))):ins(doc.$0,ins(doc.$1,pos));
  }
  parent=el.El;
  ch=DomNodes.DocChildren(el);
  x=(a=(a$1=el.El,(a$2=Runtime.GetOptional(el.Delimiters),DomNodes.Children(a$1,a$2))),DomNodes.Except(ch,a));
  a$3=(p=el.El,function(e)
  {
   DomUtility.RemoveNode(p,e);
  });
  DomNodes.Iter(a$3,x);
  v=ins(el.Children,(m=Runtime.GetOptional(el.Delimiters),(m!=null?m.$==1:false)?m.$0[1]:null));
 };
 Mailbox.StartProcessor=function(procAsync)
 {
  var st;
  function work()
  {
   return Concurrency.Delay(function()
   {
    return Concurrency.Bind(procAsync,function()
    {
     var m,x;
     m=st[0];
     return Unchecked.Equals(m,1)?(st[0]=0,Concurrency.Return(null)):Unchecked.Equals(m,2)?(st[0]=1,x=work(),Concurrency.Bind(x,function()
     {
      return Concurrency.Return(null);
     })):Concurrency.Return(null);
    });
   });
  }
  st=[0];
  return function()
  {
   var m,computation;
   m=st[0];
   Unchecked.Equals(m,0)?(st[0]=1,computation=work(),Concurrency.Start(computation,null)):Unchecked.Equals(m,1)?st[0]=2:void 0;
  };
 };
 View.Sink=function(act,a)
 {
  function loop()
  {
   var sn;
   sn=a();
   Snap.When(sn,act,function()
   {
    Async.Schedule(loop);
   });
  }
  Async.Schedule(loop);
 };
 View.Const=function(x)
 {
  var o;
  o=Snap.CreateForever(x);
  return function()
  {
   return o;
  };
 };
 View.Map2Unit=function(a,a$1)
 {
  return View.CreateLazy(function()
  {
   var s1,s2;
   s1=a();
   s2=a$1();
   return Snap.Map2Unit(s1,s2);
  });
 };
 View.Bind=function(fn,view)
 {
  return View.Join(View.Map(fn,view));
 };
 View.CreateLazy=function(observe)
 {
  var lv;
  lv={
   c:null,
   o:observe
  };
  return function()
  {
   var c;
   c=lv.c;
   return c===null?(c=lv.o(),lv.c=c,Snap.IsForever(c)?lv.o=null:Snap.WhenObsolete(c,function()
   {
    lv.c=null;
   }),c):c;
  };
 };
 View.Join=function(a)
 {
  return View.CreateLazy(function()
  {
   return Snap.Join(a());
  });
 };
 View.Map=function(fn,a)
 {
  return View.CreateLazy(function()
  {
   var a$1;
   a$1=a();
   return Snap.Map(fn,a$1);
  });
 };
 Enumerator.Get=function(x)
 {
  return x instanceof Global.Array?Enumerator.ArrayEnumerator(x):Unchecked.Equals(typeof x,"string")?Enumerator.StringEnumerator(x):x.GetEnumerator();
 };
 Enumerator.ArrayEnumerator=function(s)
 {
  return new T$1.New(0,null,function(e)
  {
   var i;
   i=e.s;
   return i<Arrays.length(s)?(e.c=Arrays.get(s,i),e.s=i+1,true):false;
  },void 0);
 };
 Enumerator.StringEnumerator=function(s)
 {
  return new T$1.New(0,null,function(e)
  {
   var i;
   i=e.s;
   return i<s.length?(e.c=s.charCodeAt(i),e.s=i+1,true):false;
  },void 0);
 };
 DocElemNode=Next.DocElemNode=Runtime.Class({
  Equals:function(o)
  {
   return this.ElKey===o.ElKey;
  },
  GetHashCode:function()
  {
   return this.ElKey;
  }
 },null,DocElemNode);
 DocElemNode.New=function(Attr,Children,Delimiters,El,ElKey,Render)
 {
  var $1;
  return new DocElemNode(($1={
   Attr:Attr,
   Children:Children,
   El:El,
   ElKey:ElKey
  },(Runtime.SetOptional($1,"Delimiters",Delimiters),Runtime.SetOptional($1,"Render",Render),$1)));
 };
 T=List.T=Runtime.Class({
  GetEnumerator:function()
  {
   return new T$1.New(this,null,function(e)
   {
    var m,xs;
    m=e.s;
    return m.$==0?false:(xs=m.$1,(e.c=m.$0,e.s=xs,true));
   },void 0);
  }
 },null,T);
 Arrays.ofList=function(xs)
 {
  var l,q;
  q=[];
  l=xs;
  while(!(l.$==0))
   {
    q.push(List.head(l));
    l=List.tail(l);
   }
  return q;
 };
 Arrays.foldBack=function(f,arr,zero)
 {
  var acc,$1,len,i,$2;
  acc=zero;
  len=arr.length;
  for(i=1,$2=len;i<=$2;i++)acc=f(arr[len-i],acc);
  return acc;
 };
 Arrays.iter=function(f,arr)
 {
  var i,$1;
  for(i=0,$1=arr.length-1;i<=$1;i++)f(arr[i]);
 };
 Arrays.map=function(f,arr)
 {
  var a,r,i,$1;
  r=(a=arr.length,new Global.Array(a));
  for(i=0,$1=arr.length-1;i<=$1;i++)r[i]=f(arr[i]);
  return r;
 };
 Arrays.choose=function(f,arr)
 {
  var q,i,$1,m;
  q=[];
  for(i=0,$1=arr.length-1;i<=$1;i++){
   m=f(arr[i]);
   m==null?void 0:q.push(m.$0);
  }
  return q;
 };
 Arrays.exists=function(f,x)
 {
  var e,i,$1,l;
  e=false;
  i=0;
  l=Arrays.length(x);
  while(!e?i<l:false)
   if(f(x[i]))
    e=true;
   else
    i=i+1;
  return e;
 };
 Arrays.filter=function(f,arr)
 {
  var r,i,$1;
  r=[];
  for(i=0,$1=arr.length-1;i<=$1;i++)if(f(arr[i]))
   r.push(arr[i]);
  return r;
 };
 Arrays.create=function(size,value)
 {
  var r,i,$1;
  r=new Global.Array(size);
  for(i=0,$1=size-1;i<=$1;i++)r[i]=value;
  return r;
 };
 Arrays.init=function(size,f)
 {
  var r,i,$1;
  size<0?Operators.FailWith("Negative size given."):null;
  r=new Global.Array(size);
  for(i=0,$1=size-1;i<=$1;i++)r[i]=f(i);
  return r;
 };
 Arrays.forall=function(f,x)
 {
  var a,i,$1,l;
  a=true;
  i=0;
  l=Arrays.length(x);
  while(a?i<l:false)
   if(f(x[i]))
    i=i+1;
   else
    a=false;
  return a;
 };
 Var.Create$1=function(v)
 {
  var _var;
  _var=null;
  _var=Var.New(false,v,Snap.CreateWithValue(v),Fresh.Int(),function()
  {
   return _var.s;
  });
  return _var;
 };
 Var=Next.Var=Runtime.Class({},null,Var);
 Var.New=function(Const,Current,Snap$1,Id,VarView)
 {
  return new Var({
   o:Const,
   c:Current,
   s:Snap$1,
   i:Id,
   v:VarView
  });
 };
 RunState.New=function(PreviousNodes,Top)
 {
  return{
   PreviousNodes:PreviousNodes,
   Top:Top
  };
 };
 NodeSet.get_Empty=function()
 {
  return{
   $:0,
   $0:new HashSet.New$3()
  };
 };
 NodeSet.FindAll=function(doc)
 {
  var q;
  function loop(node)
  {
   var b,t,a;
   if(node.$==0)
    {
     b=node.$1;
     loop(node.$0);
     loop(b);
    }
   else
    if(node.$==1)
     loopEN(node.$0);
    else
     if(node.$==2)
      loop(node.$0.Current);
     else
      if(node.$==6)
       {
        t=node.$0;
        a=t.Holes;
        Arrays.iter(loopEN,a);
       }
  }
  function loopEN(el)
  {
   q.push(el);
   loop(el.Children);
  }
  q=[];
  loop(doc);
  return{
   $:0,
   $0:new HashSet.New$2(q)
  };
 };
 NodeSet.Filter=function(f,a)
 {
  var set;
  set=a.$0;
  return{
   $:0,
   $0:HashSet$1.Filter(f,set)
  };
 };
 NodeSet.Intersect=function(a,a$1)
 {
  var a$2,b;
  a$2=a.$0;
  b=a$1.$0;
  return{
   $:0,
   $0:HashSet$1.Intersect(a$2,b)
  };
 };
 NodeSet.ToArray=function(a)
 {
  return HashSet$1.ToArray(a.$0);
 };
 NodeSet.Except=function(a,a$1)
 {
  var excluded,included;
  excluded=a.$0;
  included=a$1.$0;
  return{
   $:0,
   $0:HashSet$1.Except(excluded,included)
  };
 };
 An.get_UseAnimations=function()
 {
  return Anims.UseAnimations();
 };
 An.Play=function(anim)
 {
  return Concurrency.Delay(function()
  {
   var x,a;
   x=(a=function()
   {
   },function(a$1)
   {
    return An.Run(a,a$1);
   }(Anims.Actions(anim)));
   return Concurrency.Bind(x,function()
   {
    Anims.Finalize(anim);
    return Concurrency.Return(null);
   });
  });
 };
 An.Append=function(a,a$1)
 {
  var a$2,b;
  a$2=a.$0;
  b=a$1.$0;
  return{
   $:0,
   $0:AppendList.Append(a$2,b)
  };
 };
 An.Concat=function(xs)
 {
  return{
   $:0,
   $0:AppendList.Concat(Seq.map(Anims.List,xs))
  };
 };
 An.Run=function(k,anim)
 {
  var dur,a;
  dur=anim.Duration;
  a=function(ok)
  {
   var v;
   function loop(start,now)
   {
    var t,v$1;
    t=now-start;
    anim.Compute(t);
    k();
    return t<=dur?(v$1=Global.requestAnimationFrame(function(t$1)
    {
     loop(start,t$1);
    }),void 0):ok();
   }
   v=Global.requestAnimationFrame(function(t)
   {
    loop(t,t);
   });
  };
  return Concurrency.FromContinuations(function($1,$2,$3)
  {
   return a.apply(null,[$1,$2,$3]);
  });
 };
 Snap.When=function(snap,avail,obsolete)
 {
  var m,v,q2;
  m=snap.s;
  m.$==1?obsolete():m.$==2?(v=m.$0,m.$1.push(obsolete),avail(v)):m.$==3?(q2=m.$1,m.$0.push(avail),q2.push(obsolete)):avail(m.$0);
 };
 Snap.CreateForever=function(v)
 {
  return Snap.Make({
   $:0,
   $0:v
  });
 };
 Snap.Map2Unit=function(sn1,sn2)
 {
  var $1,$2,res,obs;
  function cont()
  {
   var $3,$4,f1,f2;
   if(!Snap.IsDone(res))
    {
     $3=Snap.ValueAndForever(sn1);
     $4=Snap.ValueAndForever(sn2);
     ($3!=null?$3.$==1:false)?($4!=null?$4.$==1:false)?(f1=$3.$0[1],f2=$4.$0[1],(f1?f2:false)?Snap.MarkForever(res,null):Snap.MarkReady(res,null)):void 0:void 0;
    }
  }
  $1=sn1.s;
  $2=sn2.s;
  return $1.$==0?$2.$==0?Snap.CreateForever():sn2:$2.$==0?sn1:(res=Snap.Create(),(obs=Snap.Obs(res),(Snap.When(sn1,cont,obs),Snap.When(sn2,cont,obs),res)));
 };
 Snap.CreateWithValue=function(v)
 {
  return Snap.Make({
   $:2,
   $0:v,
   $1:[]
  });
 };
 Snap.Make=function(st)
 {
  return{
   s:st
  };
 };
 Snap.IsForever=function(snap)
 {
  return snap.s.$==0?true:false;
 };
 Snap.WhenObsolete=function(snap,obsolete)
 {
  var m;
  m=snap.s;
  m.$==1?obsolete():m.$==2?m.$1.push(obsolete):m.$==3?m.$1.push(obsolete):void 0;
 };
 Snap.Create=function()
 {
  return Snap.Make({
   $:3,
   $0:[],
   $1:[]
  });
 };
 Snap.Obs=function(sn)
 {
  return function()
  {
   Snap.MarkObsolete(sn);
  };
 };
 Snap.IsDone=function(snap)
 {
  var m;
  m=snap.s;
  return m.$==0?true:m.$==2?true:false;
 };
 Snap.ValueAndForever=function(snap)
 {
  var m;
  m=snap.s;
  return m.$==0?{
   $:1,
   $0:[m.$0,true]
  }:m.$==2?{
   $:1,
   $0:[m.$0,false]
  }:null;
 };
 Snap.MarkForever=function(sn,v)
 {
  var m,q;
  m=sn.s;
  m.$==3?(q=m.$0,sn.s={
   $:0,
   $0:v
  },Seq.iter(function(k)
  {
   k(v);
  },q)):void 0;
 };
 Snap.MarkReady=function(sn,v)
 {
  var m,q2,q1;
  m=sn.s;
  m.$==3?(q2=m.$1,q1=m.$0,sn.s={
   $:2,
   $0:v,
   $1:q2
  },Seq.iter(function(k)
  {
   k(v);
  },q1)):void 0;
 };
 Snap.Join=function(snap)
 {
  var res,obs;
  res=Snap.Create();
  obs=Snap.Obs(res);
  Snap.When(snap,function(x)
  {
   var y;
   y=x();
   Snap.When(y,function(v)
   {
    if(Snap.IsForever(y)?Snap.IsForever(snap):false)
     Snap.MarkForever(res,v);
    else
     Snap.MarkReady(res,v);
   },obs);
  },obs);
  return res;
 };
 Snap.Map=function(fn,sn)
 {
  var m,x,res,g;
  m=sn.s;
  return m.$==0?(x=m.$0,Snap.CreateForever(fn(x))):(res=Snap.Create(),(Snap.When(sn,(g=function(v)
  {
   Snap.MarkDone(res,sn,v);
  },function(x$1)
  {
   return g(fn(x$1));
  }),Snap.Obs(res)),res));
 };
 Snap.MarkObsolete=function(sn)
 {
  var m,$1;
  m=sn.s;
  (m.$==1?true:m.$==2?($1=m.$1,false):m.$==3?($1=m.$1,false):true)?void 0:(sn.s={
   $:1
  },Seq.iter(function(k)
  {
   k();
  },$1));
 };
 Snap.MarkDone=function(res,sn,v)
 {
  if(Snap.IsForever(sn))
   Snap.MarkForever(res,v);
  else
   Snap.MarkReady(res,v);
 };
 Async.Schedule=function(f)
 {
  Concurrency.Start(Concurrency.Delay(function()
  {
   f();
   return Concurrency.Return(null);
  }),null);
 };
 Dyn.New=function(DynElem,DynFlags,DynNodes,OnAfterRender)
 {
  var $1;
  $1={
   DynElem:DynElem,
   DynFlags:DynFlags,
   DynNodes:DynNodes
  };
  Runtime.SetOptional($1,"OnAfterRender",OnAfterRender);
  return $1;
 };
 List.head=function(l)
 {
  return l.$==1?l.$0:List.listEmpty();
 };
 List.tail=function(l)
 {
  return l.$==1?l.$1:List.listEmpty();
 };
 List.listEmpty=function()
 {
  return Operators.FailWith("The input list was empty.");
 };
 SC$2.$cctor=Runtime.Cctor(function()
 {
  SC$2.EmptyAttr=null;
  SC$2.$cctor=Global.ignore;
 });
 Fresh.Int=function()
 {
  Fresh.set_counter(Fresh.counter()+1);
  return Fresh.counter();
 };
 Fresh.set_counter=function($1)
 {
  SC$4.$cctor();
  SC$4.counter=$1;
 };
 Fresh.counter=function()
 {
  SC$4.$cctor();
  return SC$4.counter;
 };
 HashSet=Collections.HashSet=Runtime.Class({
  add:function(item)
  {
   var h,arr,v;
   h=this.hash(item);
   arr=this.data[h];
   return arr==null?(this.data[h]=[item],this.count=this.count+1,true):this.arrContains(item,arr)?false:(v=arr.push(item),this.count=this.count+1,true);
  },
  IntersectWith:function(xs)
  {
   var other,all,i,$1,item,v;
   other=new HashSet.New$4(xs,this.equals,this.hash);
   all=HashSetUtil.concat(this.data);
   for(i=0,$1=all.length-1;i<=$1;i++){
    item=all[i];
    !other.Contains(item)?v=this.Remove(item):void 0;
   }
  },
  get_Count:function()
  {
   return this.count;
  },
  CopyTo:function(arr)
  {
   var i,all,i$1,$1;
   i=0;
   all=HashSetUtil.concat(this.data);
   for(i$1=0,$1=all.length-1;i$1<=$1;i$1++)Arrays.set(arr,i$1,all[i$1]);
  },
  ExceptWith:function(xs)
  {
   var e,v;
   e=Enumerator.Get(xs);
   try
   {
    while(e.MoveNext())
     {
      v=this.Remove(e.Current());
     }
   }
   finally
   {
    if("Dispose"in e)
     e.Dispose();
   }
  },
  arrContains:function(item,arr)
  {
   var c,i,$1,l;
   c=true;
   i=0;
   l=arr.length;
   while(c?i<l:false)
    if(this.equals.apply(null,[arr[i],item]))
     c=false;
    else
     i=i+1;
   return!c;
  },
  Contains:function(item)
  {
   var arr;
   arr=this.data[this.hash(item)];
   return arr==null?false:this.arrContains(item,arr);
  },
  Remove:function(item)
  {
   var arr;
   arr=this.data[this.hash(item)];
   return arr==null?false:this.arrRemove(item,arr)?(this.count=this.count-1,true):false;
  },
  arrRemove:function(item,arr)
  {
   var c,i,$1,l,v;
   c=true;
   i=0;
   l=arr.length;
   while(c?i<l:false)
    if(this.equals.apply(null,[arr[i],item]))
     {
      v=arr.splice.apply(arr,[i,1].concat([]));
      c=false;
     }
    else
     i=i+1;
   return!c;
  },
  GetEnumerator:function()
  {
   return Enumerator.Get(HashSetUtil.concat(this.data));
  }
 },null,HashSet);
 HashSet.New$3=Runtime.Ctor(function()
 {
  HashSet.New$4.call(this,[],Unchecked.Equals,Unchecked.Hash);
 },HashSet);
 HashSet.New$2=Runtime.Ctor(function(init)
 {
  HashSet.New$4.call(this,init,Unchecked.Equals,Unchecked.Hash);
 },HashSet);
 HashSet.New$4=Runtime.Ctor(function(init,equals,hash)
 {
  var e,v;
  this.equals=equals;
  this.hash=hash;
  this.data=Global.Array.prototype.constructor.apply(Global.Array,[]);
  this.count=0;
  e=Enumerator.Get(init);
  try
  {
   while(e.MoveNext())
    {
     v=this.add(e.Current());
    }
  }
  finally
  {
   if("Dispose"in e)
    e.Dispose();
  }
 },HashSet);
 Concurrency.Delay=function(mk)
 {
  return function(c)
  {
   try
   {
    (mk(null))(c);
   }
   catch(e)
   {
    c.k({
     $:1,
     $0:e
    });
   }
  };
 };
 Concurrency.Bind=function(r,f)
 {
  return Concurrency.checkCancel(function(c)
  {
   r({
    k:function(a)
    {
     var x;
     if(a.$==0)
      {
       x=a.$0;
       Concurrency.scheduler().Fork(function()
       {
        try
        {
         (f(x))(c);
        }
        catch(e)
        {
         c.k({
          $:1,
          $0:e
         });
        }
       });
      }
     else
      Concurrency.scheduler().Fork(function()
      {
       c.k(a);
      });
    },
    ct:c.ct
   });
  });
 };
 Concurrency.Return=function(x)
 {
  return function(c)
  {
   c.k({
    $:0,
    $0:x
   });
  };
 };
 Concurrency.Start=function(c,ctOpt)
 {
  var ct;
  ct=Operators.DefaultArg(ctOpt,(Concurrency.defCTS())[0]);
  Concurrency.scheduler().Fork(function()
  {
   if(!ct.c)
    c({
     k:function(a)
     {
      if(a.$==1)
       Concurrency.UncaughtAsyncError(a.$0);
     },
     ct:ct
    });
  });
 };
 Concurrency.FromContinuations=function(subscribe)
 {
  return function(c)
  {
   var continued,once;
   continued=[false];
   once=function(cont)
   {
    if(continued[0])
     Operators.FailWith("A continuation provided by Async.FromContinuations was invoked multiple times");
    else
     {
      continued[0]=true;
      Concurrency.scheduler().Fork(cont);
     }
   };
   subscribe(function(a)
   {
    once(function()
    {
     c.k({
      $:0,
      $0:a
     });
    });
   },function(e)
   {
    once(function()
    {
     c.k({
      $:1,
      $0:e
     });
    });
   },function(e)
   {
    once(function()
    {
     c.k({
      $:2,
      $0:e
     });
    });
   });
  };
 };
 Concurrency.checkCancel=function(r)
 {
  return function(c)
  {
   if(c.ct.c)
    Concurrency.cancel(c);
   else
    r(c);
  };
 };
 Concurrency.defCTS=function()
 {
  SC$5.$cctor();
  return SC$5.defCTS;
 };
 Concurrency.UncaughtAsyncError=function(e)
 {
  Global.console.log.apply(Global.console,["WebSharper: Uncaught asynchronous exception"].concat([e]));
 };
 Concurrency.cancel=function(c)
 {
  c.k({
   $:2,
   $0:new OperationCanceledException.New(c.ct)
  });
 };
 Concurrency.scheduler=function()
 {
  SC$5.$cctor();
  return SC$5.scheduler;
 };
 Anims.UseAnimations=function()
 {
  SC$3.$cctor();
  return SC$3.UseAnimations;
 };
 Anims.Actions=function(a)
 {
  var all,c;
  all=a.$0;
  return Anims.ConcatActions((c=function(a$1)
  {
   return a$1.$==1?{
    $:1,
    $0:a$1.$0
   }:null;
  },function(a$1)
  {
   return Arrays.choose(c,a$1);
  }(AppendList.ToArray(all))));
 };
 Anims.Finalize=function(a)
 {
  var all,a$1;
  all=a.$0;
  a$1=function(a$2)
  {
   if(a$2.$==0)
    a$2.$0();
  };
  (function(a$2)
  {
   Arrays.iter(a$1,a$2);
  }(AppendList.ToArray(all)));
 };
 Anims.List=function(a)
 {
  return a.$0;
 };
 Anims.ConcatActions=function(xs)
 {
  var xs$1,m,dur,m$1,xs$2;
  xs$1=Array.ofSeqNonCopying(xs);
  m=Arrays.length(xs$1);
  return m===0?Anims.Const():m===1?Arrays.get(xs$1,0):(dur=Seq.max((m$1=function(anim)
  {
   return anim.Duration;
  },function(s)
  {
   return Seq.map(m$1,s);
  }(xs$1))),(xs$2=Arrays.map(function(a)
  {
   return Anims.Prolong(dur,a);
  },xs$1),Anims.Def(dur,function(t)
  {
   Arrays.iter(function(anim)
   {
    anim.Compute(t);
   },xs$2);
  })));
 };
 Anims.Const=function(v)
 {
  return Anims.Def(0,function()
  {
   return v;
  });
 };
 Anims.Prolong=function(nextDuration,anim)
 {
  var comp,dur,last;
  comp=anim.Compute;
  dur=anim.Duration;
  last=Lazy.Create(function()
  {
   return anim.Compute(anim.Duration);
  });
  return{
   Compute:function(t)
   {
    return t>=dur?last.f():comp(t);
   },
   Duration:nextDuration
  };
 };
 Anims.Def=function(d,f)
 {
  return{
   Compute:f,
   Duration:d
  };
 };
 AppendList.Append=function(x,y)
 {
  return x.$==0?y:y.$==0?x:{
   $:2,
   $0:x,
   $1:y
  };
 };
 AppendList.Concat=function(xs)
 {
  var x,d;
  x=Array.ofSeqNonCopying(xs);
  d=AppendList.Empty();
  return Array.TreeReduce(d,AppendList.Append,x);
 };
 AppendList.ToArray=function(xs)
 {
  var out;
  function loop(xs$1)
  {
   var y,xs$2;
   if(xs$1.$==1)
    out.push(xs$1.$0);
   else
    if(xs$1.$==2)
     {
      y=xs$1.$1;
      loop(xs$1.$0);
      loop(y);
     }
    else
     if(xs$1.$==3)
      {
       xs$2=xs$1.$0;
       Arrays.iter(function(v)
       {
        out.push(v);
       },xs$2);
      }
  }
  out=[];
  loop(xs);
  return out.slice(0);
 };
 AppendList.Empty=function()
 {
  SC$6.$cctor();
  return SC$6.Empty;
 };
 T$1=Enumerator.T=Runtime.Class({
  MoveNext:function()
  {
   return this.n(this);
  },
  Current:function()
  {
   return this.c;
  },
  Dispose:function()
  {
   if(this.d)
    this.d(this);
  }
 },null,T$1);
 T$1.New=Runtime.Ctor(function(s,c,n,d)
 {
  this.s=s;
  this.c=c;
  this.n=n;
  this.d=d;
 },T$1);
 Seq.iter=function(p,s)
 {
  var e;
  e=Enumerator.Get(s);
  try
  {
   while(e.MoveNext())
    p(e.Current());
  }
  finally
  {
   if("Dispose"in e)
    e.Dispose();
  }
 };
 Seq.map=function(f,s)
 {
  return{
   GetEnumerator:function()
   {
    var en;
    en=Enumerator.Get(s);
    return new T$1.New(null,null,function(e)
    {
     return en.MoveNext()?(e.c=f(en.Current()),true):false;
    },function()
    {
     en.Dispose();
    });
   }
  };
 };
 Seq.max=function(s)
 {
  return Seq.reduce(function($1,$2)
  {
   return Unchecked.Compare($1,$2)>=0?$1:$2;
  },s);
 };
 Seq.reduce=function(f,source)
 {
  var e,r;
  e=Enumerator.Get(source);
  try
  {
   if(!e.MoveNext())
    Operators.FailWith("The input sequence was empty");
   r=e.Current();
   while(e.MoveNext())
    r=f(r,e.Current());
   return r;
  }
  finally
  {
   if("Dispose"in e)
    e.Dispose();
  }
 };
 SC$3.$cctor=Runtime.Cctor(function()
 {
  SC$3.CubicInOut=Easing.Custom(function(t)
  {
   var t2;
   t2=t*t;
   return 3*t2-2*(t2*t);
  });
  SC$3.UseAnimations=true;
  SC$3.$cctor=Global.ignore;
 });
 HashSet$1.Filter=function(ok,set)
 {
  var a;
  return new HashSet.New$2((a=HashSet$1.ToArray(set),Arrays.filter(ok,a)));
 };
 HashSet$1.Intersect=function(a,b)
 {
  var set;
  set=new HashSet.New$2(HashSet$1.ToArray(a));
  set.IntersectWith(HashSet$1.ToArray(b));
  return set;
 };
 HashSet$1.ToArray=function(set)
 {
  var arr;
  arr=Arrays.create(set.get_Count(),void 0);
  set.CopyTo(arr);
  return arr;
 };
 HashSet$1.Except=function(excluded,included)
 {
  var set;
  set=new HashSet.New$2(HashSet$1.ToArray(included));
  set.ExceptWith(HashSet$1.ToArray(excluded));
  return set;
 };
 SC$4.$cctor=Runtime.Cctor(function()
 {
  SC$4.counter=0;
  SC$4.$cctor=Global.ignore;
 });
 Scheduler=Concurrency.Scheduler=Runtime.Class({
  Fork:function(action)
  {
   var $this,v;
   $this=this;
   this.robin.push(action);
   this.idle?(this.idle=false,v=Global.setTimeout(function()
   {
    $this.tick();
   },0)):void 0;
  },
  tick:function()
  {
   var loop,$this,t,m,v;
   $this=this;
   t=Global.Date.now();
   loop=true;
   while(loop)
    {
     m=this.robin.length;
     m===0?(this.idle=true,loop=false):((this.robin.shift())(),Global.Date.now()-t>40?(v=Global.setTimeout(function()
     {
      $this.tick();
     },0),loop=false):void 0);
    }
  }
 },null,Scheduler);
 Scheduler.New=Runtime.Ctor(function()
 {
  this.idle=true;
  this.robin=[];
 },Scheduler);
 SC$5.$cctor=Runtime.Cctor(function()
 {
  SC$5.noneCT={
   c:false,
   r:[]
  };
  SC$5.scheduler=new Scheduler.New();
  SC$5.defCTS=[new CancellationTokenSource.New()];
  SC$5.GetCT=function(c)
  {
   c.k({
    $:0,
    $0:c.ct
   });
  };
  SC$5.$cctor=Global.ignore;
 });
 Easing=Next.Easing=Runtime.Class({},null,Easing);
 Easing.Custom=function(f)
 {
  return new Easing.New(f);
 };
 Easing.New=Runtime.Ctor(function(transformTime)
 {
  this.transformTime=transformTime;
 },Easing);
 DomNodes.DocChildren=function(node)
 {
  var q;
  function loop(doc)
  {
   var t,a,b;
   if(doc.$==2)
    loop(doc.$0.Current);
   else
    if(doc.$==1)
     q.push(doc.$0.El);
    else
     if(doc.$==3)
      ;
     else
      if(doc.$==5)
       q.push(doc.$0);
      else
       if(doc.$==4)
        q.push(doc.$0.Text);
       else
        if(doc.$==6)
         {
          t=doc.$0;
          a=function(a$1)
          {
           if(a$1.constructor===Global.Object)
            loop(a$1);
           else
            q.push(a$1);
          };
          (function(a$1)
          {
           Arrays.iter(a,a$1);
          }(t.Els));
         }
        else
         {
          b=doc.$1;
          loop(doc.$0);
          loop(b);
         }
  }
  q=[];
  loop(node.Children);
  return{
   $:0,
   $0:Array.ofSeqNonCopying(q)
  };
 };
 DomNodes.Children=function(elem,delims)
 {
  var n,o,rdelim,ldelim,a,v;
  if(delims!=null?delims.$==1:false)
   {
    rdelim=delims.$0[1];
    ldelim=delims.$0[0];
    a=Global.Array.prototype.constructor.apply(Global.Array,[]);
    n=ldelim.nextSibling;
    while(n!==rdelim)
     {
      v=a.push(n);
      n=n.nextSibling;
     }
    return{
     $:0,
     $0:a
    };
   }
  else
   return{
    $:0,
    $0:Arrays.init(elem.childNodes.length,(o=elem.childNodes,function(a$1)
    {
     return o[a$1];
    }))
   };
 };
 DomNodes.Except=function(a,a$1)
 {
  var excluded,included,p;
  excluded=a.$0;
  included=a$1.$0;
  return{
   $:0,
   $0:(p=function(n)
   {
    var p$1;
    p$1=function(k)
    {
     return!(n===k);
    };
    return function(a$2)
    {
     return Arrays.forall(p$1,a$2);
    }(excluded);
   },function(a$2)
   {
    return Arrays.filter(p,a$2);
   }(included))
  };
 };
 DomNodes.Iter=function(f,a)
 {
  var ns;
  ns=a.$0;
  Arrays.iter(f,ns);
 };
 OperationCanceledException=WebSharper.OperationCanceledException=Runtime.Class({},null,OperationCanceledException);
 OperationCanceledException.New=Runtime.Ctor(function(ct)
 {
  OperationCanceledException.New$1.call(this,"The operation was canceled.",null,ct);
 },OperationCanceledException);
 OperationCanceledException.New$1=Runtime.Ctor(function(message,inner,ct)
 {
  this.message=message;
  this.inner=inner;
  this.ct=ct;
 },OperationCanceledException);
 CancellationTokenSource=WebSharper.CancellationTokenSource=Runtime.Class({},null,CancellationTokenSource);
 CancellationTokenSource.New=Runtime.Ctor(function()
 {
  this.c=false;
  this.pending=null;
  this.r=[];
  this.init=1;
 },CancellationTokenSource);
 HashSetUtil.concat=function(o)
 {
  var r,k;
  r=[];
  for(var k$1 in o)r.push.apply(r,o[k$1]);
  return r;
 };
 SC$6.$cctor=Runtime.Cctor(function()
 {
  SC$6.Empty={
   $:0
  };
  SC$6.$cctor=Global.ignore;
 });
 Lazy.Create=function(f)
 {
  return{
   c:false,
   v:f,
   f:Lazy.forceLazy
  };
 };
 Lazy.forceLazy=function()
 {
  var v;
  v=this.v();
  this.c=true;
  this.v=v;
  this.f=Lazy.cachedLazy;
  return v;
 };
 Lazy.cachedLazy=function()
 {
  return this.v;
 };
 Client.Main();
}());


if (typeof IntelliFactory !=='undefined')
  IntelliFactory.Runtime.Start();
