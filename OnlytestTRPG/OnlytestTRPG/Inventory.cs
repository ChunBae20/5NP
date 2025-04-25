using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using OnlytestTRPG; //나만 추가


namespace OnlytestTRPG
{

    internal class Equipment
    {
        public string EquipmentName;
        public string EquipmentType;
        public int EquipmentStat;
        public int SellPrice;
        public bool IsEquiped = false;
        Random random = new Random();

        public Equipment(string equipmentName, string equipmentType, int equipmentStat, int sellPrice)
        {
            EquipmentName = equipmentName;
            EquipmentType = equipmentType;
            EquipmentStat = equipmentStat;
            SellPrice = sellPrice * random.Next(80, 121) / 100;
        }

        public void Equip(bool withIndex = false, int index = 0)
        {
            string status = IsEquiped ? $"[E]" : "";
            string prefix = withIndex ? $"-{index + 1}." : "-";
            Console.WriteLine($"{prefix} {status}{EquipmentName} | {EquipmentType} | +{EquipmentStat}");
        }

    }

    public class Inventory : MainSpace
    {
        internal static List<Equipment> equipment = new List<Equipment>()
        {
        };

        public void InventoryUI()
        {
            Console.Clear();
            Console.WriteLine("인벤토리");
            Console.WriteLine("캐릭터의 장비가 표시 됩니다.\n");

            for (int i = 0; i < equipment.Count; i++)
            {
                equipment[i].Equip(false, i);
            }

            Console.WriteLine("\n1. 장착");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            Console.Write(">> ");

            int num = Input(0, 1);   //int num = MainSpace.Input(0, 1);  

            switch (num)
            {
                case 0: MainSpace.MainMenu(); break;
                case 1: EquippedAndUnequipped(); break;
            }
        }

        public void EquippedAndUnequipped()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("인벤토리 - 장착");
                Console.WriteLine("캐릭터의 장비를 장착 및 해제 가능 합니다.\n");

                for (int i = 0; i < equipment.Count; i++)
                {
                    equipment[i].Equip(true, i);
                }

                Console.WriteLine("\n0. 나가기");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">> ");


                int num = Input(0, equipment.Count);

                switch (num)
                {
                    case 0: InventoryUI(); break;
                    default:
                        equipment[num - 1].IsEquiped = !equipment[num - 1].IsEquiped;
                        SearchEquipWeapon();
                        string action = equipment[num - 1].IsEquiped ? "장착" : "해제";
                        Console.WriteLine($"\n{equipment[num - 1].EquipmentName} {action} 완료");
                        Console.WriteLine("\n아무 키나 누르면 계속합니다...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        public void SearchEquipWeapon()
        {
            int weaponSTR = 0; //무기공격력    혹시라도 나중에 공격력이랑 방어력 등 두개 이상 할수도있을거같으니일단 이렇게해둠
            int weaponDEF = 0; // 방어구 공격력
            int weaponCRT = 0; // 치명타
            int weaponAVD = 0; // 회피

            for (int i = 0; i < Inventory.equipment.Count; i++)
            {
                if (Inventory.equipment[i].IsEquiped == true)                 // 장창된게 확인된다면
                {
                    if (Inventory.equipment[i].EquipmentType == "공격력")          //아이템 타입이 공격력이라면
                    {
                        weaponSTR += Inventory.equipment[i].EquipmentStat;
                    }
                    else if (Inventory.equipment[i].EquipmentType == "방어력")     //아이템 타입이 방어력이라면
                    {
                        weaponDEF += Inventory.equipment[i].EquipmentStat;         //weaponDEF에 방어력을 더해준다
                    }
                    else if (Inventory.equipment[i].EquipmentType == "치명타")     //아이템 타입이 치명타라면
                    {
                        weaponCRT += Inventory.equipment[i].EquipmentStat;         //weaponCRT에 치명타를 더해준다
                    }
                    else if (Inventory.equipment[i].EquipmentType == "회피")       //아이템 타입이 회피라면
                    {
                        weaponAVD += Inventory.equipment[i].EquipmentStat;         //weaponAVD에 회피를 더해준다
                    }
                }
                else
                {

                }
            }
            // 여기서 Status에 저장
            MainSpace.status.nowEquipSTR = weaponSTR;
            MainSpace.status.nowEquipDEF = weaponDEF;
            MainSpace.status.nowEquipCRT = weaponCRT;
            MainSpace.status.nowEquipAVD = weaponAVD;
        }
    }
}