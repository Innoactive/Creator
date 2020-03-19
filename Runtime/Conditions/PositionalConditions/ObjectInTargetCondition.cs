using System.Collections;
using System.Runtime.Serialization;
using UnityEngine;

namespace Innoactive.Creator.Core.Conditions
{
    public interface IObjectInTargetData : IConditionData
    {
        [DataMember]
        float RequiredTimeInside { get; set; }
    }

    public abstract class ObjectInTargetActiveProcess<TData> : IStageProcess<TData> where TData : IObjectInTargetData
    {
        private bool isInside;
        private float timeStarted;

        public void Start(TData data)
        {
            data.IsCompleted = false;
            isInside = IsInside(data);

            if (isInside)
            {
                timeStarted = Time.time;
            }
        }

        protected abstract bool IsInside(TData data);

        public IEnumerator Update(TData data)
        {
            while (true)
            {
                if (isInside != IsInside(data))
                {
                    isInside = !isInside;

                    if (isInside)
                    {
                        timeStarted = Time.time;
                    }
                }

                if (isInside && Time.time - timeStarted >= data.RequiredTimeInside)
                {
                    data.IsCompleted = true;
                    break;
                }

                yield return null;
            }
        }

        public void End(TData data)
        {
        }

        public void FastForward(TData data)
        {
        }
    }
}
