using ColossalFramework;
using ColossalFramework.UI;

namespace YetAnotherToolbar
{
    public class UIMainButton : UIButton
    {
        protected override void OnMouseDown(UIMouseEventParameter p)
        {
            if (p.buttons.IsFlagSet(UIMouseButton.Right))
            {
                UIQuickMenuPopUp.ShowAt(this);
            }
        }
    }
}