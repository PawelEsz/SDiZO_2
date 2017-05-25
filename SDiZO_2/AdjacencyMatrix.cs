using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SDiZO_2
{
    class AdjacencyMatrix
    {
        public int n { get; set; } //liczba wierzchołków
        public int m { get; set; } //liczba krawedzi
        public int vNS = 0;
        public int weight { get; set; }
        Edge[] edges;
        Edge e;
        const int MAXINT = int.MaxValue;

        public int[,] MatrixNS { get; set; }
        public int[,] MatrixS { get; set; }

        public AdjacencyMatrix Result;

        bool success = true;
        long[] d;
        int[] p;
        int[] S;

        public AdjacencyMatrix(int n, int m)
        {
            this.n = n;
            this.m = m;
            MatrixS = new int[n, n];
            MatrixNS = new int[n, n];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    MatrixNS[i, j] = 0;
                    MatrixS[i, j] = 0;                  
                }
            weight = 0;
        }

        public AdjacencyMatrix()
        {

        }

        void AddEdge(Edge e)
        {
            weight += e.weight;
            MatrixNS[e.v1,e.v2] = e.weight;
            MatrixNS[e.v2, e.v1] = e.weight;
            MatrixS[e.v1, e.v2] = e.weight;
        }

        public void LoadFromFile(string fileName)
        {
            //string fileName;
            string[] lines;
            string FileName = fileName + ".txt";
            //Console.WriteLine("Podaj nazwę pliku:");
            //fileName = Console.ReadLine() + ".txt";
            lines = File.ReadAllLines(FileName);
            int[] lineArray = lines[0].Split(' ').Select(int.Parse).ToArray();
            this.m = lineArray[0]; // krawedzie
            this.n = lineArray[1]; // wiersze
            if(lineArray.Length > 2)
            {
                this.vNS = lineArray[2];
            }
            this.weight = 0;
            MatrixS = new int[n, n]; //tworze macierz
            MatrixNS = new int[n, n];
            edges = new Edge[m];

            for (int i = 1; i < m; i++)
            {
                lineArray = lines[i].Split(' ').Select(int.Parse).ToArray();
                e.v1 = lineArray[0];
                e.v2 = lineArray[1];
                e.weight = lineArray[2];
                AddEdge(e);

                edges[i - 1].v1 = lineArray[0];
                edges[i - 1].v2 = lineArray[1];
                edges[i - 1].weight = lineArray[2];
            }
        }

        public void GenerateRandomGraph(int v, int g)
        {
            int i,j;
            Random random = new Random();
            this.n = v;
            this.m = v + (v * g / 100);
            MatrixNS = new int[v, v];
            MatrixS = new int[v, v];
            edges = new Edge[m];

            //zeruje macierze
            for (i = 0; i < v; i++)
            {
                for (j = 0; i < v; i++)
                {
                    MatrixS[i, j] = 0;
                    MatrixNS[i, j] = 0;
                }
            }

            for (i = 0; i < v - 1; i++)
            {
                int we = random.Next(1, 10);
                edges[i].v1 = i;
                edges[i].v2 = i + 1;
                edges[i].weight = we;

                e.v1 = i;
                e.v2 = i + 1;               
                e.weight = we;
                AddEdge(e);
            }

            MatrixNS[v - 1, 0] = random.Next(1, 10);
            MatrixNS[0, v - 1] = MatrixNS[v - 1, 0];
            MatrixS[v - 1, 0] = MatrixNS[v - 1, 0];
            edges[v - 1].v1 = v - 1;
            edges[v - 1].v2 = 0;
            edges[v - 1].weight = MatrixNS[v - 1, 0];

            for(i=v;i<m;i++)
            {
                e.v1 = random.Next(1, v - 1);
                e.v2 = random.Next(1, v - 1);
                e.weight = random.Next(1, 10);

                if (MatrixNS[e.v2, e.v1] == 0 && MatrixS[e.v1,e.v2] == 0)
                {
                    edges[v].v1 = e.v1;
                    edges[v].v2 = e.v2;
                    edges[v].weight = e.weight;
                    AddEdge(e);        
                }
                else i--;
            }

        }

        public void ShowMatrix()
        {
            Console.WriteLine("Macierz siąsiedztwa:");
            Console.Write("{0,5}", "");
            for (int i = 0; i < n; i++) Console.Write("{0,5}", i);
            Console.WriteLine();
            for (int x = 0; x < n; x++)
            {
                Console.Write("{0,5}", x);
                for (int y = 0; y < n; y++) 
                {
                    Console.Write("{0,5}", MatrixS[x, y]);
                }
                Console.WriteLine();
            }
            Console.WriteLine("AAAAAA:" + weight);
        }

        public void Kruskal()
        {
            int i;
            Queue Q = new Queue(this.m);
            DSStruct Z = new DSStruct(this.n);
            Result = new AdjacencyMatrix(n, m);
            for (i = 0; i < n; i++)
            {
                Z.MakeSet(i); //tworz eosobny zbior dla kazdego wierzcholka
            }

            //umieszczam w kolejce kolejne krawedzie grafu
            for (i = 0; i < m; i++)
            {
                Q.Push(edges[i]);
            }

            for (i = 1; i < n; i++)
            {
                do
                {
                    e = Q.front();
                    Q.Pop();
                } while ((Z.FindSet(e.v1) == Z.FindSet(e.v2)));
                Result.AddEdge(e);
                Z.UnionSets(e);
            }
        }

        public void Prim()
        {
            int i, v;

            Queue Q = new Queue(this.m);
            Result = new AdjacencyMatrix(n, m); // drzewo rozpinajace
            bool[] visited = new bool[n];

            for (i = 0; i < n; i++)
            {
                visited[i] = false;
            }

            v = 0; //wierzcholek startowt
            visited[v] = true; //odwiedzony wierzcholek startowy

            for (i = 1; i < n; i++)
            {           
                for (int j = 0; j < n; j++)
                {
                    if(MatrixNS[v,j] != 0)
                        {
                            e.v1 = v;
                            e.v2 = j;
                            e.weight = MatrixNS[v, j];
                            Q.Push(e);
                        }
                }
                do
                {
                    e = Q.front();
                    Q.Pop();
                } while (visited[e.v2]);

                Result.AddEdge(e);
                visited[e.v2] = true;
                v = e.v2;
            }
        }

        public void ShowMST()
        {
            /*Console.Write("{0,5}", "");
            for (int i = 0; i < n; i++) Console.Write("{0,5}", i);
            Console.WriteLine();
            for (int x = 0; x < n; x++)
            {
                Console.Write("{0,5}", x);
                for (int y = 0; y < n; y++)
                {
                    Console.Write("{0,5}", Result.MatrixS[x, y]);
                }
                Console.WriteLine();
            }
            Console.WriteLine("AAAAAA:" + Result.weight);*/

            Console.WriteLine("Macierz siąsiedztwa:");
            Console.Write("{0,5}", "");
            for (int i = 0; i < n; i++) Console.Write("{0,5}", i);
            Console.WriteLine();
            for (int x = 0; x < n; x++)
            {
                Console.Write("{0,5}", x);
                for (int y = 0; y < n; y++)
                {
                    Console.Write("{0,5}", Result.MatrixNS[x, y]);
                }
                Console.WriteLine();
            }
            Console.WriteLine("Waga MST:" + Result.weight);
        }

        public void Dijkstra()
        {
            
            int i, j, u, p1;

            d = new long[n];
            p = new int[n];
            bool[] QS = new bool[n];
            S = new int[n];
            success = true;

            for (i = 0; i < n; i++)
            {
                d[i] = MAXINT;
                p[i] = -1;
                QS[i] = false;
            }

            d[vNS] = 0; //koszt dojscia do v jest zerowy

            for (i = 0; i < n; i++)
            {
                for (j = 0; QS[j]; j++) ;
                for (u = j++; j < n; j++)
                    if (!QS[j] && (d[j] < d[u])) u = j;

                //znaleziony wierzcholek przenosimy do S
                QS[u] = true;

                //modyfikujemy odpowiednio wszystich sasiadow u, ktorzy sa w Q
                for (p1 = 0; p1 < n; p1++)
                {
                    if(MatrixS[u,p1] > 0)
                        {
                        if (!QS[p1] && (d[p1] > d[u] + MatrixS[u, p1])) //MatrixS[u,p1]
                        {
                            d[p1] = d[u] + MatrixS[u, p1]; //MatrixS[u,p1]
                            p[p1] = u;
                        }
                    }
                }
            }
        }

        public void BellmanFord()
        {
            int i;
            d = new long[n];
            p = new int[n];
            S = new int[n];

            for (i = 0; i < n; i++)
            {
                d[i] = MAXINT;
                p[i] = -1;
            }

            if (BellmanFord(vNS, p, d))
                success = true;
            else success = false;
        }

        private bool BellmanFord(int v, int[] p1, long[] d)
        {
            int i, j, p2;
            bool test;

            d[v] = 0;
            for (i = 1; i < n; i++)
            {
                test = true;
                for (j = 0; j < n; j++)
                    for (p2 = 0; p2 < n; p2++)
                    {
                        if (MatrixS[j, p2] != 0)
                        {
                            if (d[p2] > d[j] + MatrixS[j, p2])
                            {
                                test = false;
                                d[p2] = d[j] + MatrixS[j, p2];
                                p1[p2] = j;
                            }
                            if (test) return true;
                        }
                    }
            }

            for (j = 0; j < n; j++)
            {
                for (p2 = 0; p2 < n; p2++)
                {
                    if (MatrixS[j, p2] != 0)
                    {
                        if (d[p2] > d[j] + MatrixS[j, p2]) return false;
                    }
                }
            }
            return true;
        }

        public void ShowShortestPaths()
        {
            if (success)
            {
                int i, j;
                int sptr = 0;
                Console.WriteLine("Macierz siąsiedztwa:");
                Console.WriteLine("Wierzchołek początkowy: {0}", vNS);
                for (i = 0; i < n; i++)
                {
                    Console.Write("Dojście do wierzchołka {0}: ", i);

                    for (j = i; j > -1; j = p[j]) S[sptr++] = j;

                    while (sptr != 0) Console.Write("{0} ", S[--sptr]);

                    Console.WriteLine("| Koszt: {0}", d[i]);
                }
            }
            else Console.WriteLine("Negative Cycle found!");
        }
    }
}
