﻿using Innoactive.Hub.SDK;
using System;

namespace Innoactive.Hub.Training.HelperTasks
{
    public abstract class ChangeEntityTask : AsyncTask
    {
        protected IEntity Entity { get; set; }

        protected ChangeEntityTask(IEntity entity) : base(task => ((ChangeEntityTask)task).PerformTask())
        {
            Entity = entity;
        }

        protected abstract IDisposable PerformTask();
    }
}
