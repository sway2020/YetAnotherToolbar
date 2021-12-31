
using HarmonyLib;
using UnityEngine;
using System;

namespace YetAnotherToolbar
{
    // Get full screen rendered under the bottom panels
    [HarmonyPatch(typeof(CameraController))]
    [HarmonyPatch("UpdateFreeCamera")]
    [HarmonyPatch(new Type[] { })]
    internal static class UpdateFreeCameraPatch
    {
        [HarmonyPriority(Priority.Low)]
        private static void Postfix(Camera ___m_camera)
        {
            ___m_camera.rect = new Rect(0f, 0f, 1f, 1f);
        }
    }
}
