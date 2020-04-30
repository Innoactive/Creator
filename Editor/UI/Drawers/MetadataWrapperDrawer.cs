using System;
using System.Collections;
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
    /// This drawer receives a data structure which contains an actual object to draw and additional drawing information.
    /// It takes metadata entries one by one and recursively calls its Draw method, until no unprocessed metadata left.
    /// After that, an actual object is drawn.
    /// </summary>
    [DefaultTrainingDrawer(typeof(MetadataWrapper))]
    public class MetadataWrapperDrawer : AbstractDrawer
    {
        private readonly string separatedName = typeof(SeparatedAttribute).FullName;
        private readonly string deletableName = typeof(DeletableAttribute).FullName;
        private readonly string foldableName = typeof(FoldableAttribute).FullName;
        private readonly string drawIsBlockingToggleName = typeof(DrawIsBlockingToggleAttribute).FullName;
        private readonly string listOfName = typeof(ListOfAttribute).FullName;
        private readonly string extendableListName = typeof(ExtendableListAttribute).FullName;
        private readonly string keepPopulatedName = typeof(KeepPopulatedAttribute).FullName;

        /// <inheritdoc />
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            MetadataWrapper wrapper = (MetadataWrapper) currentValue;

            if (wrapper.Metadata.ContainsKey(separatedName))
            {
                return DrawSeparated(rect, wrapper, changeValueCallback, label);
            }

            if (wrapper.Metadata.ContainsKey(deletableName))
            {
                return DrawDeletable(rect, wrapper, changeValueCallback, label);
            }

            if (wrapper.Metadata.ContainsKey(foldableName))
            {
                return DrawFoldable(rect, wrapper, changeValueCallback, label);
            }

            if (wrapper.Metadata.ContainsKey(drawIsBlockingToggleName))
            {
                return DrawIsBlockingToggle(rect, wrapper, changeValueCallback, label);
            }

            if (wrapper.Metadata.ContainsKey(extendableListName))
            {
                return DrawExtendableList(rect, wrapper, changeValueCallback, label);
            }

            if (wrapper.Metadata.ContainsKey(keepPopulatedName))
            {
                return HandleKeepPopulated(rect, wrapper, changeValueCallback, label);
            }

            if (wrapper.Metadata.ContainsKey(listOfName))
            {
                return DrawListOf(rect, wrapper, changeValueCallback, label);
            }

            throw new NotImplementedException("Wrapper drawer for this kind of metadata is not implemented.");
        }

        /// <inheritdoc />
        public override GUIContent GetLabel(MemberInfo memberInfo, object memberOwner)
        {
            return GetLabel(ReflectionUtils.GetValueFromPropertyOrField(memberOwner, memberInfo), ReflectionUtils.GetDeclaredTypeOfPropertyOrField(memberInfo));
        }

        /// <inheritdoc />
        public override GUIContent GetLabel(object value, Type declaredType)
        {
            // Assert that value is never null, as we always call MetadataWrapper on freshly created instance.
            MetadataWrapper wrapper = value as MetadataWrapper;
            ITrainingDrawer valueDrawer = DrawerLocator.GetDrawerForValue(wrapper.Value, wrapper.ValueDeclaredType);

            return valueDrawer.GetLabel(wrapper.Value, wrapper.ValueDeclaredType);
        }

        private Rect DrawSeparated(Rect rect, MetadataWrapper wrapper, Action<object> changeValueCallback, GUIContent label)
        {
            EditorDrawingHelper.DrawRect(new Rect(0f, rect.y - 1f, rect.x + rect.width, 1f), Color.grey);

            Rect wrappedRect = rect;
            wrappedRect.y += EditorDrawingHelper.VerticalSpacing;

            wrappedRect = DrawRecursively(wrappedRect, wrapper, separatedName, changeValueCallback, label);

            wrappedRect.height += EditorDrawingHelper.VerticalSpacing;

            EditorDrawingHelper.DrawRect(new Rect(0f, wrappedRect.y + wrappedRect.height - 1f, wrappedRect.x + wrappedRect.width, 1f), Color.grey);

            rect.height = wrappedRect.height;
            return rect;
        }

        private Rect DrawDeletable(Rect rect, MetadataWrapper wrapper, Action<object> changeValueCallback, GUIContent label)
        {
            rect = DrawRecursively(rect, wrapper, deletableName, changeValueCallback, label);

            Vector2 buttonSize = new Vector2(EditorGUIUtility.singleLineHeight + 3, EditorDrawingHelper.SingleLineHeight);

            GUIStyle style = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold
            };

            if (GUI.Button(new Rect(rect.x + rect.width - buttonSize.x, rect.y, buttonSize.x, buttonSize.y), "x", style))
            {
                object oldValue = wrapper.Value;
                ChangeValue(() =>
                    {
                        wrapper.Value = null;
                        return wrapper;
                    },
                    () =>
                    {
                        wrapper.Value = oldValue;
                        return wrapper;
                    },
                    changeValueCallback);
            }

            return rect;
        }

        private Rect DrawFoldable(Rect rect, MetadataWrapper wrapper, Action<object> changeValueCallback, GUIContent label)
        {
            if (wrapper.Metadata[foldableName] == null)
            {
                wrapper.Metadata[foldableName] = true;
                changeValueCallback(wrapper);
            }

            bool oldIsFoldedOutValue = (bool) wrapper.Metadata[foldableName];

            GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 12
            };

            GUIStyle labelStyle = new GUIStyle(EditorStyles.label)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 12
            };

            Rect foldoutRect = new Rect(rect.x, rect.y, rect.width, EditorDrawingHelper.HeaderLineHeight);

            bool newIsFoldedOutValue = EditorDrawingHelper.DrawFoldoutWithReducedFocusArea(foldoutRect, oldIsFoldedOutValue, oldIsFoldedOutValue ? new GUIContent() : label, foldoutStyle, labelStyle);

            if (newIsFoldedOutValue != oldIsFoldedOutValue)
            {
                wrapper.Metadata[foldableName] = newIsFoldedOutValue;
                changeValueCallback(wrapper);
            }


            // Collapsed
            if (newIsFoldedOutValue == false)
            {
                rect.height = EditorDrawingHelper.HeaderLineHeight;
                return rect;
            }

            rect.height = 0f;

            Rect wrappedRect = rect;
            wrappedRect.x += EditorDrawingHelper.IndentationWidth;
            wrappedRect.width -= EditorDrawingHelper.IndentationWidth;

            return DrawRecursively(wrappedRect, wrapper, foldableName, (newWrapper) =>
            {
                // We want the user to be aware that value has changed even if the foldable was collapsed (for example, undo/redo).
                wrapper.Metadata[foldableName] = true;
                changeValueCallback(wrapper);
            }, label);
        }

        private Rect DrawIsBlockingToggle(Rect rect, MetadataWrapper wrapper, Action<object> changeValueCallback, GUIContent label)
        {
            IDataOwner dataOwner = wrapper.Value as IDataOwner;

            rect = DrawRecursively(rect, wrapper, drawIsBlockingToggleName, changeValueCallback, label);

            if (dataOwner == null)
            {
                Debug.LogError("The target property of the DrawIsBlockingToggleAttribute has to implement IDataOwner.");
                return rect;
            }

            IBackgroundBehaviorData backgroundBehaviorData = dataOwner.Data as IBackgroundBehaviorData;

            if (backgroundBehaviorData == null)
            {
                return rect;
            }

            ITrainingDrawer boolDrawer = DrawerLocator.GetDrawerForValue(backgroundBehaviorData.IsBlocking, typeof(bool));
            rect.height += boolDrawer.Draw(new Rect(rect.x, rect.y + rect.height, rect.width, 0), backgroundBehaviorData.IsBlocking, (newValue) =>
            {
                backgroundBehaviorData.IsBlocking = (bool)newValue;
                changeValueCallback(wrapper);
            }, "Is blocking").height;

            return rect;
        }

        private Rect DrawExtendableList(Rect rect, MetadataWrapper wrapper, Action<object> changeValueCallback, GUIContent label)
        {
            if (wrapper.Value == null || wrapper.Value is IList == false)
            {
                if (wrapper.Value != null)
                {
                    Debug.LogWarning("ExtendableListAttribute can be used only with IList members.");
                }

                return rect;
            }

            Type elementType = (wrapper.Metadata[extendableListName] as ExtendableListAttribute.SerializedTypeWrapper).Type;

            IList list = (IList) wrapper.Value;

            float currentY = 0;

            currentY += DrawRecursively(rect, wrapper, extendableListName, changeValueCallback, label).height;

            ITrainingDrawer addThingsDrawer = DrawerLocator.GetInstantiatorDrawer(elementType);

            if (addThingsDrawer != null)
            {
                currentY += addThingsDrawer.Draw(new Rect(rect.x, rect.y + currentY, rect.width, 0), null, (newValue) =>
                {
                    if (newValue == null)
                    {
                        ReflectionUtils.RemoveFromList(ref list, list.Count - 1);
                    }
                    else
                    {
                        ReflectionUtils.InsertIntoList(ref list, list.Count, newValue);
                    }

                    if (wrapper.Metadata.ContainsKey(listOfName))
                    {
                        ListOfAttribute.Metadata temp = (ListOfAttribute.Metadata) wrapper.Metadata[listOfName];
                        temp.ChildMetadata.Add(temp.ChildAttributes.ToDictionary(attribute => attribute.Name, attribute => attribute.GetDefaultMetadata(null)));
                        wrapper.Metadata[listOfName] = temp;
                    }

                    wrapper.Value = list;
                    changeValueCallback(wrapper);
                }, "").height;
            }

            rect.height = currentY;
            return rect;
        }

        private Rect HandleKeepPopulated(Rect rect, MetadataWrapper wrapper, Action<object> changeValueCallback, GUIContent label)
        {
            if (wrapper.Value == null || (wrapper.Value is IList == false))
            {
                if (wrapper.Value != null)
                {
                    Debug.LogWarning("KeepPopulated can be used only with IList members.");
                }

                return rect;
            }

            IList list = (IList) wrapper.Value;

            if (list.Count == 0)
            {
                Type entryType = (Type) wrapper.Metadata[keepPopulatedName];
                if (entryType != null)
                {
                    Type listType = ReflectionUtils.GetEntryType(list);
                    if (listType.IsAssignableFrom(entryType))
                    {
                        ReflectionUtils.InsertIntoList(ref list, 0, ReflectionUtils.CreateInstanceOfType(entryType));
                    }
                    else
                    {
                        Debug.LogErrorFormat("Trying to add an keep populuated entry with type {0} to list filled {1}", entryType.Name, listType.Name);
                    }
                }
                else
                {
                    Debug.LogError("No Type found to create default instance with");
                }
            }

            return DrawRecursively(rect, wrapper, keepPopulatedName, changeValueCallback, label);
        }

        private Rect DrawListOf(Rect rect, MetadataWrapper wrapper, Action<object> changeValueCallback, GUIContent label)
        {
            if (wrapper.Value == null || (wrapper.Value is IList == false))
            {
                if (wrapper.Value != null)
                {
                    Debug.LogWarning("ListOfAttribute can be used only with IList members.");
                }

                return rect;
            }

            ListOfAttribute.Metadata wrapperMetadata = (wrapper.Metadata[listOfName] as ListOfAttribute.Metadata);
            List<Dictionary<string, object>> listOfMetadatas = wrapperMetadata.ChildMetadata;

            IList list = (IList) wrapper.Value;

            if (listOfMetadatas == null)
            {
                listOfMetadatas = new List<Dictionary<string, object>>(list.Count);
            }

            if (listOfMetadatas.Count != list.Count)
            {
                listOfMetadatas.Clear();
                for (int i = 0; i < list.Count; i++)
                {
                    listOfMetadatas.Add(wrapperMetadata.ChildAttributes.ToDictionary(attribute => attribute.Name, attribute => attribute.GetDefaultMetadata(null)));
                }
            }

            Type entryType = ReflectionUtils.GetEntryType(wrapper.Value);

            List<MetadataWrapper> listOfWrappers = new List<MetadataWrapper>();
            for (int i = 0; i < list.Count; i++)
            {
                listOfWrappers.Add(new MetadataWrapper()
                {
                    Metadata = listOfMetadatas[i],
                    ValueDeclaredType = entryType,
                    Value = list[i],
                });
            }

            if (wrapper.Metadata.Count > 1)
            {
                throw new NotImplementedException("ListOf attribute should have the lowest priority. Check MetadataWrapperDrawer.Draw method.");
            }

            ITrainingDrawer valueDrawer = DrawerLocator.GetDrawerForValue(wrapper.Value, wrapper.ValueDeclaredType);

            return valueDrawer.Draw(rect, listOfWrappers, (newValue) =>
            {
                List<MetadataWrapper> newListOfWrappers = ((List<MetadataWrapper>) newValue).ToList();
                ReflectionUtils.ReplaceList(ref list, newListOfWrappers.Select(childWrapper => childWrapper.Value));
                wrapper.Value = list;

                (wrapper.Metadata[listOfName] as ListOfAttribute.Metadata).ChildMetadata = newListOfWrappers.Select(childWrapper => childWrapper.Metadata).ToList();
                changeValueCallback(wrapper);
            }, label);
        }

        private Rect DrawRecursively(Rect rect, MetadataWrapper wrapper, string currentDrawerName, Action<object> changeValueCallback, GUIContent label)
        {
            // There are more metadata information to handle, pass it to the next iteration.
            if (wrapper.Metadata.Count > 1)
            {
                rect = DrawWrapperRecursively(rect, wrapper, changeValueCallback, currentDrawerName, label);
            }
            else
            {
                // Draw an actual object.
                ITrainingDrawer valueDrawer = DrawerLocator.GetDrawerForValue(wrapper.Value, wrapper.ValueDeclaredType);

                Action<object> valueChanged = (newValue) =>
                {
                    wrapper.Value = newValue;
                    changeValueCallback(wrapper);
                };

                rect = valueDrawer.Draw(rect, wrapper.Value, valueChanged, label);
            }

            return rect;
        }

        private Rect DrawWrapperRecursively(Rect rect, MetadataWrapper parentWrapper, Action<object> changeValueCallback, string removedMetadataName, GUIContent label)
        {
            MetadataWrapper wrappedWrapper = new MetadataWrapper()
            {
                Value = parentWrapper.Value,
                ValueDeclaredType = parentWrapper.ValueDeclaredType,
                Metadata = parentWrapper.Metadata.Where(kvp => kvp.Key != removedMetadataName).ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
            };
            Action<object> wrappedWrapperChanged = (newValue) =>
            {
                MetadataWrapper newWrapper = (MetadataWrapper)newValue;

                foreach (string key in newWrapper.Metadata.Keys)
                {
                    parentWrapper.Metadata[key] = wrappedWrapper.Metadata[key];
                }

                parentWrapper.Value = newWrapper.Value;

                changeValueCallback(parentWrapper);
            };
            rect.height = Draw(rect, wrappedWrapper, wrappedWrapperChanged, label).height;
            return rect;
        }
    }
}
