namespace SuperSocket.ProtoBase
{
    /// <summary>
    /// 
    /// </summary>
    public class StringPackageInfo : IKeyedPackageInfo<string>, IStringPackage
    {
        public string Key { get; set; }

        public string Body { get; set; }

        public string[] Parameters { get; set; }
    }
}