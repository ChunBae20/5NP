using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlytestTRPG; //나만 추가



namespace OnlytestTRPG
{
    public class Status
    {

        public int nowEquipSTR;
        public int nowEquipDEF;
        public int nowEquipCRT;     //float으로 해야하나? 치명타 .퍼센트?
        public int nowEquipAVD;    //회피율
        public int nowEquipHP = 100;

        // 현재체력 /최대체력1차목표
        // 시작부터 왜 70이 까임

        public int level = 1;
        public JobType job = Character.player.Job;//직업 추가 000
        public string name = Character.player.Name; //이름추가000

        public int basicCRT = 100;
        public int basicSTR = Character.player.Attack;             //기본 공격력
        public int basicDEF = Character.player.Defense;             //기본 방어력
        public int basicAVD = 100;
        public int basicHP = Character.player.HP;            //기본 체력
        public int basicGold = 1500;         //기본 소지금


        public int TotalHP => basicHP + nowEquipHP; //이게 현재 hp랑 장비 hp 합산한 값이잖아 이게 최대체력이라는 뜻 아님? 아진짜 컨스트마렵네

        









        //현재체력
        //public int CurrentHP;
        //전체체력을 어떻게 구현하지?

        //근데 지수님이 우려하는 그 뭐지?힐아이템 사용시 커렌트체력증가는
        //그냥 if(현재체력<최대체력&&힐아이템사용시)
        //{
        //    현재체력에+=포션의 힐값을 //현재체력에 최대체력을 넣는다.
        //}
        // else이미최대체력입니다.




    }

}
