using UnityEngine;
using ColossalFramework.UI;

namespace YetAnotherToolbar
{
    public class UIQuickMenuPopUp : UIPanel
    {
        public static UIQuickMenuPopUp instance;
        private const float spacing = 5f;

        private UILabel numOfRowLabel;
        private UISlider numOfRowSlider;
        private UITextField numOfRowValueTextField;

        private UILabel numOfColLabel;
        private UISlider numOfColSlider;
        private UITextField numOfColValueTextField;

        private UILabel scaleLabel;
        private UILabel scaleValueLabel;
        private UISlider scaleSlider;

        private UILabel horizontalOffsetLabel;
        private UISlider horizontalOffsetSlider;
        private UITextField horizontalOffsetValueTextField;

        private UILabel verticalOffsetLabel;
        private UISlider verticalOffsetSlider;
        private UITextField verticalOffsetValueTextField;

        private UILabel backgroundLabel;
        private UIDropDown backgroundDropdown;

        private UILabel thumbnailBarBackgroundLabel;
        private UIDropDown thumbnailBarbackgroundDropdown;

        private UILabel tsBarBackgroundLabel;
        private UIDropDown tsBarBackgroundDropdown;

        private UILabel infoPanelBackgroundLabel;
        private UIDropDown infoPanelBackgroundDropdown;

