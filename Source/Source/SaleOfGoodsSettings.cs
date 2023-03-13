using System;
using UnityEngine;
using Verse;

namespace SaleOfGoods
{
    //Please refer my translation instead of labels here.Huge difference!
    public class SaleOfGoodsSettings : ModSettings
    {
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<bool>(ref goodWill, "GoodWill", true, false);
            Scribe_Values.Look<int>(ref goodWillInt, "GoodWillInt", 15, true);
            Scribe_Values.Look<bool>(ref deadBody, "deadBody", true, false);
            Scribe_Values.Look<int>(ref deadint, "deadint", 100, true);
            /*Scribe_Values.Look<bool>(ref drops, "drops", true, false);
            Scribe_Values.Look<int>(ref dropsint, "dropsint", 50, true);*/
        }
        public static bool goodWill = true;
        public static int goodWillInt = 15;
        public static bool deadBody = true;
        public static int deadint = 100;
        public static bool drops = true;
        public static int dropsint = 50;

        public Vector2 scrollPos = Vector2.zero;
    }
}
