using LethalProgression.Config;
using LethalProgression.LessShitConfig;

namespace LethalProgression.Skills.Upgrades;

internal class Silent : Skill
{
    // The noiseID used by StartOfRound when broadcasting voice-chat amplitude as audible noise.
    public const int VoiceNoiseId = 75;

    public override string ShortName => "SIL";

    public override string Name => "Silent";

    public override string Attribute => "Voice Noise";

    public override string Description => "Specialized vocal modulation. Enemies have a harder time pinpointing where your screams are coming from.";

    public override UpgradeType UpgradeType => UpgradeType.Silent;

    public override int Cost => 1;

    public override int MaxLevel {
        get {
            ISilentConfig config = LessShitConfigSystem.GetActive<ISilentConfig>();

            return config.maxLevel;
        }
    }

    public override float Multiplier {
        get {
            ISilentConfig config = LessShitConfigSystem.GetActive<ISilentConfig>();

            return config.multiplier;
        }
    }

    public override bool IsTeamShared => false;

    public static float GetReducedVoiceRange(float defaultRange)
    {
        if (LP_NetworkManager.xpInstance == null || LP_NetworkManager.xpInstance.skillList == null)
            return defaultRange;

        if (!LP_NetworkManager.xpInstance.skillList.IsSkillValid(UpgradeType.Silent))
            return defaultRange;

        float percent = LP_NetworkManager.xpInstance.skillList.skills[UpgradeType.Silent].GetTrueValue() / 100f;
        if (percent > 0.95f) percent = 0.95f;

        return defaultRange * (1f - percent);
    }
}
