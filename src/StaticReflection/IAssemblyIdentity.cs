using System.Reflection;

namespace StaticReflection
{
    public interface IAssemblyIdentity
    {
        string Name { get; }

        Version Version { get; }

        string CultureName { get; }

        AssemblyNameFlags Flags { get; }

        AssemblyContentType ContentType { get; }

        bool HasPublicKey { get; }

        IReadOnlyList<byte> PublicKey { get; }

        IReadOnlyList<byte> PublicKeyToken { get; }

        bool IsStrongName { get; }

        bool IsRetargetable { get; }
    }
}
