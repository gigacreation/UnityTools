using System.Collections.Generic;
using GigaCreation.Tools.Debugging.Core;
using GigaCreation.Tools.Service;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GigaCreation.Tools.Debugging.Ui
{
    /// <summary>
    /// デバッグモード中、表示・非表示を切り替えることができるパネルです。
    /// </summary>
    public class DebugPanel : MonoBehaviour
    {
        [SerializeField] private Transform[] _contents;

        [Space]
        [SerializeField] private CanvasGroup _showButtonCanvasGroup;
        [SerializeField] private Button _showButton;
        [SerializeField] private CanvasGroup _hideButtonCanvasGroup;
        [SerializeField] private Button _hideButton;

        private bool _visible;

        public IReadOnlyList<Transform> Contents => _contents;

        public virtual bool Visible
        {
            get => _visible;
            set => SetVisible(value);
        }

        private void Start()
        {
            if (!ServiceLocator.TryGet(out IDebugManager debugManager))
            {
                return;
            }

            _showButton
                .OnClickAsObservable()
                .Subscribe(_ =>
                {
                    Visible = true;
                })
                .AddTo(this);

            _hideButton
                .OnClickAsObservable()
                .Subscribe(_ =>
                {
                    Visible = false;
                })
                .AddTo(this);

            debugManager
                .IsDebugMode
                .Where(static x => x)
                .Subscribe(_ =>
                {
                    Visible = false;
                })
                .AddTo(this);
        }

        protected virtual void SetVisible(bool visible)
        {
            _visible = visible;

            foreach (Transform content in _contents)
            {
                content.localScale = visible ? Vector3.one : Vector3.zero;
            }

            _showButtonCanvasGroup.blocksRaycasts = !visible;
            _showButtonCanvasGroup.alpha = visible ? 0f : 1f;

            _hideButtonCanvasGroup.blocksRaycasts = visible;
            _hideButtonCanvasGroup.alpha = visible ? 1f : 0f;
        }
    }
}
