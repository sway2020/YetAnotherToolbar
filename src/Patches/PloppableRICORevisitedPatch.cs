
using HarmonyLib;
using System;

namespace YetAnotherToolbar
{
   // Harmony manual patch for Ploppable RICO Revisited to fix panel scaling issue
    public static class DrawPloppablePanelPatch
    {
        public static bool ApplyPatch(Harmony harmonyInstance)
        {
            try
            {
                Type ploppableToolType = Type.GetType("PloppableRICO.PloppableTool, ploppablerico", false);
                if (ploppableToolType != null)
                {
                    var original = ploppableToolType.GetMethod("TabClicked");
                    if (original == null) return false;
                    var prefix = typeof(DrawPloppablePanelPatch).GetMethod("Prefix");
                    var postfix = typeof(DrawPloppablePanelPatch).GetMethod("Postfix");
                    harmonyInstance.Patch(original, new HarmonyMethod(prefix), new HarmonyMethod(postfix));
                    return true;
                }
                else
                {
                    Debugging.Message($"Found enabled mod: ploppablerico. Yet Another Toolbar scale patch failed");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debugging.Message($"Found enabled mod: ploppablerico. Yet Another Toolbar scale patch failed. {ex.Message}");
                return false;
            }
        }

        public static void Prefix()
        {
            YetAnotherToolbar.instance.ResetScale();
        }

        public static void Postfix()
        {
            YetAnotherToolbar.instance.RestoreScale();
        }
    }

}
