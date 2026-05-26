using LethalProgression.LessShitConfig.Attributes;

namespace LethalProgression.Config;

[ConfigSection("Sneak")]
interface ISneakConfig
{
    [ConfigName("Sneak Enabled")]
    [ConfigDescription("Enable the Sneak skill?")]
    [ConfigDefault(true)]
    bool isEnabled { get; }

    [ConfigName("Sneak Max Level")]
    [ConfigDescription("Maximum level for sneak. Each level reduces the crouch speed penalty until at max it equals walking speed.")]
    [ConfigDefault(10)]
    int maxLevel { get; }

    [ConfigName("Sneak Multiplier")]
    [ConfigDescription("Percent of the crouch speed penalty removed per level (10 levels x 10% = walking speed).")]
    [ConfigDefault(10f)]
    float multiplier { get; }
}
