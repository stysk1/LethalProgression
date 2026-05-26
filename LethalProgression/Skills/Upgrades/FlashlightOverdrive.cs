using System.Collections.Generic;
using GameNetcodeStuff;
using LethalProgression.Config;
using LethalProgression.LessShitConfig;
using UnityEngine;

namespace LethalProgression.Skills.Upgrades;

internal class FlashlightOverdrive : Skill
{
    public override string ShortName => "FLO";

    public override string Name => "Flashlight Overdrive";

    public override string Attribute => "Flashlight Power";

    public override string Description => "The company overclocks your flashlight bulbs. Brighter, farther, slightly concerning warranty implications.";

    public override UpgradeType UpgradeType => UpgradeType.FlashlightOverdrive;

    public override int Cost => 1;

    public override int MaxLevel {
        get {
            IFlashlightOverdriveConfig config = LessShitConfigSystem.GetActive<IFlashlightOverdriveConfig>();

            return config.maxLevel;
        }
    }

    public override float Multiplier {
        get {
            IFlashlightOverdriveConfig config = LessShitConfigSystem.GetActive<IFlashlightOverdriveConfig>();

            return config.multiplier;
        }
    }

    public override bool IsTeamShared => false;

    private static readonly Dictionary<int, (float range, float intensity)> baseValues = new();

    private static void ApplyToLight(Light light, float boost)
    {
        if (light == null) return;
        int id = light.GetInstanceID();
        if (!baseValues.TryGetValue(id, out var basis))
        {
            basis = (light.range, light.intensity);
            baseValues[id] = basis;
        }
        light.range = basis.range * boost;
        light.intensity = basis.intensity * boost;
    }

    public static void ApplyOverdrive(FlashlightItem flashlight)
    {
        if (flashlight == null || !flashlight.isBeingUsed)
            return;

        PlayerControllerB holder = flashlight.playerHeldBy;
        if (holder == null || holder != GameNetworkManager.Instance?.localPlayerController)
            return;

        float boost = 1f;
        if (LP_NetworkManager.xpInstance != null
            && LP_NetworkManager.xpInstance.skillList != null
            && LP_NetworkManager.xpInstance.skillList.IsSkillValid(UpgradeType.FlashlightOverdrive))
        {
            boost = 1f + (LP_NetworkManager.xpInstance.skillList.skills[UpgradeType.FlashlightOverdrive].GetTrueValue() / 100f);
        }

        ApplyToLight(flashlight.flashlightBulb, boost);
        ApplyToLight(flashlight.flashlightBulbGlow, boost);
    }
}
