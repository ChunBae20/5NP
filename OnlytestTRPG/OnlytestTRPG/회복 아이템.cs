using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlytestTRPG
{
    public class HealItem : MainSpace
    {
        public static int potion;

        public HealItem(int potion)
        {
            HealItem.potion = potion;
        }

        public void Heal()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("회복");
                Console.WriteLine($"포션을 사용하면 체력을 30 회복 할 수 있습니다. (남은 포션 : {potion})");
                Console.WriteLine("\n\n1. 사용하기");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("\n원하시는 행동을 입력해주세요");
                Console.Write(">> ");

                int num = Input(0, 1);

                if (num == 0) MainMenu();
                if (num == 1 && potion > 0)
                {
                    potion -= 1;
                    Character.player.CurrentHP = Math.Min(Character.player.CurrentHP + 30, status.TotalHP);
                    Console.WriteLine("\n회복을 완료했습니다.");
                }
                else Console.WriteLine("\n포션이 부족합니다.");

                Console.WriteLine("\n3초 후에 다음 메시지가 출력됩니다...");
                Thread.Sleep(3000);
            }
        }

        public void AddPotion(int count)  //힐 아이템 어륨 더이상 머리에 안돌감 걍 추감 팀장이겟음
        {
            potion += count;
        }
                                            //여기까지

    }
}
