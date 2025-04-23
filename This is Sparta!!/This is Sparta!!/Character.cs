using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace This_is_Sparta__
{
    class Character
    {
        public int Level { get; }
        public string Name { get; }
        public string Job { get; }
        public int Atk { get; }
        public int Def { get; }
        public int MaxHp { get; }
        public int CurrentHp { get;  set; }
        public int Gold { get; set; }

        public int ExtraAtk { get; private set; }
        public int ExtraDef { get; private set; }

        private List<Item> Inventory = new List<Item>();
        private List<Item> EquipList = new List<Item>();

        public int InventoryCount
        {
            get
            {
                return Inventory.Count;
            }
        }
        public Character(int level, string name, string job, int atk, int def, int maxHp, int gold)
        {
            Level = level;
            Name = name;
            Job = job;
            Atk = atk;
            Def = def;
            MaxHp = maxHp;
            CurrentHp = maxHp;
            Gold = gold;
        }

        public void DisPlayCharacterInfo()
        {
            Console.WriteLine($"Lv.{Level:D2}");
            Console.WriteLine($"{Name}({Job})");
            Console.WriteLine(ExtraAtk == 0 ? $"공격력: {Atk}" : $"공격력: {Atk + ExtraAtk}(+{ExtraAtk})");
            Console.WriteLine(ExtraDef == 0 ? $"공격력: {Def}" : $"공격력: {Def + ExtraDef}(+{ExtraDef})");
            Console.WriteLine($"체력:{CurrentHp}");
            Console.WriteLine($"gold: {Gold}G");
        }
        public void DisplayInventory(bool showIdx)
        {

            for (int i = 0; i < Inventory.Count; i++)
            {
                Item targetItem = Inventory[i];

                string displayIdx = showIdx ? $"{i + 1}" : "";
                string displayEquipped = IsEquipped(targetItem) ? "[E]" : "";

                Console.WriteLine($" - {displayEquipped} {targetItem.ItemInfoText()}");
            }
        }

        public void EquipItem(Item item)
        {


            if (IsEquipped(item))
            {
                EquipList.Remove(item); //장착이 되어있다 -> 해제
                if (item.Type == 0)
                    ExtraAtk -= item.Value;
                else
                    ExtraDef -= item.Value;
            }
            else
            {
                EquipList.Add(item); //장착이 안되어있다 -> 장착
                if (item.Type == 0)
                    ExtraAtk += item.Value;
                else
                    ExtraDef += item.Value;
            }
        }

        public bool IsEquipped(Item item)
        {
            return EquipList.Contains(item);
        }

        public void BuyItem(Item item)
        {
            Gold -= item.Price;
            Inventory.Add(item);
        }
        public bool HasItem(Item item)
        {
            return Inventory.Contains(item);
        }


    }
}
