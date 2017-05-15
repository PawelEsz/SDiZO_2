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
        public int vNS { get; set; }
        public int weight { get; set; }
        public int v1 { get; set; }
        public int v2 { get; set; }
        Edge[] edges;
        Edge e;
        const int MAXINT = int.MaxValue;

        public int[,] MatrixNS { get; set; }
        public int[,] MatrixS { get; set; }

        public AdjacencyMatrix Result;

        bool success = true;
        int[] d;//  = new int[n];
        int[] p;// = new int[n];
        int[] S;// = new int[n];

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

        void ZerujMacierz()
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++) MatrixS[i, j] = 0;
        }

        void AddEdge(Edge e)
        {
            weight += e.weight;
            MatrixNS[e.v1,e.v2] = e.weight;
            MatrixNS[e.v2, e.v1] = e.weight;
            MatrixS[e.v1, e.v2] = e.weight;
        }

        public void LoadFromFile()
        {
            string fileName;
            string[] lines;
            string name = "ELO";
            char x = name[0];
            Console.WriteLine("Podaj nazwę pliku:");
            fileName = Console.ReadLine() + ".txt";
            lines = File.ReadAllLines(fileName);
            int[] lineArray = lines[0].Split(' ').Select(int.Parse).ToArray();
            this.m = lineArray[0]; // krawedzie
            this.n = lineArray[1]; // wiersze
            if(lineArray.Length > 2)
            {
                this.vNS = lineArray[2];
            }
            MatrixS = new int[n, n]; //tworze macierz
            MatrixNS = new int[n, n];
            edges = new Edge[m];

            for (int i = 1; i < lines.Length; i++)
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

        public void ShowMatrixS()
        {
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

        public void ShowMST()
        {
            Console.Write("{0,5}", "");
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
            Console.WriteLine("AAAAAA:" + Result.weight);
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

        public void Dijskra()
        {
            
            int i, j, u, p1;

            d = new int[n];
            p = new int[n];
            bool[] QS = new bool[n];
            S = new int[n];
            //int sptr = 0; //wskaznik stosu
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
                        if (!QS[p1] && (d[p1] > d[u] + MatrixS[u,p1]))
                        {
                            d[p1] = d[u] + MatrixS[u, p1];
                            p[p1] = u;
                        }
                    }
                }
            }

            /*Console.WriteLine("Wierzchołek początkowy: {0}", vNS);
            for (i = 0; i < n; i++)
            {
                Console.Write("Dojście do wierzchołka {0}: ", i);

                for (j = i; j > -1; j = p[j]) S[sptr++] = j;

                while (sptr != 0) Console.Write("{0} ", S[--sptr]);

                Console.WriteLine("| Koszt: {0}", d[i]);
            }*/

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

                    for (j = i; j > -1; j = p[j]) S[sptr++] = j;

                    while (sptr != 0) Console.Write("{0} ", S[--sptr]);

                    Console.WriteLine("| Koszt: {0}", d[i]);
                }
            }
            else Console.WriteLine("Negative Cycle found!");
        }

    }
}
