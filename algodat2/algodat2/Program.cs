using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algodat2
{
    /// <summary>
    /// Dictionairy
    /// </summary>
    interface IDictionary
    {
        bool Search(int elem);
        bool Insert(int elem);
        bool Delete(int elem);
        void Print();
    }

    /// <summary>
    /// INTERFACES
    /// </summary>
    interface IMultiSet : IDictionary { }
    interface ISet : IMultiSet { }
    interface IMultiSetSorted : IDictionary { }
    interface ISetSorted : IMultiSetSorted { }

    /// <summary>
    /// abstrakte Oberklasse Array 
    /// </summary>
    abstract class Array
    {
        public Array()
        {
            array = new int[100];
            anz = 0;
        }

        protected int[] array;
        protected int anz;
        /// <summary>
        /// Print Funktion ist für alle Arrayarten gleich
        /// </summary>
        public void Print()
        {
            foreach (int i in array)
                if (i != 0)
                    Console.WriteLine(i);
        }

        /// <summary>
        /// search die auf indivuellen search mit spezifizierteren rückgabenwerten zurückgreift
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        public bool Search(int elem)
        {
            Search(elem, out bool b);
            return b;
        }


        protected int Search(int elem, out bool b)
        {
            int i;
            b = false;
            for (i = 0; i < array.Length; i++)
                if (elem == array[i])
                {
                    b = !b;
                    return i;
                }
            return i - 1;
        }
    }

    /// <summary>
    /// Mehrfache gleiche Elemente erlaubt und unsortiert
    /// </summary>
    class MultiSetUnsortedArray : Array, IMultiSet
    {
        public MultiSetUnsortedArray() : base() { }

        public bool Insert(int elem)
        {
            if (anz == array.Length)
                return false;
            array[anz] = elem;
            anz++;
            return true;
        }
        public bool Delete(int elem)
        {
            int pos = Search(elem, out bool b);
            if (b)
            {
                array[pos] = array[anz];
                array[anz] = 0;
                anz--;
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Ein gleiches Element erlaubt aber unsortiert
    /// </summary>
    class SetUnsortedArray : MultiSetUnsortedArray, ISet
    {

        public SetUnsortedArray() : base() { }

        public new bool Insert(int elem)
        {
            if (Search(elem))
                return false;
            return base.Insert(elem);
        }
    }

    /// <summary>
    /// Mehrere gleiche Elemente erlaubt aber sortiert
    /// </summary>
    class MultiSetSortedArray : Array, IMultiSetSorted
    {
        public MultiSetSortedArray() : base() { }

        new protected int Search(int elem, out bool b)
        {
            int left = 0;
            int right = anz;
            int middle = (left + right) / 2;

            while (left <= right)
            {
                if (array[middle] == elem)
                {
                    b = true;
                    return middle;
                }
                if (array[middle] < elem)
                    left = middle + 1;
                if (array[middle] > elem)
                    right = middle - 1;
                middle = (left + right) / 2;
            }
            b = false;
            return middle + 1;
        }

        public bool Insert(int elem)
        {
            if (anz == array.Length)
                return false;
            int pos = Search(elem, out bool b);
            Insert(elem, pos);
            return true;
        }

        protected void Insert(int elem, int pos)
        {
            for (int i = anz; i >= pos; i--)
            {
                array[i + 1] = array[i];
            }
            array[pos] = elem;
            anz++;
            return;
        }

        public bool Delete(int elem)
        {
            int pos = Search(elem, out bool b);
            if (!b)
            {
                return false;
            }
            for (int i = pos; i < anz; i++)
            {
                array[i] = array[i + 1];
            }
            array[anz] = 0;
            anz--;
            return true;
        }
    }

    /// <summary>
    /// Ein gleiches Element erlaubt und sortiert
    /// </summary>
    class SetSortedArray : MultiSetSortedArray, ISetSorted
    {
        public SetSortedArray() : base() { }

        new public bool Insert(int elem)
        {
            if (anz == array.Length)
                return false;
            int pos = Search(elem, out bool b);
            if (b)
                return false;
            Insert(elem, pos);
            return true;
        }
    }

    abstract class Baum
    {
        protected class Element
        {
            public int wert;
            public Element rechts;
            public Element links;

            public Element(int wert) => this.wert = wert;
        }

        public Baum() => wurzel = null;

        protected Element wurzel;

        public void Print ()
        {
            Print(wurzel, "");
        }

        void Print(Element elem, string s)
        {
            if(elem != null)
            {
                Print(elem.rechts, s+"   ");
                Console.WriteLine(s+elem.wert);
                Print(elem.links, s + "   ");
            }
        }
    }

    enum Richtung{ rechts, links, keine}
    class BinBaum: Baum, ISetSorted
    {
        public BinBaum() : base() { }

        public bool Search(int elem)
        {
            Search(elem, out bool gefunden, out _);
            return gefunden;
        }

        /// <summary>
        /// private Search funktion
        /// </summary>
        /// <param name="wert"></param>
        /// <param name="elem"></param>
        /// <param name="gefunden"></param>
        /// <param name="richtung">1=rechts, 0=links</param>
        /// <returns></returns>
        Element Search(int wert, out bool gefunden, out Richtung R)
        {
            gefunden = true;
            if (wurzel.wert == wert)
            {
                R = Richtung.keine;
                return wurzel;
            }
            Element vorgänger = Vorgänger(wert, out bool ElemGefunden, out R);
            if (!ElemGefunden)
            {
                gefunden = false;
                return null;
            }
            if (R == Richtung.rechts)
                return vorgänger.rechts;
            return vorgänger.links;
        }

        public bool Insert(int elem)
        {
            Element neu = new Element(elem);
            if (wurzel == null)
            {
                wurzel = neu;
                return true;
            }
            Element tmp = Vorgänger(elem, out bool gefunden, out Richtung R);
            if (gefunden)
                return false;
            if (R == Richtung.rechts)
                tmp.rechts = neu;
            if (R == Richtung.links)
                tmp.links = neu;
            return true;
        }

        public bool Delete(int elem)
        {
            Element vorgänger = Vorgänger(elem, out bool gefunden, out Richtung R);
            // 1. Fall Es gibt keinen Knoten mit element e
            if (!gefunden)
                return false;

            // 2. fall Knoten mit element ist ein Blatt.
            
            Element tmp;
            if (R == Richtung.rechts)
                tmp = vorgänger.rechts;
            else if (R == Richtung.links)
                tmp = vorgänger.links;
            else
                tmp = vorgänger;
            if (tmp.rechts == null && tmp.links == null)
            {
                if (R == Richtung.rechts)
                    vorgänger.rechts = null;
                else
                    vorgänger.links = null;
                return true;
            }

            //3. Fall Knoten mit element besitzt einen nachfolger
            
            if (tmp.rechts == null)
            {
                if (R == Richtung.links)
                    vorgänger.links = vorgänger.links.links;
                else
                    vorgänger.rechts = vorgänger.rechts.links;
                return true;
            }
            if (tmp.links == null)
            {
                if (R == Richtung.links)
                    vorgänger.links = vorgänger.links.rechts;
                else
                    vorgänger.rechts = vorgänger.rechts.rechts;
                return true;
            }

            // 4. Fall Knoten mit e hat 2 Nachfolger
 
            Element tmp2 = tmp.links;
            while (tmp2.rechts != null)
                tmp2 = tmp2.rechts;
            int symvorg = tmp2.wert;
            Delete(symvorg);
            tmp.wert = symvorg;
            return true;
        }
         
        Element Vorgänger(int wert, out bool ElemGefunden, out Richtung R)
        {
            ElemGefunden = false;
            R = Richtung.keine;
            if (wurzel == null)
                return null;
            Element tmp = wurzel;
            ElemGefunden = true;
            while (tmp.links != null || tmp.rechts != null)
            {
                if (tmp.links != null && tmp.links.wert == wert)
                {
                    R = Richtung.links;
                    return tmp;
                }
                if (tmp.rechts != null && tmp.rechts.wert == wert)
                {
                    R = Richtung.rechts;
                    return tmp;
                }
                if (tmp.wert < wert)
                {
                    if (tmp.rechts != null)
                        tmp = tmp.rechts;
                    else
                    {
                        ElemGefunden = false;
                        R = Richtung.rechts;
                        return tmp;
                    }
                }
                if (tmp.wert > wert)
                {
                    if (tmp.links != null)
                        tmp = tmp.links;
                    else
                    {
                        ElemGefunden = false;
                        R = Richtung.links;
                        return tmp;
                    }
                }
                if(tmp.wert==wert)
                {
                    R = Richtung.keine;
                    return tmp;
                }
            }
            ElemGefunden = false;
            if (tmp.wert < wert)
                R = Richtung.rechts;
            if (tmp.wert > wert)
                R = Richtung.links;
            return tmp;
        }
    }

    abstract class LinkedList
    {
        protected class Element
        {
            public Element next;
            public int wert;

            public Element(int wert) => this.wert = wert;
        }

        protected Element wurzel;
        public LinkedList() => wurzel = null;

        public bool Search(int elem)
        {
            return Search(elem, out _);
        }

        protected bool Search(int wert, out Element vorgänger)
        {
            vorgänger = wurzel;
            if (wurzel != null)
            {
                for (; vorgänger.next != null; vorgänger = vorgänger.next)
                {
                    if (vorgänger.next.wert == wert)
                        return true;
                }
                if (wurzel.wert == wert)
                {
                    vorgänger = null;
                    return true;
                }
            }
            return false;
        }


        public bool Delete(int elem)
        {
            bool gefunden = Search(elem, out Element vorgänger);
            if (!gefunden)
                return false;
            if(vorgänger == null)
            {
                wurzel = wurzel.next;
                return true;
            }
            vorgänger.next = vorgänger.next.next;
            return true;
        }

        public void Print()
        {
            for (Element tmp = wurzel; tmp != null; tmp = tmp.next)
                Console.WriteLine(tmp.wert);
        }
    }

    class MultiSetUnsortedLinkedList : LinkedList, IMultiSet
    {
        public MultiSetUnsortedLinkedList() : base() { }

        public bool Insert(int elem)
        {
            Element neu = new Element(elem);
            neu.next = wurzel;
            wurzel = neu;
            return true;
        }
    }

    class SetUnsortedLinkedList : MultiSetUnsortedLinkedList
    {
        public SetUnsortedLinkedList() : base() { }

        new public bool Insert(int elem)
        {
            bool gefunden = Search(elem, out _);
            if (!gefunden)
                return false;
            base.Insert(elem);
            return true;
        }
    }

    class MultiSetSortedLinkedList : LinkedList, IMultiSetSorted
    {
        public MultiSetSortedLinkedList() : base() { }

        public bool Insert(int elem)
        {
            bool gefunden = Search(elem, out Element vorgänger);
            Insert(elem, vorgänger);
            return true;
        }

        new protected bool Search(int wert, out Element vorgänger)
        {
            bool gefunden = false;
            if (wurzel == null || wert < wurzel.wert)
            {
                vorgänger = null;
                return false;
            }
            vorgänger = wurzel;
            for (Element tmp = wurzel; tmp != null; tmp = tmp.next)
            {
                if (tmp.wert == wert)
                    gefunden = true;
                if (vorgänger.wert < tmp.wert && tmp.wert < wert)
                    vorgänger = tmp;
            }
            return gefunden;
        }

        protected void Insert(int elem, Element vorgänger)
        {
            Element neu = new Element(elem);
            if (vorgänger == null)
            {
                neu.next = wurzel;
                wurzel = neu;
                return;
            }
            neu.next = vorgänger.next;
            vorgänger.next = neu;
        }
    }

    class SetSortedLinkedList : MultiSetSortedLinkedList, ISetSorted
    {
        public SetSortedLinkedList() : base() { }

        new public bool Insert(int elem)
        {
            bool gefunden = Search(elem, out Element vorgänger);
            if (gefunden)
                return false;
            base.Insert(elem, vorgänger);
            return true;

        }
    }

    class program
    {
        public static void Main()
        {
            SetSortedLinkedList B1 = new SetSortedLinkedList();

            B1.Insert(10);
            B1.Insert(3);
            B1.Insert(12);
            B1.Insert(1);
            B1.Insert(5);
            B1.Insert(4);
            B1.Delete(5);
            

            B1.Print();
        }
    }
}