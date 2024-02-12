using System.Threading;
using Cysharp.Threading.Tasks;
using GigaCreation.Tools.Debugging.Core;
using GigaCreation.Tools.Service;
using UnityEngine;

namespace GigaCreation.Tools.Debugging.General
{
    public class DebugOptions : MonoBehaviour
    {
        private static bool s_isInitialized;
        private static IDebugManager s_debugManager;

        protected static bool IsDebugMode
        {
            get
            {
                if (!s_isInitialized)
                {
                    Debug.LogWarning("DebugOptions not initialized.");
                    return false;
                }

                return s_debugManager?.IsDebugMode.Value ?? false;
            }
        }

        private void Awake()
        {
            InitializeAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private void OnValidate()
        {
            UpdateStaticFields();
        }

        private async UniTask InitializeAsync(CancellationToken ct = default)
        {
            await UniTask.Yield(PlayerLoopTiming.EarlyUpdate, ct);

            if (ServiceLocator.TryGet(out s_debugManager))
            {
                UpdateStaticFields();
            }

            s_isInitialized = true;
        }

        protected virtual void UpdateStaticFields()
        {
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetInitializationFlag()
        {
            s_isInitialized = false;
        }
    }
}
