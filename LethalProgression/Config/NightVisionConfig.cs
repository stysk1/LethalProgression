using LethalProgression.LessShitConfig.Attributes;

namespace LethalProgression.Config;

[ConfigSection("Night Vision")]
interface INightVisionConfig
{
    [ConfigName("Night Vision Enabled")]
    [ConfigDescription("Enable the Night Vision skill?")]
    [ConfigDefault(true)]
    bool isEnabled { get; }

    [ConfigName("Night Vision Max Level")]
    [ConfigDescription("Maximum level for night vision.")]
    [ConfigDefault(99999)]
    int maxLevel { get; }

    [ConfigName("Night Vision Multiplier")]
    [ConfigDescription("Percent boost to night vision range and intensity per level.")]
    [ConfigDefault(5f)]
    float multiplier { get; }
}
