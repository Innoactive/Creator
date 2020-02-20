using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Innoactive.Hub.Unity.Tests.Training.Editor.EditorImguiTester
{
    /// <summary>
    /// Data structure for an atomic user action (click mouse, select item in a context menu).
    /// </summary>
    [DataContract(IsReference = false)]
    public class UserAction
    {
        [DataMember]
        public Event Event { get; set; }

        [DataMember]
        public List<string> PrepickedSelections { get; set; }

        public UserAction()
        {
            PrepickedSelections = new List<string>();
        }
    }
}