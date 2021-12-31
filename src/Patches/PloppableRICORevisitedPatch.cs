
using HarmonyLib;
using PloppableRICO;

namespace YetAnotherToolbar
{
   // Patch RICO Revisited to fix scaling issue
   //[HarmonyPatch(typeof(PloppableRICO.PloppableTool))]
   //[HarmonyPatch("TabClicked")]
   //[HarmonyPatch(new Type[] { typeof(int), typeof(UISprite) })]
    public static class DrawPloppablePanelPatch
    {
        public static bool Patch(Harmony harmonyInstance)
        {
            var original = typeof(PloppableRICO.PloppableTool).GetMethod("TabClicked");
            if (original == null) return false;

            var prefix = typeof(DrawPloppablePanelPatch).GetMethod("Prefix");
            var postfix = typeof(DrawPloppablePanelPatch).GetMethod("Postfix");

            harmonyInstance.Patch(original, new HarmonyMethod(prefix), new HarmonyMethod(postfix));
            return true;
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
