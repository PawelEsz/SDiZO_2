using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDiZO_2
{

    class AdjacencyList
    {
        int n, m, vNS, Alen, weight; //alen is n-1
        const int MAXINT = int.MaxValue;
        Edge e;
        public ALElement[] UndirectedGraph; //GrafNIESkierowany;
        public ALElement[] DirectedGraph;//GrafSkierowany;
        public AdjacencyList Result;
        ALElement p;
        Edge[] edges;

        //dijskra
        int[] S;
        long[] d;
        int[] p1;
        bool success = true;

        public AdjacencyList(int n)
        {
            int i;

            UndirectedGraph = new ALElement[n];
            DirectedGraph = new ALElement[n];
            for (i = 0; i < n; i++)
            {
                UndirectedGraph[i] = null;
                DirectedGraph[i] = null;
            }
            Alen = n - 1;
            weight = 0;
        }

        public AdjacencyList()
        {

        }

        public void Zeruj(int n)
        {
            int i;

            for (i = 0; i < n; i++) DirectedGraph[i] = null;
            Alen = n - 1;
            weight = 0;
        }

        private ALElement getAList(int n)
        {
            return UndirectedGraph[n];
        }

        void addEdge(Edge e)
        {
            //dodanie krawędzi do grafu nieskierowanego
            weight += e.weight;
            p = new ALElement();
            p.v = e.v2;
            p.weight = e.weight;
            p.next = UndirectedGraph[e.v1];
            UndirectedGraph[e.v1] = p;

            p = new ALElement(); // krawędź odwrotna (aby graf był nieskierowany)
            p.v = e.v1;  
            p.weight = e.weight; 
            p.next = UndirectedGraph[e.v2];
            UndirectedGraph[e.v2] = p;

            //dodanie krawędzi do grafu skierowanego
            p = new ALElement();
            p.v = e.v2;
            p.weight = e.weight;
            p.next = DirectedGraph[e.v1];
            DirectedGraph[e.v1] = p;
        }

        public void LoadFromFile()
        {
            string fileName;
            string[] lines;
            Console.WriteLine("Podaj nazwę pliku:");
            fileName = Console.ReadLine() + ".txt";
            lines = File.ReadAllLines(fileName);
            int[] lineArray = lines[0].Split(' ').Select(int.Parse).ToArray();
            this.m = lineArray[0]; // krawędzie
            this.n = lineArray[1]; // wierzchołki
            if(lineArray.Length > 2)
            {
                this.vNS = lineArray[2]; // numer wierzchołka początkowego dla algorytmów wyznaczających najkrótszą ścieżkę
            }
            this.weight = 0;
            UndirectedGraph = new ALElement[n];
            DirectedGraph = new ALElement[n];
            //Result = new ALElement[n];
            edges = new Edge[m];

            //wypelnienie listy zerami
            for (int i = 0; i < n; i++)
            {
                UndirectedGraph[i] = null;
                DirectedGraph[i] = null;
            }

            for (int i = 1; i < lines.Length; i++)
            {
                lineArray = lines[i].Split(' ').Select(int.Parse).ToArray();
                e.v1 = lineArray[0];
                e.v2 = lineArray[1];
                e.weight = lineArray[2];
                addEdge(e);
                
                edges[i - 1].v1 = lineArray[0];
                edges[i - 1].v2 = lineArray[1];
                edges[i - 1].weight = lineArray[2];
            }
        }

        public void ShowList()
        {
            for (int i = 0; i < n; i++)
            {
                Console.Write("A[" + i + "] =");
                p = UndirectedGraph[i];
                while (p != null)
                {
                    Console.Write("{0,5}:{1}", p.v, p.weight);               
                    p = p.next;
                }
                Console.WriteLine();
            }
            Console.WriteLine("AAAAAA:" + weight);
        }

        public void ShowMST()
        {
            for (int i = 0; i < n; i++)
            {
                Console.Write("A[" + i + "] =");
                p = Result.UndirectedGraph[i];
                while (p != null)
                {
                    Console.Write("{0,5}:{1}", p.v, p.weight);
                    p = p.next;
                }
                Console.WriteLine();
            }
            Console.WriteLine("AAAAAA:" + Result.weight);
        }

        public void Kruskal()
        {
            int i;
            Queue Q = new Queue(this.m);
            DSStruct Z = new DSStruct(this.n);
            Result = new AdjacencyList(n);

            for (i = 0; i < n; i++) 
            {
                Z.MakeSet(i); //tworz eosobny zbior dla kazdego wierzcholka
            }

            //umieszczam w kolejce kolejne krawedzie grafu
            for (i = 0; i < m; i++)
            {
                Q.Push(edges[i]);
            }

            /*for (i = 0; i < n; i++)
            {
                for (p = getAList(i); p != null; p = p.next)
                {
                    e.v1 = i;
                    e.v2 = p.v;
                    e.weight = p.weight;
                    Q.Push(e);
                }
            }*/

            for (i = 1; i < n; i++)
            {       
                do
                {
                    e = Q.front();
                    Q.Pop();
                } while ((Z.FindSet(e.v1) == Z.FindSet(e.v2)));
                Result.addEdge(e);
                Z.UnionSets(e);
            }
        }

        public void Prim()
        {
            int i, v;

            Queue Q = new Queue(this.m);
            Result = new AdjacencyList(n);
            bool[] visited = new bool[n];

            for (i = 0; i < n; i++)
            {
                visited[i] = false;
            }

            v = 0; //wierzcholek startowy
            visited[v] = true; //odwiedzony wierzcholek startowy

            for (i = 1; i < n; i++)
            {
                for (p = getAList(v); p != null; p = p.next)           
                    if(!visited[p.v])
                    {
                        e.v1 = v;
                        e.v2 = p.v;
                        e.weight = p.weight;
                        Q.Push(e);
                    }
                do
                {
                    e = Q.front();
                    Q.Pop();
                } while (visited[e.v2]);

                Result.addEdge(e);
                visited[e.v2] = true;
                v = e.v2;
            }
        }

        public void Dijskra()
        {
            
            int i, j, u;

            d = new long[n];
            p1 = new int[n];
            S = new int[n];
            bool[] QS = new bool[n];
            success = true;
            
            for(i = 0; i < n; i++)
            {
                d[i] = MAXINT;
                p1[i] = -1;
                QS[i] = false;
            }

            d[vNS] = 0; //koszt dojscia do v jest zerowy

            for(i = 0; i < n; i++)
            {
                for (j = 0; QS[j]; j++) ;
                for (u = j++; j < n; j++)
                    if (!QS[j] && (d[j] < d[u])) u = j;

                //znaleziony wierzcholek przenosimy do S
                QS[u] = true;

                //modyfikujemy odpowiednio wszystich sasiadow u, ktorzy sa w Q
                for(p = DirectedGraph[u]; p != null; p = p.next)
                {
                    if(!QS[p.v] && (d[p.v] > d[u] + p.weight))
                    {
                        d[p.v] = d[u] + p.weight;
                        p1[p.v] = u;
                    }
                }
            }  
        }

        public void ShowAfterDijskra()
        {
            if (success)
            {
                int i, j;
                int sptr = 0;
                Console.WriteLine("Wierzchołek początkowy: {0}", vNS);
                for (i = 0; i < n; i++)
                {
                    Console.Write("Dojście do wierzchołka {0}: ", i);

                    for (j = i; j > -1; j = p1[j]) S[sptr++] = j;

                    while (sptr != 0) Console.Write("{0} ", S[--sptr]);

                    Console.WriteLine("| Koszt: {0}", d[i]);
                }
            }
            else Console.WriteLine("Negative Cycle found!");
        }

        public void BellmanFord()
        {
            int i;
            d = new long[n];
            p1 = new int[n];
            S = new int[n];

            for (i = 0; i < n; i++)
            {
                d[i] = MAXINT;
                p1[i] = -1;
            }

            if (BellmanFord(vNS, p1, d))
                success = true;       
            else success = false;
        }

        private bool BellmanFord(int v, int[]p1, long[]d)
        {
            int i, x;
            bool test;

            d[v] = 0;
            for(i=1; i < n; i++)
            {
                test = true;
                for (x = 0; x < n; x++)
                    for (p = DirectedGraph[x]; p != null; p = p.next)
                    {
                        if(d[p.v] > d[x] + p.weight)
                        {
                            test = false;
                            d[p.v] = d[x] + p.weight;
                            p1[p.v] = x;
                        }
                        if (test) return true;
                    }
            }

            for(x=0; x<n;x++)
            {
                for(p= DirectedGraph[x]; p != null; p = p.next)
                {
                    if (d[p.v] > d[x] + p.weight) return false;
                }
            }
            return true;
        }
    }
}
