using UnifiedUI.Helpers;

namespace YetAnotherToolbar
{
    internal static class UUIIntegration
    {
        public static void AttachMainButton()
        {
            UUIHelpers.AttachAlien(YetAnotherToolbar.instance.mainButton, null);
        }

        public static void DetachMainButton()
        {
            YetAnotherToolbar.instance.mainButton.Hide();
            UUIHelpers.Destroy(YetAnotherToolbar.instance.mainButton);
            YetAnotherToolbar.instance.mainButton = null;
            YetAnotherToolbar.instance.mainButton = YetAnotherToolbar.instance.CreatMainButton();
        }
    }
}
