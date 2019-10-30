using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StateMachine.MdlReader;
using StateMachine.Path;

namespace StateMachine.Algorithm
{
    class Graph
    {
        public Dictionary<string, Node> Nodes { get; set; }
        public Node Start { get; set; }
        public Node End { get; set; }
        public Node this[string quid]
        {
            get
            {
                return Nodes[quid];
            }
        }

        public Graph(MDLFile file)
        {
            //states
            var logicView = (from property in (from property in (from element in file.Elements.OfType<MdlReader.Object>()
                                                                 where element.Name == "\"Logical View\""
                                                                 select element.Properties).First()
                                               where property.Name == "root_category"
                                               select property.Value).Cast<ElementVal>()
                             select property.Value).Cast<MdlReader.Object>().First();
            var stateMachine = (MdlReader.Object)(from property in logicView.Properties
                                                  where property.Name == "statemachine"
                                                  select property.Value).Cast<MdlReader.ElementVal>().First().Value;
            var states = (MdlReader.List)(from property in stateMachine.Properties
                                          where property.Name == "states"
                                          select property.Value).Cast<MdlReader.ElementVal>().First().Value;
            var transitions = (MdlReader.List)(from property in stateMachine.Properties
                                               where property.Name == "transitions"
                                               select property.Value).Cast<MdlReader.ElementVal>().First().Value;

            Nodes = new Dictionary<string, Node>();
            //create node
            foreach (MdlReader.Object obj in states.Elements)
            {
                Node node = new Node();
                node.Name = obj.Name;
                node.Quid = (from property in obj.Properties
                             where property.Name == "quid"
                             select property.Value).Cast<StringVal>().First().Value;
                node.Type = (from property in obj.Properties
                             where property.Name == "type"
                             select property.Value).Cast<StringVal>().First().Value;
                if (node.Type == "\"StartState\"") Start = node;
                else if (node.Type == "\"EndState\"") End = node;
                Nodes.Add(node.Quid, node);
            }
            //create edges
            foreach (MdlReader.Object obj in transitions.Elements)
            {
                string supplier = (from property in obj.Properties
                                   where property.Name == "supplier_quidu"
                                   select property.Value).Cast<StringVal>().First().Value;
                string client = (from property in obj.Properties
                                 where property.Name == "client_quidu"
                                 select property.Value).Cast<StringVal>().First().Value;
                Nodes[client].To.Add(supplier);
                Nodes[supplier].From.Add(client);
                // //add in degree
                // ++Nodes[client].Out;
                // //add out degree
                // ++Nodes[supplier].In;
            }
        }


        // public List<NodePath> VerticesCover()
        // {
        //     BipartiteGraph bipartite = new BipartiteGraph(Nodes);
        //     var match = bipartite.MaxMatch().ToList();
        //     PassingClosure passingClosure = new PassingClosure(Nodes);
        //     for (var i = 0; i < match.Count; ++i)
        //     {
        //         match[i] = passingClosure.Path(Start.Quid, match[i].First) + match[i] + passingClosure.Path(match[i].Last, End.Quid);
        //     }
        //     return match;
        // }

        public string ToPlantuml()
        {
            string uml = "@startuml\r\n";
            //add nodes
            foreach (Node node in Nodes.Values)
            {
                foreach (string quid in node.To)
                {
                    Node to = Nodes[quid];
                    uml += string.Format("{0} --> {1}\r\n", node.Type.Contains("Normal") ? node.Name.Trim('\"') : "[*]", to.Type.Contains("Normal") ? to.Name.Trim('\"') : "[*]");
                }
            }
            uml += "@enduml";
            return uml;
        }
    }
    class VerticesCover
    {
        public Graph Graph { get; set; }
        public VerticesCover(Graph graph)
        {
            this.Graph = graph;
        }
        public List<NodePath> VerticesCoverPaths()
        {
            BipartiteGraph bipartite = new BipartiteGraph(Graph.Nodes);
            var match = bipartite.MaxMatch().ToList();
            PassingClosure passingClosure = new PassingClosure(Graph.Nodes);
            for (var i = 0; i < match.Count; ++i)
            {
                match[i] = passingClosure.Path(Graph.Start.Quid, match[i].First) + match[i] + passingClosure.Path(match[i].Last, Graph.End.Quid);
            }
            return match;
        }
    }
    class EdgesCover
    {
        public Graph Graph { get; set; }
        List<Node> stk;
        HashSet<string> vis;
        public EdgesCover(Graph graph)
        {
            this.Graph = graph;
        }
        public List<NodePath> EdgesCoverPaths()
        {
            // List<NodePath> paths = new List<NodePath>();
            // stk = new List<Node>();
            // vis = new HashSet<string>();
            // stk.Add(Graph.Start);
            // Dfs(Graph.Start.Quid, paths, vis);
            // return paths;

            EulerDiagram eulerDiagram = new EulerDiagram(Graph);
            NodePath path = eulerDiagram.EulerLoop();
            return path.Split(Graph.End);
        }
        void Dfs(string u, List<NodePath> paths, HashSet<string> vis)
        {
            if (u == Graph.End.Quid)
            {
                NodePath path = new NodePath();
                foreach (var i in stk) path = path + i;
                paths.Add(path);
            }
            foreach (string q in Graph.Nodes[u].To)
            {
                Node v = Graph.Nodes[q];
                if (!vis.Contains(v.Quid))
                {
                    vis.Add(v.Quid);
                    stk.Add(v);
                    Dfs(v.Quid, paths, vis);
                    stk.Remove(v);
                    vis.Remove(v.Quid);
                }
            }
        }
    }
}