        public override void Start()
        {
            name = "YetAnotherToolbar_UIQuickMenuPopUp";
            atlas = SamsamTS.UIUtils.GetAtlas("Ingame");
            backgroundSprite = "GenericPanelWhite";
            size = new Vector2(460, 530);
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

            numOfRowSlider = SamsamTS.UIUtils.CreateSlider(this, Settings.numOfRows, 2.0f, 10.0f, 1.0f);
            numOfRowSlider.relativePosition = new Vector3(numOfRowLabel.relativePosition.x, numOfRowLabel.relativePosition.y + numOfRowLabel.height + 5);
            numOfRowSlider.eventValueChanged += (c, p) =>
            {
                if (numOfRowSlider.value == Settings.numOfRows) return;
                numOfRowValueTextField.text = $"{numOfRowSlider.value}";
            };

            numOfRowValueTextField = SamsamTS.UIUtils.CreateTextField(this);
            numOfRowValueTextField.text = $"{Settings.numOfRows}";
            numOfRowValueTextField.width = 70;
            numOfRowValueTextField.relativePosition = new Vector3(numOfRowSlider.relativePosition.x + numOfRowSlider.width + 15, numOfRowSlider.relativePosition.y - 10);
            numOfRowValueTextField.eventTextChanged += (c, p) =>
            {
                if (!int.TryParse(numOfRowValueTextField.text, out int newValue)) return;
                if (newValue < 2 || newValue > 10) return; // too many rows will cause performance issues or even crash the game
                Settings.numOfRows = newValue;
                XMLUtils.SaveSettings();
                numOfRowSlider.value = newValue;
                if (!Settings.expanded)
                {
                    Settings.expanded = true;
                    XMLUtils.SaveSettings();
                    YetAnotherToolbar.instance.mainButton.normalFgSprite = "Collapse";
                }
                YetAnotherToolbar.instance.Expand();
            };

            numOfColLabel = AddUIComponent<UILabel>();
            numOfColLabel.text = Translations.Translate("YAT_QM_COL");
            numOfColLabel.textScale = 0.8f;
            numOfColLabel.textColor = new Color32(0, 0, 0, 255);
            numOfColLabel.relativePosition = new Vector3(title.relativePosition.x, numOfRowLabel.relativePosition.y + numOfRowLabel.height + 30);

            numOfColSlider = SamsamTS.UIUtils.CreateSlider(this, Settings.numOfCols, 7.0f, 25.0f, 1.0f);
            numOfColSlider.relativePosition = new Vector3(numOfColLabel.relativePosition.x, numOfColLabel.relativePosition.y + numOfColLabel.height + 5);
            numOfColSlider.eventValueChanged += (c, p) =>
            {
                if (numOfColSlider.value == Settings.numOfCols) return;
                numOfColValueTextField.text = $"{numOfColSlider.value}";
            };

            numOfColValueTextField = SamsamTS.UIUtils.CreateTextField(this);
            numOfColValueTextField.text = $"{Settings.numOfCols}";
            numOfColValueTextField.width = 70;
            numOfColValueTextField.relativePosition = new Vector3(numOfColSlider.relativePosition.x + numOfColSlider.width + 15, numOfColSlider.relativePosition.y - 10);
            numOfColValueTextField.eventTextChanged += (c, p) =>
            {
                if (!int.TryParse(numOfColValueTextField.text, out int newValue)) return;
                if (newValue < 7 || newValue > 25) return; // too many cols will cause performance issues or even crash the game

                Settings.numOfCols = newValue;
                XMLUtils.SaveSettings();
                numOfColSlider.value = newValue;
                if (Settings.expanded)
                {
                    YetAnotherToolbar.instance.Expand();
                }
                else
                {
                    YetAnotherToolbar.instance.Collapse();
                }
            };

            scaleLabel = AddUIComponent<UILabel>();
            scaleLabel.text = Translations.Translate("YAT_QM_SCL");
            scaleLabel.textScale = 0.8f;
            scaleLabel.textColor = new Color32(0, 0, 0, 255);
            scaleLabel.relativePosition = new Vector3(title.relativePosition.x, numOfColLabel.relativePosition.y + numOfColLabel.height + 30);

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
            scaleValueLabel.textScale = 1.0f;
            scaleValueLabel.textColor = new Color32(0, 0, 0, 255);
            scaleValueLabel.relativePosition = new Vector3(scaleSlider.relativePosition.x + scaleSlider.width + 30, scaleSlider.relativePosition.y);

            horizontalOffsetLabel = AddUIComponent<UILabel>();
            horizontalOffsetLabel.text = Translations.Translate("YAT_QM_HOR");
            horizontalOffsetLabel.textScale = 0.8f;
            horizontalOffsetLabel.textColor = new Color32(0, 0, 0, 255);
            horizontalOffsetLabel.relativePosition = new Vector3(title.relativePosition.x, scaleSlider.relativePosition.y + scaleSlider.height + 20);

            horizontalOffsetSlider = SamsamTS.UIUtils.CreateSlider(this, Settings.horizontalOffset, -2000.0f, 2000f, 10.0f);
            horizontalOffsetSlider.relativePosition = new Vector3(horizontalOffsetLabel.relativePosition.x, horizontalOffsetLabel.relativePosition.y + horizontalOffsetLabel.height + 5);
            horizontalOffsetSlider.eventValueChanged += (c, p) =>
            {
                if (horizontalOffsetSlider.value == Settings.horizontalOffset) return;
                horizontalOffsetValueTextField.text = $"{horizontalOffsetSlider.value}";
            };

            horizontalOffsetValueTextField = SamsamTS.UIUtils.CreateTextField(this);
            horizontalOffsetValueTextField.text = $"{Settings.horizontalOffset}";
            horizontalOffsetValueTextField.width = 70;
            horizontalOffsetValueTextField.relativePosition = new Vector3(horizontalOffsetSlider.relativePosition.x + horizontalOffsetSlider.width + 15, horizontalOffsetSlider.relativePosition.y - 10);
            horizontalOffsetValueTextField.eventTextChanged += (c, p) =>
            {
                if (!int.TryParse(horizontalOffsetValueTextField.text, out int newValue)) return;
                if (newValue < -2000 || newValue > 2000) return; // reasonable range
                Settings.horizontalOffset = newValue;
                XMLUtils.SaveSettings();
                horizontalOffsetSlider.value = newValue;
                YetAnotherToolbar.instance.UpdatePanelPosition();
            };

            verticalOffsetLabel = AddUIComponent<UILabel>();
            verticalOffsetLabel.text = Translations.Translate("YAT_QM_VER");
            verticalOffsetLabel.textScale = 0.8f;
            verticalOffsetLabel.textColor = new Color32(0, 0, 0, 255);
            verticalOffsetLabel.relativePosition = new Vector3(title.relativePosition.x, horizontalOffsetLabel.relativePosition.y + horizontalOffsetLabel.height + 30);

            verticalOffsetSlider = SamsamTS.UIUtils.CreateSlider(this, Settings.verticalOffset, -1500.0f, 1500f, 10.0f);
            verticalOffsetSlider.relativePosition = new Vector3(verticalOffsetLabel.relativePosition.x, verticalOffsetLabel.relativePosition.y + verticalOffsetLabel.height + 5);
            verticalOffsetSlider.eventValueChanged += (c, p) =>
            {
                if (verticalOffsetSlider.value == Settings.verticalOffset) return;
                verticalOffsetValueTextField.text = $"{verticalOffsetSlider.value}";
            };

            verticalOffsetValueTextField = SamsamTS.UIUtils.CreateTextField(this);
            verticalOffsetValueTextField.text = $"{Settings.verticalOffset}";
            verticalOffsetValueTextField.width = 70;
            verticalOffsetValueTextField.relativePosition = new Vector3(verticalOffsetSlider.relativePosition.x + verticalOffsetSlider.width + 15, verticalOffsetSlider.relativePosition.y - 10);
            verticalOffsetValueTextField.eventTextChanged += (c, p) =>
            {
                if (!int.TryParse(verticalOffsetValueTextField.text, out int newValue)) return;
                if (newValue < -1500 || newValue > 1500) return; // reasonable range
                Settings.verticalOffset = newValue;
                XMLUtils.SaveSettings();
                verticalOffsetSlider.value = newValue;
                YetAnotherToolbar.instance.UpdatePanelPosition();
            };

            backgroundLabel = AddUIComponent<UILabel>();
            backgroundLabel.text = Translations.Translate("YAT_QM_BAC");
            backgroundLabel.textScale = 0.8f;
            backgroundLabel.textColor = new Color32(0, 0, 0, 255);
            backgroundLabel.relativePosition = new Vector3(title.relativePosition.x, verticalOffsetLabel.relativePosition.y + verticalOffsetLabel.height + 30);

            backgroundDropdown = SamsamTS.UIUtils.CreateDropDown(this);
            backgroundDropdown.normalBgSprite = "TextFieldPanelHovered";
            backgroundDropdown.size = new Vector2(250, 30);
            backgroundDropdown.listHeight = 270;
            backgroundDropdown.itemHeight = 30;
            backgroundDropdown.AddItem("Original");
            backgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 100%");
            backgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 90%");
            backgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 80%");
            backgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 70%");
            backgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 60%");
            backgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 50%");
            backgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 25%");
            backgroundDropdown.AddItem(Translations.Translate("YAT_QM_TRS"));

            backgroundDropdown.selectedIndex = Settings.backgroundOption;
            backgroundDropdown.relativePosition = new Vector3(title.relativePosition.x, backgroundLabel.relativePosition.y + backgroundLabel.height + 5);
            backgroundDropdown.eventSelectedIndexChanged += (c, p) =>
            {
                Settings.backgroundOption = backgroundDropdown.selectedIndex;
                XMLUtils.SaveSettings();
                YetAnotherToolbar.instance.UpdateMainPanelBackground();
            };


            thumbnailBarBackgroundLabel = AddUIComponent<UILabel>();
            thumbnailBarBackgroundLabel.text = Translations.Translate("YAT_QM_THM");
            thumbnailBarBackgroundLabel.textScale = 0.8f;
            thumbnailBarBackgroundLabel.textColor = new Color32(0, 0, 0, 255);
            thumbnailBarBackgroundLabel.relativePosition = new Vector3(title.relativePosition.x, backgroundLabel.relativePosition.y + backgroundLabel.height + 50);

            thumbnailBarbackgroundDropdown = SamsamTS.UIUtils.CreateDropDown(this);
            thumbnailBarbackgroundDropdown.normalBgSprite = "TextFieldPanelHovered";
            thumbnailBarbackgroundDropdown.size = new Vector2(250, 30);
            thumbnailBarbackgroundDropdown.listHeight = 270;
            thumbnailBarbackgroundDropdown.itemHeight = 30;
            thumbnailBarbackgroundDropdown.AddItem("Original");
            thumbnailBarbackgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 100%");
            thumbnailBarbackgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 90%");
            thumbnailBarbackgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 80%");
            thumbnailBarbackgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 70%");
            thumbnailBarbackgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 60%");
            thumbnailBarbackgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 50%");
            thumbnailBarbackgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 25%");
            thumbnailBarbackgroundDropdown.AddItem(Translations.Translate("YAT_QM_TRS"));

            thumbnailBarbackgroundDropdown.selectedIndex = Settings.thumbnailBarBackgroundOption;
            thumbnailBarbackgroundDropdown.relativePosition = new Vector3(title.relativePosition.x, thumbnailBarBackgroundLabel.relativePosition.y + thumbnailBarBackgroundLabel.height + 5);
            thumbnailBarbackgroundDropdown.eventSelectedIndexChanged += (c, p) =>
            {
                Settings.thumbnailBarBackgroundOption = thumbnailBarbackgroundDropdown.selectedIndex;
                XMLUtils.SaveSettings();
                YetAnotherToolbar.instance.UpdateThumbnailBarBackground();
            };


            tsBarBackgroundLabel = AddUIComponent<UILabel>();
            tsBarBackgroundLabel.text = Translations.Translate("YAT_QM_TSB");
            tsBarBackgroundLabel.textScale = 0.8f;
            tsBarBackgroundLabel.textColor = new Color32(0, 0, 0, 255);
            tsBarBackgroundLabel.relativePosition = new Vector3(title.relativePosition.x, thumbnailBarBackgroundLabel.relativePosition.y + thumbnailBarBackgroundLabel.height + 50);

            tsBarBackgroundDropdown = SamsamTS.UIUtils.CreateDropDown(this);
            tsBarBackgroundDropdown.normalBgSprite = "TextFieldPanelHovered";
            tsBarBackgroundDropdown.size = new Vector2(250, 30);
            tsBarBackgroundDropdown.listHeight = 270;
            tsBarBackgroundDropdown.itemHeight = 30;
            tsBarBackgroundDropdown.AddItem("Original");
            tsBarBackgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 100%");
            tsBarBackgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 90%");
            tsBarBackgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 80%");
            tsBarBackgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 70%");
            tsBarBackgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 60%");
            tsBarBackgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 50%");
            tsBarBackgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 25%");
            tsBarBackgroundDropdown.AddItem(Translations.Translate("YAT_QM_TRS"));

            tsBarBackgroundDropdown.selectedIndex = Settings.tsBarBackgroundOption;
            tsBarBackgroundDropdown.relativePosition = new Vector3(title.relativePosition.x, tsBarBackgroundLabel.relativePosition.y + tsBarBackgroundLabel.height + 5);
            tsBarBackgroundDropdown.eventSelectedIndexChanged += (c, p) =>
            {
                Settings.tsBarBackgroundOption = tsBarBackgroundDropdown.selectedIndex;
                XMLUtils.SaveSettings();
                YetAnotherToolbar.instance.UpdateTSBarBackground();
            };

            infoPanelBackgroundLabel = AddUIComponent<UILabel>();
            infoPanelBackgroundLabel.text = Translations.Translate("YAT_QM_IPB");
            infoPanelBackgroundLabel.textScale = 0.8f;
            infoPanelBackgroundLabel.textColor = new Color32(0, 0, 0, 255);
            infoPanelBackgroundLabel.relativePosition = new Vector3(title.relativePosition.x, tsBarBackgroundLabel.relativePosition.y + tsBarBackgroundLabel.height + 50);

            infoPanelBackgroundDropdown = SamsamTS.UIUtils.CreateDropDown(this);
            infoPanelBackgroundDropdown.normalBgSprite = "TextFieldPanelHovered";
            infoPanelBackgroundDropdown.size = new Vector2(250, 30);
            infoPanelBackgroundDropdown.listHeight = 270;
            infoPanelBackgroundDropdown.itemHeight = 30;
            infoPanelBackgroundDropdown.AddItem("Original");
            infoPanelBackgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 100%");
            infoPanelBackgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 90%");
            infoPanelBackgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 80%");
            infoPanelBackgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 70%");
            infoPanelBackgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 60%");
            infoPanelBackgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 50%");
            infoPanelBackgroundDropdown.AddItem(Translations.Translate("YAT_QM_DRK") + " - 25%");
            infoPanelBackgroundDropdown.AddItem(Translations.Translate("YAT_QM_TRS"));

            infoPanelBackgroundDropdown.selectedIndex = Settings.infoPanelBackgroundOption;
            infoPanelBackgroundDropdown.relativePosition = new Vector3(title.relativePosition.x, infoPanelBackgroundLabel.relativePosition.y + infoPanelBackgroundLabel.height + 5);
            infoPanelBackgroundDropdown.eventSelectedIndexChanged += (c, p) =>
            {
                Settings.infoPanelBackgroundOption = infoPanelBackgroundDropdown.selectedIndex;
                XMLUtils.SaveSettings();
                YetAnotherToolbar.instance.UpdateInfoPanelBackground();
            };
        }

        private static void Close()
        {
            if (instance != null)
            {
                // UIView.PopModal();
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

                // UIView.PushModal(instance);
            }
            instance.Show(true);
        }

        private Vector3 deltaPosition;
        protected override void OnMouseDown(UIMouseEventParameter p)
        {
            if (p.buttons.IsFlagSet(UIMouseButton.Right))
            {
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
            }
        }

    }
}