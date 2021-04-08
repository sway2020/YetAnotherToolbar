using ColossalFramework;
using ColossalFramework.UI;

namespace YetAnotherToolbar
{
    public class UIMainButton : UIButton
    {
        public UIDragHandle dragHandle;
        protected override void OnMouseDown(UIMouseEventParameter p)
        {
            if (p.buttons.IsFlagSet(UIMouseButton.Right))
            {
                UIQuickMenuPopUp.ShowAt(this);
            }
        }

    }
}