using JetBrains.Annotations;
using UnityEngine;
#if UNITASK_DOTWEEN_SUPPORT
using DG.Tweening;
#endif

namespace GigaCreation.Tools.General
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public static class ForgetExtensions
    {
        public static void Forget(this AsyncOperation self)
        {
        }

#if UNITASK_DOTWEEN_SUPPORT
        public static void Forget(this Tween self)
        {
        }
#endif
    }
}
