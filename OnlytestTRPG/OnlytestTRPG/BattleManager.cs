using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks.Dataflow;

namespace OnlytestTRPG
{
    class Enemy
    {
        public int Level;
        public string Name;
        public int Atk;
        public int MaxHp;
        public int CurrentHP;
        public bool IsDead = false;

        public Enemy(string name, int level, int atk, int maxHp, int currentHP)
        {
            Name = name;
            Level = level;
            Atk = atk;
            MaxHp = maxHp;
            CurrentHP = currentHP;

        }
        public string EnemyInfoText()
        {

            string status = IsDead ? $"[Dead]" : $" HP {CurrentHP}/{MaxHp}";
            if (IsDead)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(($"{status} Lv. {Level} {Name} "));
                Console.ResetColor();
                return "";
            }
            else
            {
                return $"Lv.{Level} {Name} {status}";
            }

        }
    }

    public class BattleScene : MainSpace
    {
        //private static Character? player;
        private static Item[]? itemDb;
        static Random random = new Random();
        public static int BeforeHP = status.CurrentHP;

        internal static Enemy[] enemyDb = new Enemy[]
{
            new Enemy("독고벌레", 2, 5, 15, 15),
            new Enemy("바위게", 3, 9, 10, 10),
            new Enemy("늑대 고블린", 4, 7, 20, 20),
            new Enemy("칼날구울", 5, 8, 25, 25),
            new Enemy("핏빛 리자드맨", 8, 8, 30, 30),
            new Enemy("쌍둥이 골렘", 10, 10, 70, 70)
};

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
            int result = Input(0, 1);
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
                        int damage = CalculateDamage(status.BasicSTR + status.NowEquipSTR, 0, 0, status.BasicSTR + status.NowEquipSTR);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Battle!");
                        Console.ResetColor();
                        Console.WriteLine();
                        Console.WriteLine($"{status.Name}의 공격!");
                        Console.WriteLine($"Lv.{targetEnemy.Level} {targetEnemy.Name}을(를) 맞췄습니다. [데미지: {damage}]");
                        Console.WriteLine();
                        Console.WriteLine($"Lv.{targetEnemy.Level} {targetEnemy.Name}");

                        if (damage >= targetEnemy.CurrentHP)
                        {
                            Console.WriteLine($"HP {targetEnemy.CurrentHP} -> Dead");
                            targetEnemy.CurrentHP = 0;
                            targetEnemy.IsDead = true;
                        }
                        else
                        {
                            int beforeHP = targetEnemy.CurrentHP;
                            targetEnemy.CurrentHP -= damage;
                            Console.WriteLine($"HP {beforeHP} -> {targetEnemy.CurrentHP}");
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

        static List<Enemy> currentEnemies = new List<Enemy>();
        static void EnemyGenerate()
        {
            int enemyCount = random.Next(1, 5); //1~4마리
            currentEnemies.Clear();

            for (int i = 0; i < enemyCount; i++)
            {
                Enemy template = enemyDb[random.Next(enemyDb.Length)];
                Enemy enemyIstance = new Enemy(template.Name, template.Level, template.Atk, template.MaxHp, template.CurrentHP);
                currentEnemies.Add(enemyIstance);
            }
        }
        void ShowEnemy(List<Enemy> enemies, bool showIdx)
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
            int criticalPercent = random.Next(0, 100);
            if (criticalPercent < baseCrt)
            {
                rawDamage = rawDamage * 2; //critcalValue; //크리티컬 데미지 계수 추가?(직업별, 도적이나 궁수는 크뎀높게+아이템에도 크리티컬 계수추가)
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
    for (int i = 0; i < currentEnemies.Count; i++)//지민 수정 적의 마지막 공격 표시를 위해 foreach 에서 for문으로 수정
    {
        var enemy = currentEnemies[i];
        if (enemy.IsDead) continue;
        Console.WriteLine($"Lv.{enemy.Level} {enemy.Name}의 공격 대기중");
        Console.WriteLine("0.눌러 진행");
        int wait = Input(0, 0);
        Console.Clear();
        int enemyDamage = CalculateDamage(enemy.Atk, status.BasicSTR + status.NowEquipSTR, status.BasicSTR + status.NowEquipSTR, 0);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Battle!");
        Console.ResetColor();
        Console.WriteLine($"Lv.{enemy.Level} {enemy.Name}의 공격!");

        Console.WriteLine();
        Console.WriteLine($"{status.Name}를 맞췄습니다. [데미지: {enemyDamage}]");
        Console.WriteLine($"Lv.{status.Level} {status.Name}");
        Console.WriteLine($"HP {status.CurrentHP} -> {status.CurrentHP - enemyDamage}");
        status.CurrentHP -= enemyDamage;

        //♥
        if (status.CurrentHP <= 0) ////민종곤 damageTaken 삭제
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
        if (i == currentEnemies.Count - 1)
        {
            Console.WriteLine();
            Console.WriteLine("0. 눌러 내 턴으로");
            Input(0, 0);
            Console.Clear();    //지민 수정 (적의 마지막 공격이 표시되지않음)
        }

    }

}

        public void BattleCharacterInfo()
        {
            Console.WriteLine("[내정보]");
            Console.WriteLine($"Lv.{status.Level} {status.Name}({status.Job})");
            Console.Write($"HP {status.CurrentHP}/");
            Console.WriteLine(BeforeHP);
        }
    }
}

