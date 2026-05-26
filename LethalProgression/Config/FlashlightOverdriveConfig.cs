using LethalProgression.LessShitConfig.Attributes;

namespace LethalProgression.Config;

[ConfigSection("Flashlight Overdrive")]
interface IFlashlightOverdriveConfig
{
    [ConfigName("Flashlight Overdrive Enabled")]
    [ConfigDescription("Enable the Flashlight Overdrive skill?")]
    [ConfigDefault(true)]
    bool isEnabled { get; }

    [ConfigName("Flashlight Overdrive Max Level")]
    [ConfigDescription("Maximum level for flashlight overdrive.")]
    [ConfigDefault(99999)]
    int maxLevel { get; }

    [ConfigName("Flashlight Overdrive Multiplier")]
    [ConfigDescription("Percent boost to flashlight range and intensity per level.")]
    [ConfigDefault(5f)]
    float multiplier { get; }
}
