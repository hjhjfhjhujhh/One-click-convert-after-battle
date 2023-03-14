using System;
using UnityEngine;
using Verse;

namespace SaleOfGoods
{
    //Please refer to my translation instead of labels here.Huge difference!
    public class SaleOfGoodsSettings : ModSettings
    {
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<bool>(ref goodWill, "Convert_GoodWill", true, false);
            Scribe_Values.Look<int>(ref goodWillInt, "Convert_GoodWillInt", 40, true);
            Scribe_Values.Look<bool>(ref deadBody, "deadBody", true, false);
            Scribe_Values.Look<int>(ref deadint, "deadint", 100, true);
            Scribe_Values.Look<bool>(ref drops, "drops", true, false);
            Scribe_Values.Look<int>(ref dropsint, "dropsint", 50, true);
            Scribe_Values.Look<bool>(ref cleangoodWill, "CleanGoodWill", true, false);
            Scribe_Values.Look<int>(ref cleangoodWillInt, "CleanGoodWillInt", 10, true);

            /*Scribe_Values.Look<bool>(ref player_downed_drop_equipment, "drops", false, false);
            Scribe_Values.Look<bool>(ref player_downed_drop_inventory, "drops", false, false);
            Scribe_Values.Look<bool>(ref nonplayer_downed_drop_equipment, "drops", false, false);
            Scribe_Values.Look<bool>(ref nonplayer_downed_drop_inventory, "drops", false, false);

            Scribe_Values.Look<bool>(ref player_killed_drop_equipment, "drops", false, false);
            Scribe_Values.Look<bool>(ref player_killed_drop_inventory, "drops", false, false);
            Scribe_Values.Look<bool>(ref nonplayer_killed_drop_equipment, "drops", false, false);
            Scribe_Values.Look<bool>(ref nonplayer_killed_drop_inventory, "drops", false, false);*/
        }

        public static bool goodWill = true;
        public static int goodWillInt = 40;
        public static bool deadBody = true;
        public static int deadint = 100;
        public static bool drops = true;
        public static int dropsint = 50;
        public static bool cleangoodWill = true;
        public static int cleangoodWillInt = 10;

        /*public static bool player_downed_drop_equipment = false;
        public static bool player_downed_drop_inventory = false;
        public static bool player_killed_drop_equipment = false;
        public static bool player_killed_drop_inventory = false;
        public static bool nonplayer_downed_drop_equipment = false;
        public static bool nonplayer_downed_drop_inventory = false;
        public static bool nonplayer_killed_drop_equipment = false;
        public static bool nonplayer_killed_drop_inventory = false;
        public static bool corpse_display_equipment = false;*/

        public Vector2 scrollPos = Vector2.zero;
    }
}
