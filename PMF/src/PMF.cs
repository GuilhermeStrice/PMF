using PMF.Managers;

namespace PMF
{
    public delegate void OnPackageMessage(string message);

    public static class PMF
    {
        public static event OnPackageMessage OnPackageMessage;

        public static void Start()
        {
            PackageManager.Start();
        }

        public static void Stop()
        {
            PackageManager.Stop();
        }

        internal static void InvokePackageMessageEvent(string message)
        {
            OnPackageMessage?.Invoke(message);
        }
    }
}
