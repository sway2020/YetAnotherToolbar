// Originally written by algernon for Find It 2.
// Modified by sway

using System;
using UnityEngine;
using ICities;
using ColossalFramework.UI;

namespace YetAnotherToolbar
{
    public class UIThreading : ThreadingExtensionBase
    {
        // Flags.
        private bool _processed = false;

        /// <summary>
        /// Look for keypress to open GUI.
        /// </summary>
        /// <param name="realTimeDelta"></param>
        /// <param name="simulationTimeDelta"></param>
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            // Local references.
            UIButton mainButton = YetAnotherToolbar.instance?.mainButton;
            if (mainButton == null) return;

            KeyCode modeToggleKey = (KeyCode)(Settings.modeToggleKey.keyCode);
            KeyCode quickMenuKey = (KeyCode)(Settings.quickMenuKey.keyCode);
            KeyCode hideMenuKey = (KeyCode)(Settings.hideMenuKey.keyCode);

            // Null checks for safety.
            // Check modifier keys according to settings.
            bool altPressed = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.AltGr);
            bool ctrlPressed = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            bool shiftPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

            // Has hotkey been pressed?
            if (modeToggleKey != KeyCode.None && Input.GetKey(modeToggleKey) && CheckHotkey(Settings.modeToggleKey, altPressed, ctrlPressed, shiftPressed))
            {
                ProcessPressedKey(0);
            }
            else if (quickMenuKey != KeyCode.None && Input.GetKey(quickMenuKey) && CheckHotkey(Settings.quickMenuKey, altPressed, ctrlPressed, shiftPressed))
            {
                ProcessPressedKey(1);
            }
            else if (quickMenuKey != KeyCode.None && Input.GetKey(hideMenuKey) && CheckHotkey(Settings.hideMenuKey, altPressed, ctrlPressed, shiftPressed))
            {
                ProcessPressedKey(2);
            }
            else
            {
                // Relevant keys aren't pressed anymore; this keystroke is over, so reset and continue.
                _processed = false;
            }
        }

        public bool CheckHotkey(KeyBinding keyBinding, bool altPressed, bool ctrlPressed, bool shiftPressed)
        {
            if ((altPressed != keyBinding.alt) || (ctrlPressed != keyBinding.control) || (shiftPressed != keyBinding.shift)) return false;
            return true;
        }

        public void ProcessPressedKey(int index)
        {
            // Cancel if key input is already queued for processing.
            if (_processed) return;

            _processed = true;
            try
            {
                if (index == 0)
                {
                    if (Settings.expanded)
                    {
                        Settings.expanded = false;
                        XMLUtils.SaveSettings();
                        YetAnotherToolbar.instance.Collapse();
                        YetAnotherToolbar.instance.mainButton.normalFgSprite = "Expand";
                    }
                    else
                    {
                        Settings.expanded = true;
                        XMLUtils.SaveSettings();
                        YetAnotherToolbar.instance.Expand();
                        YetAnotherToolbar.instance.mainButton.normalFgSprite = "Collapse";
                    }
                    
                }
                else if (index == 1)
                {
                    UIQuickMenuPopUp.ShowAt(YetAnotherToolbar.instance.mainButton);
                }
                else if (index == 2)
                {
                    YetAnotherToolbar.instance.ToggleMenuVisibility();
                }

            }
            catch (Exception e)
            {
                Debugging.LogException(e);
            }
        }

    }
}