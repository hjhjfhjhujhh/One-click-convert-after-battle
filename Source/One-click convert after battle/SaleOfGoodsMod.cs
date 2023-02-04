using System;
using System.Security.Cryptography;
using UnityEngine;
using Verse;

namespace SaleOfGoods
{
    [StaticConstructorOnStartup]
    internal class SaleOfGoodsMod : Mod
    {
        public static SaleOfGoodsMod Instance { get; private set; }
        public SaleOfGoodsMod(ModContentPack content) : base(content)
        {
            SaleOfGoodsMod.Settings = base.GetSettings<SaleOfGoodsSettings>();
            SaleOfGoodsMod.Instance = this;
        }
        public override string SettingsCategory()
        {
            return Translator.Translate("SaleOfGoods_Setting");
        }
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(GenUI.ContractedBy(inRect, 60f));
            //Rect rect2;
            //可注释
            listing_Standard.Label(Translator.Translate("GoodWill"), -1f, null);
            listing_Standard.CheckboxLabeled(Translator.Translate("Cost_GoodWill"), ref SaleOfGoodsMod.Settings.goodWill, null, 0f, 1f);
            //测试是否超限
            this.TextFieldNumericLabeled<int>(listing_Standard, Translator.Translate("GoodWillInt"), ref SaleOfGoodsMod.Settings.goodWillInt, 0f, 200f);
            listing_Standard.Label(Translator.Translate("What_to_convert"), -1f, null);
            listing_Standard.CheckboxLabeled(Translator.Translate("Corpses_With_Clothes"), ref SaleOfGoodsMod.Settings.deadBody, null, 0f, 1f);
            this.TextFieldNumericLabeled<int>(listing_Standard, Translator.Translate("Deadint"), ref SaleOfGoodsMod.Settings.deadint, 0f, 1000000f);
            listing_Standard.CheckboxLabeled(Translator.Translate("With_Drops"), ref SaleOfGoodsMod.Settings.drops, null, 0f, 1f);
            this.TextFieldNumericLabeled<int>(listing_Standard, Translator.Translate("Searching_area"), ref SaleOfGoodsMod.Settings.searcharea, 0f, 1000f);
            listing_Standard.End();
        }
        private void TextFieldNumericLabeled<T>(Listing_Standard ls, string label, ref T val, float min, float max) where T : struct
        {
            string text = val.ToString();
            ls.TextFieldNumericLabeled<T>(label, ref val, ref text, min, max);
        }

        public static SaleOfGoodsSettings Settings;
    }
}
