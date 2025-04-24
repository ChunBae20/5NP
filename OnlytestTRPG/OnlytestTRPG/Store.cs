using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OnlytestTRPG
{
    public  class Item  //인터널에서 바꿨음 -팀장-
    {
        public string ItemName;
        public string ItemType;
        public int ItemStat;
        public int Price;
        public bool IsBuy = false;
        Random random = new Random();

        public Item(string itemName, string itemType, int itemStat, int price)
        {
            ItemName = itemName;
            ItemType = itemType;
            ItemStat = itemStat;
            Price = price * random.Next(80, 121) / 100; ;
        }

        public void BuyAndSell(bool withIndex = false, int index = 0)
        {
            string status = IsBuy ? "SoldOut" : $"{Price}";
            string prefix = withIndex ? $"-{index + 1}." : "-";
            Console.WriteLine($"{prefix} {ItemName} | {ItemType} | +{ItemStat} | {status}");
        }

    }
    public class Store : MainSpace
    {
        //이거 보상도 이걸 좀 끌어다 써야겠는데?어떻게하지? 이거 하나더만들고 이름만 다르게해서 또 불러야하나?
        public static List<Item> itemList = new List<Item>()  //이거도 퍼블릭으로 바꿈 -팀장-
        {
            new("낡은 검", "공격력", 2, 500),
            new("낡은 옷", "방어력", 2, 500)
        };

        public void EnterStore()
        {
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine($"아이템을 구매와 판매 수 있는 상점입니다.");
            Console.WriteLine($"\n{status.basicGold} G ");
            Console.WriteLine("\n[아이템 목록]");

            for (int i = 0; i < itemList.Count; i++)
            {
                itemList[i].BuyAndSell(false, i);
            }

            Console.WriteLine("\n\n1. 구메");
            Console.WriteLine("2. 판매");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요");
            Console.Write(">> ");

            int num = Input(0, 2);

            switch (num)
            {
                case 0: MainMenu(); break;
                case 1: BuyItem(); break;
                case 2: SellItem(); break;
            }
        }

        public void BuyItem()
        {
            Console.Clear();
            Console.WriteLine("상점 - 구매");
            Console.WriteLine($"아이템을 구매와 판매 수 있는 상점입니다.");
            Console.WriteLine($"\n{status.basicGold}G ");
            Console.WriteLine("\n[아이템 목록]");

            for (int i = 0; i < itemList.Count; i++)
            {
                itemList[i].BuyAndSell(true, i);
            }

            Console.WriteLine("\n\n0. 나가기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요");
            Console.Write(">> ");

            int num = Input(0, itemList.Count);

            if (num == 0)
            { 
                EnterStore(); 
                return; 
            }

            var selectedItem = itemList[num - 1];

            if (status.basicGold >= selectedItem.Price && selectedItem.IsBuy == false)
            {
                status.basicGold -= selectedItem.Price;
                selectedItem.IsBuy = true;

                Inventory.equipment.Add(new Equipment(selectedItem.ItemName, selectedItem.ItemType, selectedItem.ItemStat, selectedItem.Price));

                Console.WriteLine($"{selectedItem.ItemName}을 구매하셨습니다. (잔액: {status.basicGold}G)");
            }
            else if (status.basicGold >= selectedItem.Price && selectedItem.IsBuy == true) Console.WriteLine("이미 구매하셨습니다.");
            else Console.WriteLine("Gold가 부족합니다.");

            Console.WriteLine("\n아무 키나 누르면 계속합니다...");
            Console.ReadKey();
            EnterStore();
        }

        public void SellItem()
        {
            Console.Clear();
            Console.WriteLine("상점 - 판매");
            Console.WriteLine($"아이템을 구매와 판매 수 있는 상점입니다");
            Console.WriteLine($"{status.basicGold}G ");
            Console.WriteLine("[아이템 목록]");

            var inventory = Inventory.equipment;

            if (inventory.Count == 0)
            {
                Console.WriteLine("판매할 아이템이 없습니다.");
                Console.WriteLine("\n아무 키나 누르면 돌아갑니다...");
                Console.ReadKey();
                EnterStore();
                return;
            }

            for (int i = 0; i < inventory.Count; i++)
            {
                inventory[i].Equip(true,i);
            }

            Console.WriteLine("\n\n0. 나가기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요");
            Console.Write(">> ");

            int num = Input(0, inventory.Count);

            switch (num)
            {
                case 0: MainMenu(); break;
                default:
                    var selected = inventory[num - 1];

                    int sellPrice = selected.SellPrice;
                    sellPrice = (int)(sellPrice * 0.7);

                    inventory.RemoveAt(num - 1);
                    status.basicGold += sellPrice;

                    foreach (var item in itemList)
                    {
                        if(item.ItemName == selected.EquipmentName && item.IsBuy)
                        {
                            item.IsBuy = false;
                            break;
                        }
                    }
                    Console.WriteLine($"{selected.EquipmentName}을 판매하였습니다. (+{sellPrice}G)");
                    Console.WriteLine($"남은 골드: {status.basicGold}G");
                    Console.WriteLine("\n아무 키나 누르면 계속합니다...");
                    Console.ReadKey();
                    EnterStore();
                    break;
            }
        }
    }
}
