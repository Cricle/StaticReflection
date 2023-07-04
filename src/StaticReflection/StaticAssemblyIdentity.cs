using System.Reflection;

namespace StaticReflection
{
    public class StaticAssemblyIdentity : IAssemblyIdentity
    {
        public StaticAssemblyIdentity(string name, Version version, string cultureName, AssemblyNameFlags flags, AssemblyContentType contentType, bool hasPublicKey, IReadOnlyList<byte> publicKey, IReadOnlyList<byte> publicKeyToken, bool isStrongName, bool isRetargetable)
        {
            Name = name;
            Version = version;
            CultureName = cultureName;
            Flags = flags;
            ContentType = contentType;
            HasPublicKey = hasPublicKey;
            PublicKey = publicKey;
            PublicKeyToken = publicKeyToken;
            IsStrongName = isStrongName;
            IsRetargetable = isRetargetable;
        }

        public string Name { get; }

        public Version Version { get; }

        public string CultureName { get; }

        public AssemblyNameFlags Flags { get; }

        public AssemblyContentType ContentType { get; }

        public bool HasPublicKey { get; }

        public IReadOnlyList<byte> PublicKey { get; }

        public IReadOnlyList<byte> PublicKeyToken { get; }

        public bool IsStrongName { get; }

        public bool IsRetargetable { get; }
    }
}
