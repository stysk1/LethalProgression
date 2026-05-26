using LethalProgression.LessShitConfig.Attributes;

namespace LethalProgression.Config;

[ConfigSection("Scan Radius")]
interface IScanRadiusConfig
{
    [ConfigName("Scan Radius Enabled")]
    [ConfigDescription("Enable the Scan Radius skill?")]
    [ConfigDefault(true)]
    bool isEnabled { get; }

    [ConfigName("Scan Radius Max Level")]
    [ConfigDescription("Maximum level for scan radius.")]
    [ConfigDefault(20)]
    int maxLevel { get; }

    [ConfigName("Scan Radius Multiplier")]
    [ConfigDescription("Percent boost to scan distance per level.")]
    [ConfigDefault(5f)]
    float multiplier { get; }
}
