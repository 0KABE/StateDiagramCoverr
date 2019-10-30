using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachine.Path
{
    class Node
    {
        public string Name { get; set; }
        public string Quid { get; set; }
        public string Type { get; set; }
        public List<string> To { get; set; }
        public List<string> From { get; set; }
        public int In { get { return From.Count; } }
        public int Out { get { return To.Count; } }
        public Node()
        {
            To = new List<string>();
            From = new List<string>();
        }
        public Node(Node node)
        {
            this.Name = node.Name;
            this.Quid = node.Quid;
            this.Type = node.Type;
            this.To = new List<string>();
            this.From = new List<string>();
        }
    }
    class NodePath
    {
        public List<Node> Path { get; set; }
        public string First { get { return Path.First().Quid; } }
        public string Last { get { return Path.Last().Quid; } }
        //support up to short.maxvalue nodes
        public NodePath() { Path = new List<Node>(); }
        public List<NodePath> Split(Node seperator)
        {
            List<NodePath> paths = new List<NodePath>();
            for (int i = 0, j = 0; j < Path.Count;)
            {
                if (seperator.Quid == Path[j].Quid)
                {
                    NodePath path = new NodePath();
                    path.Path.AddRange(Path.GetRange(i, j - i + 1));
                    paths.Add(path);
                    i = j + 1; ++j;
                }
                else
                {
                    ++j;
                }
            }
            return paths;
        }
        public override string ToString()
        {
            return string.Join(" -> ", Path.ConvertAll<string>(node => node.Name));
        }
        public static NodePath operator +(NodePath path1, NodePath path2)
        {
            NodePath path = new NodePath();
            path.Path.AddRange(path1.Path.GetRange(0, path1.Path.Count - 1));
            path.Path.AddRange(path2.Path);
            return path;
        }
        public static NodePath operator +(NodePath path1, Node node)
        {
            NodePath path = new NodePath();
            path.Path.AddRange(path1.Path);
            path.Path.Add(node);
            return path;
        }
        public static NodePath operator +(Node node, NodePath path1)
        {
            NodePath path = new NodePath();
            path.Path.Add(node);
            path.Path.AddRange(path1.Path);
            return path;
        }
    }
}
