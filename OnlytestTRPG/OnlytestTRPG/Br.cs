using OnlytestTRPG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace OnlytestTRPG
{


    
        // public static void BattleUI() { 

        enum GameScene { Start, Battle }
        enum BattleResult { Victory, Defeat }
        enum ResultChoice { GoStart, GoNextStage }

        public class Reward
        {
            public string EquipmentName { get; }
            public int Amount { get; }
            public Dictionary<string, int> Inventory { get; } = new();

            public Reward(string name, int amount)
            {
                EquipmentName = name;
                Amount = amount;
            }

            public void AddRewards(IEnumerable<Reward> rewards)
            {
                foreach (var reward in rewards)
                {
                    if (!Inventory.ContainsKey(reward.EquipmentName))
                        Inventory[reward.EquipmentName] = 0;
                    Inventory[reward.EquipmentName] += reward.Amount;
                }
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




        // ==================================================================
        //  Program – 메인 루프, 전투, 결과 UI                                
        // ==================================================================




        internal class Program : MainSpace
        {

            BattleScene battleScene = new BattleScene();
            // --------------------------------------------------------------
            // 몬스터 드롭 테이블                                            
            // --------------------------------------------------------------
            static readonly Dictionary<string, List<Reward>> dropTable = new()
        {
            {"미니언"    , new(){ new("Gold",50) } },
            {"공허충"    , new(){ new("Gold",150)} },
            {"대포미니언", new(){ new("Gold",300), new("포션",1), new("낡은검",1)} }
        };

        // 배틀 종료
        // --------------------------------------------------------------
        // Battle – 데모용 전투 로직                                     
        // --------------------------------------------------------------
        public BattleResult Battle(Reward reward, List<string> defeatedTypes, out ResultChoice postChoice)
        {
            int hpBeforeFight = status.CurrentHP;

            var rewardList = CollectAllRewards(defeatedTypes);

            bool victory = status.CurrentHP > 0;
            if (victory)
                reward.AddRewards(rewardList);

            int damageTaken = hpBeforeFight - status.CurrentHP;

            postChoice = ShowResult(
                defeatedTypes.Count,
                damageTaken,
                rewardList,
                victory ? BattleResult.Victory : BattleResult.Defeat
            );

            return victory ? BattleResult.Victory : BattleResult.Defeat;
        }


        // --------------------------------------------------------------
        // 결과 화면                                                     
        // --------------------------------------------------------------

        public ResultChoice ShowResult(int killCount, int damageTaken,
                                   List<Reward> rewardList, BattleResult result)
            {
                Console.Clear();
                Console.WriteLine("Battle - Result\n");
                Console.WriteLine((result == BattleResult.Victory ? "Victory" : "You Lose") + "\n");

                if (result == BattleResult.Victory)
                    Console.WriteLine($"몬스터 {killCount}마리를 처치했습니다.\n");

                Console.WriteLine("[캐릭터]");
                Console.WriteLine($"HP {status.TotalHP} -> {status.CurrentHP}  (-{damageTaken})\n");

                Console.WriteLine("[획득 보상]");
                if (result == BattleResult.Victory)
                {
                    foreach (var reward in rewardList)
                        Console.WriteLine($"{reward.EquipmentName} +{reward.Amount}");
                }
                else
                    Console.WriteLine("없음");
                Console.WriteLine();
                // ─────────────────────────────────────────────
                // 선택지 출력 (승리/패배에 따라 다르게)
                // ─────────────────────────────────────────────
                if (result == BattleResult.Victory)
                {
                    Console.WriteLine("0. 시작 화면으로");
                    int num = Input(0, 1);
                    if (num == 0) MainMenu();
                    return ResultChoice.GoStart;
                }

                else
                {
                    Console.WriteLine("0. 시작 화면으로");
                    int num = Input(0, 0);           // 아무 키 대기
                    if (num == 0) MainMenu();
                    return ResultChoice.GoStart;
                }
            }


            // --------------------------------------------------------------
            // 드롭 리스트 합산                                               
            // --------------------------------------------------------------
            static List<Reward> CollectAllRewards(IEnumerable<string> defeatedTypes)
            {
                var totals = new Dictionary<string, int>();

                foreach (var monsterType in defeatedTypes)
                {
                    if (!dropTable.TryGetValue(monsterType, out var dropList))
                        continue;

                    foreach (var reward in dropList)
                    {
                        if (!totals.ContainsKey(reward.EquipmentName))
                            totals[reward.EquipmentName] = 0;
                        totals[reward.EquipmentName] += reward.Amount;
                    }
                }

                var mergedRewards = new List<Reward>();
                foreach (var itemEntry in totals)
                    mergedRewards.Add(new Reward(itemEntry.Key, itemEntry.Value));

                return mergedRewards;
            }


        }

    }

    






