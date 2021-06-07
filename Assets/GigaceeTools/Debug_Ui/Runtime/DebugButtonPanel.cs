using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GigaceeTools
{
    public class DebugButtonPanel : MonoBehaviour
    {
        [SerializeField] private Button _showButton;
        [SerializeField] private Button _hideButton;
        [SerializeField] private Transform[] _targets;

        private void Start()
        {
            if (!ServiceLocator.TryGetInstance(out IDebugCore debugCore))
            {
                return;
            }

            _showButton.onClick.AddListener(Present);
            _hideButton.onClick.AddListener(Dismiss);

            debugCore.IsDebugMode
                .Where(x => x)
                .Subscribe(x =>
                {
                    Dismiss();
                })
                .AddTo(this);
        }

        private void OnDestroy()
        {
            foreach (DebugButtonPanel debugButtonPanel in FindObjectsOfType<DebugButtonPanel>().Where(x => x != this))
            {
                debugButtonPanel.transform.localScale = Vector3.one;
            }
        }

        private void Present()
        {
            foreach (Transform target in _targets)
            {
                target.localScale = Vector3.one;
            }

            _showButton.transform.localScale = Vector3.zero;
            _hideButton.transform.localScale = Vector3.one;

            foreach (DebugButtonPanel debugButtonPanel in FindObjectsOfType<DebugButtonPanel>().Where(x => x != this))
            {
                debugButtonPanel.transform.localScale = Vector3.zero;
            }
        }

        private void Dismiss()
        {
            foreach (Transform target in _targets)
            {
                target.localScale = Vector3.zero;
            }

            _showButton.transform.localScale = Vector3.one;
            _hideButton.transform.localScale = Vector3.zero;

            foreach (DebugButtonPanel debugButtonPanel in FindObjectsOfType<DebugButtonPanel>().Where(x => x != this))
            {
                debugButtonPanel.transform.localScale = Vector3.one;
            }
        }
    }
}
