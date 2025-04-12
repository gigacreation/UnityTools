using GigaCreation.Tools.Debugging.Core;
using GigaCreation.Tools.Service;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GigaCreation.Tools.Debugging.Ui
{
    [RequireComponent(typeof(CanvasGroup), typeof(Button))]
    public class DisableDebugModeButton : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _button;

        private IDebugManager _debugManager;

        private void Start()
        {
            if (!ServiceLocator.TryGet(out _debugManager))
            {
                return;
            }

            _button
                .OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _debugManager.IsDebugMode.Value = false;
                })
                .AddTo(this);

            _debugManager
                .IsDebugMode
                .Subscribe(x =>
                {
                    _canvasGroup.alpha = x ? 1f : 0f;
                    _canvasGroup.interactable = x;
                    _canvasGroup.blocksRaycasts = x;
                })
                .AddTo(this);
        }

        private void Reset()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _button = GetComponent<Button>();
        }
    }
}
