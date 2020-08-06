[1mdiff --git a/Editor/CourseAssets/CourseAssetManager.cs b/Editor/CourseAssets/CourseAssetManager.cs[m
[1mindex 4746e75..cd858aa 100644[m
[1m--- a/Editor/CourseAssets/CourseAssetManager.cs[m
[1m+++ b/Editor/CourseAssets/CourseAssetManager.cs[m
[36m@@ -4,6 +4,7 @@[m
 using Innoactive.Creator.Core;[m
 using Innoactive.Creator.Core.Serialization;[m
 using Innoactive.CreatorEditor.Configuration;[m
[32m+[m[32musing Innoactive.CreatorEditor.UI.Drawers.Metadata;[m
 using UnityEditor;[m
 using UnityEngine;[m
 [m
[1mdiff --git a/Runtime/Metadata/ReorderableElementMetadata.cs b/Runtime/Metadata/ReorderableElementMetadata.cs[m
[1mindex d0b0007..c60b14e 100644[m
[1m--- a/Runtime/Metadata/ReorderableElementMetadata.cs[m
[1m+++ b/Runtime/Metadata/ReorderableElementMetadata.cs[m
[36m@@ -1,4 +1,4 @@[m
[31m-namespace Innoactive.CreatorEditor.UI.Drawers.Metadata[m
[32m+[m[32mnamespace Innoactive.Creator.Core.UI.Drawers.Metadata[m
 {[m
     /// <summary>[m
     /// Metadata to make <see cref="Innoactive.Creator.Core.Attributes.ReorderableListOfAttribute"/> reorderable.[m
[1mdiff --git a/Runtime/Serialization/NewtonsoftJsonSerializer/NewtonsoftJsonCourseSerializer.cs b/Runtime/Serialization/NewtonsoftJsonSerializer/NewtonsoftJsonCourseSerializer.cs[m
[1mindex 15ab93b..805fb11 100644[m
[1m--- a/Runtime/Serialization/NewtonsoftJsonSerializer/NewtonsoftJsonCourseSerializer.cs[m
[1m+++ b/Runtime/Serialization/NewtonsoftJsonSerializer/NewtonsoftJsonCourseSerializer.cs[m
[36m@@ -2,12 +2,11 @@[m
 using System.Collections.Generic;[m
 using System.Linq;[m
 using System.Text;[m
[32m+[m[32musing Innoactive.Creator.Core.UI.Drawers.Metadata;[m
 using Innoactive.Creator.Core.Utils;[m
[31m-using Innoactive.CreatorEditor.UI.Drawers.Metadata;[m
 using Newtonsoft.Json;[m
 using Newtonsoft.Json.Linq;[m
 using Newtonsoft.Json.Serialization;[m
[31m-using UnityEngine;[m
 [m
 namespace Innoactive.Creator.Core.Serialization.NewtonsoftJson[m
 {[m
[36m@@ -112,7 +111,7 @@[m [minternal class CourseSerializationBinder : DefaultSerializationBinder[m
         {[m
             public override Type BindToType(string assemblyName, string typeName)[m
             {[m
[31m-                if (typeName == typeof(ReorderableElementMetadata).FullName)[m
[32m+[m[32m                if (typeName == "Innoactive.CreatorEditor.UI.Drawers.Metadata.ReorderableElementMetadata")[m
                 {[m
                     return typeof(ReorderableElementMetadata);[m
                 }[m
