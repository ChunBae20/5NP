using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks.Dataflow;

namespace OnlytestTRPG
{
    
    
    public class BattleScene : MainSpace
    {
        
        private static Character? player;
        private static Item[]? itemDb;
        private static Enemy[]? enemyDb;
        static Random random = new Random();
        //battle!
        //적 몬스터 출현 1~4마리 출현
        //[내정보]
        //1. 공격
        //외에는 잘못된 입력
        public void DisplayBattleScene()
        {
            Console.Clear();
            EnemyGenerate();

            Console.WriteLine();
            ShowEnemy(currentEnemies, false);
            Console.WriteLine();
            BattleCharacterInfo();
            Console.WriteLine("1.공격");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요");
            int result = Input(0,1);
            switch (result)
            {
                case 1:
                    JoinBattleScene();
                    break;
            }
        }

        public void JoinBattleScene()
        {
            Console.Clear();
            bool allDead = true;
            foreach (Enemy enemy in currentEnemies)
            {
                if (!enemy.IsDead)
                {
                    allDead = false;
                    break;
                }
            }

            if (allDead)
            {
                Console.WriteLine("모든 적을 처치했습니다!");

                // 실제 죽은 몬스터 이름만 추출
                List<string> defeated = new List<string>();
                foreach (var enemy in currentEnemies)
                {
                    if (enemy.IsDead)
                        defeated.Add(enemy.Name);
                }

                // 보상용 리워드 객체 생성
                Reward reward = new Reward("Gold", 0);
                OnlytestTRPG.Program program = new OnlytestTRPG.Program();
                program.Battle(reward, defeated, out ResultChoice choice);

                return;
            }

            Console.Clear();
            Console.WriteLine();
            ShowEnemy(currentEnemies, true);
            Console.WriteLine();
            BattleCharacterInfo();
            Console.WriteLine();
            Console.WriteLine("공격할 적을 입력해주세요");
            int result = Input(1, currentEnemies.Count);

            switch (result)
            {
                default:
                    int EnemyIdx = result - 1;
                    Enemy targetEnemy = currentEnemies[EnemyIdx];

                    if (targetEnemy.IsDead)
                    {
                        Console.WriteLine("이미 죽어있는 적입니다. 살아있는 적을 공격해주세요.");
                        Thread.Sleep(1000);
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, Console.CursorTop);
                    }
                    else
                    {
                        Console.Clear();
                        int damage = CalculateDamage(status.basicSTR + status.nowEquipSTR, 0, 0, status.basicCRT + status.nowEquipCRT);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Battle!");
                        Console.ResetColor();
                        Console.WriteLine();
                        Console.WriteLine($"{status.name}의 공격!");
                        Console.WriteLine($"Lv.{targetEnemy.Level} {targetEnemy.Name}을(를) 맞췄습니다. [데미지: {damage}]");
                        Console.WriteLine();
                        Console.WriteLine($"Lv.{targetEnemy.Level} {targetEnemy.Name}");

                        if (damage >= targetEnemy.CurrentHp)
                        {
                            Console.WriteLine($"HP {targetEnemy.CurrentHp} -> Dead");
                            targetEnemy.CurrentHp = 0;
                            targetEnemy.IsDead = true;
                        }
                        else
                        {
                            int beforeHP = targetEnemy.CurrentHp;
                            targetEnemy.CurrentHp -= damage;
                            Console.WriteLine($"HP {beforeHP} -> {targetEnemy.CurrentHp}");
                        }
                        EnemyAttackPhase();
                    }

                    

                    if (Character.player.CurrentHP > 0)
                    {
                        Console.WriteLine();
                        JoinBattleScene();
                    }
                    break;
            }

        }

