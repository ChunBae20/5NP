using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using OnlytestTRPG;


namespace OnlytestTRPG
{

    public class Equipment : MainSpace
    {
        public string EquipmentName;
        public string EquipmentType;
        public int EquipmentStat;
        public int SellPrice;
        public bool IsEquiped = false;
        Random random = new();

        public Equipment(string equipmentName, string equipmentType, int equipmentStat, int sellPrice)
        {
            EquipmentName = equipmentName;
            EquipmentType = equipmentType;
            EquipmentStat = equipmentStat;
            SellPrice = sellPrice * random.Next(80, 121) / 100; // 판매 시 랜덤 부여
        }

        public void Equip(bool withIndex = false, int index = 0) // 장착 여부, 장비 앞 숫자 생성/비생성, 장비 List 출력
        {
            string status = IsEquiped ? $"[E]" : "";
            string prefix = withIndex ? $"-{index + 1}." : "-";
            Console.WriteLine($"{prefix} {status}{EquipmentName} | {EquipmentType} | +{EquipmentStat}");
        }

        public int GetStat(string type)
        {
            return Inventory.equipment
                .Where(e => e.IsEquiped && e.EquipmentType == type)
                .Sum(e => e.EquipmentStat);
        }
        public void SearchEquipWeapon() //장비 스탯 -> 플레이어 스탯
        {
            status.NowEquipSTR = GetStat("공격력");
            status.NowEquipDEF = GetStat("방어력");
            status.NowEquipCRT = GetStat("치명타");
            status.NowEquipAVD = GetStat("회피");
        }
    }
}

public class Inventory : MainSpace
{
    internal static List<Equipment> equipment = new List<Equipment>() // 상점, 게임 결과 장비 -> 인베토리 리스트
    {
    };

    public void InventoryUI() // 인벤토리 UI
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

        int num = Input(0, 1);

        switch (num)
        {
            case 0: MainSpace.MainMenu(); break;
            case 1: EquippedAndUnequipped(); break;
        }
    }

    public void EquippedAndUnequipped() // 장착 / 해제 관리 시스템
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


            int num = Input(0, equipment.Count);

            switch (num)
            {
                case 0: InventoryUI(); break;
                default:
                    var selectedEquip = equipment[num - 1];

                    selectedEquip.IsEquiped = !selectedEquip.IsEquiped;
                    equipment[num - 1].SearchEquipWeapon();
                    string action = selectedEquip.IsEquiped ? "장착" : "해제";

                    Console.WriteLine($"\n{selectedEquip.EquipmentName} {action} 완료");
                    Console.WriteLine("\n아무 키나 누르면 계속합니다...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}