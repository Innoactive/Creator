using System.IO;

namespace Innoactive.Creator.Core.IO
{
    public static class FileUtils
    {
        /// <summary>
        /// Returns true if the given <paramref name="filePath"/> represents a training course file.
        /// </summary>
        public static bool IsTrainingCourseFile(string filePath)
        {
            StreamReader reader = new StreamReader(filePath);

            string line;
            int counter = 0;
            while ((line = reader.ReadLine()) != null)
            {
                if (counter == 2)
                {
                    return line.Contains("$type\": \"Innoactive.Hub.Training.Course");
                }

                counter++;
            }

            return false;
        }
    }
}
