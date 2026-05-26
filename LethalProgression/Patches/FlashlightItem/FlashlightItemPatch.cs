using HarmonyLib;
using LethalProgression.Skills.Upgrades;

namespace LethalProgression.Patches;

[HarmonyPatch]
internal class FlashlightItemPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(FlashlightItem), "Update")]
    private static void UpdatePostfix(FlashlightItem __instance)
    {
        FlashlightOverdrive.ApplyOverdrive(__instance);
    }
}
