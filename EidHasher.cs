//public sealed class EidHasher(IConfiguration config) : IEidHasher
//{
//    private readonly byte[] _key = Convert.FromBase64String(config["Itsme:EidHashKey"]!);
//    public string Hash(string sub) =>
//        Convert.ToHexStringLower(HMACSHA256.HashData(_key, Encoding.UTF8.GetBytes(sub.Trim())));
//}
