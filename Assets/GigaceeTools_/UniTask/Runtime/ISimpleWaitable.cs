using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace GigaceeTools
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public interface ISimpleWaitable
    {
        /// <summary>
        /// タスクが実行中かどうかを返します。
        /// </summary>
        /// <returns>タスクが実行中なら true を、そうでないなら false を返します。</returns>
        bool IsPending { get; }

        /// <summary>
        /// タスクが完了になるまで待機し、呼ばれてから完了するまでに掛かった時間を返します。
        /// </summary>
        /// <param name="ct">キャンセルトークン。</param>
        /// <returns>完了を待機するタスク。</returns>
        UniTask<float> WaitForCompletionAsync(CancellationToken ct = default);
    }
}
