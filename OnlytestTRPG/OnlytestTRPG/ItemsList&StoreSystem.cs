using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OnlytestTRPG
{
    public class Item
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

        public void BuyAndSell(bool withIndex = false, int index = 0) //아이템 구매 여부, 아이템 앞 숫자 생성/비생성, 아이템 List 출력
        {
            string status = IsBuy ? "SoldOut" : $"{Price}";
            string prefix = withIndex ? $"-{index + 1}." : "-";
            Console.WriteLine($"{prefix} {ItemName} | {ItemType} | +{ItemStat} | {status}");

        }

    }
    public class Store : MainSpace
    {
        public static List<Item> itemList = new List<Item>() // 아이템 List
        {
            new("녹슨 검", "공격력", 2, 500),
            new("녹슨 갑옷", "빙어력", 2, 500),
            new("단궁", "공격력", 2, 500),
            new("나무 지팡이", "공격력", 2, 500),
            new("철제 검", "공격력", 4, 700),
            new("일반 갑옷", "방어력", 4, 700),
            new("복합궁", "공격력", 4, 700),
            new("청동 지팡이", "공격력", 4, 700)
        };

        public void QuestWorking(string questProcess) // 지정된 퀘스트 진행 관리 시스템
        {
            QuestInfo? activeQuest = Quest.questList.FirstOrDefault(q => q.IsSelected && !q.IsFinished && q.QuestProcess == questProcess);
            if (activeQuest != null)
            {
                Quest questSystem = new Quest();
                questSystem.UpdateQuestProcess(questProcess, activeQuest);
            }
        }

        public void EnterStore() // 상점 UI
        {
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine($"아이템을 구매와 판매 수 있는 상점입니다.");
            Console.WriteLine($"\n{status.BasicGold} G ");
            Console.WriteLine("\n[아이템 목록]");

            for (int i = 0; i < itemList.Count; i++) //아이템 List 불러오기
            {
                itemList[i].BuyAndSell(false, i);
            }

            Console.WriteLine("\n\n1. 구메");
            Console.WriteLine("2. 판매");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요");

            int num = Input(0, 2);

            switch (num)
            {
                case 0: MainMenu(); break;
                case 1: BuyItem(); break;
                case 2: SellItem(); break;
            }
        }

        public void BuyItem() //구매
        {
            Console.Clear();
            Console.WriteLine("상점 - 구매");
            Console.WriteLine($"아이템을 구매와 판매 수 있는 상점입니다.");
            Console.WriteLine($"\n{status.BasicGold}G ");
            Console.WriteLine("\n[아이템 목록]");

            for (int i = 0; i < itemList.Count; i++) //아이템 List 불러오기
            {
                itemList[i].BuyAndSell(true, i);
            }

            Console.WriteLine("\n\n0. 나가기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요");

            int num = Input(0, itemList.Count);

            if (num == 0)
            { 
                EnterStore(); 
                return; 
            }

            var selectedItem = itemList[num - 1];

            if (status.BasicGold >= selectedItem.Price && selectedItem.IsBuy == false) // 구매 시
            {
                status.BasicGold -= selectedItem.Price;
                selectedItem.IsBuy = true; // 재 구매 불가

                Inventory.equipment.Add(new Equipment(selectedItem.ItemName, selectedItem.ItemType, selectedItem.ItemStat, selectedItem.Price)); // 장비 List에 추가

                Console.WriteLine($"{selectedItem.ItemName}을 구매하셨습니다. (잔액: {status.BasicGold}G)");
                QuestWorking("아이템 구매"); // 퀘스트 수행
            }
            else if (status.BasicGold >= selectedItem.Price && selectedItem.IsBuy == true) Console.WriteLine("이미 구매하셨습니다."); // Sold out 아이템 선택 시
            else Console.WriteLine("Gold가 부족합니다."); // 돈 부족

            Console.WriteLine("\n아무 키나 누르면 계속합니다...");
            Console.ReadKey();
            EnterStore();
        }

        public void SellItem() //판매
        {
            Console.Clear();
            Console.WriteLine("상점 - 판매");
            Console.WriteLine($"아이템을 구매와 판매 수 있는 상점입니다");
            Console.WriteLine($"{status.BasicGold}G ");
            Console.WriteLine("[아이템 목록]");

            var inventory = Inventory.equipment;

            for (int i = 0; i < inventory.Count; i++) //장비 리스트 불러오기
            {
                inventory[i].Equip(true, i);
            }

            if (inventory.Count == 0)
            {
                Console.WriteLine("판매할 아이템이 없습니다.");
                Console.WriteLine("\n아무 키나 누르면 돌아갑니다...");
                Console.ReadKey();
                EnterStore();
                return;
            }

            Console.WriteLine("\n\n0. 나가기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요");

            int num = Input(0, inventory.Count);

            switch (num)
            {
                case 0: MainMenu(); break;
                default:
                    var selected = inventory[num - 1];

                    int sellPrice = selected.SellPrice; // 장비 판매 시
                    sellPrice = (int)(sellPrice * 0.7);

                    inventory.RemoveAt(num - 1);
                    status.BasicGold += sellPrice;

                    foreach (var item in itemList) // 상점에서 구매했던 장비 -> 상점에서 재 구매 가능 
                    {
                        if(item.ItemName == selected.EquipmentName && item.IsBuy)
                        {
                            item.IsBuy = false;
                            break;
                        }
                    }
                    Console.WriteLine($"{selected.EquipmentName}을 판매하였습니다. (+{sellPrice}G)");
                    Console.WriteLine($"남은 골드: {status.BasicGold}G");
                    Console.WriteLine("\n아무 키나 누르면 계속합니다...");
                    Console.ReadKey();
                    EnterStore();
                    break;
            }
        }
    }
}
