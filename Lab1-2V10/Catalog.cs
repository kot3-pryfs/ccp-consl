using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_2V10
{
    public readonly struct Catalog
    {
        private readonly double _price;
        private readonly uint _quantity;
        private readonly string _category;
        private readonly string _name;

        public double Price { get { return _price; } }
        public uint Quantity { get { return _quantity; } }
        public string Category { get { return _category; } }
        public string Name { get { return _name; } }

        public Catalog(double price, uint quantity, string category, string name)
        {
            _price = price;
            _quantity = quantity;
            _category = category;
            _name = name;
        }
    }
}
