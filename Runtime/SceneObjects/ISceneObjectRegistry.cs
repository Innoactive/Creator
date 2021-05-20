using System;

namespace VPG.Core.SceneObjects
{
    public interface ISceneObjectRegistry
    {
        /// <summary>
        /// Returns if the Guid is registered in the registry.
        /// </summary>
        bool ContainsGuid(Guid guid);

        /// <summary>
        /// Returns if the name is registered in the registry.
        /// </summary>
        bool ContainsName(string name);

        /// <summary>
        /// Returns the ITrainingSceneEntity belonging to the given Guid.
        /// If there is no fitting Entity found a MissingEntityException will be thrown.
        /// </summary>
        ISceneObject GetByGuid(Guid guid);

        /// <summary>
        /// Returns the ITrainingSceneEntity belonging to the given unique name.
        /// If there is no fitting Entity found a MissingEntityException will be thrown.
        /// </summary>
        ISceneObject GetByName(string name);

        /// <summary>
        /// Registers an SceneObject in the registry. If there is an SceneObject with the same name
        /// already registered, an NameNotUniqueException will be thrown. Also if the Guid
        /// is already known an SceneObjectAlreadyRegisteredException will be thrown.
        /// </summary>
        void Register(ISceneObject obj);

        /// <summary>
        /// Removes the SceneObject completely from the Registry.
        /// </summary>
        bool Unregister(ISceneObject obj);

        /// <summary>
        /// Shortcut for GetByName(string name) method.
        /// </summary>
        ISceneObject this[string name] { get; }

        /// <summary>
        /// Shortcut for GetByGuid(Guid guid) method.
        /// </summary>
        ISceneObject this[Guid guid] { get; }

        /// <summary>
        /// Registers all SceneObject in scene, independent of there state.
        /// </summary>
        void RegisterAll();
    }
}
