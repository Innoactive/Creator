using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.Core.Tests.Utils.Mocks;
using Innoactive.Hub.Training;

namespace Innoactive.Creator.Core.Tests.Utils
{
    public class TestLinearChapterBuilder
    {
        public readonly string Name;

        public readonly List<Step> Steps = new List<Step>();
        public readonly List<EndlessConditionMock> StepTriggerConditions = new List<EndlessConditionMock>();

        public TestLinearChapterBuilder(string name)
        {
            Name = name;
        }

        public Step AddStep(string name)
        {
            Step step = new Step(name);

            step.Data.Transitions.Data.Transitions.Add(new Transition());

            if (Steps.Count > 0)
            {
                Steps.Last().Data.Transitions.Data.Transitions.First().Data.TargetStep = step;
            }

            Steps.Add(step);
            return step;
        }

        public Chapter Build()
        {
            Chapter result = new Chapter(Name, Steps.First());
            result.Data.Steps = Steps.Cast<IStep>().ToList();
            return result;
        }

        public static TestLinearChapterBuilder SetupChapterBuilder(int steps = 3, bool addTriggerConditions = true)
        {
            TestLinearChapterBuilder builder = new TestLinearChapterBuilder("Chapter1");
            for (int i = 0; i < steps; i++)
            {
                builder.AddStep("Step" + (i + 1));
                if (addTriggerConditions)
                {
                    EndlessConditionMock conditionMock = new EndlessConditionMock();
                    builder.Steps[i].Data.Transitions.Data.Transitions.First().Data.Conditions.Add(conditionMock);
                    builder.StepTriggerConditions.Add(conditionMock);
                }
            }
            return builder;
        }
    }
}
