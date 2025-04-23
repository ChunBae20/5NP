using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace This_is_Sparta__
{
    enum JobType
    {
        Warrior = 1,
        Mage = 2,
        Archer = 3
    }
    internal class Charter : MainSpace
    {
        static Player player = new Player();
        static void PlayerMain(string[] args)
        {
            MainMenu();
        }

        private static void MainMenu()
        {
            throw new NotImplementedException();
        }

        static void ShowIntro()
        {
            Console.Clear();
            Console.WriteLine("당신은 위대한 영웅들의 고향 스파르타 마을에 도착했습니다...");
            Console.WriteLine("위대한 영웅들 처럼 모험을 시작하려면 캐릭터를 생성하세요!");
            CreateCharacter();
        }

        static void CreateCharacter()
        {
            Console.Write("캐릭터 이름을 입력하세요: ");
            player.Name = Console.ReadLine();

            Console.WriteLine("\n직업을 선택하세요:");
            Console.WriteLine("1. 전사 (공격력 15 / 방어력 10 / 체력 120)");
            Console.WriteLine("2. 마법사 (공격력 25 / 방어력 5 / 체력 80)");
            Console.WriteLine("3. 궁수 (공격력 18 / 방어력 7 / 체력 100)");
            Console.Write("선택 (1~3): ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    player.Job = JobType.Warrior;
                    player.Attack = 15;
                    player.Defense = 10;
                    player.HP = 120;
                    break;
                case "2":
                    player.Job = JobType.Mage;
                    player.Attack = 25;
                    player.Defense = 5;
                    player.HP = 80;
                    break;
                case "3":
                    player.Job = JobType.Archer;
                    player.Attack = 18;
                    player.Defense = 7;
                    player.HP = 100;
                    break;
                default:
                    Console.WriteLine("잘못된 선택입니다. 다시 입력하세요.");
                    CreateCharacter(); // 재귀 호출
                    return;
            }

            player.Level = 1;
            player.Gold = 100;
            player.Inventory = new List<Item>();
            player.EquippedItem = null;

            Console.WriteLine($"\n{player.Job} 직업의 {player.Name}님으로 게임을 시작합니다!");
            MainMenu();
        }

        private class Player
        {
            public int Level = 1;
            public string Name;
            public JobType Job; // string → JobType으로 변경
            public int Attack;
            public int Defense;
            public int HP;
            public int Gold;

            public List<Item> Inventory = new List<Item>();
            public Item EquippedItem = null;

            public void ShowStatus()
            {
                Console.Clear();
                Console.WriteLine("상태 보기");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");

                int totalAttack = Attack + (EquippedItem?.AttackBonus ?? 0);
                int totalDefense = Defense + (EquippedItem?.DefenseBonus ?? 0);

                Console.WriteLine($"Lv .{Level:D2}");
                Console.WriteLine($"{Name} ({Job})");
                Console.WriteLine($"공격력 : {totalAttack}");
                Console.WriteLine($"방어력 : {totalDefense}");
                Console.WriteLine($"체 력 : {HP}");
                Console.WriteLine($"Gold : {Gold}G\n");

                Console.WriteLine("0. 나가기");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                string input = Console.ReadLine();

                if (input == "0")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    ShowStatus(); // 다시 표시
                }
            }

        }
    }
}
