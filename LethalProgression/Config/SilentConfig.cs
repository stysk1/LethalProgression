using LethalProgression.LessShitConfig.Attributes;

namespace LethalProgression.Config;

[ConfigSection("Silent")]
interface ISilentConfig
{
    [ConfigName("Silent Enabled")]
    [ConfigDescription("Enable the Silent skill?")]
    [ConfigDefault(true)]
    bool isEnabled { get; }

    [ConfigName("Silent Max Level")]
    [ConfigDescription("Maximum level for silent. Each level shrinks the radius at which enemies hear your voice.")]
    [ConfigDefault(20)]
    int maxLevel { get; }

    [ConfigName("Silent Multiplier")]
    [ConfigDescription("Percent reduction to voice noise range per level (capped at 95% total).")]
    [ConfigDefault(4f)]
    float multiplier { get; }
}
