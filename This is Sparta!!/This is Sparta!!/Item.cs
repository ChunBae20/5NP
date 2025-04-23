using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace This_is_Sparta__
{
    class Item
    {
        public string Names { get; }
        public int Type { get; }
        public int Value { get; }
        public string Desc { get; }
        public int Price { get; }
        public string DisPlayTypeText
        {
            get
            {
                return Type == 0 ? "공격력" : "방어력";
            }
        }
        public Item(string names, int type, int value, string desc, int price)
        {
            Names = names;
            Type = type;
            Value = value;
            Desc = desc;
            Price = price;
        }
        public string ItemInfoText()
        {
            return $"{Names} | {DisPlayTypeText} +{Value} | {Desc}";
        }
    }
}
