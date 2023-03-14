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
            DiaOption diaOption = new DiaOption(Translator.Translate("SaleOfGoods"));
            bool flag = ((int)faction.PlayerRelationKind != 2);
            DiaOption result;
            if (flag)
            {
                diaOption.Disable(Translator.Translate("Convert_MustBeAlly"));
                result = diaOption;
            }
            else
            {
                DiaNode diaNode = new DiaNode(Translator.Translate("Convert_SellBooty"));
                ThingDef mine;
                GetGoods.GetDrops(out mine);

                string text;
                if (SaleOfGoodsSettings.goodWill)
                {
                    text = TranslatorFormattedStringExtensions.Translate("Convert_Silver_Yes", SaleOfGoodsSettings.goodWillInt);
                }
                else
                {
                    text = Translator.Translate("Convert_Silver_No");
                }
                DiaOption diaOption2 = new DiaOption(text);
                diaOption2.action = delegate ()
                {
                    Thing thing = ThingMaker.MakeThing(mine, null);
                    int num = GetGoods.RemoveCorpses();
                    thing.stackCount = num;
                    TradeUtility.SpawnDropPod(DropCellFinder.TradeDropSpot(map), map, thing);
                    Find.LetterStack.ReceiveLetter(TranslatorFormattedStringExtensions.Translate("Convert_DropPod", thing), TranslatorFormattedStringExtensions.Translate("Sell"), LetterDefOf.PositiveEvent, thing, null, null, null, null);
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

                DiaOption diaOption3 = new DiaOption(TranslatorFormattedStringExtensions.Translate("RemoveSnow_Home", SaleOfGoodsSettings.cleangoodWillInt));
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
                DiaOption diaOption4 = new DiaOption(TranslatorFormattedStringExtensions.Translate("RemoveSnow_Whole", 2 * SaleOfGoodsSettings.cleangoodWillInt));
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

                DiaOption diaOption5 = new DiaOption(TranslatorFormattedStringExtensions.Translate("RemoveFilth_Home", SaleOfGoodsSettings.cleangoodWillInt));
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

                DiaOption diaOption6 = new DiaOption(TranslatorFormattedStringExtensions.Translate("RemoveFilth_Whole", 2 * SaleOfGoodsSettings.cleangoodWillInt));
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
