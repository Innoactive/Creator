namespace Innoactive.Hub.Training.Utils.Serialization
{
    /// <summary>
    /// A serializer which can de/serialize courses and steps to a certain format.
    /// </summary>
    public interface ICourseSerializer
    {
        /// <summary>
        /// Display name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// File format used for this serializer. For example, 'json'.
        /// </summary>
        string FileFormat { get; }

        /// <summary>
        /// Serializes a given course into a byte array.
        /// </summary>
        byte[] CourseToByteArray(ICourse target);

        /// <summary>
        /// Deserializes a given course to a usable object.
        /// </summary>
        ICourse CourseFromByteArray(byte[] data);

        /// <summary>
        /// Serializes a given step into a byte array. The implementation should trim target steps of the step.
        /// </summary>
        byte[] StepToByteArray(IStep step);

        /// <summary>
        /// Deserializes a given step to a usable object.
        /// </summary>
        IStep StepFromByteArray(byte[] data);
    }
}
