using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlytestTRPG
{
    class MonsterManager
    {
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

        public static List<Monster> GetMonstersByStage(int stage)
        {
            return GenerateMonsters().FindAll(m => stage >= m.AppearStage);
        }
    }
}
