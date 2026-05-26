using System;
using System.Collections.Generic;
using LethalProgression.Config;
using LethalProgression.LessShitConfig;
using LethalProgression.Skills.Upgrades;

namespace LethalProgression.Skills;

internal class SkillList
{
    public Dictionary<UpgradeType, Skill> skills = new();

    public bool IsSkillValid(UpgradeType upgrade) => skills.ContainsKey(upgrade) && skills[upgrade].CurrentLevel > 0;

    public Skill GetSkill(UpgradeType upgrade)
    {
        if (!IsSkillValid(upgrade))
        {
            return null;
        }

        return skills[upgrade];
    }

    public Dictionary<UpgradeType, Skill> GetSkills() => skills;

    public void InitializeSkills()
    {
        IHealthRegenConfig hpConfig = LessShitConfigSystem.GetActive<IHealthRegenConfig>();
        IMaxHealthConfig maxHpConfig = LessShitConfigSystem.GetActive<IMaxHealthConfig>();
        IStaminaConfig staminaConfig = LessShitConfigSystem.GetActive<IStaminaConfig>();
        IBatteryLifeConfig batteryConfig = LessShitConfigSystem.GetActive<IBatteryLifeConfig>();
        IHandSlotsConfig handSlotConfig = LessShitConfigSystem.GetActive<IHandSlotsConfig>();
        ILootValueConfig lootValueConfig = LessShitConfigSystem.GetActive<ILootValueConfig>();
        IStrengthConfig strengthConfig = LessShitConfigSystem.GetActive<IStrengthConfig>();
        IJumpHeightConfig jumpHeightConfig = LessShitConfigSystem.GetActive<IJumpHeightConfig>();
        ISprintSpeedConfig sprintSpeedConfig = LessShitConfigSystem.GetActive<ISprintSpeedConfig>();
        IShipHangarDoorConfig shipDoorBatteryConfig = LessShitConfigSystem.GetActive<IShipHangarDoorConfig>();
        INightVisionConfig nightVisionConfig = LessShitConfigSystem.GetActive<INightVisionConfig>();
        IFlashlightOverdriveConfig flashlightOverdriveConfig = LessShitConfigSystem.GetActive<IFlashlightOverdriveConfig>();
        IScanRadiusConfig scanRadiusConfig = LessShitConfigSystem.GetActive<IScanRadiusConfig>();
        ISneakConfig sneakConfig = LessShitConfigSystem.GetActive<ISneakConfig>();
        ISilentConfig silentConfig = LessShitConfigSystem.GetActive<ISilentConfig>();

        if (hpConfig.isEnabled)
            skills.Add(UpgradeType.HPRegen, new HPRegen());

        if (maxHpConfig.isEnabled)
            skills.Add(UpgradeType.MaxHealth, new MaxHP());

        if (staminaConfig.isEnabled)
            skills.Add(UpgradeType.Stamina, new Stamina());

        if (batteryConfig.isEnabled)
            skills.Add(UpgradeType.Battery, new BatteryLife());

        if (handSlotConfig.isEnabled && !LethalPlugin.ReservedSlots)
            skills.Add(UpgradeType.HandSlot, new HandSlots());

        if (lootValueConfig.isEnabled)
            skills.Add(UpgradeType.Value, new LootValue());
        
        if (strengthConfig.isEnabled)
            skills.Add(UpgradeType.Strength, new Strength());

        if (jumpHeightConfig.isEnabled)
            skills.Add(UpgradeType.JumpHeight, new JumpHeight());

        if (sprintSpeedConfig.isEnabled)
            skills.Add(UpgradeType.SprintSpeed, new SprintSpeed());

        if (shipDoorBatteryConfig.isEnabled)
            skills.Add(UpgradeType.ShipDoorBattery, new ShipDoorBattery());

        if (nightVisionConfig.isEnabled)
            skills.Add(UpgradeType.NightVision, new NightVision());

        if (flashlightOverdriveConfig.isEnabled)
            skills.Add(UpgradeType.FlashlightOverdrive, new FlashlightOverdrive());

        if (scanRadiusConfig.isEnabled)
            skills.Add(UpgradeType.ScanRadius, new ScanRadius());

        if (sneakConfig.isEnabled)
            skills.Add(UpgradeType.Sneak, new Sneak());

        if (silentConfig.isEnabled)
            skills.Add(UpgradeType.Silent, new Silent());
    }
}
