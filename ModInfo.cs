using ICities;

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
    }
}
