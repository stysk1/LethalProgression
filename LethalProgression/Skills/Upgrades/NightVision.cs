using System.Collections.Generic;
using GameNetcodeStuff;
using LethalProgression.Config;
using LethalProgression.LessShitConfig;
using UnityEngine;

namespace LethalProgression.Skills.Upgrades;

internal class NightVision : Skill
{
    public override string ShortName => "NV";

    public override string Name => "Night Vision";

    public override string Attribute => "Night Vision";

    public override string Description => "The company supplies you with better lenses, brightening every dark hallway in the facility.";

    public override UpgradeType UpgradeType => UpgradeType.NightVision;

    public override int Cost => 1;

    public override int MaxLevel {
        get {
            INightVisionConfig config = LessShitConfigSystem.GetActive<INightVisionConfig>();

            return config.maxLevel;
        }
    }

    public override float Multiplier {
        get {
            INightVisionConfig config = LessShitConfigSystem.GetActive<INightVisionConfig>();

            return config.multiplier;
        }
    }

    public override bool IsTeamShared => false;

    private static readonly Dictionary<int, (float range, float intensity)> baseValues = new();

    public static void ApplyNightVisionBoost(PlayerControllerB player)
    {
        if (player == null || player.nightVision == null)
            return;

        if (player != GameNetworkManager.Instance?.localPlayerController)
            return;

        Light light = player.nightVision;
        int id = light.GetInstanceID();

        if (!baseValues.TryGetValue(id, out var basis))
        {
            basis = (light.range, light.intensity);
            baseValues[id] = basis;
        }

        float boost = 1f;
        if (LP_NetworkManager.xpInstance != null
            && LP_NetworkManager.xpInstance.skillList != null
            && LP_NetworkManager.xpInstance.skillList.IsSkillValid(UpgradeType.NightVision))
        {
            boost = 1f + (LP_NetworkManager.xpInstance.skillList.skills[UpgradeType.NightVision].GetTrueValue() / 100f);
        }

        light.range = basis.range * boost;
        light.intensity = basis.intensity * boost;
    }
}
