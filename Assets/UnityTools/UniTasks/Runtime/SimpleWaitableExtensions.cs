using JetBrains.Annotations;

namespace GigaCreation.Tools.UniTasks
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public static class SimpleWaitableExtensions
    {
        public static bool IsActive(this ISimpleWaitable self)
        {
            return self is { IsPending: true };
        }
    }
}
