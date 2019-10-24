using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachine
{
    class NodePath
    {
        public List<Node> Path { get; set; }
        public string First { get { return Path.First().Quid; } }
        public string Last { get { return Path.Last().Quid; } }
        //support up to short.maxvalue nodes
        public NodePath() { Path = new List<Node>(); }
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
