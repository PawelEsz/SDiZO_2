using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDiZO_2
{
    class Program
    {
        static void Main(string[] args)
        {
            string opt;
            string fileName;
            int v;
            int g;
            AdjacencyList AdjacencyList = new AdjacencyList();
            AdjacencyMatrix AdjacencyMatrix = new AdjacencyMatrix();

            do
            {
                Console.WriteLine("1. Wczytaj z pliku");
                Console.WriteLine("2. Wygeneruj graf");
                Console.WriteLine("3. Wyświetl");
                Console.WriteLine("4. Algorytm Prima");
                Console.WriteLine("5. Algorytm Kruskala");
                Console.WriteLine("6. Algorytm Dijkstry");
                Console.WriteLine("7. Algorytm Bellmana-Forda");
                Console.WriteLine("0. Wyjdź");
                opt = Convert.ToString(Console.ReadLine());

                switch (opt)
                {
                    case "1":
                        Console.Write("Podaj nazwę pliku: ");
                        fileName = Console.ReadLine();
                        AdjacencyList.LoadFromFile(fileName);
                        AdjacencyMatrix.LoadFromFile(fileName);
                        AdjacencyList.ShowList();
                        AdjacencyMatrix.ShowMatrix();
                        break;

                    case "2":
                        Console.WriteLine("Podaj liczbę wierzchołków: ");
                        v = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Podaj gęstość: ");
                        g = Convert.ToInt32(Console.ReadLine());
                        AdjacencyList.GenerateRandomGraph(v, g);
                        AdjacencyList.ShowList();
                        AdjacencyMatrix.GenerateRandomGraph(v, g);
                        AdjacencyMatrix.ShowMatrix();
                        break;

                    case "3":
                        AdjacencyList.ShowList();
                        AdjacencyMatrix.ShowMatrix();
                        break;

                    case "4":
                        AdjacencyList.Prim();
                        AdjacencyMatrix.Prim();
                        AdjacencyList.ShowMST();
                        AdjacencyMatrix.ShowMST();
                        break;

                    case "5":
                        AdjacencyList.Kruskal();
                        AdjacencyMatrix.Kruskal();
                        AdjacencyList.ShowMST();
                        AdjacencyMatrix.ShowMST();
                        break;

                    case "6":
                        AdjacencyList.Dijkstra();
                        AdjacencyMatrix.Dijkstra();
                        AdjacencyList.ShowShortestPaths();
                        AdjacencyMatrix.ShowShortestPaths();
                        break;

                    case "7":
                        AdjacencyList.BellmanFord();
                        AdjacencyMatrix.BellmanFord();
                        AdjacencyList.ShowShortestPaths();
                        AdjacencyMatrix.ShowShortestPaths();
                        break;
                }
            } while (opt != "0");
        }
    }
}
