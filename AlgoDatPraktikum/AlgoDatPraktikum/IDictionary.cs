using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDatPraktikum
{
    interface IDictionary
    {
        bool search(int elem);   // true = gefunden

        bool insert(int elem);   // true = hinzugefügt

        bool delete(int elem);   // true = gelöscht

        void print();            // Einfache Ausgabe der Elemente des Wörterbuchs auf der Console,
                                 // so dass Inhalt und Struktur daraus eindeutig erkennbar
                                 // Für die Ausgabe von Bäumen wird das in der Übung behandelte Verfahren verwendet
    }

    interface IMultiSet : IDictionary { }

    interface ISet : IMultiSet { }

    interface IMultiSetSorted : IDictionary { }

    interface ISetSorted : IMultiSetSorted { }
}
