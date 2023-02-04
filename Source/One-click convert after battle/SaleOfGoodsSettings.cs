using System;
using UnityEngine;
using Verse;

namespace SaleOfGoods
{
    public class SaleOfGoodsSettings : ModSettings
    {
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<bool>(ref this.goodWill, "GoodWill", true, false);
            Scribe_Values.Look<int>(ref this.goodWillInt, "GoodWillInt", 15, true);
            Scribe_Values.Look<bool>(ref this.deadBody, "deadBody", true, false);
            Scribe_Values.Look<int>(ref this.deadint, "deadint", 100, true);
            Scribe_Values.Look<bool>(ref this.drops, "drops", true, false);
            Scribe_Values.Look<int>(ref this.searcharea, "searcharea", 1, true);
        }
        public void InitData()
        {
            this.goodWill = true;
            this.goodWillInt = 15;
            this.deadBody = true;
            this.deadint = 100;
            this.drops = true;
            this.searcharea = 1;
        }
        public bool goodWill = true;
        public int goodWillInt = 15;
        public bool deadBody = true;
        public int deadint = 100;
        public bool drops = true;
        public int searcharea = 1;

        public Vector2 scrollPos = Vector2.zero;
    }
}
