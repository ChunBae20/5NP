using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace This_is_Sparta__
{
    enum JobType                                                //jobtype을 열거할게요 , 열거해서 1,2,3에 각각 저장할게요
    {
        Warrior = 1,
        Mage = 2,
        Archer = 3
    }
    public class Charter //이거도 그냥 클래스 를 호출하는식으로: MainSpace   // internal 클래스는 이거 상속 또 스택오버플로되는거아님?이게 전부 캐릭터클래스니까.
    {
        static Player player = new Player();     //여기서 플레이어 라는걸 새로 생성한다.
        /*static void PlayerMain(string[] args)
        {
            MainMenu();                 //여기에 분기점을 넣을려고 하신건가?메인메뉴로 바꿔야 겠는데? 위에 playermain을 일단 퍼블릭으로 바꾸자.
        }                                //public으로 바꾸고 메인 Main에서 실행되게그리고 반드시 PlayerMain은 MainMenu();보다 위에
        */       //♥♥♥♥♥♥이거는 삭제를 좀해야할거같네용                           //그냥 mainmenu에서 먼저 불러오고 메인창에서 기존대로 실행한다.
        /* private static void MainMenu()  //그리고 
         {
             throw new NotImplementedException();//이거도 그냥 필요없는것 같은데 무슨 씬을 여기서 불러와야하지?
         }

         */  //♥♥♥♥♥♥이거도 삭제를 좀해야할거같네용 

        public static void ShowIntro()
        {
            Console.Clear();
            Console.WriteLine("당신은 위대한 영웅들의 고향 스파르타 마을에 도착했습니다...");
            Console.WriteLine("위대한 영웅들 처럼 모험을 시작하려면 캐릭터를 생성하세요!");
            CreateCharacter();
        }

        static void CreateCharacter()
        {
            Console.Write("캐릭터 이름을 입력하세요: ");
            player.Name = Console.ReadLine();

            Console.WriteLine("\n직업을 선택하세요:");
            Console.WriteLine("1. 전사 (공격력 15 / 방어력 10 / 체력 120)");
            Console.WriteLine("2. 마법사 (공격력 25 / 방어력 5 / 체력 80)");
            Console.WriteLine("3. 궁수 (공격력 18 / 방어력 7 / 체력 100)");
            Console.Write("선택 (1~3): ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    player.Job = JobType.Warrior;
                    player.Attack = 15;
                    player.Defense = 10;
                    player.HP = 120;
                    break;
                case "2":
                    player.Job = JobType.Mage;
                    player.Attack = 25;
                    player.Defense = 5;
                    player.HP = 80;
                    break;
                case "3":
                    player.Job = JobType.Archer;
                    player.Attack = 18;
                    player.Defense = 7;
                    player.HP = 100;
                    break;
                default:
                    Console.WriteLine("잘못된 선택입니다. 다시 입력하세요.");
                    CreateCharacter(); // 재귀 호출
                    return;
            }

            player.Level = 1;
            player.Gold = 100;
            player.Inventory = new List<Item>();
            player.EquippedItem = null;

            Console.WriteLine($"\n{player.Job} 직업의 {player.Name}님으로 게임을 시작합니다!");
            // MainMenu;  // //♥♥♥♥♥♥일단 없애 쇼우인트로 끝나면 어차피 메인함수로직생김 오류 4개->2개
        }

        private class Player
        {
            public int Level = 1;
            public string Name;
            public JobType Job; // string → JobType으로 변경
            public int Attack;
            public int Defense;
            public int HP;
            public int Gold;

            public List<Item> Inventory = new List<Item>();    
            public Item EquippedItem = null; //이게 뭐지? 아래에 쇼우 스테이터스 때문에 정의한건가요?아니면 인벤토리를 사용하는 기능을 추가구현때문에 이걸 하신건가요?예를들면 보상?드롭몬스터의 보상?

           /*
            public void ShowStatus() 
            {
                Console.Clear();
                Console.WriteLine("상태 보기");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");

                int totalAttack = Attack + (EquippedItem?.AttackBonus ?? 0);  //아이템이 장착 되어 있으면 보너스 어택을 추가한다?
                int totalDefense = Defense + (EquippedItem?.DefenseBonus ?? 0);   //근데 이거는 이미 메인과 인벤토리에서 보는건데 
                                                                                //showStatus는 없어도 되지 않나?
                                                                                //일단쇼우 스테이터스에 특별한 로직이
                                                                                //그냥 장비랑 공격력 +한거 보여주는거면 그냥 어차피 메인이랑 똑같은 로직이니까 그냥 없애죠?



                Console.WriteLine($"Lv .{Level:D2}");
                Console.WriteLine($"{Name} ({Job})");
                Console.WriteLine($"공격력 : {totalAttack}");
                Console.WriteLine($"방어력 : {totalDefense}");
                Console.WriteLine($"체 력 : {HP}");
                Console.WriteLine($"Gold : {Gold}G\n");

                Console.WriteLine("0. 나가기");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                string input = Console.ReadLine();

                if (input == "0")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    ShowStatus(); // 다시 표시
                }
            }
           */
        }
    }
}