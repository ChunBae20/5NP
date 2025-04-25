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

        public QuestInfo(string questName, string questDescription, string questRequest, int questProgress, int questGoal, string[] questReward, string questProcess)
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
        public static List<QuestInfo> questList = new List<QuestInfo>()
        {
            new QuestInfo("마을에서 아이템 구매",
                "이봐! 모험을 떠나는것도 좋지만 아무런 준비 없이 모험을 떠날건 아니지?\r\n마을에는 상점이 있다네, 그러니 상점으로 가서 필요한 장비를 구매해보게!",
                "상점에서 아이템을 구매해보자",
                0,
                1, new string[] { "500000G" },
                "아이템 구매")


        };

        public void QuestOption()
        {
            Console.Clear();
            Console.WriteLine("Quest!!");

            for (int i = 0; i < questList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {questList[i].QuestName}");
            }

            Console.WriteLine("\n\n원하시는 퀘스트를 선택해주세요.");
            Console.Write(">> ");

            int num = Input(1, questList.Count);

            QuestSelect(questList[num - 1]);

        }

        public void QuestSelect(QuestInfo selectQuest)
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
            Console.Write(">> ");

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

        public void UpdateQuestProcess(string questProcess, QuestInfo selectQuest)
        {
            foreach (var quest in questList)
            {
                if (!quest.IsFinished && quest.IsSelected && quest.QuestProcess == questProcess)
                {
                    quest.QuestProgress++;

                    if (quest.QuestProgress >= quest.QuestGoal)
                    {
                        quest.QuestProgress = quest.QuestGoal;
                        Console.WriteLine($"\n[퀘스트 완료] {quest.QuestName}");
                        quest.IsFinished = true;
                        QuestSuccess(selectQuest);
                    }
                }
            }
        }

        public void QuestSuccess(QuestInfo selectQuest)
        {
            Console.Clear();
            Console.WriteLine("Quest!!");
            Console.WriteLine($"\n{selectQuest.QuestName}");
            Console.WriteLine($"\n{selectQuest.QuestDescription}");
            Console.WriteLine($"\n- {selectQuest.QuestRequest}  ( {selectQuest.QuestProgress} / {selectQuest.QuestGoal})");

            Console.WriteLine("- 보상 -");
            Console.WriteLine($"\n{string.Join("\n", selectQuest.QuestReward)}");
            Console.WriteLine("\n\n1. 보상 받기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요");
            Console.Write(">> ");

            int num = Input(1, 1);

            switch (num)
            {
                case 1: Reward(selectQuest); break;
            }
        }

        public void Reward(QuestInfo selectQuest)
        {
            Console.Clear();
            Console.WriteLine("보상을 받았습니다!");

            foreach (var reward in selectQuest.QuestReward)
            {
                if (reward.Contains("G"))
                {
                    string numberPart = new string(reward.Where(char.IsDigit).ToArray());
                    int goldAmount = int.Parse(numberPart);

                    status.basicGold += goldAmount;
                    Console.WriteLine($"+{goldAmount}G (총 보유: {status.basicGold})");
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
