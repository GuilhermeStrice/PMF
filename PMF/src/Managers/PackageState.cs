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
