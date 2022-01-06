using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace YetAnotherToolbar
{
    public class UIMainButton : UIButton
    {
        public string expandSprite = "Expand";
        public string collapseSprite = "Collapse";
        public string expandInvertedSprite = "Expand-Inverted";
        public string collapseInvertedSprite = "Collapse-Inverted";

        public bool uuiMode = false;

        public void UseNormalSprites()
        {
            expandSprite = "Expand";
            collapseSprite = "Collapse";
            expandInvertedSprite = "Expand-Inverted";
            collapseInvertedSprite = "Collapse-Inverted";

            UpdateSprites();
        }

        public void UseUUISprites()
        {
            expandSprite = "Expand-UUI";
            collapseSprite = "Collapse-UUI";
            expandInvertedSprite = "Expand-Inverted-UUI";
            collapseInvertedSprite = "Collapse-Inverted-UUI";

            UpdateSprites();
        }

        private void UpdateSprites()
        {
            if (Settings.expanded && !YetAnotherToolbar.instance.hideMenuFlag) normalFgSprite = expandSprite;
            else if (!Settings.expanded && !YetAnotherToolbar.instance.hideMenuFlag) normalFgSprite = collapseSprite;
            else if (Settings.expanded && YetAnotherToolbar.instance.hideMenuFlag) normalFgSprite = expandInvertedSprite;
            else normalFgSprite = collapseInvertedSprite;
        }


        private Vector3 deltaPosition;
        protected override void OnMouseDown(UIMouseEventParameter p)
        {
            if (p.buttons.IsFlagSet(UIMouseButton.Right))
            {
                UIQuickMenuPopUp.ShowAt(this);

                if (!uuiMode)
                {
                    Vector3 mousePosition = Input.mousePosition;
                    mousePosition.y = m_OwnerView.fixedHeight - mousePosition.y;
                    deltaPosition = absolutePosition - mousePosition;
                    BringToFront();
                }
            }
        }

        protected override void OnMouseMove(UIMouseEventParameter p)
        {
            if (p.buttons.IsFlagSet(UIMouseButton.Right) && !uuiMode)
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