using HarmonyLib;
using Verse;

namespace SaleOfGoods.Patches
{
    [HarmonyPatch(typeof(ThingWithComps), "ExposeData")]
    static class ThingWithComps_ExposeData_SaleOfGoodsPatch
    {
        internal static void Postfix(ThingWithComps __instance)
        {
            if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                bool a = false;
                Scribe_Values.Look(ref a, "SaleOfGoodsShouldStrip", defaultValue: false);
                if (a) CompStripChecker.GetChecker(__instance, a);
            }
        }
    }
}
