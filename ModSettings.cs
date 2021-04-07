// Originally written by algernon for Find It 2.
// Modified by sway
using System.Xml.Serialization;

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
        internal static int backgroundOption = 0;
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

        [XmlElement("backgroundOption")]
        public int BackgroundOption { get => Settings.backgroundOption; set => Settings.backgroundOption = value; }
    }
}