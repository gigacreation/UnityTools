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
                    ShowContent();
                })
                .AddTo(this);

            _hideButton
                .OnClickAsObservable()
                .Subscribe(_ =>
                {
                    HideContent();
                })
                .AddTo(this);

            debugManager
                .IsDebugMode
                .Where(static x => x)
                .Subscribe(_ =>
                {
                    HideContent();
                })
                .AddTo(this);
        }

        public virtual void ShowContent()
        {
            foreach (Transform content in _contents)
            {
                content.localScale = Vector3.one;
            }

            _showButtonCanvasGroup.alpha = 0f;
            _showButtonCanvasGroup.blocksRaycasts = false;

            _hideButtonCanvasGroup.alpha = 1f;
            _hideButtonCanvasGroup.blocksRaycasts = true;
        }

        public virtual void HideContent()
        {
            foreach (Transform content in _contents)
            {
                content.localScale = Vector3.zero;
            }

            _showButtonCanvasGroup.alpha = 1f;
            _showButtonCanvasGroup.blocksRaycasts = true;

            _hideButtonCanvasGroup.alpha = 0f;
            _hideButtonCanvasGroup.blocksRaycasts = false;
        }
    }
}