        public void SetData()
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
             enemyDb= new Enemy[]
            {
                 new Enemy(name:"독고벌레", level:2, atk: 5, maxHp: 15),
                 new Enemy(name:"바위게", level:3, atk: 9, maxHp: 10),
                 new Enemy(name:"늑대 고블린", level:4, atk: 7, maxHp:20),
                 new Enemy(name:"칼날구울", level:5, atk: 8, maxHp:25),
                 new Enemy(name:"핏빛 리자드맨", level:8, atk: 8, maxHp:30),
                 new Enemy(name:"쌍둥이 골렘", level:10, atk: 10, maxHp:70)
            };
    }
    static List<Enemy> currentEnemies = new List<Enemy>(); 
        static void EnemyGenerate()
        {
            int enemyCount = random.Next(1, 5); //1~4마리
            currentEnemies.Clear();

            for (int i = 0; i < enemyCount; i++)
            {
                Enemy template = enemyDb[random.Next(enemyDb.Length)];
                Enemy enemyIstance = new Enemy(template.Name, template.Level, template.Atk, template.MaxHp);
                currentEnemies.Add(enemyIstance);
            }
        }
        void ShowEnemy(List<Enemy> enemies,bool showIdx)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Battle!");
            Console.ResetColor();
            Console.WriteLine();
            for (int i = 0; i < enemies.Count; i++)
            {
                showIdx = !enemies[i].IsDead;
                string displayIdx = showIdx ? $"{i + 1}." : "";
                Console.WriteLine($"{displayIdx} {enemies[i].EnemyInfoText()}");
            }
            Console.WriteLine();
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
                //Console.WriteLine("공격을 회피하였습니다."); -> 여유 되면 회피 출력(잠수함으로 데미지 0들어옵니다) 지우지말아주세요 지민
                return 0;

            }
            bool isCritical = false;
            int criticalPercent = random.Next(0, 100);
            if (criticalPercent < baseCrt)
            {
                rawDamage = rawDamage * 2; //critcalValue; //크리티컬 데미지 계수 추가?(직업별, 도적이나 궁수는 크뎀높게+아이템에도 크리티컬 계수추가)
                isCritical = true;
                //Console.WriteLine("크리티컬!");-> 여유 되면 크리티컬 출력(잠수함으로 데미지 2배 들어옵니다) 지우지말아주세요 지민
            }
            int finalDamage = rawDamage - baseDef;
            if (finalDamage < 1)
            {
                finalDamage = 1;
            }
            return finalDamage;
        }



        void EnemyAttackPhase()
        {
            foreach (var enemy in currentEnemies)
            {
                if(enemy.IsDead) continue;
                Console.WriteLine($"Lv.{enemy.Level} {enemy.Name}이 공격 대기중");
                Console.WriteLine("0.눌러 진행");
                int wait = Input(0,0);
                Console.Clear();
                int enemyDamage = CalculateDamage(enemy.Atk, status.basicDEF + status.nowEquipDEF, status.basicAVD + status.nowEquipAVD, 0);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Battle!");
                Console.ResetColor();
                Console.WriteLine($"Lv.{enemy.Level} {enemy.Name}의 공격!");
                
                Console.WriteLine();
                Console.WriteLine($"{status.name}를 맞췄습니다. [데미지: {enemyDamage}]");
                Console.WriteLine($"Lv.{status.level} {status.name}");
                Console.WriteLine($"HP {Character.player.CurrentHP} -> {Character.player.CurrentHP - enemyDamage}");
                Character.player.CurrentHP -= enemyDamage;

                //♥
                if (Character.player.CurrentHP <= 0) ////민종곤 damageTaken 삭제  
                {
                    var emptyRewards = new List<Reward>();   // 패배는 보상 없음
                    Program.ShowResult(     // ← 민종곤 방금 만든 static 메서드 showResult 부분이랑 연결
                        damageTaken: 0,
                        killCount: 0,
                        rewardList: emptyRewards,
                        result: BattleResult.Defeat);

                    /* ShowResult() 안에서 Environment.Exit(0) 이 호출되므로
                       아래 return 은 형식상 남겨 둡니다. */
                    return; // 민종곤 전투 종료 로직 연결

                }

                //♥
            }

        }


        static void BattleCharacterInfo()
        {
            Console.WriteLine("[내정보]");
            Console.WriteLine($"Lv.{status.level} {status.name}({status.job})");
            Console.Write($"HP {Character.player.CurrentHP}/" );//아 이거 이거였네 100고정이아니라 걍 
            Console.WriteLine(Character.player.maxmaxHP);
        }



        
        }
    }

