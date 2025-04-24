using System;
using System.Runtime.InteropServices;
using OnlytestTRPG; //나만 추가

namespace OnlytestTRPG
{
// 자식 : 부모 힐아이템 인벤토리 다수정
public class MainSpace                        
{


    static void Main(string[] args)
    {
            
            Character.ShowIntro();
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

            string Maininput = Console.ReadLine();

            switch (Maininput)
            {
                case "1":
                    StatusScene();
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
                        //battle.SetData();
                        //battle.DisplayBattleScene();
                        BattleScene battle = new BattleScene();
                        battle.SetData(stage: 3); // 스테이지 값 전달
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
    public static BattleScene battle = new BattleScene();
    public static Store store = new Store();
    public static Quest quest = new Quest();
    public static HealItem healItem = new HealItem(3);
    public static Inventory inven = new Inventory(); //Inventory의 클래스를 가져와서 inven이라는 새로운 인스턴스 생성한다.
    public static Status status = new Status();      //Status의 클래스를 가져와서 status라는 새로운 인스턴스를 생성한다.
        private static object targetEnemy;

        static void StatusScene()
    {

        //기본 체력부
        int basicstr = status.basicSTR;
        int basicdef = status.basicDEF;
        int basicHP = status.basicHP;
        int basicGold = status.basicGold;

        Console.Clear();
        Console.WriteLine(" 캐릭터의 정보가 표시됩니다.\n\n\n");
        Console.WriteLine("공격력 : " + basicstr + " (+ " + status.nowEquipSTR + ")");     // 기본 공력력10 (+장비 공격력)
        Console.WriteLine("방어력 : " + basicdef + " (+ " + status.nowEquipDEF + ")");      // 기본 방어력5  (+장비 방어력)
        Console.WriteLine("체력 :" + basicHP + " ( " + status.nowEquipHP + ")");             // 기본 체력100  (+장비 체력)
        Console.WriteLine("Gold : " + basicGold);

    backagain:
        Console.WriteLine("\n\n\n원하는 행동을 입력하세요");
        Console.WriteLine(" 0. 나가기\n\n\n");

        string gotoStartScene = "0";
        string gotoexit = Console.ReadLine();
        if (gotoexit == gotoStartScene)
            {
                MainMenu();                         
            }
        else
            {
                goto backagain;
            }
        Console.WriteLine("상태보기 입니다. \n아무 키나 누르면 돌아갑니다.");
        Console.ReadKey();
     }

    public int Input(int min, int max)   
    {
        while (true)
        {
            string input = Console.ReadLine() ?? "";
            bool IsSelect = int.TryParse(input, out int number);

            if (IsSelect && min <= number && max >= number)
            {
                return number;
            }
            Console.WriteLine("잘못된 입력입니다");
        }
    }

    static void BattleScene()
    {
            /*Console.Clear();
            Console.WriteLine("전투화면입니다. \n아무 키누르면 프로그램 종료");
            Console.ReadKey(); */
            Console.Clear();
            Console.WriteLine("=== 전투를 시작합니다 ===");

            Console.Write("현재 스테이지를 입력하세요: ");
            if (int.TryParse(Console.ReadLine(), out int stage))
            {
                BattleScene battle = new BattleScene();
                battle.SetData(stage); // 몬스터 설정 및 출력
                                       // 추후 battle.Start() 같은 전투 진행 로직 연결 가능
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }
      }

    static void TeamMembers()
    {

        Console.Clear();

        Console.WriteLine(" 테크니컬 리더 님 감사합니다. ");
        Console.WriteLine(" 님 감사합니다. ");
        Console.WriteLine(" 님 감사합니다.");
        Console.WriteLine(" 님 감사합니다.");
        Console.WriteLine("  ");

        Console.WriteLine(" 1조의 팀원들입니다. \n아무 키누르면 프로그램 종료");
        Console.ReadKey();



    }

    }
}
