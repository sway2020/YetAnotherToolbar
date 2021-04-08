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
        public const string version = "0.4";
        public string Name => "Yet Another Toolbar [Test] " + version;
        public string Description => "Another toolbar mod";

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
                UICheckBox hideMainButton = (UICheckBox)group.AddCheckbox("Hide main button", Settings.hideMainButton, (b) =>
                {
                    Settings.hideMainButton = b;
                    XMLUtils.SaveSettings();
                    if (YetAnotherToolbar.instance?.mainButton != null)
                    {
                        YetAnotherToolbar.instance.mainButton.isVisible = !Settings.hideMainButton;
                    }
                });
                group.AddSpace(10);

                UIButton mainButtonPositionReset = (UIButton)group.AddButton("Reset main button position", () =>
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

                // Disable update notice
                UICheckBox disableUpdateNotice = (UICheckBox)group.AddCheckbox("Disable future update notice", Settings.disableUpdateNotice, (b) =>
                {
                    Settings.disableUpdateNotice = b;
                    XMLUtils.SaveSettings();
                });
                group.AddSpace(10);

                // show path to YetAnotherToolbarConfig.xml
                string path = Path.Combine(DataLocation.executableDirectory, "YetAnotherToolbarConfig.xml");
                UITextField ConfigFilePath = (UITextField)group.AddTextfield("Configuration file path", path, _ => { }, _ => { });
                ConfigFilePath.width = panel.width - 30;

                // from aubergine10's AutoRepair
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    group.AddButton("Open in file explorer", () => System.Diagnostics.Process.Start("explorer.exe", "/select," + path));
                }

                // shortcut keys
                panel.gameObject.AddComponent<ModeToggleKeyMapping>();
                
                /*
                panel.gameObject.AddComponent<AllKeyMapping>();
                panel.gameObject.AddComponent<NetworkKeyMapping>();
                panel.gameObject.AddComponent<PloppableKeyMapping>();
                panel.gameObject.AddComponent<GrowableKeyMapping>();
                panel.gameObject.AddComponent<RicoKeyMapping>();
                panel.gameObject.AddComponent<GrwbRicoKeyMapping>();
                panel.gameObject.AddComponent<PropKeyMapping>();
                panel.gameObject.AddComponent<DecalKeyMapping>();
                panel.gameObject.AddComponent<TreeKeyMapping>();
                panel.gameObject.AddComponent<RandomSelectionKeyMapping>();
                group.AddSpace(10);
                */

            }
            catch (Exception e)
            {
                Debugging.Message("OnSettingsUI failed");
                Debugging.LogException(e);
            }
        }
    }
}
