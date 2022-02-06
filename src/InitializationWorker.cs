using UnityEngine;

namespace YetAnotherToolbar
{
    public class InitializationWorker : MonoBehaviour
    {
        public static InitializationWorker instance;
        public void Start()
        {
            instance = this;
            Debugging.Message($"InitializationWorker started");
        }

        public void Update()
        {
            if (YetAnotherToolbar.instance.InitializationCheck()) // return true if initialized
            {
                Debugging.Message($"Destroying InitializationWorker");
                Destroy(instance.gameObject);
                InitializationWorker.instance = null;
            }
        }
    }
}
