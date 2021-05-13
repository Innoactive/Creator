﻿using System;
 using VPG.Creator.Core.SceneObjects;

 namespace VPG.Creator.Core.Exceptions
{
    public class PropertyNotFoundException : TrainingException
    {
        public PropertyNotFoundException(string message) : base(message) { }
        public PropertyNotFoundException(ISceneObject sourceObject, Type missingType) : base(string.Format("SceneObject '{0}' does not contain a property of type '{1}'", sourceObject.UniqueName, missingType.Name)) { }
    }
}
