using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace GigaceeTools
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public interface ICommonWaitable
    {
        /// <summary>
        /// タスクが完了になるまで待機し、呼ばれてから完了するまでに掛かった時間を返します。
        /// </summary>
        /// <param name="ct">キャンセルトークン。</param>
        /// <returns>完了を待機するタスク。</returns>
        UniTask<float> WaitForCompletionAsync(CancellationToken ct = default);
    }
}
