using OnlytestTRPG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlytestTRPG
{
    enum BattleResult { Victory, Defeat } // 열거형, 가독성, 실수 방지, 관리 편함, 디버깅 개꿀
    enum ResultChoice { GoStart }

    public class Reward
    {
        public string RewardName { get; }
        public int RewardAmount { get; }
        public Dictionary<string, int> RewardInventory { get; } = new();

        public Reward(string rewardname, int rewardAmount)
        {
            RewardName = rewardname;
            RewardAmount = rewardAmount;
        }

        public void AddRewards(IEnumerable<Reward> rewards) // 보상 추가 및 중첩(처음부터 끝까지 하나씩 꺼낼 수 있음을 보장, 순회)
        {
            foreach (var reward in rewards)
            {
                if (!RewardInventory.ContainsKey(reward.RewardName)) RewardInventory[reward.RewardName] = 0; // 초기화 코드
                RewardInventory[reward.RewardName] += reward.RewardAmount;
            }
        }
    }

    internal class Program : MainSpace
    {

        // --------------------------------------------------------------
        // 몬스터별 드랍 데이터
        // --------------------------------------------------------------
        static readonly Dictionary<string, List<Reward>> dropTable = new() // 키 -값 쌍으로 저장, 찾는 속도 빠름, 도서관?
        {
            {"독고벌레", new(){ new("Gold",50),} },
            {"바위게", new(){ new("Gold",100)} },
            {"늑대 고블린", new(){ new("Gold",150)} },
            {"칼날구울", new(){ new("Gold",200)} },
            {"핏빛 리자드맨", new(){ new("Gold",250)} },
            {"쌍둥이 골렘", new(){ new("Gold",300)} },
        };

        // --------------------------------------------------------------
        // Battle – 데모용 전투 로직
        // --------------------------------------------------------------
        public BattleResult Battle(Reward reward, List<string> defeatedTypes, out ResultChoice goStart)
        {

            var rewardList = CollectAllRewards(defeatedTypes);

            bool victory = status.CurrentHP > 0;
            if (victory)
                reward.AddRewards(rewardList);

            int damageTaken = status.TotalHP - status.CurrentHP;

            goStart = ShowResult(
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
        public int damageTaken = BattleScene.BeforeHP - status.CurrentHP;
        public static ResultChoice ShowResult(int killCount, int damageTaken,
                                   List<Reward> rewardList, BattleResult result)
        {
            Console.Clear();
            Console.WriteLine("Battle - Result\n");
            Console.WriteLine((result == BattleResult.Victory ? "Victory" : "You Lose") + "\n");

            if (result == BattleResult.Victory)
                Console.WriteLine($"몬스터 {killCount}마리를 처치했습니다.\n");

            Console.WriteLine("[캐릭터]");
            Console.WriteLine($"HP {BattleScene.BeforeHP} -> {status.CurrentHP} (-{damageTaken})\n");

            Console.WriteLine("[획득 보상]");
            if (result == BattleResult.Victory)
            {
                foreach (var reward in rewardList)
                {

                    if (reward.RewardName == "Gold")                                                                 //골드추가 시작
                    {
                        status.BasicGold += reward.RewardAmount;
                        Console.WriteLine($"Gold +{reward.RewardAmount} (보유 골드: {MainSpace.status.BasicGold})");          //골드추가 종료
                    }

                    else if (reward.RewardName == "포션")
                    {
                        HealItem.Potion += reward.RewardAmount;
                        Console.WriteLine($"포션 +{reward.RewardAmount} (총 {HealItem.Potion}개 보유)");
                    }

                    var itemData = Store.itemList.FirstOrDefault(item => item.ItemName == reward.RewardName);

                    if (itemData != null)
                    {

                        for (int i = 0; i < reward.RewardAmount; i++)
                        {
                            var newEquip = new Equipment(itemData.ItemName, itemData.ItemType, itemData.ItemStat, itemData.Price);
                            Inventory.equipment.Add(newEquip);
                        }

                        Console.WriteLine($"{reward.RewardName} x{reward.RewardAmount} (인벤토리에 추가됨)");
                    }
                }
            }
            else Console.WriteLine("없음");
 

            Console.WriteLine("\n0. 시작 화면으로");
            int num = Input(0, 0);
            if (num == 0) MainMenu();
            return ResultChoice.GoStart;
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
                    if (!totals.ContainsKey(reward.RewardName))
                        totals[reward.RewardName] = 0;
                    totals[reward.RewardName] += reward.RewardAmount;
                }
            }
            var mergedRewards = new List<Reward>();
            foreach (var itemEntry in totals)
                mergedRewards.Add(new Reward(itemEntry.Key, itemEntry.Value));

            return mergedRewards;
        }
    }
}