using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Attributes;
using Innoactive.Creator.Core.Utils;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Drawers
{
    /// <summary>
    /// Training drawer for object properties. Used when everything else does not fit.
    /// </summary>
    [DefaultTrainingDrawer(typeof(object))]
    public class ObjectDrawer : AbstractDrawer
    {
        /// <inheritdoc />
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            Rect nextPosition = new Rect(rect.x, rect.y, rect.width, EditorDrawingHelper.HeaderLineHeight);
            float height = 0;

            if (currentValue == null)
            {
                EditorGUI.LabelField(rect, label);
                height += nextPosition.height;
                rect.height += height;
                return rect;
            }

            if (label != null && label != GUIContent.none && (label.image != null || label.text != null))
            {
                height += DrawLabel(nextPosition, currentValue, changeValueCallback, label);
            }

            foreach (MemberInfo memberInfoToDraw in GetMembersToDraw(currentValue))
            {
                height += EditorDrawingHelper.VerticalSpacing;
                nextPosition.y = rect.y + height;

                MemberInfo closuredMemberInfo = memberInfoToDraw;

                if (closuredMemberInfo.GetAttributes<MetadataAttribute>(true).Any())
                {
                    height += CreateAndDrawMetadataWrapper(nextPosition, currentValue, closuredMemberInfo, changeValueCallback);
                }
                else
                {
                    ITrainingDrawer memberDrawer = DrawerLocator.GetDrawerForMember(closuredMemberInfo, currentValue);

                    object memberValue = ReflectionUtils.GetValueFromPropertyOrField(currentValue, closuredMemberInfo);

                    GUIContent displayName = memberDrawer.GetLabel(closuredMemberInfo, currentValue);

                    height += memberDrawer.Draw(nextPosition, memberValue, (value) =>
                    {
                        ReflectionUtils.SetValueToPropertyOrField(currentValue, closuredMemberInfo, value);
                        changeValueCallback(currentValue);
                    }, displayName).height;
                }
            }

            rect.height = height;
            return rect;
        }

        /// <summary>
        /// Draw a label for an object.
        /// </summary>
        protected virtual float DrawLabel(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            GUIStyle labelStyle = new GUIStyle(EditorStyles.label)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 12,
            };

            EditorGUI.LabelField(rect, label, labelStyle);

            return rect.height;
        }

        private float CreateAndDrawMetadataWrapper(Rect rect, object ownerObject, MemberInfo drawnMemberInfo, Action<object> changeValueCallback)
        {
            PropertyInfo metadataProperty = ownerObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(property => typeof(Metadata).IsAssignableFrom(property.PropertyType));
            FieldInfo metadataField = ownerObject.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(field => typeof(Metadata).IsAssignableFrom(field.FieldType));
            Metadata ownerObjectMetadata = null;

            if (metadataProperty != null)
            {
                ownerObjectMetadata = (Metadata)metadataProperty.GetValue(ownerObject, null) ?? new Metadata();
            }
            else if (metadataField != null)
            {
                ownerObjectMetadata = (Metadata)metadataField.GetValue(ownerObject) ?? new Metadata();
            }
            else
            {
                throw new MissingFieldException($"No metadata property on object {ownerObject}.");
            }

            object memberValue = ReflectionUtils.GetValueFromPropertyOrField(ownerObject, drawnMemberInfo);
            ITrainingDrawer memberDrawer = DrawerLocator.GetDrawerForMember(drawnMemberInfo, ownerObject);

            MetadataWrapper wrapper = new MetadataWrapper()
            {
                Metadata = ownerObjectMetadata.GetMetadata(drawnMemberInfo),
                ValueDeclaredType = ReflectionUtils.GetDeclaredTypeOfPropertyOrField(drawnMemberInfo),
                Value = memberValue
            };

            Action<object> wrapperChangedCallback = (newValue) =>
            {
                MetadataWrapper newWrapper = (MetadataWrapper)newValue;
                foreach (string key in newWrapper.Metadata.Keys.ToList())
                {
                    wrapper.Metadata[key] = newWrapper.Metadata[key];
                }

                foreach (string key in newWrapper.Metadata.Keys)
                {
                    ownerObjectMetadata.SetMetadata(drawnMemberInfo, key, newWrapper.Metadata[key]);
                }

                if (metadataField != null)
                {
                    metadataField.SetValue(ownerObject, ownerObjectMetadata);
                }

                if (metadataProperty != null)
                {
                    metadataProperty.SetValue(ownerObject, ownerObjectMetadata, null);
                }

                ReflectionUtils.SetValueToPropertyOrField(ownerObject, drawnMemberInfo, newWrapper.Value);

                changeValueCallback(ownerObject);
            };

            bool isMetadataDirty = false;

            List<MetadataAttribute> declaredAttributes = drawnMemberInfo.GetAttributes<MetadataAttribute>(true).ToList();

            Dictionary<string, object> obsoleteMetadataRemoved = wrapper.Metadata.Keys.ToList().Where(key => declaredAttributes.Any(attribute => attribute.Name == key)).ToDictionary(key => key, key => wrapper.Metadata[key]);

            if (obsoleteMetadataRemoved.Count < wrapper.Metadata.Count)
            {
                wrapper.Metadata = obsoleteMetadataRemoved;
                isMetadataDirty = true;
            }

            foreach (MetadataAttribute metadataAttribute in declaredAttributes)
            {
                if (wrapper.Metadata.ContainsKey(metadataAttribute.Name) == false)
                {
                    wrapper.Metadata[metadataAttribute.Name] = metadataAttribute.GetDefaultMetadata(drawnMemberInfo);
                    isMetadataDirty = true;
                }
                else if (metadataAttribute.IsMetadataValid(wrapper.Metadata[metadataAttribute.Name]) == false)
                {
                    wrapper.Metadata[metadataAttribute.Name] = metadataAttribute.GetDefaultMetadata(drawnMemberInfo);
                    isMetadataDirty = true;
                }
            }

            if (isMetadataDirty)
            {
                wrapperChangedCallback(wrapper);
            }

            ITrainingDrawer wrapperDrawer = DrawerLocator.GetDrawerForValue(wrapper, typeof(MetadataWrapper));

            GUIContent displayName = memberDrawer.GetLabel(drawnMemberInfo, ownerObject);
            return wrapperDrawer.Draw(rect, wrapper, wrapperChangedCallback, displayName).height;
        }

        /// <inheritdoc />
        public override GUIContent GetLabel(MemberInfo memberInfo, object memberOwner)
        {
            return MergeGuiContents(base.GetLabel(memberInfo, memberOwner), GetTypeNameLabel(ReflectionUtils.GetValueFromPropertyOrField(memberOwner, memberInfo), ReflectionUtils.GetDeclaredTypeOfPropertyOrField(memberInfo)));
        }

        /// <inheritdoc />
        public override GUIContent GetLabel(object value, Type declaredType)
        {
            return MergeGuiContents(base.GetLabel(value, declaredType), GetTypeNameLabel(value, declaredType));
        }

        protected virtual IEnumerable<MemberInfo> GetMembersToDraw(object value)
        {
            return EditorReflectionUtils.GetFieldsAndPropertiesToDraw(value);
        }

        private GUIContent MergeGuiContents(GUIContent name, GUIContent typeName)
        {
            GUIContent result;
            if (name == null || string.IsNullOrEmpty(name.text))
            {
                result = new GUIContent(string.Format("{0}", typeName.text))
                {
                    image = name.image
                };
            }
            else
            {
                result = new GUIContent(string.Format("{0} ({1})", name.text, typeName.text));
            }

            if (result.image == null)
            {
                result.image = typeName.image;
            }

            return new GUIContent(result);
        }

        protected virtual GUIContent GetTypeNameLabel(object value, Type declaredType)
        {
            Type actualType = declaredType;
            if (value != null)
            {
                actualType = value.GetType();
            }

            DisplayNameAttribute typeNameAttribute = actualType.GetAttributes<DisplayNameAttribute>(true).FirstOrDefault();
            if (typeNameAttribute != null)
            {
                return new GUIContent(typeNameAttribute.Name);
            }

            return new GUIContent(actualType.FullName);
        }
    }
}
