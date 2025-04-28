using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OnlytestTRPG
{
   public  enum JobType                           
    {
        Warrior = 1,
        Mage = 2,
        Archer = 3
    }
    public class Character : MainSpace
    {
        public void Intro() // 이름 입력
        {
            Console.Clear();
            Console.WriteLine("당신은 위대한 영웅들의 고향 스파르타 마을에 도착했습니다...");
            Console.WriteLine("위대한 영웅들 처럼 모험을 시작하려면 캐릭터를 생성하세요!");
            Console.Write("캐릭터 이름을 입력하세요: ");
            status.Name = Console.ReadLine() ?? "";
            SelectJob();
        }

        public static void SelectJob() // 직업 선택
        {
            Console.Clear();
            Console.WriteLine("\n직업을 선택하세요:");
            Console.WriteLine("1. 전사 (공격력 15 / 방어력 10 / 체력 120)");
            Console.WriteLine("2. 마법사 (공격력 25 / 방어력 5 / 체력 80)");
            Console.WriteLine("3. 궁수 (공격력 18 / 방어력 7 / 체력 100)");
            Console.Write("선택 (1~3): ");

            int num = Input(1, 3);

            switch (num)
            {
                case 1:
                    status.Job = JobType.Warrior;
                    status.BasicSTR = 15;
                    status.BasicDEF = 10;
                    status.BasicHP = 120;
                    status.CurrentHP = status.BasicHP + status.NowEquipHP;
                    break;
                case 2:
                    status.Job = JobType.Mage;
                    status.BasicSTR = 25;
                    status.BasicDEF = 5;
                    status.BasicHP = 80;
                    status.CurrentHP = status.BasicHP + status.NowEquipHP;
                    break;
                case 3:
                    status.Job = JobType.Archer;
                    status.BasicSTR = 18;
                    status.BasicDEF = 7;
                    status.BasicHP = 100;
                    status.CurrentHP = status.BasicHP + status.NowEquipHP;
                    break;
            }
            Console.WriteLine($"\n{status.Job} 직업의 {status.Name}님으로 게임을 시작합니다!");
        }
    }
}