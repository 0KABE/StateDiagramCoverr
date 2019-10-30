using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StateMachine.Path;

namespace StateMachine.Algorithm
{
    class PassingClosure
    {
        Dictionary<string, Dictionary<string, NodePath>> shortestPath;
        Dictionary<string, Dictionary<string, int>> distance;
        const int INF = ushort.MaxValue;
        public PassingClosure(Dictionary<string, Node> nodes)
        {
            shortestPath = new Dictionary<string, Dictionary<string, NodePath>>();
            distance = new Dictionary<string, Dictionary<string, int>>();
            foreach (var u in nodes)
            {
                Dictionary<string, NodePath> path = new Dictionary<string, NodePath>();
                Dictionary<string, int> dist = new Dictionary<string, int>();
                foreach (var v in nodes)
                {
                    path[v.Key] = new NodePath() + v.Value;
                    dist[v.Key] = u.Key == v.Key ? 0 : INF;
                }
                shortestPath[u.Key] = path;
                distance[u.Key] = dist;
            }

            foreach (var u in nodes.Values)
            {
                //Dictionary<string, NodePath> path = new Dictionary<string, NodePath>();
                foreach (var v in u.To)
                {
                    // shortestPath[u.Quid][v.Quid] = u + shortestPath[u.Quid][v.Quid] + v;
                    shortestPath[u.Quid][v] = u + shortestPath[v][v];
                    distance[u.Quid][v] = 1;
                }
            }

            //floyd
            foreach (var i in shortestPath.Keys)
            {
                foreach (var j in shortestPath.Keys)
                {
                    foreach (var k in shortestPath.Keys)
                    {
                        if (distance[i][j] > distance[i][k] + distance[k][j])
                        {
                            shortestPath[i][j] = shortestPath[i][k] + shortestPath[k][j];
                            distance[i][j] = distance[i][k] + distance[k][j];
                        }
                    }
                }
            }
        }
        public Dictionary<string, Node> AddPassingClosure(Dictionary<string, Node> graph)
        {
            Dictionary<string, Node> res = new Dictionary<string, Node>();
            foreach (var u in graph.Values)
            {
                res[u.Quid] = new Node(u);
            }

            foreach (var u in graph.Keys)
            {
                foreach (var v in graph.Keys)
                {
                    if (u != v && ExistPath(u, v))
                    {
                        res[u].To.Add(v);
                    }
                }
            }
            return res;
        }
        public bool ExistPath(string start, string end)
        {
            return distance[start][end] != INF;
        }
        public NodePath Path(string start, string end)
        {
            return shortestPath[start][end];
        }
        public IEnumerable<string> AllShortestPaths()
        {
            List<string> paths = new List<string>();
            foreach (var u in shortestPath.Keys)
            {
                foreach (var v in shortestPath.Keys)
                {
                    if (u != v && distance[u][v] != INF)

                        paths.Add(shortestPath[u][v].ToString());
                }
            }
            return paths;
        }
    }
}
