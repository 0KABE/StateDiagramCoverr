using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StateMachine.Path;

namespace StateMachine.Algorithm
{
    class EulerDiagram
    {
        Graph graph;
        public EulerDiagram(Graph graph)
        {
            this.graph = graph;
            BuildEuler();
        }
        public NodePath EulerLoop()
        {
            string u = graph.Start.Quid;
            NodePath path = new NodePath() + graph.Start;
            while (graph[u].Out > 0)
            {
                string v = graph[u].To.First();
                path = path + graph[v];
                graph[u].To.Remove(v);
                graph[v].From.Remove(u);
                u = v;
            }
            return path;
        }
        private void BuildEuler()
        {
            graph.End.To.Add(graph.Start.Quid);
            graph.Start.From.Add(graph.End.Quid);
            
            bool updated = true;
            while (updated)
            {
                updated = false;
                foreach (Node node in graph.Nodes.Values)
                {
                    if (node.In - node.Out > 0)
                    {
                        string min = null;
                        foreach (string quid in node.To)
                        {
                            if (min == null || graph[min].In - graph[min].Out > graph[quid].In - graph[quid].Out)
                            {
                                min = quid;
                            }
                        }
                        node.To.Add(min);
                        graph[min].From.Add(node.Quid);
                        // ++node.Out;
                        // ++graph[min].In;
                        updated = true;
                    }
                    else if (node.In - node.Out < 0)
                    {
                        string max = null;
                        foreach (string quid in node.From)
                        {
                            if (max == null || graph[max].In - graph[max].Out < graph[quid].In - graph[quid].Out)
                            {
                                max = quid;
                            }
                        }
                        graph[max].To.Add(node.Quid);
                        node.From.Add(max);
                        // ++graph[max].Out;
                        // ++node.In;
                        updated = true;
                    }
                }
            }
        }
    }
}
