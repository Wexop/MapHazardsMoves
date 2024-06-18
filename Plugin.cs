using System.Collections.Concurrent;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using LethalConfig;
using LethalConfig.ConfigItems;
using LethalConfig.ConfigItems.Options;
using MapHazardsMoves.Patches;
using MapHazardsMoves.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MapHazardsMoves
{
    [BepInDependency(StaticNetcodeLib.StaticNetcodeLib.Guid)]
    [BepInPlugin(GUID, NAME, VERSION)]
    public class MapHazardsMoves : BaseUnityPlugin
    {

        const string GUID = "wexop.maphazardsmoves";
        const string NAME = "MapHazardsMoves";
        const string VERSION = "1.0.0";

        public static MapHazardsMoves instance;

        public Dictionary<ulong, HazardObject> HazardsObjects = new Dictionary<ulong, HazardObject>();
        
        public ConfigEntry<float> minMovementDistanceEntry;
        public ConfigEntry<float> maxMovementDistanceEntry;
        
        public ConfigEntry<float> minMovementDelayEntry;
        public ConfigEntry<float> maxMovementDelayEntry;
        
        public ConfigEntry<float> landmineSpeedEntry;
        public ConfigEntry<bool> landmineEnabledEntry;
        
        public ConfigEntry<float> turretSpeedEntry;
        public ConfigEntry<bool> turretEnabledEntry;
        
        public ConfigEntry<float> spikeTrapSpeedEntry;
        public ConfigEntry<bool> spikeTrapEnabledEntry;

        public ConfigEntry<bool> enableDevLogsEntry;

        void Awake()
        {
            instance = this;
            
            Logger.LogInfo($"MapHazardsMoves starting....");
            
            //CONFIGS
            
            //GENERAL
            
            minMovementDistanceEntry = Config.Bind("General", "minMovementDistance", 5f,
                "The minimum distance that hazard can walk to. No need to restart the game :)");
            CreateFloatConfig(minMovementDistanceEntry, 0f, 100f);
            
            maxMovementDistanceEntry = Config.Bind("General", "maxMovementDistance", 15f,
                "The maximum distance that hazard can walk to. No need to restart the game :)");
            CreateFloatConfig(maxMovementDistanceEntry, 0f, 100f);
            
            minMovementDelayEntry = Config.Bind("General", "minMovementDelay", 10f,
                "The minimum delay between each hazard movement. No need to restart the game :)");
            CreateFloatConfig(minMovementDelayEntry, 0f, 100f);
            
            maxMovementDelayEntry = Config.Bind("General", "maxMovementDelay", 30f,
                "The maximum delay between each hazard movement. No need to restart the game :)");
            CreateFloatConfig(maxMovementDelayEntry, 1f, 300f);
            
            //LANDMINE
            
            landmineEnabledEntry = Config.Bind("LandMine", "landmineEnabled", true,
                "Enable landmines to move. No need to restart the game :)");
            CreateBoolConfig(landmineEnabledEntry);
            
            landmineSpeedEntry = Config.Bind("LandMine", "landmineSpeed", 4f,
                "Landmines movement speed. No need to restart the game :)");
            CreateFloatConfig(landmineSpeedEntry, 1f, 100f);
            
            //TURRET
            
            turretEnabledEntry = Config.Bind("Turret", "turretEnabled", true,
                "Enable turrets to move. No need to restart the game :)");
            CreateBoolConfig(turretEnabledEntry);
            
            turretSpeedEntry = Config.Bind("Turret", "turretSpeed", 4f,
                "Turrets movement speed . No need to restart the game :)");
            CreateFloatConfig(turretSpeedEntry, 1f, 100f);
            
            //SPIKETRAP
            
            spikeTrapEnabledEntry = Config.Bind("SpikeTrap", "spikeTrapEnabled", true,
                "Enable spike traps to move. No need to restart the game :)");
            CreateBoolConfig(spikeTrapEnabledEntry);
            
            spikeTrapSpeedEntry = Config.Bind("SpikeTrap", "spikeTrapSpeed", 4f,
                "Spike traps movement speed. No need to restart the game :)");
            CreateFloatConfig(spikeTrapSpeedEntry, 1f, 100f);
            
            //DEV
            enableDevLogsEntry = Config.Bind("Dev", "enableLogs", false,
                "Enable dev logs. No need to restart the game :)");
            CreateBoolConfig(enableDevLogsEntry);

            Harmony.CreateAndPatchAll(typeof(LandminePatch));
            Harmony.CreateAndPatchAll(typeof(SpikeRoofTrapPatch));
            Harmony.CreateAndPatchAll(typeof(TurretPatch));
            Harmony.CreateAndPatchAll(typeof(RoundManagerPatch));
            Logger.LogInfo($"MapHazardsMoves is patched!");
        }

        public float GetNewTimer()
        {
            return Random.Range(minMovementDelayEntry.Value, maxMovementDelayEntry.Value);
        }

        public Vector3 GetNewPos(Vector3 pos)
        {

            var min = minMovementDistanceEntry.Value;
            var max = maxMovementDistanceEntry.Value;

            var rx = Random.Range(min, max);
            var rz = Random.Range(min, max);

            if (Random.Range(0, 2) == 1) rx *= -1;
            if (Random.Range(0, 2) == 1) rz *= -1;

            return pos + new Vector3(rx, 0, rz);
        }
        
        private void CreateFloatConfig(ConfigEntry<float> configEntry, float min = 0f, float max = 30f)
        {
            var exampleSlider = new FloatSliderConfigItem(configEntry, new FloatSliderOptions
            {
                Min = min,
                Max = max,
                RequiresRestart = false
            });
            LethalConfigManager.AddConfigItem(exampleSlider);
        }
        
        private void CreateBoolConfig(ConfigEntry<bool> configEntry)
        {
            var exampleSlider = new BoolCheckBoxConfigItem(configEntry, new BoolCheckBoxOptions
            {
                RequiresRestart = false
            });
            LethalConfigManager.AddConfigItem(exampleSlider);
        }


    }
}