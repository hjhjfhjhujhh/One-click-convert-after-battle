using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using SaleOfGoods;
using Verse;

public static class GetGoods
{
    public static int GetCorpses()
    {
        Map currentMap = Find.CurrentMap;
        int count = 0;
        if(currentMap != null)
        {
            List<Thing> list = currentMap.listerThings.ThingsInGroup((ThingRequestGroup)8); 
            count = list.Count;
            for (int i = list.Count - 1; i > -1; i--)
            {
                Corpse corpse = (Corpse)list[i];
                Pawn pawn = corpse.InnerPawn;
                if (pawn.IsColonist || pawn.IsSlaveOfColony || pawn.IsPrisonerOfColony)
                {
                    count--;
                    continue;
                }
            }
        }
        return count;
    }
    public static int RemoveCorpses()
    {
        Map currentMap = Find.CurrentMap;
        int result = 0;
        if(currentMap != null)
        {
            List<Thing> list = currentMap.listerThings.ThingsInGroup((ThingRequestGroup)8);
            int count = list.Count;
            for (int i = list.Count - 1; i > -1; i--)
            {
                Corpse corpse = (Corpse)list[i];
                Pawn pawn = corpse.InnerPawn;
                if (pawn.IsColonist || pawn.IsSlaveOfColony || pawn.IsPrisonerOfColony)
                {
                    count--;
                    continue;
                }
                corpse.DeSpawn(0);
                bool flag2 = !corpse.Destroyed;
                if (flag2)
                {
                    corpse.Destroy(0);
                }
                bool flag3 = !corpse.Discarded;
                if (flag3)
                {
                    corpse.Discard(false);
                }
            }
            if(SaleOfGoodsSettings.deadBody)
            {
                result = count * SaleOfGoodsSettings.deadint;
                if(SaleOfGoodsSettings.drops)
                    result += count *  SaleOfGoodsSettings.dropsint;
            }
        }
        return result;
    }
    public static void GetDrops(out ThingDef drop)
    {
        drop = null;
        List<ThingDef> mineables = ((GenStep_PreciousLump)GenStepDefOf.PreciousLump.genStep).mineables;
        ThingDef thingDef = mineables[1];
        drop = thingDef.building.mineableThing;
    }
}
