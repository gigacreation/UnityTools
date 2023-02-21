using GigaCreation.Tools.Service;
using UniRx;
using UnityEngine;

namespace GigaCreation.Tools
{
    /// <summary>
    /// デバッグモードがオンのときに表示され、オフのときに非表示になります。
    /// </summary>
    public class DebugDisplay : MonoBehaviour
    {
        private void Start()
        {
            if (ServiceLocator.TryGet(out IDebugCore debugCore))
            {
                debugCore
                    .IsDebugMode
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
