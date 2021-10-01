using System.Diagnostics.CodeAnalysis;
using UnityEngine;
#if UNITASK_DOTWEEN_SUPPORT
using DG.Tweening;
#endif

namespace GigaceeTools
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
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
