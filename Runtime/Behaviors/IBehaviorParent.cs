﻿﻿using System;
using System.Collections.ObjectModel;

namespace Innoactive.Creator.Core.Behaviors
{
    public class BehaviorCollectionChangedEventArgs : EventArgs { }

    public interface IBehaviorParent
    {
        /// <summary>
        /// Invoked when behavior is added or removed from this object.
        /// </summary>
        event EventHandler<BehaviorCollectionChangedEventArgs> BehaviorCollectionChanged;

        /// <summary>
        /// List of behaviors associated with this object.
        /// </summary>
        ReadOnlyCollection<IBehavior> Behaviors { get; }

        /// <summary>
        /// Returns true if this object has given behavior.
        /// </summary>
        bool CheckHasBehavior(IBehavior behavior);

        /// <summary>
        /// Add behavior to this object. Implementation of this method should invoke BehaviorCollectionChanged event.
        /// </summary>
        /// <param name="behavior">Behavior to be added.</param>
        void AddBehavior(IBehavior behavior);

        /// <summary>
        /// Insert the <paramref name="behavior"/> into the collection of behaviors at <paramref name="index"/>.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="behavior"></param>
        void InsertBehavior(int index, IBehavior behavior);

        /// <summary>
        /// Remove behavior from this object. Implementation of this method should invoke BehaviorCollectionChanged event.
        /// </summary>
        bool RemoveBehavior(IBehavior behavior);
    }
}
