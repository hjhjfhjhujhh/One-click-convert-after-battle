using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using JetBrains.Annotations;
using RimWorld;
using Verse;


//It doesn't work.
//And what I want is holding things when dead instead of falling down.

namespace SaleOfGoods
{
    
    /*    [HarmonyPatch(typeof(UnityEngine.UIElements.DropdownMenu))]
        [HarmonyPatch("DropAndForbidEverything")]
        public static class Patch_Pawn_DropAndForbidEverything
        {
            public static bool Prefix()
            {
                return false;
            }
        }

        [HarmonyPatch(typeof(Pawn_EquipmentTracker))]
        [HarmonyPatch("DropAllEquipment")]
        public static class Patch_Pawn_EquipmentTracker_DropAllEquipment
        {
            public static bool Prefix()
            {
                return false;
            }
        }
        */
    internal static class Pawn_HealthTracker_MakeDowned_Patch
    {
        /// <summary>
        /// pawns shouldn't litter all their stuff when they get downed.
        /// </summary>
        [HarmonyPatch(typeof(Pawn_HealthTracker), nameof(Pawn_HealthTracker.MakeDowned))]
        public class MakeDowned
        {
            private static readonly MethodInfo original = AccessTools.Method(typeof(Pawn), nameof(Pawn.DropAndForbidEverything));
            private static readonly MethodInfo replacement = AccessTools.Method(typeof(Pawn_HealthTracker_MakeDowned_Patch), nameof(DropAndForbidEverythingReplacement));

            [HarmonyTranspiler]    
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> source)
            {
                //Log.Message($"Patching: {original?.FullDescription()} >> {replacement?.FullDescription()}");
                foreach (var instruction in source)
                {
                    //IL_00d3: callvirt instance void Verse.Pawn::DropAndForbidEverything(bool)
                    if (instruction.opcode == OpCodes.Callvirt && instruction.operand.Equals(original))
                    {
                        //Log.Message($"Replaced instruction {instruction.operand} with {replacement}.");
                        yield return instruction.Clone(replacement);
                    }
                    else yield return instruction;
                }
            }
        }

        [UsedImplicitly]
        public static void DropAndForbidEverythingReplacement(this Pawn pawn, bool keepInventoryAndEquipmentIfInBed, bool rememberPrimary)
        {
            if (false)//ShouldDrop(pawn))
            {
                // Run original code
                pawn.DropAndForbidEverything(keepInventoryAndEquipmentIfInBed);
            }
        }

        private static bool ShouldDrop(Pawn pawn)
        {
            if (pawn == null || pawn.IsColonist || pawn.IsSlaveOfColony || pawn.IsPrisonerOfColony) 
                return true; // Only affect guests and caravans (who might come in, get drunk and drop all their loot)
            return !SaleOfGoodsSettings.drops;
        }

        /// <summary>
        /// Upon loading, downed pawns spill their stuff. Do the check.
        /// </summary>
        [HarmonyPatch(typeof(Pawn_EquipmentTracker), nameof(Pawn_EquipmentTracker.Notify_PawnSpawned))]
        public class Notify_PawnSpawned
        {
            private static readonly MethodInfo original = AccessTools.Method(typeof(Pawn_EquipmentTracker), nameof(Pawn_EquipmentTracker.DropAllEquipment));
            private static readonly MethodInfo replacement = AccessTools.Method(typeof(Pawn_HealthTracker_MakeDowned_Patch), nameof(DropAllEquipmentReplacement));

            [HarmonyTranspiler]    
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> source)
            {
                foreach (var instruction in source)
                {
                    //IL_004a: call instance void Verse.Pawn_EquipmentTracker::DropAllEquipment(valuetype Verse.IntVec3, bool)
                    if (instruction.opcode == OpCodes.Call && instruction.operand.Equals(original))
                    {
                        //Log.Message($"Replaced instruction {instruction.operand} with {replacement}.");
                        var replaced = instruction.Clone(replacement);
                        yield return replaced;
                    }
                    else yield return instruction;
                }
            }
        }

        [UsedImplicitly]
        public static void DropAllEquipmentReplacement(this Pawn_EquipmentTracker equipment, IntVec3 pos, bool forbid = true, bool rememberPrimary = false)
        {
            if (false)//ShouldDrop(equipment.pawn))
        {
                // Run original code
                equipment.DropAllEquipment(pos, forbid);
            }
        }
    }
}