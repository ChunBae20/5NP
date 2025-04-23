using System;
using System.Collections.Generic;

namespace This_is_Sparta__
{

    public class Reward
    {
        public string Name;
        public int Amount;

        public Reward(string name, int amount)
        {
            Name = name;
            Amount = amount;
        }
    }
    public class MonsterReward
    {
        public string MonsterType;
        public Func<Reward> GetReward;

        public MonsterReward(string type, Func<Reward> rewardFunc)
        {
            MonsterType = type;
            GetReward = rewardFunc;
        }
    }
    internal class Program
    {

        private static int level;
        private static string name;
        private static int maxHp;
        private static int currentHp;
        private static int mostersDefeated;

        static readonly Dictionary<string, List<Reward>> dropTable = new Dictionary<string, List<Reward>>
        {
           { "미니언",      new List<Reward>{ new Reward("Gold",  50) } },
           { "공허충",      new List<Reward>{ new Reward("Gold", 150) } },
           { "대포미니언",  new List<Reward>{
          new Reward("Gold",  300),
          new Reward("포션", 1),
          new Reward("낡은검", 1)
              }}
        };


        private static int goldReward;
        private static int potionCount;
        private static int swordCount;

        static void Main(string[] args)
        {
            setDate();

            var kills = new List<string> { "미니언", "공허충", "대포미니언" };
            int killsCount = kills.Count;
            int initHp = maxHp;
            int finalHp = currentHp;
            var rewards = CollectAllRewards(kills);

            ShowBattleResult(killsCount, initHp, finalHp, rewards);

            Console.WriteLine("\n--- 패배 화면 테스트 (HP 0) ---");
    ShowBattleResult(killsCount, initHp, 0, new List<Reward>());
        }
        static void setDate()
        {
            level = 1;
            name = "Chad";
            maxHp = 100;
            currentHp = 74;
            mostersDefeated = 3;
        }

        // — 보상 합산 메서드 (dropTable 기반) —
        static List<Reward> CollectAllRewards(List<string> kills)
        {
            // 1) 임시로 보상 합계를 저장할 딕셔너리
            var aggregated = new Dictionary<string, int>();

            // 2) kills 리스트에 기록된 몬스터별로 dropTable에서 보상 꺼내오기
            foreach (var mType in kills)
            {
                // dropTable에서 이 몬스터의 보상 목록(drops)을 얻고
                if (!dropTable.TryGetValue(mType, out var drops))
                    continue;  // 없는 몬스터면 건너뛰기

                // 각각의 Reward를 aggregated에 누적
                foreach (var r in drops)
                {
                    // 없으면 0으로 시작
                    if (!aggregated.ContainsKey(r.Name))
                        aggregated[r.Name] = 0;
                    aggregated[r.Name] += r.Amount;
                }
            }

            // 3) 딕셔너리를 List<Reward>로 변환해서 반환
            var result = new List<Reward>();
            foreach (var kv in aggregated)
                result.Add(new Reward(kv.Key, kv.Value));

            return result;
        }
        // 기존: static void ShowBattleResult(bool allMonstersDead, List<string> kills, int initHp, int finalHp)
        static void ShowBattleResult(int killsCount, int initHp, int finalHp, List<Reward> rewards)
        {
            // 1) 체력 0 이하 → 무조건 패배, 보상 없음
            if (finalHp <= 0)
            {
                DisplayResultUI(
                    "You Lose",
                    killsCount,
                    initHp,
                    finalHp,
                    new List<Reward>()   // 패배 시 획득 보상 비우기
                );
                return;
            }

            // 2) 체력 남았고, 목표 몹 전부 처치 시 → 승리, 실제 보상 보여 주기
            if (killsCount >= mostersDefeated)
            {
                DisplayResultUI(
                    "Victory",
                    killsCount,
                    initHp,
                    finalHp,
                    rewards             // 승리 시 원래 합산된 보상
                );
            }
            else
            {
                // 3) HP는 남았지만 몹을 다 못 잡은 경우 → 패배로 처리
                DisplayResultUI(
                    "You Lose",
                    killsCount,
                    initHp,
                    finalHp,
                    new List<Reward>()  // 보상 없음
                );
            }
        }

        static void DisplayResultUI(
            string resultText,
            int monsterCount,
            int initHp,
            int finalHp,
            List<Reward> rewards)
        {
            Console.Clear();
            Console.WriteLine("Battle!! - Result\n");
            Console.WriteLine(resultText + "\n");
            if (resultText == "Victory")
             {
                 Console.WriteLine($"던전에서 몬스터 {monsterCount}마리를 잡았습니다.\n");
             }

            Console.WriteLine("[캐릭터 정보]");
            Console.WriteLine($"HP {initHp} -> {finalHp}\n");

            Console.WriteLine("[획득 보상]");
            if (rewards.Count == 0)
            {
                Console.WriteLine("없음\n");
            }
            else
            {
                foreach (var r in rewards)
                    Console.WriteLine($"{r.Name} +{r.Amount}");
                Console.WriteLine();
            }

            Console.WriteLine("0. 다음");
            CheckInput(0, 0);
            // → 이후 메인UI로 복귀하거나 게임종료 처리
        }


        static int CheckInput(int min, int max)
        {
            int result;
            while (true)
            {
                string input = Console.ReadLine().Trim();
                bool isNumber = int.TryParse(input, out result);
                if (isNumber)
                {
                    if (result >= min && result <= max)
                        return result;
                }
                Console.WriteLine("잘못된 입력입니다.");
            }


        }


    }
}

