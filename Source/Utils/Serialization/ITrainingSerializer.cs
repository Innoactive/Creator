namespace Innoactive.Hub.Training.Utils.Serialization
{
    /// <summary>
    /// Training course serializer which can de/serialize trainings from different formats.
    /// </summary>
    public interface ITrainingSerializer
    {
        /// <summary>
        /// Display name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Can deserialize training courses.
        /// </summary>
        bool CanDeserialize { get; }

        /// <summary>
        /// Can serialize training courses.
        /// </summary>
        bool CanSerialize { get; }

        /// <summary>
        /// File format used for this serializer for example 'json'.
        /// </summary>
        string FileFormat { get; }

        /// <summary>
        /// Serializes given training course into bytes.
        /// </summary>
        byte[] ToByte(ICourse course);

        /// <summary>
        /// Deserializes given data to an usable training course.
        /// </summary>
        ICourse ToCourse(byte[] data);
    }
}
