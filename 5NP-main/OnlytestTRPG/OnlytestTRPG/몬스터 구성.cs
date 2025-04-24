using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace OnlytestTRPG
{
    class Monster
    {
        public int Level { get; }
        public string Name { get; }
        public int Atk { get; }
        public int MaxHp { get; }
        public int CurrentHp { get; set; }
        public bool IsDead { get; set; } = false;
        public int GoldReward { get; set; }
        public int AppearStage { get; internal set; }

        public Monster(string name,int level, int maxHp, int atk, int goldReward)
        {
            Name = name;
            Level = level;
            Atk = atk;
            MaxHp = maxHp;
            CurrentHp = maxHp;
            GoldReward = goldReward;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"몬스터: {Name}");
            Console.WriteLine($"레벨: {Level}");
            Console.WriteLine($"체력: {MaxHp}");
            Console.WriteLine($"공격력: {Atk}");
           // Console.WriteLine($"방어력: {Defense}");
            Console.WriteLine($"획득 골드: {GoldReward}G");
        }

        public static List<Monster> GenerateMonsters()
        {
            return new List<Monster>
            {
                new Monster("독고벌레",1, 20, 8, 20),
                new Monster("바위게", 2, 25, 9, 10),
                new Monster("칼날구울", 2, 15, 13, 10), 
                new Monster("늑대 고블린", 2, 20, 25, 10),
                new Monster("붉은 도마뱀", 2, 30, 15, 50),
                new Monster("쌍둥이 골렘", 2, 100, 10, 50),
                new Monster("던전 악령", 2, 150, 40, 50),
                new Monster("황금 드래곤", 20, 200, 50, 10000),
            };
        }

        public static void ShowMonsterList()
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

        internal object MonsterInfoText()
        {
            throw new NotImplementedException();
        }
    }

}