namespace Innoactive.Creator.Core.Serialization
{
    public interface IOnBuildExportSerialization
    {
        byte[] ConvertTrainingCourseForExport(ICourse course);
    }
}
