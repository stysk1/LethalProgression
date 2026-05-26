using System.Collections.Generic;
using HarmonyLib;
using LethalProgression.Skills.Upgrades;

namespace LethalProgression.Patches;

[HarmonyPatch]
internal class ScanRadiusPatch
{
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(HUDManager), "AssignNewNodes")]
    private static IEnumerable<CodeInstruction> AssignNewNodesTranspiler(IEnumerable<CodeInstruction> instructions)
    {
        return ScanRadius.AssignNewNodesOpCode(new List<CodeInstruction>(instructions));
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(HUDManager), "MeetsScanNodeRequirements")]
    private static IEnumerable<CodeInstruction> MeetsScanNodeRequirementsTranspiler(IEnumerable<CodeInstruction> instructions)
    {
        return ScanRadius.MeetsScanNodeRequirementsOpCode(new List<CodeInstruction>(instructions));
    }
}
