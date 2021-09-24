using UnityEngine;

namespace GigaceeTools
{
    /// <summary>
    /// デバッグモードがオンのときに表示され、オフのときに非表示になります。
    /// </summary>
    public class DebugDisplay : FireOnSwitchingDebugMode
    {
        protected override void OnEnterDebugMode()
        {
            transform.localScale = Vector3.one;
        }

        protected override void OnExitDebugMode()
        {
            transform.localScale = Vector3.zero;
        }
    }
}
