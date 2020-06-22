﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PMF
{
    public enum PackageState
    {
        // Local
        NotInstalled,
        UpToDate,
        Installed,
        AlreadyInstalled,

        // Remote
        NotExisting,
        VersionNotFound,

        // Common
        Failed
    }
}
