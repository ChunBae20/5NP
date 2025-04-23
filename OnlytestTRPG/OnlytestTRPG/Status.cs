using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using This_is_Sparta__;
using OnlytestTRPG; //나만 추가



namespace OnlytestTRPG
{
    public class Status
    {

        public int nowEquipSTR;
        public int nowEquipDEF;
        public int nowEquipCRT;     //float으로 해야하나? 치명타 .퍼센트?
        public int nowEquipAVD;    //회피율
        public int nowEquipHP;


        
        public int basicSTR = 5;             //기본 공격력
        public int basicDEF = 3;             //기본 방어력
        public int basicHP = 100;            //기본 체력
        public int basicGold = 1500;         //기본 소지금
        public int CurrentHP;
    }

}
