using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlytestTRPG
{
    class Monster
    {
        public string Name { get; set; }
        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int GoldReward { get; set; }

        public Monster(string name, int hp, int attack, int defense, int goldReward)
        {
            Name = name;
            HP = hp;
            Attack = attack;
            Defense = defense;
            GoldReward = goldReward;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"몬스터: {Name}");
            Console.WriteLine($"체력: {HP}");
            Console.WriteLine($"공격력: {Attack}");
            Console.WriteLine($"방어력: {Defense}");
            Console.WriteLine($"획득 골드: {GoldReward}G");
        }

        static List<Monster> GenerateMonsters()
        {
            return new List<Monster>
            {
                new Monster("고블린", 30, 8, 2, 20),
                new Monster("슬라임", 20, 5, 1, 10),
                new Monster("오크", 50, 12, 4, 50),
                new Monster("드래곤", 200, 25, 10, 500),
            };
        }

        static void ShowMonsterList()
        {
            List<Monster> monsters = GenerateMonsters();
            Console.WriteLine("\n=== 몬스터 도감 ===");
            for (int i = 0; i < monsters.Count; i++)
            {
                Console.WriteLine($"\n[{i + 1}]");
                monsters[i].ShowInfo();
            }

            Console.WriteLine("\n0. 나가기");
            Console.Write(">> ");
            Console.ReadLine(); // 대기
        }

    }

}