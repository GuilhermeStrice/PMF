namespace PMF
{
    public enum PackageType
    {
        //None is there but should be pretty much impossible to get, only if something bad happens with the json sent from the server
        None, 
        Plugin,
        Library
    }
}
