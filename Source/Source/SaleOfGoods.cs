using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
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
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            harmony.Patch(typeof(FactionDialogMaker).GetMethod("FactionDialogFor"), null, new HarmonyMethod(typeof(SaleOfGoods), "FactionDialogForPostFix", null), null, null);
        }
        /*public void Save()
        {
            LoadedModManager.GetMod<SaleOfGoods>().GetSettings<Settings>().Write();
        }*/
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
            string text;
            if (SaleOfGoodsSettings.goodWill)
            {
                text = TranslatorFormattedStringExtensions.Translate("SaleOfGoodsYes", SaleOfGoodsSettings.goodWillInt);
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
                    if (SaleOfGoodsSettings.goodWill)
                    {
                        Faction.OfPlayer.TryAffectGoodwillWith(faction, 0 - SaleOfGoodsSettings.goodWillInt, false, true, HistoryEventDefOf.RequestedTrader, null);
                    }
                };
                diaOption2.linkLateBind = FactionDialogMaker.ResetToRoot(faction, negotiator);
                diaNode.options.Add(diaOption2);
                if (GetGoods.GetCorpses() == 0)
                {
                    diaOption2.Disable(Translator.Translate("NoCorpses"));
                    result = diaOption2;
                }

                DiaOption diaOption3 = new DiaOption(Translator.Translate("RemoveSnow_Home"));
                diaOption3.action = delegate ()
                {
                    Cleanser.RemoveSnow(map, true);
                    if (SaleOfGoodsSettings.cleangoodWill)
                    {
                        Faction.OfPlayer.TryAffectGoodwillWith(faction, 0 - SaleOfGoodsSettings.cleangoodWillInt, false, true, HistoryEventDefOf.RequestedTrader, null);
                    }
                };
                diaOption3.linkLateBind = FactionDialogMaker.ResetToRoot(faction, negotiator);
                diaNode.options.Add(diaOption3);
                DiaOption diaOption4 = new DiaOption(Translator.Translate("RemoveSnow_Whole"));
                diaOption4.action = delegate ()
                {
                    Cleanser.RemoveSnow(map, false);
                    if (SaleOfGoodsSettings.cleangoodWill)
                    {
                        Faction.OfPlayer.TryAffectGoodwillWith(faction, 0 - 2 * SaleOfGoodsSettings.cleangoodWillInt, false, true, HistoryEventDefOf.RequestedTrader, null);
                    }
                };
                diaOption4.linkLateBind = FactionDialogMaker.ResetToRoot(faction, negotiator);
                diaNode.options.Add(diaOption4);

                DiaOption diaOption5 = new DiaOption(Translator.Translate("RemoveFilth_Home"));
                diaOption5.action = delegate ()
                {
                    Cleanser.RemoveFilth(map, true);
                    if (SaleOfGoodsSettings.cleangoodWill)
                    {
                        Faction.OfPlayer.TryAffectGoodwillWith(faction, 0 - SaleOfGoodsSettings.cleangoodWillInt, false, true, HistoryEventDefOf.RequestedTrader, null);
                    }
                };
                diaOption5.linkLateBind = FactionDialogMaker.ResetToRoot(faction, negotiator);
                diaNode.options.Add(diaOption5);

                DiaOption diaOption6 = new DiaOption(Translator.Translate("RemoveFilth_Whole"));
                diaOption6.action = delegate ()
                {
                    Cleanser.RemoveFilth(map, false);
                    if (SaleOfGoodsSettings.cleangoodWill)
                    {
                        Faction.OfPlayer.TryAffectGoodwillWith(faction, 0 - 2 * SaleOfGoodsSettings.cleangoodWillInt, false, true, HistoryEventDefOf.RequestedTrader, null);
                    }
                };
                diaOption6.linkLateBind = FactionDialogMaker.ResetToRoot(faction, negotiator);
                diaNode.options.Add(diaOption6);

                diaNode.options.Add(new DiaOption(Translator.Translate("GoBack"))
                {
                    linkLateBind = FactionDialogMaker.ResetToRoot(faction, negotiator)
                });
                diaOption.link = diaNode;
                result = diaOption;
            }
            return result;
        }
    }
}
