using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mapping
{

    class Program
    {
        static void Main(string[] args)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
          
            Dictionary<int, List<Tuple<int, Tuple<float, float>>>> edges = new Dictionary<int, List<Tuple<int, Tuple<float, float>>>>();
            Dictionary<int, Tuple<float, float>> vertices = new Dictionary<int, Tuple<float, float>>();
            List<Tuple<Tuple<Tuple<float, float>, Tuple<float, float>>, float>> queries = new List<Tuple<Tuple<Tuple<float, float>, Tuple<float, float>>, float>>();
            dijkstra dij = new dijkstra();

            vertices = readvertices("E:\\FCIS\\cases\\map.txt");
            int linenum = vertices.Count + 1;
            edges = readedges("E:\\FCIS\\cases\\map.txt", linenum);
            int numberofqueries = read_queries("E:\\FCIS\\cases\\q.txt", queries);

            StreamWriter sw = new StreamWriter("E:\\FCIS\\3rdyear\\Algorithm\\cases\\output.txt");
        

            for (int i = 0; i < numberofqueries; i++)
            {
                var Q = System.Diagnostics.Stopwatch.StartNew();
                Q.Start();

                List<int> changesofstarting = new List<int>();
                List<int> changesofending = new List<int>();
 
          
                changesofstarting = Possible_going_nodes(vertices,edges, queries[i].Item1.Item1.Item1, queries[i].Item1.Item1.Item2, queries[i].Item2, -1);

                changesofending = Possible_going_nodes(vertices,edges, queries[i].Item1.Item2.Item1, queries[i].Item1.Item2.Item2, queries[i].Item2, -2);
               

                List<int> path = new List<int>();


                var w = System.Diagnostics.Stopwatch.StartNew();
                w.Start();
                dij.find_shortesttime(edges,-1, -2, path);
                w.Stop();
               // Console.WriteLine(w.ElapsedMilliseconds);
               
                double shortesttime = dij.finaldistandtimes[-2].Item2;
                double totaldistance = dij.finaldistandtimes[-2].Item1;
                double walkdistance = 0;
                double ridedistance = 0;



                
                int num = path.Count-1;

                int vertexnum = path.ElementAt(num);
                int lastnode = path.ElementAt(0);
                double walkdistancefromsource = dij.finaldistandtimes[vertexnum].Item1;
                double walkdistancetodes = dij.finaldistandtimes[-2].Item1 - dij.finaldistandtimes[lastnode].Item1;
                walkdistance = walkdistancefromsource + walkdistancetodes;
                ridedistance = totaldistance - walkdistance;

                while (num>=0)
                {
                   // Console.Write(path.ElementAt(num));
                    //Console.Write(" ");
                    sw.Write(path.ElementAt(num));
                    sw.Write(" ");
                    num--;
                }

                shortesttime = Math.Round(shortesttime * 60, 2);
                totaldistance = Math.Round(totaldistance, 2);
                walkdistance = Math.Round(walkdistance, 2);
                ridedistance = Math.Round(ridedistance, 2);
                Q.Stop();
                // Console.WriteLine();
                sw.WriteLine();

              //  Console.WriteLine("The shortest time :" + shortesttime + " mins");
                sw.WriteLine(shortesttime + " mins");
                //Console.WriteLine("The total distance " + totaldistance + " Km");
                sw.WriteLine(totaldistance + " km");
                //Console.WriteLine("The walk distance " + totalwalkdistance + " Km");
                sw.WriteLine(walkdistance + " km");
                //Console.WriteLine("The ride distance " + ridedistance + " Km");
                sw.WriteLine(ridedistance + " km");
                // Console.WriteLine("The execution_time " + Output.total_execution_time + " ms");
                sw.WriteLine(Q.ElapsedMilliseconds);

                sw.WriteLine();

                dij.clear();


                

                for (int s=0;s<changesofstarting.Count;s++)
                {
                    int indinedges = changesofstarting[s];
                    int lastelementoflist = edges[indinedges].Count - 1;
                    edges[indinedges].RemoveAt(lastelementoflist);
                    edges.Remove(-1);
                }

                for (int e = 0; e < changesofending.Count; e++)
                {
                    int indinedges = changesofending[e];
                    int lastelementoflist = edges[indinedges].Count - 1;
                    edges[indinedges].RemoveAt(lastelementoflist);
                    edges.Remove(-2);
                }
                
            }
            
            Console.WriteLine("Done");
            watch.Stop();
            sw.WriteLine(watch.ElapsedMilliseconds+ "ms");
            sw.Close();
        }
        
        public static List<int> Possible_going_nodes(Dictionary<int, Tuple<float, float>> vertices, Dictionary<int, List<Tuple<int, Tuple<float, float>>>> versionedges, float X, float Y, float R,int edgeind)
        { 
        
            R = R / 1000;
            List<int> changes = new List< int>();
            for (int i = 0; i < vertices.Count; i++)
            {
                
                double dist = Math.Sqrt(Math.Pow(vertices[i].Item1 - X, 2) + Math.Pow(vertices[i].Item2 - Y, 2));
     

                if (dist <= R)
                {
                    
                        Tuple<float, float> distandspeed = new Tuple<float, float>((float)dist, 5);
                        Tuple<int, Tuple<float, float>> newedge= new Tuple<int, Tuple<float, float>>(i, distandspeed);
                        if (versionedges.ContainsKey(edgeind))
                        {
                           versionedges[edgeind].Add(newedge);
                            
                        }
                        else
                        {
                            List<Tuple<int, Tuple<float, float>>> listofvers = new List<Tuple<int, Tuple<float, float>>>();
                            listofvers.Add(newedge);
                            versionedges.Add(edgeind, listofvers);
                        }
                
                        Tuple<int, Tuple<float, float>> reverseedge = new Tuple<int, Tuple<float, float>>(edgeind, distandspeed);
                        versionedges[i].Add(reverseedge);
                        changes.Add(i);
                   
                }
            
            }
            
            return changes;
          }




        public static Dictionary<int, Tuple<float, float>> readvertices(string path )
        {
            int lineind = 0;
            Dictionary<int, Tuple<float, float>> vertices;


            string[] lines = File.ReadAllLines(path);
            int numvertices = Convert.ToInt32(lines[lineind]);
            vertices = new Dictionary<int, Tuple<float, float>>(numvertices);

            lineind++;
            for (int i = 0; i < numvertices; i++)
            {
                string[] details = lines[lineind].Split(' ');

                int vertexnumber = Int16.Parse(details[0]);
                float x_vertex = float.Parse(details[1]);
                float y_vertex = float.Parse(details[2]);
                Tuple<float, float> XandY = new Tuple<float, float>(x_vertex, y_vertex);
                vertices.Add(vertexnumber, XandY);
                lineind++;
            }
            return vertices;
        }


        public static Dictionary<int, List<Tuple<int, Tuple<float, float>>>> readedges(string path,int lineind)
        {
            Dictionary<int, List<Tuple<int, Tuple<float, float>>>> edges;
            string[] lines = File.ReadAllLines(path);
            int num_edges = Convert.ToInt32(lines[lineind]);
            lineind++;
            edges = new Dictionary<int, List<Tuple<int, Tuple<float, float>>>>(num_edges);

            for (int i = lineind; i<lines.Length; i++)
            {
                string[] details = lines[i].Split(' ');


                 int currentvertexnumber = Convert.ToInt32(details[0]);
                 int reachvertexnumber = Convert.ToInt32(details[1]);
                 float Lengthofroad = float.Parse(details[2]);
                 float speedofroad = float.Parse(details[3]);

               Tuple<float, float> roadlength_and_speed = new Tuple<float, float>(Lengthofroad, speedofroad);
               Tuple<int, Tuple<float, float>> fedge = new Tuple<int, Tuple<float, float>>(reachvertexnumber, roadlength_and_speed);
                if (edges.ContainsKey(currentvertexnumber))
                {
                    edges[currentvertexnumber].Add(fedge);
                 }
                else
                {
                    List<Tuple<int, Tuple<float, float>>> newvalues = new List<Tuple<int, Tuple<float, float>>>();
                    newvalues.Add(fedge);
                    edges.Add(currentvertexnumber, newvalues);
                }

                
                Tuple<int, Tuple<float, float>> secondedge = new Tuple<int, Tuple<float, float>>(currentvertexnumber, roadlength_and_speed);
  
                if (edges.ContainsKey(reachvertexnumber))
                {
                    edges[reachvertexnumber].Add(secondedge);
                }
                else
                {
                    List<Tuple<int, Tuple<float, float>>> newvalues = new List<Tuple<int, Tuple<float, float>>>();
                     newvalues.Add(secondedge);
                    edges.Add(reachvertexnumber, newvalues);
                }
                
            }
            return edges;
        }



        public static int read_queries(string path, List<Tuple<Tuple<Tuple<float, float>, Tuple<float, float>>, float>> Q)
        {

            string[] lines = File.ReadAllLines(path);
            int number = Convert.ToInt32(lines[0]);

            for (int i = 1; i <= number; i++)
            {
                string[] details = lines[i].Split(' ');
                float xsource = float.Parse(details[0]);
                float ysource = float.Parse(details[1]);
                float xdes = float.Parse(details[2]);
                float ydes = float.Parse(details[3]);
                float r = float.Parse(details[4]);
                Tuple<float, float> source = new Tuple<float, float>(xsource, ysource);
                Tuple<float, float> dest = new Tuple<float, float>(xdes, ydes);
                Tuple<Tuple<float, float>, Tuple<float, float>> twocoordinates = new Tuple<Tuple<float, float>, Tuple<float, float>>(source, dest);
                Tuple<Tuple<Tuple<float, float>, Tuple<float, float>>, float> allinfo = new Tuple<Tuple<Tuple<float, float>, Tuple<float, float>>, float>(twocoordinates, r);
                Q.Add(allinfo);
            }
            return number;
        }
    }
}




















