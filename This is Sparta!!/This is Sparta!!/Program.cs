using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks.Dataflow;

namespace This_is_Sparta__
{
    
    
    internal class Program
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
        static void DisplayBattleScene()
        {

            EnemyGenerate();

            Console.WriteLine();
            ShowEnemy(currentEnemies, false);
            Console.WriteLine();
            BattleCharacterInfo();
            Console.WriteLine("1.공격");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요");
            int result = CheckInput(0,1);
            switch (result)
            {
                case 1:
                    JoinBattleScene();
                    break;
            }
        }

        static void JoinBattleScene()
        {
            Console.Clear();
            Console.WriteLine();
            ShowEnemy(currentEnemies, true);
            Console.WriteLine();
            BattleCharacterInfo();
            Console.WriteLine();
            Console.WriteLine("공격할 적을 입력해주세요");
            int result = CheckInput(1, currentEnemies.Count);
            switch (result)
            {
                default:
                    int EnemyIdx = result - 1;
                    Enemy targetEnemy = currentEnemies[EnemyIdx];

                    if (targetEnemy.IsDead)
                    {
                        Console.WriteLine("이미 죽어있는 적입니다.");
                        JoinBattleScene();
                    }
                    else
                    {
                        int damage = CalculateDamage(player.Atk);
                        Console.WriteLine($"{targetEnemy.Name}에게 {damage}의 피해를 입혔습니다.");

                        if (damage >= targetEnemy.CurrentHp)
                        {
                            targetEnemy.CurrentHp = 0;
                            targetEnemy.IsDead = true;
                            Console.WriteLine($"{targetEnemy.Name}이(가) 죽었습니다.");
                            
                        }
                        else
                        {
                            targetEnemy.CurrentHp -= damage;
                        }
                    }
                    EnemyAttackPhase();
                    if (player.CurrentHp > 0)
                    {
                        Console.WriteLine();
                        JoinBattleScene();
                        
                    }
                    break;
            }
        }

        static void SetData()
        {
            player = new Character(level: 1, name: "Chad", job: "전사", atk: 10, def: 5, maxHp: 100, gold: 10000);
            itemDb = new Item[]
            {
                new Item(names:"수련자의 갑옷",type:1,value:5, desc:"수련에 도움을 주는 갑옷입니다.", price:1000),
                new Item(names:"무쇠 갑옷",type:1,value:9, desc:"무쇠로 만들어져 튼튼한 갑옷입니다..", price:2000),
                new Item(names:"스파르타의 갑옷",type:1,value:15, desc:"수련에 도움을 주는 갑옷입니다.", price:3500),
                new Item(names:"낡은 검",type:0,value:2, desc:"쉽게 볼 수 있는 낡은 검입니다.", price:600),
                new Item(names:"청동 도끼",type:0,value:5, desc:"어디선가 사용됐던거 같은 도끼입니다.", price:1500),
                new Item(names:"스파르타의 창",type:0,value:7, desc:"스파르타의 전사들이 사용했다는 전설의 창입니다.", price:2500)

            };
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
                string displayIdx = showIdx ? $"{i + 1}. " : "";
                Console.WriteLine($"{displayIdx}{enemies[i].EnemyInfoText()}");
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


        static int CalculateDamage(int baseAtk)
        {
            int errorRange = (int)Math.Ceiling(baseAtk * 0.1);
            int min = baseAtk - errorRange;
            int max = baseAtk + errorRange;

            return random.Next(min, max+1);

        }



        static void EnemyAttackPhase()
        {
            foreach (var enemy in currentEnemies)
            {
                if(enemy.IsDead) continue;
                Console.WriteLine($"{enemy.Name}이 공격 대기중");
                Console.WriteLine("0.눌러 진행");
                int wait = CheckInput(0,0);
                int enemyDamage = CalculateDamage(enemy.Atk);
                Console.WriteLine($"{enemy.Name}이(가) {player.Name}에게 {enemyDamage}의 피해를 입힘");

                player.CurrentHp -= enemyDamage;

                if (player.CurrentHp <= 0)
                {
                    player.CurrentHp = 0;
                    Console.WriteLine($"{player.Name}이(가) 쓰러졌습니다.");
                    Console.WriteLine("GameOver");
                    return;

                }
            }

        }


        static void BattleCharacterInfo()
        {
            Console.WriteLine("[내정보]");
            Console.WriteLine($"Lv.{player.Level} {player.Name}({player.Job})");
            Console.WriteLine($"HP {player.CurrentHp}/{player.MaxHp}");
        }



        static void Main(string[] args)
            {
            SetData();    
            DisplayBattleScene();


            
            }
        }
    }

