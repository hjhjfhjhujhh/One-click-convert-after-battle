using System;
using HarmonyLib;
using Verse;

namespace SaleOfGoods.Patches
{
    [HarmonyPatch(typeof(GenDrop), nameof(GenDrop.TryDropSpawn))]
    static class GenPlace_TryPlaceThing_SaleOfGoodsPatch
    {

        static void Postfix(Thing thing, IntVec3 dropCell, Map map, ThingPlaceMode mode, Thing resultingThing, Action<Thing, int> placedAction, Predicate<IntVec3> nearPlaceValidator)
        {
            CompStripChecker UChecker = resultingThing.TryGetComp<CompStripChecker>();
            if (UChecker != null)
            {
                UChecker.ShouldStrip = false;
            }
        }
    }
}
