using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDiZO_2
{
    struct Edge //krawedz
    {
        public int v1, v2, weight; //wierzcholek poczatkowy, koncowy, waga krawedzi
    }

    class Queue
    {
        Edge[] Heap;
        int hpos;

        public Queue(int n)
        {
            Heap = new Edge[n];
            hpos = 0;
        }

        public Edge front()
        {
            return Heap[0];
        }

        public void Push(Edge e)
        {
            int i, j;

            i = hpos++;                     // i ustawiamy na koniec kopca
            j = (i - 1) >> 1;               // Obliczamy pozycję rodzica

            // Szukamy miejsca w kopcu dla e

            while (i > 0 && (Heap[j].weight > e.weight))
            {
                Heap[i] = Heap[j];
                i = j;
                j = (i - 1) >> 1;
            }
            Heap[i] = e;
        }

        public void Pop()
        {
            int i, j;
            Edge e;

            if (hpos > 0)
            {
                e = Heap[--hpos];
                i = 0;
                j = 1;

                while (j < hpos)
                {
                    if ((j + 1 < hpos) && (Heap[j + 1].weight < Heap[j].weight)) j++;
                    if (e.weight <= Heap[j].weight) break;
                    Heap[i] = Heap[j];
                    i = j;
                    j = (j << 1) + 1;
                }
                Heap[i] = e;
            }
        }
    }

    struct DSNode
    {
        public int up, rank;
    }

    class DSStruct
    {
        private DSNode[] Z;

        public DSStruct(int n)
        {
            Z = new DSNode[n];
        }

        public void MakeSet(int v)
        {
            Z[v].up = v;
            Z[v].rank = 0;
        }

        public int FindSet(int v)
        {
            if (Z[v].up != v)
                Z[v].up = FindSet(Z[v].up);
            return Z[v].up;
        }

        public void UnionSets(Edge e)
        {
            int ru, rv;
            ru = FindSet(e.v1); //korzen drzewa z wezlem u
            rv = FindSet(e.v2); //korzen drzewa z wezlem v
            if (ru != rv)
            {
                if (Z[ru].rank > Z[rv].rank) //porownanie rang drzew
                    Z[rv].up = ru; //ru wieksze, dolaczamy rv
                else
                {
                    Z[ru].up = rv;  //rowne lub rv wieksze, dolaczamy ru
                    if (Z[ru].rank == Z[rv].rank) Z[rv].rank++;
                }
            }
        }
    }

    class ALElement
    {
        public ALElement next;
        public int v, weight;

    }

}
