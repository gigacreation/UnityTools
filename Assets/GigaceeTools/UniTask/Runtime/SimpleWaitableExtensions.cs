namespace GigaceeTools
{
    public static class SimpleWaitableExtensions
    {
        public static bool IsActive(this SimpleWaitable self)
        {
            return (self != null) && self.IsActive();
        }
    }
}
