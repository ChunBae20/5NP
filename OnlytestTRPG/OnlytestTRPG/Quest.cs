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
        public bool IsFinished = false;

        public QuestInfo(string questName, string questDescription, string questRequest, int questProgress, int questGoal, string[] questReward)
        {
            QuestName = questName;
            QuestDescription = questDescription;
            QuestRequest = questRequest;
            QuestProgress = questProgress;
            QuestGoal = questGoal;
            QuestReward = questReward;

        }
    }

    public class Quest : MainSpace
    {
        static List<QuestInfo> questList = new List<QuestInfo>()
        {
            new QuestInfo("마을을 위협하는 미니언 처치",
                "이봐! 마을 근처에 미니언들이 너무 많아졌다고 생각하지 않나?\r\n마을주민들의 안전을 위해서라도 저것들 수를 좀 줄여야 한다고!\r\n모험가인 자네가 좀 처치해주게!",
                "미니언 5마리 처치",
                0, 
                5, new string[] { "5G" })
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

            switch (num)
            {
                case 1: QuestAccept(selectQuest); break;
                case 2: MainMenu(); break;
            }
        }

        public void QuestAccept(QuestInfo selectQuest)
        {
            if (selectQuest.QuestProgress == selectQuest.QuestGoal)
            {
                selectQuest.IsFinished = true;
                QuestSuccess(selectQuest);
            }
            else
            {
                Console.WriteLine("\n퀘스트가 수락되었습니다!\n");
                Thread.Sleep(1000);
                MainMenu();
            }
        }

        public void UpdateQuestProcess()
        {

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
            Console.WriteLine("2. 돌아가기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요");
            Console.Write(">> ");

            int num = Input(1, 2);

            switch (num)
            {
                case 1: Reward(selectQuest); break;
                case 2: MainMenu(); break;
            }
        }

        public void Reward(QuestInfo selectquest)
        {
            Console.Clear();
            Console.WriteLine("보상을 받았습니다!");

            foreach (var reward in selectquest.QuestReward)
            {
                if(reward.Contains("G"))
                {
                    string numberPart = new string(reward.Where(char.IsDigit).ToArray());
                    int goldAmount = int.Parse(numberPart);

                    status.basicGold += goldAmount;
                    Console.WriteLine($"+{goldAmount}G (총 보유: {status.basicGold})");
                }
            }
            Thread.Sleep(2000);
            MainMenu();
        }
    }
}
