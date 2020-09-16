using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping
{
    class dijkstra
    {
        public Dictionary<int, Tuple<float, float>> finaldistandtimes;
        public Dictionary<int, int> previousvertexforeachnode;
        priorityQueue Queue;
  
        public dijkstra()
        {
            finaldistandtimes = new Dictionary<int, Tuple<float, float>>();
            previousvertexforeachnode = new Dictionary<int, int>();
            Queue = new priorityQueue();
        }

        public void clear()
        { 
            finaldistandtimes = new Dictionary<int, Tuple<float, float>>();
            previousvertexforeachnode = new Dictionary<int, int>();
            Queue = new priorityQueue();
        }

        public void find_shortesttime(Dictionary<int, List<Tuple<int, Tuple<float, float>>>> edges, int sourceind, int endnodeind, List<int> currentpath)
        {


            Tuple<float, float> distandtimesource = new Tuple<float, float>(0, 0);
            finaldistandtimes.Add(sourceind, distandtimesource);

            Queue.insert(finaldistandtimes[sourceind].Item1, sourceind);

            Tuple<float, float> distandtimefordes = new Tuple<float, float>(float.MaxValue, float.MaxValue);
            finaldistandtimes.Add(endnodeind, distandtimefordes);

                for (int i = 0; i < edges.Count - 2; i++)
                {
                Tuple<float, float> distandtime = new Tuple<float, float>(float.MaxValue, float.MaxValue);
                finaldistandtimes.Add(i, distandtime);
                }

            while (!Queue.Isempty())
            {
                
                Tuple<float, int> mintimetuple = new Tuple<float, int>(float.MaxValue, int.MaxValue);

                mintimetuple = Queue.heap_extract_min();
                int indexinedges = mintimetuple.Item2;
           
                 for (int neig = 0; neig < edges[indexinedges].Count; neig++)
                    {
                      
                        int reachvertexnumber = edges[indexinedges].ElementAt(neig).Item1;
                        float roadspeed = edges[indexinedges][neig].Item2.Item2;
                        float roadlength = edges[indexinedges][neig].Item2.Item1;
                        float roadtime = roadlength / roadspeed;

                        if (finaldistandtimes[reachvertexnumber].Item2 > finaldistandtimes[indexinedges].Item2 + roadtime)
                        { 
                            
                        
                        float newtime = ((finaldistandtimes[indexinedges].Item2) + (roadtime));
                        float newdistance = ((finaldistandtimes[indexinedges].Item1) + roadlength);

                            Tuple<float, float> newvalueforvertex = new Tuple<float, float>(newdistance, newtime);

                            finaldistandtimes[reachvertexnumber] = newvalueforvertex;
                            Queue.insert(newtime, reachvertexnumber);
                            if (previousvertexforeachnode.ContainsKey(reachvertexnumber))
                            {
                                previousvertexforeachnode[reachvertexnumber] = indexinedges;
                            }
                            else
                            {
                                previousvertexforeachnode.Add(reachvertexnumber, indexinedges);
                            }

                        }
                    
                 }
                   
           }

                int vertexnumber = previousvertexforeachnode[endnodeind];
                while (vertexnumber != sourceind)
                {
                    currentpath.Add(vertexnumber);
                    vertexnumber = previousvertexforeachnode[vertexnumber];
                }

       }


        }
    }
