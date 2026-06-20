using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;

namespace NowYouSleep
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        private Harmony _harmony;
        private ConfigEntry<bool> _modEnabled;
        private ConfigEntry<bool> _debugModeEnabled;

        internal const string PluginGuid = "hex.nowyousleep";
        internal const string PluginName = "NowYouSleep";
        internal const string PluginVersion = "1.0.2";

        internal static Plugin Instance { get; private set; }
        internal static ManualLogSource Log { get; private set; }

        internal bool IsNowYouSleepEnabled => _modEnabled != null && _modEnabled.Value;
        internal bool IsDebugModeEnabled => _debugModeEnabled != null && _debugModeEnabled.Value;

        private void Awake()
        {
            Instance = this;
            Log = Logger;

            _modEnabled = Config.Bind(
                "General",
                "Enabled",
                true,
                $"Whether {PluginName} is enabled.");

            _debugModeEnabled = Config.Bind(
                "General",
                "DebugMode",
                false,
                $"Whether to enable debug logging for {PluginName}.");

            _harmony = new Harmony(PluginGuid);
            _harmony.PatchAll();

            Log.LogInfo($"{PluginName} v{PluginVersion} loaded.");
        }

        private void OnDestroy()
        {
            Log.LogInfo($"{PluginName} v{PluginVersion} unloaded.");

            _harmony?.UnpatchSelf();
            _harmony = null;
            Instance = null;
            Log = null;
        }

        internal void LogDebug(string message)
        {
            if (IsDebugModeEnabled)
            {
                Log.LogInfo($"[Debug] {message}");
            }
        }
    }

    [HarmonyPatch(typeof(Game), nameof(Game.EverybodyIsTryingToSleep))]
    internal static class GameEverybodyIsTryingToSleepPatch
    {
        [HarmonyPrefix]
        private static bool Prefix(ref bool __result)
        {
            if (Plugin.Instance == null || !Plugin.Instance.IsNowYouSleepEnabled)
            {
                return true;
            }

            if (ZNet.instance == null)
            {
                Plugin.Instance.LogDebug("ZNet.instance is null. Falling back to vanilla sleep check.");
                return true;
            }

            if (!ZNet.instance.IsServer())
            {
                Plugin.Instance.LogDebug("Not running on server. Falling back to vanilla sleep check.");
                return true;
            }

            List<ZDO> allCharacterZdos = ZNet.instance.GetAllCharacterZDOS();

            if (allCharacterZdos == null || allCharacterZdos.Count == 0)
            {
                return true;
            }

            foreach (ZDO zdo in allCharacterZdos)
            {
                if (zdo.GetBool(ZDOVars.s_inBed, false))
                {
                    Plugin.Instance.LogDebug("At least one player is trying to sleep. Allowing sleep skip.");

                    __result = true;
                    return false;
                }
            }

            return true;
        }
    }
}