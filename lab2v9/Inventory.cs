using System;
using System.Collections.Generic;

namespace Lab2
{
    public class Inventory
    {
        private List<string> items = new List<string>();

        public int Count => items.Count;

        public string this[int index]
        {
            get
            {
                if (index >= 0 && index < items.Count)
                    return items[index];
                else
                    throw new IndexOutOfRangeException("Індекс за межами списку.");
            }
            set
            {
                if (index >= 0 && index < items.Count)
                    items[index] = value;
                else
                    throw new IndexOutOfRangeException("Індекс за межами списку.");
            }
        }

        public int this[string name]
        {
            get
            {
                return items.IndexOf(name);
            }
        }

        public static Inventory operator +(Inventory inv, string item)
        {
            inv.items.Add(item);
            return inv;
        }

        public override string ToString()
        {
            return $"Інвентар ({items.Count}): {string.Join(", ", items)}";
        }
    }
}
