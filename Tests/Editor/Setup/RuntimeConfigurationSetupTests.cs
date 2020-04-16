using System.Linq;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Configuration;
using Innoactive.Creator.Tests.Builder;
using Innoactive.Creator.Tests.Utils.Mocks;
using Innoactive.Creator.Unity;
using Innoactive.CreatorEditor.Utils;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.Tests
{
    public class RuntimeConfigurationSetupTests
    {
        [Test]
        public void ConfigNotCreated()
        {
            // When the Runtime configuration setup is ran.
            new RuntimeConfigurationSetup().Setup();
            // Then there should be an GameObject with fitting name in scene.
            GameObject obj = GameObject.Find(RuntimeConfigurationSetup.TrainingConfiugrationName);
            Assert.NotNull(obj);
        }

        [Test]
        public void IsConfigWithoutMissingScriptTest()
        {
            // When the Runtime configuration setup is ran.
            new RuntimeConfigurationSetup().Setup();
            // Then the found GameObject should not have missing scripts.
            GameObject obj = GameObject.Find(RuntimeConfigurationSetup.TrainingConfiugrationName);
            Assert.NotNull(obj);
            Assert.False(SceneUtils.ContainsMissingScripts(obj));
        }
    }
}
