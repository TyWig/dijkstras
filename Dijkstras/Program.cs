using System;
using System.Collections.Generic;
using System.Linq;

namespace Dijkstras
{
    class Program
    {
        static void Main(string[] args)
        {
            //Initialize vertices and edges based on graph given
            List<Vertex> vertices = initVertices();
            List<Edge> edges = initEdges(vertices);

            //Generate single source shortest path based on graph given
            Dijkstras d = new Dijkstras(vertices, edges);
            d.Generate();
            Console.WriteLine(d.ToString());
        }

        /// <summary>
        /// Initializes the vertices
        /// </summary>
        /// <returns>List of vertices based on given graph</returns>
        static List<Vertex> initVertices()
        {
            return new List<Vertex>
            {
                new Vertex {Name = "a", Visited = false },
                new Vertex {Name = "b", Visited = false },
                new Vertex {Name = "c", Visited = false },
                new Vertex {Name = "d", Visited = false },
                new Vertex {Name = "e", Visited = false }
            };
        }
        
        /// <summary>
        /// Creates a list of edges from the given vertices
        /// </summary>
        /// <param name="vertices">Vertices to be associated with edges</param>
        /// <returns>List of edges based on vertices</returns>
        static List<Edge> initEdges(List<Vertex> vertices)
        {
            return new List<Edge>
            {
                new Edge {Weight = 7, To = vertices.First(v => v.Name.Equals("d")), From = vertices.First(v => v.Name.Equals("a"))},
                new Edge {Weight = 3, To = vertices.First(v => v.Name.Equals("a")), From = vertices.First(v => v.Name.Equals("b"))},
                new Edge {Weight = 2, To = vertices.First(v => v.Name.Equals("b")), From = vertices.First(v => v.Name.Equals("d"))},
                new Edge {Weight = 4, To = vertices.First(v => v.Name.Equals("c")), From = vertices.First(v => v.Name.Equals("b"))},
                new Edge {Weight = 5, To = vertices.First(v => v.Name.Equals("c")), From = vertices.First(v => v.Name.Equals("d"))},
                new Edge {Weight = 4, To = vertices.First(v => v.Name.Equals("d")), From = vertices.First(v => v.Name.Equals("e"))},
                new Edge {Weight = 6, To = vertices.First(v => v.Name.Equals("e")), From = vertices.First(v => v.Name.Equals("c"))},
            };
        } 
    }

    /// <summary>
    /// Class representing Dijkstra's algorithm
    /// </summary>
    class Dijkstras
    {
        private List<Vertex> vertices { get; set; }
        private List<Edge> edges { get; set; }
        private Dictionary<string, int> distance;
        private Dictionary<string, string> predecessor;
        private List<Vertex> queue;

        public Dijkstras(List<Vertex> vertices, List<Edge> edges)
        {
            this.vertices = vertices;
            this.edges = edges;
            distance = new Dictionary<string, int>();
            predecessor = new Dictionary<string, string>();
        }

        public void Generate()
        {
            queue = new List<Vertex>(vertices);
            foreach (Vertex v in vertices)
            {
                distance.Add(v.Name, int.MaxValue/2);
            }
            distance["a"] = 0;
            while(queue.Count > 0)
            {
                string key = getMin();
                Vertex u = queue.Where(v => v.Name == key).First();
                queue.Remove(u);
                var adj = getAdj(u);
                foreach(Edge v in adj)
                {
                    if (distance[v.To.Name] > (distance[u.Name] + v.Weight))
                    {
                        distance[v.To.Name] = distance[u.Name] + v.Weight;
                        predecessor[v.To.Name] = u.Name;
                    }
                }
                u.Visited = true;
            }
        }

        private List<Edge> getAdj(Vertex v)
        {
            return edges.Where(e => e.From.Name == v.Name).ToList();
        }

        private string getMin()
        {
            int min = int.MaxValue - 1;
            string key = null;
            foreach(Edge e in edges)
            {
                if(e.From.Visited == false && distance[e.From.Name] <= min)
                {
                    min = e.Weight;
                    key = e.From.Name;
                }
            }
            return key;
        }

        public override string ToString()
        {
            string toString = "Distance Table: \n";
            foreach(KeyValuePair<string, int> entry in distance)
            {
                toString += "Vertex: " + entry.Key + "\t|\t Distance: " + entry.Value + "\n";
            }
            toString += "\nPredecessor Table: \n";
            foreach(KeyValuePair<string,string> entry in predecessor)
            {
                toString += "Vertex: " + entry.Key + "\t|\t Predecessor: " + entry.Value + "\n";
            }
            return toString;
        }
    }

    class Vertex
    {
        public string Name { get; set; }
        public bool Visited { get; set; }
    }

    class Edge
    {
        public Vertex From { get; set; }
        public Vertex To { get; set; }
        public int Weight { get; set; }
    }
}
