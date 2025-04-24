using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks.Dataflow;

namespace OnlytestTRPG
{
    
    
    public class BattleScene : MainSpace
    {
        private static Character player;
        private static Item[] itemDb;
        private static Monster[] monsterDb;
        static Random random = new Random();
        //battle!
        //적 몬스터 출현 1~4마리 출현
        //[내정보]
        //1. 공격
        //외에는 잘못된 입력
        public void DisplayBattleScene()
        {

            MonsterGenerate(GetMonsterstance());

            Console.WriteLine();
            ShowEnemy(currentMonster, false);
            Console.WriteLine();
            BattleCharacterInfo();
            Console.WriteLine("1.공격");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요");
            int result = CheckInput(0,1);
            switch (result)
            {
                case 1:
                    JoinBattleScene(currentMonster);
                    break;
            }
        }

        static void JoinBattleScene(List<Monster> currentMonster)
        {
            Console.Clear();
            Console.WriteLine();
            ShowEnemy(currentMonster, true);
            Console.WriteLine();
            BattleCharacterInfo();
            Console.WriteLine();
            Console.WriteLine("공격할 적을 입력해주세요");
            int result = CheckInput(1, currentMonster.Count);
            switch (result)
            {
                default:
                    int EnemyIdx = result - 1;
                    Monster targetMonster = currentMonster[EnemyIdx];

                    if (targetMonster.IsDead)
                    {
                        Console.WriteLine("이미 죽어있는 적입니다.");
                        JoinBattleScene(currentMonster);
                    }
                    else
                    {
                        int damage = CalculateDamage(status.basicSTR + status.nowEquipSTR, 0, 0, status.basicCRT + status.nowEquipCRT);
                        Console.WriteLine($"{targetMonster.Name}에게 {damage}의 피해를 입혔습니다.");

                        if (damage >= targetMonster.CurrentHp)
                        {
                            targetMonster.CurrentHp = 0;
                            targetMonster.IsDead = true;
                            Console.WriteLine(value: $"{targetMonster.Name}이(가) 죽었습니다.");

                        }
                        else
                        {
                            targetMonster.CurrentHp -= damage;
                        }
                    }
                    EnemyAttackPhase();
                    if (status.CurrentHP > 0)
                    {
                        Console.WriteLine();
                        JoinBattleScene(currentMonster);

                    }
                    break;
            }
        }

        private Monster GetMonsterstance()
        {
            throw new NotImplementedException();
        }

        private static void ShowEnemy(List<Monster> currentMonster, bool v)
        {
            throw new NotImplementedException();
        }

        public void SetData(int stage)
        {
            //    player = new Character(level: 1, name: "Chad", job: "전사", atk: 10, def: 5, maxHp: 100, gold: 10000);
            //    itemDb = new Item[]
            //    {
            //        new Item(name:"수련자의 갑옷",type:1,value:5, desc:"수련에 도움을 주는 갑옷입니다.", price:1000),
            //        new Item(name:"무쇠 갑옷",type:1,value:9, desc:"무쇠로 만들어져 튼튼한 갑옷입니다..", price:2000),
            //        new Item(name:"스파르타의 갑옷",type:1,value:15, desc:"수련에 도움을 주는 갑옷입니다.", price:3500),
            //        new Item(name:"낡은 검",type:0,value:2, desc:"쉽게 볼 수 있는 낡은 검입니다.", price:600),
            //        new Item(name:"청동 도끼",type:0,value:5, desc:"어디선가 사용됐던거 같은 도끼입니다.", price:1500),
            //        new Item(name:"스파르타의 창",type:0,value:7, desc:"스파르타의 전사들이 사용했다는 전설의 창입니다.", price:2500)

            //    };
            List<Monster> monsters = MonsterManager.GetMonstersByStage(stage);

            Console.WriteLine($"스테이지 {stage} 전투를 준비합니다...");
            Console.WriteLine($"등장 가능한 몬스터 수: {monsters.Count}");

            // 예시: 랜덤 몬스터 선택
            Random rand = new Random();
            Monster selectedMonster = monsters[rand.Next(monsters.Count)];

            Console.WriteLine($"몬스터 등장: {selectedMonster.Name}!");
            selectedMonster.ShowInfo();
        }

