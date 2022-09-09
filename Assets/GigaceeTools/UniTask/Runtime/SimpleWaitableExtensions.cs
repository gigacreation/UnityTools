namespace GigaceeTools
{
    public static class SimpleWaitableExtensions
    {
        public static bool IsActive(this ISimpleWaitable self)
        {
            return self is { Active: true };
        }
    }
}
