using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OnlytestTRPG
{
    enum JobType                                               
    {
        Warrior = 1,
        Mage = 2,
        Archer = 3
    }
    public class Character
    {
        static Player player = new Player();     
       

        public static void ShowIntro()
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
        }

        private class Player
        {
            public int Level = 1;
            public string Name;
            public JobType Job; 
            public int Attack;
            public int Defense;
            public int HP;
            public int Gold;

            public List<Item> Inventory = new List<Item>();    
            public Item EquippedItem = null;

        }
    }
}