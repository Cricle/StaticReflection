using System;
using System.Collections.Generic;
using System.Text;

namespace StaticReflection
{
    public interface IFieldDefine: IAttributeDefine, IMemberDefine
    {
        Type PropertyType { get; }

        bool IsConst { get; }

        bool IsReadOnly { get; }

        bool IsVolatile { get; }

        bool IsRequired { get; }

        bool IsFixedSizeBuffer { get; }

        int FixedSize { get; }

        bool HasConstantValue { get; }

        bool IsExplicitlyNamedTupleElement { get; }

        StaticRefKind RefKind { get; }
    }
}
