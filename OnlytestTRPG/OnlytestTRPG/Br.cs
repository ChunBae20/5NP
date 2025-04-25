using OnlytestTRPG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            public Dictionary<string, int> Inventory { get; } = new(); //인벤토리를 따온다고? 애드는 어딧지

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
            {"대포미니언", new(){ new("Gold",300), new("포션",1), new("낡은 검",1)} }
        };

        
          static List<Item> itemList = new List<Item>()
        {
            new("낡은 검", "공격력", 2, 500),
            new("낡은 옷", "방어력", 2, 500)
        };

        

        // 배틀 종료
        // --------------------------------------------------------------
        // Battle – 데모용 전투 로직                                     
        // --------------------------------------------------------------
        public BattleResult Battle(Reward reward, List<string> defeatedTypes, out ResultChoice postChoice)
        {
            int hpBeforeFight = Character.player.CurrentHP;

            var rewardList = CollectAllRewards(defeatedTypes);

            bool victory = Character.player.CurrentHP > 0;
            if (victory)
                reward.AddRewards(rewardList);

            int damageTaken = Character.player.maxmaxHP - Character.player.CurrentHP;

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
        // public static int hpBeforeBattle = Character.player.CurrentHP;
        // public static int hpAfterBattle = Character.player.maxmaxHP;
        public static int hpBeforeBattledamageTaken = Character.player.maxmaxHP - Character.player.CurrentHP;

                public static ResultChoice ShowResult(int killCount, int damageTaken, 
                                   List<Reward> rewardList, BattleResult result)  //민종곤 damageTaken 삭제 static 추가
            {

                // int hpBeforeBattledamageTaken = hpBeforeBattle - Character.player.CurrentHP;

                Console.Clear();
                Console.WriteLine("Battle - Result\n");
                Console.WriteLine((result == BattleResult.Victory ? "Victory" : "You Lose") + "\n");

                if (result == BattleResult.Victory)
                    Console.WriteLine($"몬스터 {killCount}마리를 처치했습니다.\n");

                Console.WriteLine("[캐릭터]");
                Console.WriteLine($"HP {Character.player.maxmaxHP} -> {Character.player.CurrentHP} (-{hpBeforeBattledamageTaken})\n"); //민종곤 damageTaken 삭제 

                Console.WriteLine("[획득 보상]");   //여기 이프문추가함
                if (result == BattleResult.Victory)
                {
                    // foreach (var reward in rewardList)
                    //     Console.WriteLine($"{reward.EquipmentName} +{reward.Amount}");

                foreach (var reward in rewardList)
                {

                    if (reward.EquipmentName == "Gold")                                                                 //골드추가 시작
                    {
                        MainSpace.status.basicGold += reward.Amount;
                        Console.WriteLine($"Gold +{reward.Amount} (보유 골드: {MainSpace.status.basicGold})");          //골드추가 종료
                    }

                    else if (reward.EquipmentName == "포션")
                    {
                        MainSpace.healItem.AddPotion(reward.Amount);
                        Console.WriteLine($"포션 +{reward.Amount} (총 {HealItem.potion}개 보유)");
                    }

                    var itemData = itemList.FirstOrDefault(item => item.ItemName == reward.EquipmentName);

                    if (itemData != null)
                    {
                       
                        for (int i = 0; i < reward.Amount; i++)
                        {
                            var newEquip = new Equipment(itemData.ItemName, itemData.ItemType, itemData.ItemStat, itemData.Price);
                            Inventory.equipment.Add(newEquip);
                        }

                        Console.WriteLine($"{reward.EquipmentName} x{reward.Amount} (인벤토리에 추가됨)");//x는 그냥 관상용임 문법아니니까 안심하셈
                    }

                    // else
                    // {
                    //     // 골드나 포션 같은 일반 보상 출력
                    //     Console.WriteLine($"{reward.EquipmentName} +{reward.Amount}");
                    // }
                }


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

    






