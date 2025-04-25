using System;
using System.Runtime.InteropServices;
using OnlytestTRPG;

namespace OnlytestTRPG
{

    public class MainSpace
    {


        static void Main(string[] args) //프로그램 시작 시 하기 순으로 실행
        {
            character.Intro();
            MainMenu();
        }

        public static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(" 스파르타 던전에 오신 여러분 환영합니다.");
                Console.WriteLine(" 이제 전투를 시작할 수 있습니다. \n\n\n");

                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 회복 아이템");
                Console.WriteLine("4. 퀘스트");
                Console.WriteLine("5. 상점");
                Console.WriteLine("6. 전투 시작");
                Console.WriteLine("원하시는 행동을 입력해주세요.....");

                string? Maininput = Console.ReadLine();

                switch (Maininput)
                {
                    case "1":
                        status.StatusScene();
                        break;
                    case "2":
                        inven.InventoryUI();
                        break;
                    case "3":
                        healItem.Heal();
                        break;
                    case "4":
                        quest.QuestOption();
                        break;
                    case "5":
                        store.EnterStore();
                        break;
                    case "6":
                        battle.DisplayBattleScene();
                        return;
                    case "team5NP":
                        TeamMembers();
                        break;

                    default:
                        Console.WriteLine("잘못된 입력입니다. 아무 키나 누르면 다시 선택화면으로 돌아갑니다.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        //Main.cs에서 각 파일 가져오기 위해 새인스턴스 생성
        public static Character character = new Character();
        public static BattleScene battle = new BattleScene();
        public static Store store = new Store();
        public static Quest quest = new Quest();
        public static HealItem healItem = new HealItem(3);
        public static Inventory inven = new Inventory();
        public static Status status = new Status();


        public static int Input(int min, int max) // 잘못 입력 시
        {
            while (true)
            {
                Console.Write(">>");
                string input = Console.ReadLine() ?? "";

                bool isValid = int.TryParse(input, out int number);

                if (isValid && number >= min && number <= max)
                {
                    return number;
                }

                Console.WriteLine("잘못된 입력입니다");
                Thread.Sleep(1000);

                // 메시지 지우기
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop);
            }
        }

        static void TeamMembers()
        {

            Console.Clear();

            Console.WriteLine("********************************");
            Console.WriteLine("*  이주현 팀장님 감사합니다.  *");
            Console.WriteLine("********************************");
            Console.WriteLine("★ 윤지민 발표자님 ★ 감사합니다.");
            Console.WriteLine(" 김지수님 감사합니다. ");
            Console.WriteLine(" 최용선님 감사합니다.");
            Console.WriteLine(" 민종곤님 감사합니다.");
            Console.WriteLine("  ");

            Console.WriteLine(" 사랑스런 1조의 팀원들입니다. \n아무 키누르면 프로그램 종료");
            Console.ReadKey();



        }
    }
}
