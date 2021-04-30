using ICities;
using System;
using ColossalFramework.UI;
using System.IO;
using ColossalFramework.IO;
using UnityEngine;

namespace YetAnotherToolbar
{
    public class ModInfo : IUserMod
    {
        public const string version = "0.7";
        public string Name => "Yet Another Toolbar [Test] " + version;
        public string Description
        {
            get { return Translations.Translate("YAT_DESC"); }
        }

        public const double updateNoticeDate = 20210413;
        public const string updateNotice =

            "- Custom panel background now also works in map & asset editors\n\n";

        public void OnEnabled()
        {
            XMLUtils.LoadSettings();
        }

        public void OnSettingsUI(UIHelperBase helper)
        {
            try
            {
                UIHelper group = helper.AddGroup(Name) as UIHelper;
                UIPanel panel = group.self as UIPanel;

                // Hide main button
                UICheckBox hideMainButton = (UICheckBox)group.AddCheckbox(Translations.Translate("YAT_SET_HMB"), Settings.hideMainButton, (b) =>
                {
                    Settings.hideMainButton = b;
                    XMLUtils.SaveSettings();
                    if (YetAnotherToolbar.instance?.mainButton != null)
                    {
                        YetAnotherToolbar.instance.mainButton.isVisible = !Settings.hideMainButton;
                    }
                });
                group.AddSpace(10);

                UIButton mainButtonPositionReset = (UIButton)group.AddButton(Translations.Translate("YAT_SET_HMBRST"), () =>
                {
                    Settings.mainButtonX = 538.0f;
                    Settings.mainButtonY = 947.0f;
                    XMLUtils.SaveSettings();
                    if (YetAnotherToolbar.instance?.mainButton != null)
                    {
                        YetAnotherToolbar.instance.mainButton.absolutePosition = new Vector3(Settings.mainButtonX, Settings.mainButtonY);
                    }
                });
                group.AddSpace(10);

                // Hide Advisor Button
                UICheckBox hideAdvisorButton = (UICheckBox)group.AddCheckbox(Translations.Translate("YAT_SET_HAB"), Settings.hideAdvisorButton, (b) =>
                {
                    Settings.hideAdvisorButton = b;
                    XMLUtils.SaveSettings();
                    if (YetAnotherToolbar.instance != null)
                    {
                        YetAnotherToolbar.instance.SetAdvisorButtonVisibility();
                    }
                });
                group.AddSpace(10);

                /*
                // Hide Filter Panels
                UICheckBox hideFilterPanels = (UICheckBox)group.AddCheckbox(Translations.Translate("YAT_SET_HFP"), Settings.hideFilterPanels, (b) =>
                {
                    Settings.hideFilterPanels = b;
                    XMLUtils.SaveSettings();
                    if (YetAnotherToolbar.instance != null)
                    {
                        YetAnotherToolbar.instance.hideFilterPanels = Settings.hideFilterPanels;
                        YetAnotherToolbar.instance.SetFilterPanelsVisibility();
                    }
                });
                group.AddSpace(10);
                */

                // Disable update notice
                UICheckBox disableUpdateNotice = (UICheckBox)group.AddCheckbox(Translations.Translate("YAT_SET_DUN"), Settings.disableUpdateNotice, (b) =>
                {
                    Settings.disableUpdateNotice = b;
                    XMLUtils.SaveSettings();
                });
                group.AddSpace(10);

                // languate settings
                UIDropDown languageDropDown = (UIDropDown)group.AddDropdown(Translations.Translate("TRN_CHOICE"), Translations.LanguageList, Translations.Index, (value) =>
                {
                    Translations.Index = value;
                    XMLUtils.SaveSettings();
                });

                languageDropDown.width = 300;
                group.AddSpace(10);

                // show path to YetAnotherToolbarConfig.xml
                string path = Path.Combine(DataLocation.executableDirectory, "YetAnotherToolbarConfig.xml");
                UITextField ConfigFilePath = (UITextField)group.AddTextfield(Translations.Translate("YAT_SET_CFP"), path, _ => { }, _ => { });
                ConfigFilePath.width = panel.width - 30;

                // from aubergine10's AutoRepair
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    group.AddButton(Translations.Translate("YAT_SET_OFE"), () => System.Diagnostics.Process.Start("explorer.exe", "/select," + path));
                }

                // shortcut keys
                panel.gameObject.AddComponent<ModeToggleKeyMapping>();
                panel.gameObject.AddComponent<QuickMenuKeyMapping>();
                group.AddSpace(10);

            }
            catch (Exception e)
            {
                Debugging.Message("OnSettingsUI failed");
                Debugging.LogException(e);
            }
        }
    }
}