    static List<Monster> currentMonster = new List<Monster>();

       /* private static GetMonsterstance()
        {
            GetMonsterstance;
        }*/

        static void MonsterGenerate(Monster monsterstance)
        {
            int enemyCount = random.Next(1, 5); //1~4마리
            currentMonster.Clear();

            for (int i = 0; i < enemyCount; i++)
            {
                Monster template = monsterDb[random.Next(monsterDb.Length)];
                Monster Monsterstance = new Monster(template.Name, template.Level, template.Atk, template.MaxHp, template.GoldReward);
                currentMonster.Add(monsterstance);
            }
        }
        static void ShowMonster(List<Monster> enemies,bool showIdx)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Battle!");
            Console.ResetColor();
            Console.WriteLine();
            for (int i = 0; i < enemies.Count; i++)
            {
                string displayIdx = showIdx ? $"{i + 1}. " : "";
                Console.WriteLine($"{displayIdx}{enemies[i].MonsterInfoText()}");
            }
            Console.WriteLine();
        }

        static int CheckInput(int min, int max)
        {
            int result;
            while (true)
            {
                string input = Console.ReadLine();
                bool isNumber = int.TryParse(input, out result);
                if (isNumber)
                {
                    if (result >= min && result <= max)
                        return result;
                }
                Console.WriteLine("잘못된 입력입니다!!!!");
            }

        }


        static int CalculateDamage(int baseAtk, int baseDef, int baseAvd, int baseCrt)
        {
            int errorRange = (int)Math.Ceiling(baseAtk * 0.1);
            int min = baseAtk - errorRange;
            int max = baseAtk + errorRange;
            int rawDamage = random.Next(min, max + 1) - baseDef;

            int avoidPercent = random.Next(0, 100);
            if (avoidPercent < baseAvd)
            {
                Console.WriteLine("공격을 회피하였습니다.");
                return 0;

            }
            bool isCritical = false;
            int criticalPercent = random.Next(0, 100);
            if (criticalPercent < baseCrt)
            {
                rawDamage = rawDamage * 2; //critcalValue; //크리티컬 데미지 계수 추가?(직업별, 도적이나 궁수는 크뎀높게+아이템에도 크리티컬 계수추가)
                isCritical = true;
                Console.WriteLine("크리티컬!");
            }
            int finalDamage = rawDamage - baseDef;
            if (finalDamage < 1)
            {
                finalDamage = 1;
            }
            return finalDamage;
        }



        static void EnemyAttackPhase()
        {
            foreach (var Monster in currentMonster)
            {
                if(Monster.IsDead) continue;
                Console.WriteLine($"{Monster.Name}이 공격 대기중");
                Console.WriteLine("0.눌러 진행");
                int wait = CheckInput(0,0);
                int MonsterDamage = CalculateDamage(Monster.Atk, status.basicDEF + status.nowEquipDEF, status.basicAVD + status.nowEquipAVD, 0);
                Console.WriteLine($"{Monster.Name}이(가) {player}에게 {MonsterDamage}의 피해를 입힘");

                status.CurrentHP -= MonsterDamage;

                if (status.CurrentHP <= 0)
                {
                    status.CurrentHP = 0;
                    Console.WriteLine($"{player}이(가) 쓰러졌습니다.");
                    Console.WriteLine("GameOver");
                    return;

                }
            }

        }


        static void BattleCharacterInfo()
        {
            Console.WriteLine("[내정보]");
            Console.WriteLine($"Lv.{status.level} {player}({status.job})");
            Console.WriteLine($"HP {status.CurrentHP}/100");
        }



        
        }
    }

