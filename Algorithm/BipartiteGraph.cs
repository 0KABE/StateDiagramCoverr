using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StateMachine.Path;

namespace StateMachine.Algorithm
{
    class BipartiteGraph
    {
        List<Dictionary<string, Node>> bipartite;
        PassingClosure closure;
        List<Dictionary<string, string>> matching;
        List<Dictionary<string, bool>> vis;
        public BipartiteGraph(Dictionary<string, Node> graph)
        {
            bipartite = new List<Dictionary<string, Node>>{
                new Dictionary<string, Node>(),
                new Dictionary<string, Node>()
            };

            closure = new PassingClosure(graph);
            var g = closure.AddPassingClosure(graph);
            //build bipartite graph vertices
            for (int i = 0; i < 2; ++i)
            {
                foreach (var u in g.Values)
                {
                    Node node = new Node(u);
                    // bipartite[i].Add(u.Quid, u);
                    bipartite[i][node.Quid] = node;
                }
            }
            //build graph edges
            foreach (var u in g.Keys)
            {
                foreach (var v in g[u].To)
                {
                    bipartite[0][u].To.Add(bipartite[1][v].Quid);
                    bipartite[1][v].To.Add(bipartite[0][u].Quid);
                }
            }
        }
        public IEnumerable<NodePath> MaxMatch()
        {
            matching = BuildMatching();

            int ans = 0;
            foreach (var u in bipartite[0].Keys)
            {
                if (matching[0][u] == null)
                {
                    vis = BuildVis();
                    ans += Dfs(u, 0);
                }
            }

            List<NodePath> paths = new List<NodePath>();
            foreach (var p in matching[0])
            {
                if (p.Value != null)
                {
                    paths.Add(closure.Path(p.Key, p.Value));
                }
            }
            for (var i = 0; i < paths.Count;)
            {
                bool update = false;
                var j = i + 1;
                while (j < paths.Count)
                {
                    if (paths[i].Last == paths[j].First)
                    {
                        paths[i] = paths[i] + paths[j];
                        paths.RemoveAt(j);
                        update = true;
                    }
                    else
                    {
                        ++j;
                    }
                }
                //i = update ? i + 1 : i;
                i = update ? i : i + 1;
            }
            return paths;
        }
        List<Dictionary<string, string>> BuildMatching()
        {
            List<Dictionary<string, string>> ret = new List<Dictionary<string, string>>{
                new Dictionary<string, string>(),
                new Dictionary<string, string>()
            };
            for (int i = 0; i < 2; ++i)
            {
                foreach (var u in bipartite[i].Keys)
                {
                    ret[i][u] = null;
                }
            }
            return ret;
        }
        List<Dictionary<string, bool>> BuildVis()
        {
            List<Dictionary<string, bool>> ret = new List<Dictionary<string, bool>>{
                new Dictionary<string, bool>(),
                new Dictionary<string, bool>()
            };
            for (int i = 0; i < 2; ++i)
            {
                foreach (var u in bipartite[i].Keys)
                {
                    ret[i][u] = false;
                }
            }
            return ret;
        }
        int Dfs(string u, int from)
        {
            int to = (from + 1) % 2;
            foreach (var v in bipartite[from][u].To)
            {
                if (!vis[to][v])
                {
                    vis[to][v] = true;
                    // if (matching[to][v] == null || Dfs(matching[to][v], to) == 1)
                    if (matching[to][v] == null || Dfs(matching[to][v], from) == 1)
                    {
                        matching[to][v] = u;
                        matching[from][u] = v;
                        return 1;
                    }
                }
            }
            return 0;
        }
    }
}
