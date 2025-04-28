using OnlytestTRPG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlytestTRPG
{
    public class QuestInfo
    {
        public string QuestName;
        public string QuestDescription;
        public string QuestRequest;
        public int QuestProgress;
        public int QuestGoal;
        public string[] QuestReward;
        public string QuestProcess;
        public bool IsFinished = false;
        public bool IsSelected = false;

        public QuestInfo(string questName, string questDescription, string questRequest, int questProgress, int questGoal, string[] questReward, string questProcess) //퀘스트 하나 만들때 빠르게 값을 초기화
        {
            QuestName = questName;
            QuestDescription = questDescription;
            QuestRequest = questRequest;
            QuestProgress = questProgress;
            QuestGoal = questGoal;
            QuestReward = questReward;
            QuestProcess = questProcess;
        }
    }

    public class Quest : MainSpace
    {
        public static List<QuestInfo> questList = new() // 퀘스트 리스트
        {
            new QuestInfo("마을에서 아이템 구매",
                "이봐! 모험을 떠나는것도 좋지만 아무런 준비 없이 모험을 떠날건 아니지?\r\n마을에는 상점이 있다네, 그러니 상점으로 가서 필요한 장비를 구매해보게!",
                "상점에서 아이템을 구매해보자",
                0,
                1,
                new string[] { "500000G" , "100000G" },
                "아이템 구매")
        };

        public void QuestOption() //퀘스트 선택
        {
            Console.Clear();
            Console.WriteLine("Quest!!\n");

            for (int i = 0; i < questList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {questList[i].QuestName}");
            }

            Console.WriteLine("\n\n원하시는 퀘스트를 선택해주세요.");

            int num = Input(1, questList.Count);

            QuestSelect(questList[num - 1]);

        }

        public void QuestSelect(QuestInfo selectQuest) //선택한 퀘스트 수락/거절
        {
            Console.Clear();
            Console.WriteLine("Quest!!");
            Console.WriteLine($"\n{selectQuest.QuestName}");
            Console.WriteLine($"\n{selectQuest.QuestDescription}");
            Console.WriteLine($"\n- {selectQuest.QuestRequest} ({selectQuest.QuestProgress} / {selectQuest.QuestGoal})");
            Console.WriteLine("- 보상 -");
            Console.WriteLine($"\n{string.Join("\n", selectQuest.QuestReward)}");
            Console.WriteLine("\n\n1. 수락");
            Console.WriteLine("2. 거절");
            Console.WriteLine("\n원하시는 행동을 입력해주세요");

            int num = Input(1, 2);

            if (selectQuest.IsSelected == false && num == 1)
            {
                selectQuest.IsSelected = true;
                Console.WriteLine("\n퀘스트가 수락되었습니다!\n");
                Thread.Sleep(1000);
                MainMenu();
            }
            else if (selectQuest.IsSelected == true && num == 2)
            {
                selectQuest.IsSelected = false;
                Console.WriteLine("\n퀘스트를 포기했습니다.\n");
                Thread.Sleep(1000);
                MainMenu();
            }
            else if (selectQuest.IsSelected == true)
            {
                Console.WriteLine("\n이미 퀘스트를 수락하셨습니다.\n");
                Thread.Sleep(1000);
                MainMenu();
            }

        }

        public void UpdateQuestProcess(string questProcess, QuestInfo selectQuest) // 퀘스트 진행 완료 여부 확인 시스템
        {
            foreach (var quest in questList)
            {
                if (!quest.IsFinished && quest.IsSelected && quest.QuestProcess == questProcess)  //퀘스트 진행
                {
                    quest.QuestProgress++;

                    if (quest.QuestProgress >= quest.QuestGoal) // 퀘스트 진행 완료 시
                    {
                        quest.QuestProgress = quest.QuestGoal;
                        Console.WriteLine($"\n[퀘스트 완료] {quest.QuestName}");
                        quest.IsFinished = true;
                        QuestSuccess(selectQuest);
                    }
                }
            }
        }

        public void QuestSuccess(QuestInfo selectQuest) // 퀘스트 성공 UI
        {
            Console.Clear();
            Console.WriteLine("Quest!!");
            Console.WriteLine($"\n{selectQuest.QuestName}");
            Console.WriteLine($"\n{selectQuest.QuestDescription}");
            Console.WriteLine($"\n- {selectQuest.QuestRequest}  ( {selectQuest.QuestProgress} / {selectQuest.QuestGoal})");

            Console.WriteLine("- 보상 -");
            Console.WriteLine($"\n{string.Join("\n", selectQuest.QuestReward)}"); //배열에 각 문자열마다 \n을 넣음
            Console.WriteLine("\n\n1. 보상 받기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요");

            int num = Input(1, 1);

            switch (num)
            {
                case 1: Reward(selectQuest); break;
            }
        }

        public static void Reward(QuestInfo selectQuest) // 퀘스트 보상 적용 시스템
        {
            Console.WriteLine("보상을 받았습니다!");

            foreach (var reward in selectQuest.QuestReward) //각 보상 적용(장비 보상 관련 적용하려했으나 포기)
            {
                if (reward.Contains("G"))
                {
                    string numberPart = new string(reward.Where(char.IsDigit).ToArray()); // 각 문자를 숫자인지 확인 후 추출 -> 배열로 만든 후 다시 하나의 문자열로 만듬
                    int goldAmount = int.Parse(numberPart);
                    
                    status.BasicGold += goldAmount; // 수당 지급
                    Console.WriteLine($"+{goldAmount}G (총 보유: {status.BasicGold})");
                    if(selectQuest.IsFinished == true)
                    {
                        selectQuest.IsSelected = false;
                        selectQuest.IsFinished = false;
                        selectQuest.QuestProgress = 0;
                    }
                }
            }
            Thread.Sleep(2000);
            MainMenu();
        }
    }
}
