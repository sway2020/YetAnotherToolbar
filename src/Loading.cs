using ICities;
using UnityEngine;

namespace YetAnotherToolbar
{
    public class Loading : LoadingExtensionBase
    {
        public override void OnLevelLoaded(LoadMode mode)
        {
            if (YetAnotherToolbar.instance == null)
            {
                YetAnotherToolbar.instance = new GameObject("YetAnotherToolbar").AddComponent<YetAnotherToolbar>();
            }
            else
            {
                YetAnotherToolbar.instance.Start();
            }
        }
    }
}