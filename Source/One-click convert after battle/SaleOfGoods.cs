using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;

namespace SaleOfGoods
{
    [StaticConstructorOnStartup]
    public static class SaleOfGoods
    {
        static SaleOfGoods()
        {
            Harmony harmony = new Harmony("SaleOfGoods");
            harmony.Patch(typeof(FactionDialogMaker).GetMethod("FactionDialogFor"), null, new HarmonyMethod(typeof(SaleOfGoods), "FactionDialogForPostFix", null), null, null);
        }
        public static void FactionDialogForPostFix(ref DiaNode __result, Pawn negotiator, Faction faction)
        {
            bool flag = FactionUtility.AllyOrNeutralTo(faction, Faction.OfPlayer);
            if (flag)
            {
                __result.options.Insert(0, SaleOfGoods.SellingOption(faction, negotiator, negotiator.Map, __result));
            }
        }
        private static DiaOption SellingOption(Faction faction, Pawn negotiator, Map map, DiaNode original)
        {
            bool goodWill = SaleOfGoodsMod.Settings.goodWill;
            string text;
            if (goodWill)
            {
                text = TranslatorFormattedStringExtensions.Translate("SaleOfGoodsYes", SaleOfGoodsMod.Settings.goodWillInt);
            }
            else
            {
                text = Translator.Translate("SaleOfGoodsNo");
            }
            DiaOption diaOption = new DiaOption(text);
            bool flag = ((int)faction.PlayerRelationKind != 2);
            DiaOption result;
            if (flag)
            {
                diaOption.Disable(Translator.Translate("MustBeAlly"));
                result = diaOption;
            }
            else
            {
                if (GetGoods.GetCorpses() == 0)
                {
                    diaOption.Disable(Translator.Translate("NoCorpses"));
                    result = diaOption;
                }
                else
                {
                    DiaNode diaNode = new DiaNode(Translator.Translate("SellBooty"));
                    ThingDef mine;
                    GetGoods.GetDrops(out mine);
                    DiaOption diaOption2 = new DiaOption(Translator.Translate("Silver"));
                    diaOption2.action = delegate ()
                    {
                        Thing thing = ThingMaker.MakeThing(mine, null);
                        int num = GetGoods.RemoveCorpses();
                        thing.stackCount = num;
                        TradeUtility.SpawnDropPod(DropCellFinder.TradeDropSpot(map), map, thing);
                        Find.LetterStack.ReceiveLetter(TranslatorFormattedStringExtensions.Translate("DropPod", thing), TranslatorFormattedStringExtensions.Translate("Sell"), LetterDefOf.PositiveEvent, thing, null, null, null, null);
                        bool goodWill2 = SaleOfGoodsMod.Settings.goodWill;
                        if (goodWill2)
                        {
                            Faction.OfPlayer.TryAffectGoodwillWith(faction, 0 - SaleOfGoodsMod.Settings.goodWillInt, false, true, HistoryEventDefOf.RequestedTrader, null);
                        }
                    };
                    diaOption2.linkLateBind = FactionDialogMaker.ResetToRoot(faction, negotiator);
                    diaNode.options.Add(diaOption2);
                    diaNode.options.Add(new DiaOption(Translator.Translate("GoBack"))
                    {
                        linkLateBind = FactionDialogMaker.ResetToRoot(faction, negotiator)
                    });
                    diaOption.link = diaNode;
                    result = diaOption;
                }
            }
            return result;
        }
    }
}
