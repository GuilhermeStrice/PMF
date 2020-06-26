using System;
using System.Collections.Generic;
using System.Text;

namespace PMF
{
    public delegate void OnPackageMessage(string message);

    public static class PMF
    {
        public static event OnPackageMessage OnPackageMessage;

        internal static void InvokePackageMessageEvent(string message)
        {
            OnPackageMessage?.Invoke(message);
        }
    }
}
