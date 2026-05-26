using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using LethalProgression.Config;
using LethalProgression.LessShitConfig;

namespace LethalProgression.Skills.Upgrades;

internal class Sneak : Skill
{
    public override string ShortName => "SNK";

    public override string Name => "Sneak";

    public override string Attribute => "Crouch Speed";

    public override string Description => "Years of company-mandated stealth training. Each level brings your crouching speed closer to your walking speed.";

    public override UpgradeType UpgradeType => UpgradeType.Sneak;

    public override int Cost => 1;

    public override int MaxLevel {
        get {
            ISneakConfig config = LessShitConfigSystem.GetActive<ISneakConfig>();

            return config.maxLevel;
        }
    }

    public override float Multiplier {
        get {
            ISneakConfig config = LessShitConfigSystem.GetActive<ISneakConfig>();

            return config.multiplier;
        }
    }

    public override bool IsTeamShared => false;

    // The base game divides movement speed by 1.5f while crouched. Each skill level
    // reduces that penalty by (Multiplier%) of the gap between 1.5f and 1f, clamped at 1f.
    public static float GetCrouchDivisor(float defaultDivisor)
    {
        if (LP_NetworkManager.xpInstance == null || LP_NetworkManager.xpInstance.skillList == null)
            return defaultDivisor;

        if (!LP_NetworkManager.xpInstance.skillList.IsSkillValid(UpgradeType.Sneak))
            return defaultDivisor;

        float percentRemoved = LP_NetworkManager.xpInstance.skillList.skills[UpgradeType.Sneak].GetTrueValue() / 100f;
        if (percentRemoved > 1f) percentRemoved = 1f;

        float penalty = defaultDivisor - 1f;
        return 1f + penalty * (1f - percentRemoved);
    }

    public static List<CodeInstruction> CrouchSpeedOpCode(List<CodeInstruction> codes)
    {
        // Match `Ldc_R4 1.5f` followed by `Div` (the `num3 /= 1.5f` line when isCrouching).
        for (int index = 0; index < codes.Count - 1; index++)
            if (codes[index].opcode == OpCodes.Ldc_R4
                && codes[index].operand is float f && f == 1.5f
                && codes[index + 1].opcode == OpCodes.Div)
            {
                codes.Insert(index + 1, new CodeInstruction(OpCodes.Call, typeof(Sneak).GetMethod(nameof(GetCrouchDivisor))));
            }

        return codes;
    }
}
