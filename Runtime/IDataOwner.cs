using System.Runtime.Serialization;

namespace Innoactive.Creator.Core
{
    public interface IDataOwner<out TData> : IDataOwner
    {
        [DataMember]
        new TData Data { get; }
    }

    public interface IDataOwner
    {
        IData Data { get; }
    }
}
