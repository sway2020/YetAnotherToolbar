using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace YetAnotherToolbar
{
    public class UIMainButton : UIButton
    {
        private Vector3 deltaPosition;
        protected override void OnMouseDown(UIMouseEventParameter p)
        {
            if (p.buttons.IsFlagSet(UIMouseButton.Right))
            {
                UIQuickMenuPopUp.ShowAt(this);

                Vector3 mousePosition = Input.mousePosition;
                mousePosition.y = m_OwnerView.fixedHeight - mousePosition.y;
                deltaPosition = absolutePosition - mousePosition;
                BringToFront();
            }
        }

        protected override void OnMouseMove(UIMouseEventParameter p)
        {
            if (p.buttons.IsFlagSet(UIMouseButton.Right))
            {
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.y = m_OwnerView.fixedHeight - mousePosition.y;

                absolutePosition = mousePosition + deltaPosition;
                UIView view = UIView.GetAView();
                Vector2 screenResolution = view.GetScreenResolution();
                Settings.mainButtonX = absolutePosition.x * 1920f / screenResolution.x;
                Settings.mainButtonY = absolutePosition.y * 1080f / screenResolution.y;
                XMLUtils.SaveSettings();
            }
        }
    }
}