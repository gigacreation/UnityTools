using UniRx;
using UnityEngine;

namespace GcTools
{
    public class DebugDisplay : MonoBehaviour
    {
        private void Start()
        {
            if (ServiceLocator.TryGetInstance(out IDebugCore debugCore))
            {
                debugCore.IsDebugMode
                    .Subscribe(x =>
                    {
                        if (x)
                        {
                            Present();
                        }
                        else
                        {
                            Dismiss();
                        }
                    })
                    .AddTo(this);
            }
        }

        private void Present()
        {
            transform.localScale = Vector3.one;
        }

        private void Dismiss()
        {
            transform.localScale = Vector3.zero;
        }
    }
}
