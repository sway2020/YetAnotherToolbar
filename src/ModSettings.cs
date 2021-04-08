// Originally written by algernon for Find It 2.
// Modified by sway
using System.Xml.Serialization;
using UnityEngine;

namespace YetAnotherToolbar
{
    /// <summary>
    /// Class to hold global mod settings.
    /// </summary>
    [XmlRoot(ElementName = "YetAnotherToolbar", Namespace = "", IsNullable = false)]
    internal static class Settings
    {
        internal static float toolbarScale = 1.0f;
        internal static int numOfRows = 2;
        internal static int numOfCols = 7;
        internal static int verticalOffset = 0;
        internal static int horizontalOffset = 0;
        internal static bool expanded = true;
        internal static bool hideMainButton = false;
        internal static bool disableUpdateNotice = false;
        internal static double lastUpdateNotice = 0.0;
        internal static int backgroundOption = 0;
        internal static float mainButtonX = 538.0f;
        internal static float mainButtonY = 947.0f;

        internal static KeyBinding modeToggleKey = new KeyBinding { keyCode = (int)KeyCode.T, control = false, shift = false, alt = true };
    }

    /// <summary>
    /// Defines the XML settings file.
    /// </summary>
    [XmlRoot(ElementName = "YetAnotherToolbar", Namespace = "", IsNullable = false)]
    public class XMLSettingsFile
    {
        [XmlElement("toolbarScale")]
        public float ToolbarScale { get => Settings.toolbarScale; set => Settings.toolbarScale = value; }

        [XmlElement("numOfRows")]
        public int NumOfRows { get => Settings.numOfRows; set => Settings.numOfRows = value; }

        [XmlElement("numOfCols")]
        public int NumOfCols { get => Settings.numOfCols; set => Settings.numOfCols = value; }

        [XmlElement("verticalOffset")]
        public int VerticalOffset { get => Settings.verticalOffset; set => Settings.verticalOffset = value; }

        [XmlElement("horizontalOffset")]
        public int HorizontalOffset { get => Settings.horizontalOffset; set => Settings.horizontalOffset = value; }

        [XmlElement("expanded")]
        public bool Expanded { get => Settings.expanded; set => Settings.expanded = value; }

        [XmlElement("hideMainButton")]
        public bool HideMainButton { get => Settings.hideMainButton; set => Settings.hideMainButton = value; }

        [XmlElement("disableUpdateNotice")]
        public bool DisableUpdateNotice { get => Settings.disableUpdateNotice; set => Settings.disableUpdateNotice = value; }

        [XmlElement("LastUpdateNotice")]
        public double LastUpdateNotice { get => Settings.lastUpdateNotice; set => Settings.lastUpdateNotice = value; }

        [XmlElement("backgroundOption")]
        public int BackgroundOption { get => Settings.backgroundOption; set => Settings.backgroundOption = value; }

        [XmlElement("mainButtonX")]
        public float MainButtonX { get => Settings.mainButtonX; set => Settings.mainButtonX = value; }

        [XmlElement("mainButtonY")]
        public float MainButtonY { get => Settings.mainButtonY; set => Settings.mainButtonY = value; }

        [XmlElement("modeToggleKey")]
        public KeyBinding ModeToggleKey { get => Settings.modeToggleKey; set => Settings.modeToggleKey = value; }
    }

    /// <summary>
    /// Basic keybinding class - code and modifiers.
    /// </summary>
    public struct KeyBinding
    {
        [XmlAttribute("KeyCode")]
        public int keyCode;

        [XmlAttribute("Control")]
        public bool control;

        [XmlAttribute("Shift")]
        public bool shift;

        [XmlAttribute("Alt")]
        public bool alt;
    }
}