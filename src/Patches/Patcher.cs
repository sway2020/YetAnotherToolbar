
using HarmonyLib;
using CitiesHarmony.API;
using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;
using System;

namespace YetAnotherToolbar
{
    public static class Patcher
    {
        // Unique harmony identifier.
        private const string harmonyID = "com.github.sway2020.YetAnotherToolbar";

        // Flag.
        internal static bool Patched => _patched;
        private static bool _patched = false;

        /// <summary>
        /// Apply all Harmony patches.
        /// </summary>
        public static void PatchAll()
        {
            // Don't do anything if already patched.
            if (!_patched)
            {
                // Ensure Harmony is ready before patching.
                if (HarmonyHelper.IsHarmonyInstalled)
                {
                    Debugging.Message("deploying Harmony patches");

                    // Apply all annotated patches and update flag.
                    Harmony harmonyInstance = new Harmony(harmonyID);
                    harmonyInstance.PatchAll();
                    _patched = true;
                }
                else
                {
                    Debugging.Message("Harmony not ready");
                }
            }
        }


        public static void UnpatchAll()
        {
            // Only unapply if patches appplied.
            if (_patched)
            {
                Debugging.Message("reverting Harmony patches");

                // Unapply patches, but only with our HarmonyID.
                Harmony harmonyInstance = new Harmony(harmonyID);
                harmonyInstance.UnpatchAll(harmonyID);
                _patched = false;
            }
        }
    }

    [HarmonyPatch(typeof(CameraController))]
    [HarmonyPatch("UpdateFreeCamera")]
    [HarmonyPatch(new Type[] {})]
    internal static class LateUpdatePatch
    {
        [HarmonyPriority(Priority.Low)]
        private static void Postfix(Camera ___m_camera)
        {
            ___m_camera.rect = new Rect(0f, 0f, 1f, 1f);
        }
    }
}
