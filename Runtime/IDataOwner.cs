using System.Runtime.Serialization;

namespace VPG.Creator.Core
{
    /// <summary>
    /// Abstract holder of data.
    /// </summary>
    public interface IDataOwner
    {
        /// <summary>
        /// Abstract data.
        /// </summary>
        IData Data { get; }
    }

    /// <summary>
    /// Abstract holder of data.
    /// </summary>
    public interface IDataOwner<out TData> : IDataOwner
    {
        /// <summary>
        /// Abstract data.
        /// </summary>
        [DataMember]
        new TData Data { get; }
    }
}
