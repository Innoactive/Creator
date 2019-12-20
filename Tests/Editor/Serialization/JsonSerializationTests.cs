#if UNITY_EDITOR

using Innoactive.Hub.Training.Utils.Serialization;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;

namespace Innoactive.Hub.Tests.Serialization
{
    public class JsonSerializationTests
    {
        [Test]
        public void CanSerializeVector2()
        {
            // Given the JsonTrainingSerializer.SerializerSettings are used to serialize.
            // When a Vector2 is serialized
            string data = JsonConvert.SerializeObject(Vector2.up, JsonTrainingSerializer.SerializerSettings);
            // Then the output is not null
            Assert.IsFalse(string.IsNullOrEmpty(data));
        }

        [Test]
        public void SerializedVector2CanBeReadAgain()
        {
            // Given a Vector2 input
            Vector2 input = Vector2.up;

            // When parsed into json and back
            string data = JsonConvert.SerializeObject(input, JsonTrainingSerializer.SerializerSettings);
            Vector2 output = JsonConvert.DeserializeObject<Vector2>(data, JsonTrainingSerializer.SerializerSettings);

            // Then the input and output are equal
            Assert.AreEqual(input, output);
        }

        [Test]
        public void CanSerializeVector3()
        {
            // Given the JsonTrainingSerializer.SerializerSettings are used to serialize.
            // When a Vector3 is serialized
            string data = JsonConvert.SerializeObject(Vector3.up, JsonTrainingSerializer.SerializerSettings);
            // Then the output is not null
            Assert.IsFalse(string.IsNullOrEmpty(data));
        }

        [Test]
        public void SerializedVector3CanBeReadAgain()
        {
            // Given a Vector3 input
            Vector3 input = Vector3.up;

            // When parsed into json and back
            string data = JsonConvert.SerializeObject(input, JsonTrainingSerializer.SerializerSettings);
            Vector3 output = JsonConvert.DeserializeObject<Vector3>(data, JsonTrainingSerializer.SerializerSettings);

            // Then the input and output are equal
            Assert.AreEqual(input, output);
        }

        [Test]
        public void CanSerializeVector4()
        {
            // Given the JsonTrainingSerializer.SerializerSettings are used to serialize.
            // When a Vector4 is serialized
            string data = JsonConvert.SerializeObject(Vector4.one, JsonTrainingSerializer.SerializerSettings);
            // Then the output is not null
            Assert.IsFalse(string.IsNullOrEmpty(data));
        }

        [Test]
        public void SerializedVector4CanBeReadAgain()
        {
            // Given a Vector4 input
            Vector4 input = Vector4.one;

            // When parsed into json and back
            string data = JsonConvert.SerializeObject(input, JsonTrainingSerializer.SerializerSettings);
            Vector4 output = JsonConvert.DeserializeObject<Vector4>(data, JsonTrainingSerializer.SerializerSettings);

            // Then the input and output are equal
            Assert.AreEqual(input, output);
        }


        [Test]
        public void CanSerializeColor()
        {
            // Given the JsonTrainingSerializer.SerializerSettings are used to serialize.
            // When a unity color is serialized
            string data = JsonConvert.SerializeObject(Color.blue, JsonTrainingSerializer.SerializerSettings);
            // Then the output is not null
            Assert.IsFalse(string.IsNullOrEmpty(data));
        }

        [Test]
        public void SerializedColorCanBeReadAgain()
        {
            // Given a Color input
            Color input = Color.blue;

            // When parsed into json and back
            string data = JsonConvert.SerializeObject(input, JsonTrainingSerializer.SerializerSettings);
            Color output = JsonConvert.DeserializeObject<Color>(data, JsonTrainingSerializer.SerializerSettings);

            // Then the input and output are equal
            Assert.AreEqual(input, output);
        }
    }
}
#endif
