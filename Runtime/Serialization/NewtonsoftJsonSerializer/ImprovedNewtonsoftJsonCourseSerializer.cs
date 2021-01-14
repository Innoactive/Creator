using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.Serialization.NewtonsoftJson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Innoactive.Creator.Core.Serialization
{
    /// <summary>
    /// Improved version of the NewtonsoftJsonCourseSerializer, which now allows to serialize very long chapters.
    /// </summary>
    public class ImprovedNewtonsoftJsonCourseSerializer : NewtonsoftJsonCourseSerializer
    {
        /// <inheritdoc/>
        public override string Name { get; } = "Improved Newtonsoft Json Importer";

        protected override int Version { get; } = 2;

        /// <inheritdoc/>
        public override ICourse CourseFromByteArray(byte[] data)
        {
            string stringData = new UTF8Encoding().GetString(data);
            JObject dataObject = JsonConvert.DeserializeObject<JObject>(stringData, CourseSerializerSettings);

            // Check if course was serialized with version 1
            int version = dataObject.GetValue("$serializerVersion").ToObject<int>();
            if (version == 1)
            {
                return base.CourseFromByteArray(data);
            }

            CourseWrapper wrapper = Deserialize<CourseWrapper>(data, CourseSerializerSettings);
            return wrapper.GetCourse();
        }

        /// <inheritdoc/>
        public override byte[] CourseToByteArray(ICourse course)
        {
            CourseWrapper wrapper = new CourseWrapper(course);
            JObject jObject = JObject.FromObject(wrapper, JsonSerializer.Create(CourseSerializerSettings));
            jObject.Add("$serializerVersion", Version);
            // This line is required to undo the changes applied to the course.
            wrapper.GetCourse();

            return new UTF8Encoding().GetBytes(jObject.ToString());
        }

        [Serializable]
        private class CourseWrapper
        {
            [DataMember]
            public List<IStep> Steps = new List<IStep>();

            [DataMember]
            public ICourse Course;

            public CourseWrapper()
            {

            }

            public CourseWrapper(ICourse course)
            {
                foreach (IChapter chapter in course.Data.Chapters)
                {
                    Steps.AddRange(chapter.Data.Steps);
                }

                foreach (IStep step in Steps)
                {
                    foreach (ITransition transition in step.Data.Transitions.Data.Transitions)
                    {
                        if (transition.Data.TargetStep != null)
                        {
                            transition.Data.TargetStep = new StepRef() { PositionIndex = Steps.IndexOf(transition.Data.TargetStep) };
                        }
                    }
                }
                Course = course;
            }

            public ICourse GetCourse()
            {
                foreach (IStep step in Steps)
                {
                    foreach (ITransition transition in step.Data.Transitions.Data.Transitions)
                    {
                        if (transition.Data.TargetStep == null)
                        {
                            continue;
                        }

                        StepRef stepRef = (StepRef) transition.Data.TargetStep;
                        transition.Data.TargetStep = stepRef.PositionIndex >= 0 ? Steps[stepRef.PositionIndex] : null;
                    }
                }

                return Course;
            }

            [Serializable]
            public class StepRef : IStep
            {
                [DataMember]
                public int PositionIndex = -1;

                IData IDataOwner.Data { get; } = null;

                IStepData IDataOwner<IStepData>.Data { get; } = null;

                public ILifeCycle LifeCycle { get; } = null;

                public IProcess GetActivatingProcess()
                {
                    throw new NotImplementedException();
                }

                public IProcess GetActiveProcess()
                {
                    throw new NotImplementedException();
                }

                public IProcess GetDeactivatingProcess()
                {
                    throw new NotImplementedException();
                }

                public void Configure(IMode mode)
                {
                    throw new NotImplementedException();
                }

                public void Update()
                {
                    throw new NotImplementedException();
                }

                public StepMetadata StepMetadata { get; set; }
            }
        }
    }
}
