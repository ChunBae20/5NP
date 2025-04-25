using System.Collections.Generic;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Threading.Tasks.Dataflow;

namespace OnlytestTRPG
{
    
    
    public class BattleScene : MainSpace
    {
        
        private static Character player;
        private static Item[] itemDb;
        private static Enemy[] enemyDb;
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
                        Console.WriteLine($"{player}의 공격!");
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
                    
                    

                    if (status.CurrentHP > 0)
                    {
                        Console.WriteLine();
                        JoinBattleScene();
                    }
                    break;
            }

        }

        public void SetData()
        {
     
            enemyDb = new Enemy[]
            {
                new Enemy(name:"미니언", level:2, atk: 5, maxHp: 15),
                new Enemy(name:"공허충", level:3, atk: 9, maxHp: 10),
                new Enemy(name:"대포미니언", level:5, atk: 8, maxHp:25)
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
        static void ShowEnemy(List<Enemy> enemies,bool showIdx)
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
            bool isCrit = false;
            int avoidPercent = random.Next(0, 100);
            if (avoidPercent < baseAvd)
            {
                
                
                return 0;
                

            }
            
            int criticalPercent = random.Next(0, 100);
            if (criticalPercent < baseCrt)
            {
                rawDamage = rawDamage * 2; //critcalValue; //크리티컬 데미지 계수 추가?(직업별, 도적이나 궁수는 크뎀높게+아이템에도 크리티컬 계수추가)
                isCrit = true;
                
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
                Console.WriteLine(); Console.WriteLine($"{player}을(를) 맞췄습니다. [데미지:{enemyDamage}]");
                Console.WriteLine($"Lv.{status.level} {player}");
                Console.WriteLine($"HP {status.CurrentHP} -> {status.CurrentHP - enemyDamage}");

                status.CurrentHP -= enemyDamage;
                
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

