using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StateMachine.MdlReader;

namespace StateMachine
{
    class Node
    {
        public string Name { get; set; }
        public string Quid { get; set; }
        public string Type { get; set; }
        public List<Node> To { get; set; }
        // public List<Node> From { get; set; }
        public int In { get; set; }
        public int Out { get; set; }
        public Node()
        {
            To = new List<Node>();
        }
        public Node(Node node)
        {
            this.Name = node.Name;
            this.Quid = node.Quid;
            this.Type = node.Type;
            this.To = new List<Node>();
        }
    }
    class Graph
    {
        Dictionary<string, Node> Nodes { get; set; }
        public Node Start { get; set; }
        public Node End { get; set; }
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
                Nodes[client].To.Add(Nodes[supplier]);
                //add in degree
                ++Nodes[client].Out;
                //add out degree
                ++Nodes[supplier].In;
            }
        }

        public List<NodePath> VerticesCover()
        {
            BipartiteGraph bipartite = new BipartiteGraph(Nodes);
            var match = bipartite.MaxMatch().ToList();
            PassingClosure passingClosure = new PassingClosure(Nodes);
            for (var i = 0; i < match.Count; ++i)
            {
                match[i] = passingClosure.Path(Start.Quid, match[i].First) + match[i] + passingClosure.Path(match[i].Last, End.Quid);
            }
            return match;
        }
        List<Node> stk;
        HashSet<string> vis;
        void Dfs(string u, List<NodePath> paths, HashSet<string> vis)
        {
            if (u == End.Quid)
            {
                NodePath path = new NodePath();
                foreach (var i in stk) path = path + i;
                paths.Add(path);
            }
            foreach (var v in Nodes[u].To)
            {
                if (!vis.Contains(v.Quid) && v.Out > 0)
                {
                    vis.Add(v.Quid);
                    stk.Add(v);
                    --v.Out;
                    Dfs(v.Quid, paths, vis);
                    stk.Remove(v);
                    vis.Remove(v.Quid);
                }
            }
        }
        public List<NodePath> EdgesCover()
        {
            List<NodePath> paths = new List<NodePath>();
            stk = new List<Node>();
            vis = new HashSet<string>();
            stk.Add(Start);
            Dfs(Start.Quid, paths, vis);
            return paths;
        }
    }
}
