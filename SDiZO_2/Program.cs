using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDiZO_2
{
    class Program
    {
        static void Main(string[] args)
        {
            AdjacencyList al = new AdjacencyList();
            al.LoadFromFile();
            al.ShowList();
            //al.Dijskra();
            //al.Kruskal();
            //al.ShowResultList();
            //al.Prim();
            //al.ShowResultList();
            al.BellmanFord();
            //al.ShowList();
            //al.ShowAfterDijskra();
            al.ShowList();
            al.ShowAfterDijskra();
            /*al.LoadFromFile();
            al.Prim();
            al.ShowList();*/

            /*AdjacencyMatrix am = new AdjacencyMatrix();
            am.LoadFromFile();
            am.ShowMatrix();
            am.Prim();
            am.ShowMatrix();*/
            Console.ReadLine();
        }
    }
}
