using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlytestTRPG;



namespace OnlytestTRPG
{
    public class Status : MainSpace
    {
        public int Level = 1;
        public string? Name;
        public JobType Job;
        public int BasicSTR;
        public int BasicDEF;
        public int BasicHP;
        public int BasicCRT = 0; //치명타
        public int BasicAVD = 10; // 회피율
        public int BasicGold = 1500;
        public int TotalHP => BasicHP + NowEquipHP;
        public int CurrentHP;

        //장비 추가
        public int NowEquipSTR;
        public int NowEquipDEF;
        public int NowEquipCRT;
        public int NowEquipAVD;
        public int NowEquipHP;

        public void StatusScene() // 상태창
        {
            Console.Clear();
            Console.WriteLine("캐릭터의 정보가 표시됩니다.\n\n\n");
            Console.WriteLine("이름 : " + Name);
            Console.WriteLine("직업 : " + Job);
            Console.WriteLine("공격력 : " + BasicSTR + " ( + " + NowEquipSTR + " )");
            Console.WriteLine("방어력 : " + BasicDEF + " ( + " + NowEquipDEF + " )");
            Console.WriteLine("체력: " + CurrentHP + " / " + BasicHP + "( " + NowEquipHP + " )");
            Console.WriteLine("Gold : " + BasicGold);
            Console.WriteLine("\n\n원하는 행동을 입력하세요");
            Console.WriteLine("0. 나가기\n");

            int num = Input(0, 0);

            switch (num)
            {
                case 0: MainMenu(); break;
            }
        }
    }
}
