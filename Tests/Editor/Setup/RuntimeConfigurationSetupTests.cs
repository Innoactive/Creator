using System.Linq;
using VPG.Core;
using VPG.Core.Configuration;
using VPG.Tests.Builder;
using VPG.Tests.Utils.Mocks;
using VPG.Unity;
using VPG.Editor.Utils;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace VPG.Editor.Tests
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
