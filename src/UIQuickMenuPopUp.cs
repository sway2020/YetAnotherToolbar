using UnityEngine;
using ColossalFramework.UI;

namespace YetAnotherToolbar
{
    public class UIQuickMenuPopUp : UIPanel
    {
        public static UIQuickMenuPopUp instance;
        private const float spacing = 5f;

        private UILabel numOfRowLabel;
        private UILabel numOfRowValueLabel;
        private UISlider numOfRowSlider;

        private UILabel numOfColLabel;
        private UILabel numOfColValueLabel;
        private UISlider numOfColSlider;

        private UILabel scaleLabel;
        private UILabel scaleValueLabel;
        private UISlider scaleSlider;

        private UILabel horizonOffsetLabel;
        private UILabel horizonOffsetValueLabel;
        private UISlider horizonOffsetSlider;

        private UILabel verticalOffsetLabel;
        private UILabel verticalOffsetValueLabel;
        private UISlider verticalOffsetSlider;

        private UILabel backgroundLabel;
        private UIDropDown backgroundDropdown;

        public override void Start()
        {
            name = "YetAnotherToolbar_UIQuickMenuPopUp";
            atlas = SamsamTS.UIUtils.GetAtlas("Ingame");
            backgroundSprite = "GenericPanelWhite";
            size = new Vector2(430, 400);
            instance = this;

            UILabel title = AddUIComponent<UILabel>();
            title.text = Translations.Translate("YAT_QM_TIT");
            title.textColor = new Color32(0, 0, 0, 255);
            title.relativePosition = new Vector3(spacing * 3, spacing * 3);

            UIButton close = AddUIComponent<UIButton>();
            close.size = new Vector2(30f, 30f);
            close.text = "X";
            close.textScale = 0.9f;
            close.textColor = new Color32(0, 0, 0, 255);
            close.focusedTextColor = new Color32(0, 0, 0, 255);
            close.hoveredTextColor = new Color32(109, 109, 109, 255);
            close.pressedTextColor = new Color32(128, 128, 128, 102);
            close.textPadding = new RectOffset(8, 8, 8, 8);
            close.canFocus = false;
            close.playAudioEvents = true;
            close.relativePosition = new Vector3(width - close.width, 0);
            close.eventClicked += (c, p) => Close();

            close.Focus();

            numOfRowLabel = AddUIComponent<UILabel>();
            numOfRowLabel.text = Translations.Translate("YAT_QM_ROW");
            numOfRowLabel.textScale = 0.8f;
            numOfRowLabel.textColor = new Color32(0, 0, 0, 255);
            numOfRowLabel.relativePosition = new Vector3(title.relativePosition.x, title.relativePosition.y + title.height + 20);

            numOfRowSlider = SamsamTS.UIUtils.CreateSlider(this, Settings.numOfRows, 2.0f, 6.0f, 1.0f);
            numOfRowSlider.relativePosition = new Vector3(numOfRowLabel.relativePosition.x, numOfRowLabel.relativePosition.y + numOfRowLabel.height + 5);
            numOfRowSlider.eventValueChanged += (c, p) =>
            {
                Settings.numOfRows = (int)numOfRowSlider.value;
                XMLUtils.SaveSettings();
                numOfRowValueLabel.text = $"{Settings.numOfRows}";

                if (Settings.expanded)
                {
                    YetAnotherToolbar.instance.Expand();
                }
                else
                {
                    YetAnotherToolbar.instance.Collapse();
                }
            };

            numOfRowValueLabel = AddUIComponent<UILabel>();
            numOfRowValueLabel.text = $"{Settings.numOfRows}";
            numOfRowValueLabel.textScale = 0.8f;
            numOfRowValueLabel.textColor = new Color32(0, 0, 0, 255);
            numOfRowValueLabel.relativePosition = new Vector3(numOfRowSlider.relativePosition.x + numOfRowSlider.width + 10, numOfRowSlider.relativePosition.y);


            numOfColLabel = AddUIComponent<UILabel>();
            numOfColLabel.text = Translations.Translate("YAT_QM_COL");
            numOfColLabel.textScale = 0.8f;
            numOfColLabel.textColor = new Color32(0, 0, 0, 255);
            numOfColLabel.relativePosition = new Vector3(title.relativePosition.x, numOfRowValueLabel.relativePosition.y + numOfRowValueLabel.height + 20);

            numOfColSlider = SamsamTS.UIUtils.CreateSlider(this, Settings.numOfCols, 7.0f, 25.0f, 1.0f);
            numOfColSlider.relativePosition = new Vector3(numOfColLabel.relativePosition.x, numOfColLabel.relativePosition.y + numOfColLabel.height + 5);
            numOfColSlider.eventValueChanged += (c, p) =>
            {
                Settings.numOfCols = (int)numOfColSlider.value;
                XMLUtils.SaveSettings();
                numOfColValueLabel.text = $"{Settings.numOfCols}";

                if (Settings.expanded)
                {
                    YetAnotherToolbar.instance.Expand();
                }
                else
                {
                    YetAnotherToolbar.instance.Collapse();
                }
            };

            numOfColValueLabel = AddUIComponent<UILabel>();
            numOfColValueLabel.text = $"{Settings.numOfCols}";
            numOfColValueLabel.textScale = 0.8f;
            numOfColValueLabel.textColor = new Color32(0, 0, 0, 255);
            numOfColValueLabel.relativePosition = new Vector3(numOfColSlider.relativePosition.x + numOfColSlider.width + 10, numOfColSlider.relativePosition.y);


            scaleLabel = AddUIComponent<UILabel>();
            scaleLabel.text = Translations.Translate("YAT_QM_SCL");
            scaleLabel.textScale = 0.8f;
            scaleLabel.textColor = new Color32(0, 0, 0, 255);
            scaleLabel.relativePosition = new Vector3(title.relativePosition.x, numOfColValueLabel.relativePosition.y + numOfColValueLabel.height + 20);

            scaleSlider = SamsamTS.UIUtils.CreateSlider(this, Settings.toolbarScale, 0.5f, 1.5f, 0.05f);
            scaleSlider.relativePosition = new Vector3(scaleLabel.relativePosition.x, scaleLabel.relativePosition.y + scaleLabel.height + 5);
            scaleSlider.eventValueChanged += (c, p) =>
            {
                Settings.toolbarScale = scaleSlider.value;
                XMLUtils.SaveSettings();
                scaleValueLabel.text = $"{Settings.toolbarScale * 100} %";
                YetAnotherToolbar.instance.UpdateScale(Settings.toolbarScale);
                YetAnotherToolbar.instance.UpdatePanelPosition();
            };

            scaleValueLabel = AddUIComponent<UILabel>();
            scaleValueLabel.text = $"{Settings.toolbarScale * 100} %";
            scaleValueLabel.textScale = 0.8f;
            scaleValueLabel.textColor = new Color32(0, 0, 0, 255);
            scaleValueLabel.relativePosition = new Vector3(scaleSlider.relativePosition.x + scaleSlider.width + 10, scaleSlider.relativePosition.y);


            horizonOffsetLabel = AddUIComponent<UILabel>();
            horizonOffsetLabel.text = Translations.Translate("YAT_QM_HOR");
            horizonOffsetLabel.textScale = 0.8f;
            horizonOffsetLabel.textColor = new Color32(0, 0, 0, 255);
            horizonOffsetLabel.relativePosition = new Vector3(title.relativePosition.x, scaleSlider.relativePosition.y + scaleSlider.height + 20);

            horizonOffsetSlider = SamsamTS.UIUtils.CreateSlider(this, Settings.horizontalOffset, -1000.0f, 1000f, 10.0f);
            horizonOffsetSlider.relativePosition = new Vector3(horizonOffsetLabel.relativePosition.x, horizonOffsetLabel.relativePosition.y + horizonOffsetLabel.height + 5);
            horizonOffsetSlider.eventValueChanged += (c, p) =>
            {
                Settings.horizontalOffset = (int)horizonOffsetSlider.value;
                XMLUtils.SaveSettings();
                horizonOffsetValueLabel.text = $"{Settings.horizontalOffset}";
                YetAnotherToolbar.instance.UpdatePanelPosition();
            };

            horizonOffsetValueLabel = AddUIComponent<UILabel>();
            horizonOffsetValueLabel.text = $"{Settings.horizontalOffset}";
            horizonOffsetValueLabel.textScale = 0.8f;
            horizonOffsetValueLabel.textColor = new Color32(0, 0, 0, 255);
            horizonOffsetValueLabel.relativePosition = new Vector3(horizonOffsetSlider.relativePosition.x + horizonOffsetSlider.width + 10, horizonOffsetSlider.relativePosition.y);


            verticalOffsetLabel = AddUIComponent<UILabel>();
            verticalOffsetLabel.text = Translations.Translate("YAT_QM_VER");
            verticalOffsetLabel.textScale = 0.8f;
            verticalOffsetLabel.textColor = new Color32(0, 0, 0, 255);
            verticalOffsetLabel.relativePosition = new Vector3(title.relativePosition.x, horizonOffsetValueLabel.relativePosition.y + horizonOffsetValueLabel.height + 20);

            verticalOffsetSlider = SamsamTS.UIUtils.CreateSlider(this, Settings.verticalOffset, -1000.0f, 1000f, 10.0f);
            verticalOffsetSlider.relativePosition = new Vector3(verticalOffsetLabel.relativePosition.x, verticalOffsetLabel.relativePosition.y + verticalOffsetLabel.height + 5);
            verticalOffsetSlider.eventValueChanged += (c, p) =>
            {
                Settings.verticalOffset = (int)verticalOffsetSlider.value;
                XMLUtils.SaveSettings();
                verticalOffsetValueLabel.text = $"{Settings.verticalOffset}";
                YetAnotherToolbar.instance.UpdatePanelPosition();
            };

            verticalOffsetValueLabel = AddUIComponent<UILabel>();
            verticalOffsetValueLabel.text = $"{Settings.verticalOffset}";
            verticalOffsetValueLabel.textScale = 0.8f;
            verticalOffsetValueLabel.textColor = new Color32(0, 0, 0, 255);
            verticalOffsetValueLabel.relativePosition = new Vector3(verticalOffsetSlider.relativePosition.x + verticalOffsetSlider.width + 10, verticalOffsetSlider.relativePosition.y);


            backgroundLabel = AddUIComponent<UILabel>();
            backgroundLabel.text = Translations.Translate("YAT_QM_BAC");
            backgroundLabel.textScale = 0.8f;
            backgroundLabel.textColor = new Color32(0, 0, 0, 255);
            backgroundLabel.relativePosition = new Vector3(title.relativePosition.x, verticalOffsetValueLabel.relativePosition.y + verticalOffsetValueLabel.height + 20);

            backgroundDropdown = SamsamTS.UIUtils.CreateDropDown(this);
            backgroundDropdown.normalBgSprite = "TextFieldPanelHovered";
            backgroundDropdown.size = new Vector2(250, 30);
            backgroundDropdown.listHeight = 270;
            backgroundDropdown.itemHeight = 30;
            backgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 100%");
            backgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 75%");
            backgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 50%");
            backgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 25%");
            backgroundDropdown.AddItem(Translations.Translate("YAT_QM_TRS"));
            backgroundDropdown.AddItem(Translations.Translate("YAT_QM_LGH") + " - 100%");
            backgroundDropdown.AddItem(Translations.Translate("YAT_QM_LGH") + " - 75%");
            backgroundDropdown.AddItem(Translations.Translate("YAT_QM_LGH") + " - 50%");
            backgroundDropdown.AddItem(Translations.Translate("YAT_QM_LGH") + " - 25%");

            backgroundDropdown.selectedIndex = Settings.backgroundOption;
            backgroundDropdown.relativePosition = new Vector3(title.relativePosition.x, backgroundLabel.relativePosition.y + backgroundLabel.height + 5);
            backgroundDropdown.eventSelectedIndexChanged += (c, p) =>
            {
                Settings.backgroundOption = backgroundDropdown.selectedIndex;
                XMLUtils.SaveSettings();
                YetAnotherToolbar.instance.UpdateBackground();
            };
        }

        private static void Close()
        {
            if (instance != null)
            {
                UIView.PopModal();
                instance.isVisible = false;
                Destroy(instance.gameObject);
                instance = null;
            }
        }

        protected override void OnKeyDown(UIKeyEventParameter p)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                p.Use();
                Close();
            }

            base.OnKeyDown(p);
        }

        public static void ShowAt(UIComponent component)
        {
            if (instance == null)
            {
                instance = UIView.GetAView().AddUIComponent(typeof(UIQuickMenuPopUp)) as UIQuickMenuPopUp;
                instance.relativePosition += new Vector3(-200, -400);

                UIView.PushModal(instance);
            }
            instance.Show(true);
        }

    }
}