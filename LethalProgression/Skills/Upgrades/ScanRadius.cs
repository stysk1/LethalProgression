using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using LethalProgression.Config;
using LethalProgression.LessShitConfig;

namespace LethalProgression.Skills.Upgrades;

internal class ScanRadius : Skill
{
    public override string ShortName => "SCN";

    public override string Name => "Scan Radius";

    public override string Attribute => "Scan Distance";

    public override string Description => "The company tunes your suit scanner, letting it pick up scrap and signs from much farther away.";

    public override UpgradeType UpgradeType => UpgradeType.ScanRadius;

    public override int Cost => 1;

    public override int MaxLevel {
        get {
            IScanRadiusConfig config = LessShitConfigSystem.GetActive<IScanRadiusConfig>();

            return config.maxLevel;
        }
    }

    public override float Multiplier {
        get {
            IScanRadiusConfig config = LessShitConfigSystem.GetActive<IScanRadiusConfig>();

            return config.multiplier;
        }
    }

    public override bool IsTeamShared => false;

    public static float GetScanMultiplier()
    {
        if (LP_NetworkManager.xpInstance == null || LP_NetworkManager.xpInstance.skillList == null)
            return 1f;

        if (!LP_NetworkManager.xpInstance.skillList.IsSkillValid(UpgradeType.ScanRadius))
            return 1f;

        return 1f + (LP_NetworkManager.xpInstance.skillList.skills[UpgradeType.ScanRadius].GetTrueValue() / 100f);
    }

    // Replaces the 80f max-distance argument in HUDManager.AssignNewNodes' SphereCastNonAlloc.
    public static float GetSphereCastDistance(float defaultDistance) => defaultDistance * GetScanMultiplier();

    // Replaces the node.maxRange used in MeetsScanNodeRequirements.
    public static float GetNodeMaxRange(float defaultMaxRange) => defaultMaxRange * GetScanMultiplier();

    public static List<CodeInstruction> AssignNewNodesOpCode(List<CodeInstruction> codes)
    {
        // Pattern: Ldc_R4 80f used as the sphere-cast max distance argument.
        for (int index = 0; index < codes.Count; index++)
            if (codes[index].opcode == OpCodes.Ldc_R4 && codes[index].operand is float f && f == 80f)
                codes.Insert(index + 1, new CodeInstruction(OpCodes.Call, typeof(ScanRadius).GetMethod(nameof(GetSphereCastDistance))));

        return codes;
    }

    public static List<CodeInstruction> MeetsScanNodeRequirementsOpCode(List<CodeInstruction> codes)
    {
        // After `node.maxRange` is loaded and converted with Conv_R4, scale it.
        for (int index = 0; index < codes.Count - 1; index++)
            if (codes[index].opcode == OpCodes.Ldfld
                && codes[index].operand is System.Reflection.FieldInfo fi
                && fi.Name == nameof(ScanNodeProperties.maxRange)
                && codes[index + 1].opcode == OpCodes.Conv_R4)
            {
                codes.Insert(index + 2, new CodeInstruction(OpCodes.Call, typeof(ScanRadius).GetMethod(nameof(GetNodeMaxRange))));
            }

        return codes;
    }
}
