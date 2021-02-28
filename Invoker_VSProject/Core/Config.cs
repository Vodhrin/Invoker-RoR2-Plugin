using System;
using BepInEx.Configuration;

namespace Invoker.Core
{
    static class Config
    {
        public static ConfigEntry<float> baseMaxHealth;
        public static ConfigEntry<float> levelMaxHealth;
        public static ConfigEntry<float> baseRegen;
        public static ConfigEntry<float> levelRegen;
        public static ConfigEntry<float> baseMaxShield;
        public static ConfigEntry<float> levelMaxShield;
        public static ConfigEntry<float> baseMoveSpeed;
        public static ConfigEntry<float> levelMoveSpeed;
        public static ConfigEntry<float> baseAcceleration;
        public static ConfigEntry<float> baseJumpPower;
        public static ConfigEntry<float> levelJumpPower;
        public static ConfigEntry<int> baseJumpCount;
        public static ConfigEntry<float> baseDamage;
        public static ConfigEntry<float> levelDamage;
        public static ConfigEntry<float> baseAttackSpeed;
        public static ConfigEntry<float> levelAttackSpeed;
        public static ConfigEntry<float> baseCrit;
        public static ConfigEntry<float> levelCrit;
        public static ConfigEntry<float> baseArmor;
        public static ConfigEntry<float> levelArmor;
        public static ConfigEntry<float> sprintingSpeedMultiplier;

        public static ConfigEntry<float> primaryBaseAttackDuration;
        public static ConfigEntry<float> primaryDamageCoefficient;
        public static ConfigEntry<float> primaryProcCoefficient;

        public static void Read()
        {
            baseMaxHealth = InvokerPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Max Health"), Core.Constants.invokerBaseMaxHealth, new ConfigDescription("", null, Array.Empty<object>()));
            levelMaxHealth = InvokerPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Max Health"), Core.Constants.invokerLevelMaxHealth, new ConfigDescription("", null, Array.Empty<object>()));
            baseRegen = InvokerPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Regen"), Core.Constants.invokerBaseRegen, new ConfigDescription("", null, Array.Empty<object>()));
            levelRegen = InvokerPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Regen"), Core.Constants.invokerLevelRegen, new ConfigDescription("", null, Array.Empty<object>()));
            baseMaxShield = InvokerPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "LBase Max Shield"), Core.Constants.invokerBaseMaxShield, new ConfigDescription("", null, Array.Empty<object>()));
            levelMaxShield = InvokerPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Max Shield"), Core.Constants.invokerLevelMaxShield, new ConfigDescription("", null, Array.Empty<object>()));
            baseMoveSpeed = InvokerPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Move Speed"), Core.Constants.invokerBaseMoveSpeed, new ConfigDescription("", null, Array.Empty<object>()));
            levelMoveSpeed = InvokerPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Move Speed"), Core.Constants.invokerLevelMoveSpeed, new ConfigDescription("", null, Array.Empty<object>()));
            baseAcceleration = InvokerPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Acceleration"), Core.Constants.invokerBaseAcceleration, new ConfigDescription("", null, Array.Empty<object>()));
            baseJumpPower = InvokerPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Jump Power"), Core.Constants.invokerBaseJumpPower, new ConfigDescription("", null, Array.Empty<object>()));
            levelJumpPower = InvokerPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Jump Power"), Core.Constants.invokerLevelJumpPower, new ConfigDescription("", null, Array.Empty<object>()));
            baseJumpCount = InvokerPlugin.instance.Config.Bind<int>(new ConfigDefinition("01 - Character Stats", "Base Jump Count"), Core.Constants.invokerBaseJumpCount, new ConfigDescription("", null, Array.Empty<object>()));
            baseDamage = InvokerPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Damage"), Core.Constants.invokerBaseDamage, new ConfigDescription("", null, Array.Empty<object>()));
            levelDamage = InvokerPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Damage"), Core.Constants.invokerLevelDamage, new ConfigDescription("", null, Array.Empty<object>()));
            baseAttackSpeed = InvokerPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Attack Speed"), Core.Constants.invokerBaseAttackSpeed, new ConfigDescription("", null, Array.Empty<object>()));
            levelAttackSpeed = InvokerPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Attack Speed"), Core.Constants.invokerLevelAttackSpeed, new ConfigDescription("", null, Array.Empty<object>()));
            baseCrit = InvokerPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Crit Chance"), Core.Constants.invokerBaseCrit, new ConfigDescription("", null, Array.Empty<object>()));
            levelCrit = InvokerPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Crit Chance"), Core.Constants.invokerLevelCrit, new ConfigDescription("", null, Array.Empty<object>()));
            baseArmor = InvokerPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Armor"), Core.Constants.invokerBaseArmor, new ConfigDescription("", null, Array.Empty<object>()));
            levelArmor = InvokerPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Armor"), Core.Constants.invokerLevelArmor, new ConfigDescription("", null, Array.Empty<object>()));
            sprintingSpeedMultiplier = InvokerPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Sprinting Speed Multiplier"), Core.Constants.invokerSprintingSpeedMultiplier, new ConfigDescription("", null, Array.Empty<object>()));

            primaryDamageCoefficient = InvokerPlugin.instance.Config.Bind<float>(new ConfigDefinition("03 - Primary - Sharp Claws", "Damage Coefficient"), Core.Constants.invokerPrimaryDamageCoefficient, new ConfigDescription("", null, Array.Empty<object>()));
            primaryBaseAttackDuration = InvokerPlugin.instance.Config.Bind<float>(new ConfigDefinition("03 - Primary - Sharp Claws", "Base Attack Duration"), Core.Constants.invokerPrimaryBaseAttackDuration, new ConfigDescription("", null, Array.Empty<object>()));
            primaryProcCoefficient = InvokerPlugin.instance.Config.Bind<float>(new ConfigDefinition("03 - Primary - Sharp Claws", "Proc Coefficient"), Core.Constants.invokerPrimaryProcCoefficient, new ConfigDescription("", null, Array.Empty<object>()));
        }
    }
}
