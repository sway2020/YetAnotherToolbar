using ICities;
using UnityEngine;

namespace YetAnotherToolbar
{
    public class Loading : LoadingExtensionBase
    {
        public override void OnLevelLoaded(LoadMode mode)
        {
            if (mode == LoadMode.NewMap || mode == LoadMode.LoadMap ||
                mode == LoadMode.NewAsset || mode == LoadMode.LoadAsset ||
                mode == LoadMode.NewTheme || mode == LoadMode.LoadTheme) YetAnotherToolbar.isEditorMode = true;

            if (mode == LoadMode.NewAsset || mode == LoadMode.LoadAsset) YetAnotherToolbar.isAssetEditorMode = true;

            if (YetAnotherToolbar.instance == null)
            {
                YetAnotherToolbar.instance = new GameObject("YetAnotherToolbar").AddComponent<YetAnotherToolbar>();
            }
            else
            {
                YetAnotherToolbar.instance.Start();
            }
            if (InitializationWorker.instance == null)
            {
                InitializationWorker.instance = new GameObject("InitializationWorker").AddComponent<InitializationWorker>();
            }
            else
            {
                InitializationWorker.instance.Start();
            }
        }
    }
}