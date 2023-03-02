using GigaCreation.Tools.Debugging.Core;
using GigaCreation.Tools.Service;
using UniRx;
using UnityEngine;

namespace GigaCreation.Tools.Debugging
{
    /// <summary>
    /// デバッグモードがオンのときに表示され、オフのときに非表示になります。
    /// </summary>
    public class DebugDisplay : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;

        private void Start()
        {
            if (!ServiceLocator.TryGet(out IDebugService debugService))
            {
                return;
            }

            debugService
                .IsDebugMode
                .Subscribe(ChangeVisibility)
                .AddTo(this);
        }

        private void Reset()
        {
            _canvas = GetComponent<Canvas>();
        }

        /// <summary>
        /// 自身の表示・非表示を切り替えます。
        /// </summary>
        /// <param name="visible">true なら表示をし、false なら非表示にします。</param>
        private void ChangeVisibility(bool visible)
        {
            if (_canvas)
            {
                _canvas.enabled = visible;
                return;
            }

            transform.localScale = visible ? Vector3.one : Vector3.zero;
        }
    }
}
