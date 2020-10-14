using System;
using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Configuration;
using Innoactive.CreatorEditor.CourseValidation;
using Innoactive.CreatorEditor.UI.Graphics;
using Innoactive.CreatorEditor.UndoRedo;
using Innoactive.CreatorEditor.Utils;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Windows
{
    internal class ChapterRepresentation
    {
        public EditorGraphics Graphics { get; private set; }

        private IChapter currentChapter;
        private StepNode lastSelectedStepNode;

        private WorkflowEditorGrid grid;
        private float gridCellSize = 10f;

        private bool isUpdated = false;

        public Rect BoundingBox
        {
            get { return Graphics.BoundingBox; }
        }

        public ChapterRepresentation()
        {
            Graphics = new EditorGraphics(WorkflowEditorColorPalette.GetDefaultPalette());
        }

        private void SetupNode(EditorNode node, Action<Vector2> setPositionInModel)
        {
            Vector2 positionBeforeDrag = node.Position;
            Vector2 deltaOnPointerDown = Vector2.zero;

            node.GraphicalEventHandler.PointerDown += (sender, args) =>
            {
                positionBeforeDrag = node.Position;
                deltaOnPointerDown = node.Position - args.PointerPosition;
            };

            node.GraphicalEventHandler.PointerUp += (sender, args) =>
            {
                if (Mathf.Abs((positionBeforeDrag - node.Position).sqrMagnitude) < 0.001f)
                {
                    return;
                }

                Vector2 positionAfterDrag = node.Position;
                Vector2 closuredPositionBeforeDrag = positionBeforeDrag;

                RevertableChangesHandler.Do(new CourseCommand(() =>
                {
                    setPositionInModel(positionAfterDrag);
                    MarkToRefresh();
                }, () =>
                {
                    setPositionInModel(closuredPositionBeforeDrag);
                    MarkToRefresh();
                }));
            };

            node.GraphicalEventHandler.PointerDrag += (sender, args) =>
            {
                SetNewPositionOnGrid(node, args.PointerPosition, deltaOnPointerDown);
            };
        }

        /// Clamps the position of the node on the background grid.
        private void SetNewPositionOnGrid(EditorNode node, Vector2 position, Vector2 delta)
        {
            // Add original delta pointer position onto the absolute position of the node.
            node.Position = position + delta;
            Vector2 newPos = node.RelativePosition;

            // Calculate x and y offset dependent on the size of the grid cells.
            float xOffset = newPos.x % gridCellSize;
            float addedX = xOffset < gridCellSize / 2 ? -xOffset : gridCellSize - xOffset;

            float yOffset = newPos.y % gridCellSize;
            float addedY = yOffset < gridCellSize / 2 ? -yOffset : gridCellSize - yOffset;

            // Add offsets and subtract bounding box offsets
            newPos += new Vector2(addedX, addedY);
            newPos -= new Vector2(node.LocalBoundingBox.x % gridCellSize, node.LocalBoundingBox.y % gridCellSize);

            node.RelativePosition = newPos;
        }

        private void DeleteStepWithUndo(IStep step, StepNode ownerNode)
        {
            IList<ITransition> incomingTransitions = currentChapter.Data.Steps.SelectMany(s => s.Data.Transitions.Data.Transitions).Where(transition => transition.Data.TargetStep == step).ToList();

            bool wasFirstStep = step == currentChapter.Data.FirstStep;

            RevertableChangesHandler.Do(new CourseCommand(
                () =>
                {
                    foreach (ITransition transition in incomingTransitions)
                    {
                        transition.Data.TargetStep = null;
                    }

                    DeleteStep(step);

                    if (wasFirstStep)
                    {
                        currentChapter.Data.FirstStep = null;
                    }
                },
                () =>
                {
                    AddStep(step);

                    if (wasFirstStep)
                    {
                        currentChapter.Data.FirstStep = step;
                    }

                    foreach (ITransition transition in incomingTransitions)
                    {
                        transition.Data.TargetStep = step;
                    }

                    SelectStepNode(ownerNode);
                }
            ));
        }

        private StepNode CreateNewStepNode(IStep step)
        {
            StepNode node = new StepNode(Graphics, currentChapter, step);

            node.GraphicalEventHandler.ContextClick += (sender, args) =>
            {
                TestableEditorElements.DisplayContextMenu(new List<TestableEditorElements.MenuOption>
                {
                    new TestableEditorElements.MenuItem(new GUIContent("Copy"), false, () =>
                    {
                        CopyStep(step);
                    }),
                    new TestableEditorElements.MenuItem(new GUIContent("Cut"), false, () =>
                    {
                        CutStep(step, node);
                    }),
                    new TestableEditorElements.MenuItem(new GUIContent("Delete"), false, () =>
                    {
                        DeleteStepWithUndo(step, node);
                    })
                });
            };

            node.GraphicalEventHandler.PointerDown += (sender, args) =>
            {
                UserSelectStepNode(node);
            };

            node.RelativePositionChanged += (sender, args) =>
            {
                node.Step.StepMetadata.Position = node.Position;
            };

            node.GraphicalEventHandler.PointerUp += (sender, args) =>
            {
                Graphics.CalculateBoundingBox();
            };

            // ReSharper disable once ImplicitlyCapturedClosure
            node.GraphicalEventHandler.PointerDown += (sender, args) => UserSelectStepNode(node);

            node.CreateTransitionButton.GraphicalEventHandler.PointerClick += (sender, args) =>
            {
                ITransition transition = EntityFactory.CreateTransition();

                RevertableChangesHandler.Do(new CourseCommand(
                    () =>
                    {
                        step.Data.Transitions.Data.Transitions.Add(transition);
                        MarkToRefresh();
                    },
                    () =>
                    {
                        step.Data.Transitions.Data.Transitions.Remove(transition);
                        MarkToRefresh();
                    }
                ));
            };

            if (currentChapter.ChapterMetadata.LastSelectedStep == step)
            {
                SelectStepNode(node);
            }

            SetupNode(node, position => node.Step.StepMetadata.Position = position);

            return node;
        }

        private void SelectStepNode(StepNode stepNode)
        {
            IStep step = stepNode == null ? null : stepNode.Step;

            if (lastSelectedStepNode != null)
            {
                lastSelectedStepNode.IsLastSelectedStep = false;
            }

            lastSelectedStepNode = stepNode;
            currentChapter.ChapterMetadata.LastSelectedStep = step;

            if (stepNode != null)
            {
                stepNode.IsLastSelectedStep = true;
            }

            GlobalEditorHandler.ChangeCurrentStep(step);
        }

        private void UserSelectStepNode(StepNode stepNode)
        {
            SelectStepNode(stepNode);
            Graphics.BringToTop(stepNode);
            GlobalEditorHandler.StartEditingStep();
        }

        private void MarkToRefresh()
        {
            isUpdated = false;
        }

        private EntryNode CreateEntryNode(IChapter chapter)
        {
            EntryNode entryNode = new EntryNode(Graphics);

            ExitJoint joint = new ExitJoint(Graphics, entryNode)
            {
                RelativePosition = new Vector2(entryNode.LocalBoundingBox.xMax, entryNode.LocalBoundingBox.center.y),
            };

            entryNode.ExitJoints.Add(joint);

            entryNode.Position = chapter.ChapterMetadata.EntryNodePosition;

            entryNode.RelativePositionChanged += (sender, args) =>
            {
                chapter.ChapterMetadata.EntryNodePosition = entryNode.Position;
            };

            entryNode.GraphicalEventHandler.PointerUp += (sender, args) =>
            {
                Graphics.CalculateBoundingBox();
            };

            entryNode.GraphicalEventHandler.ContextClick += (sender, args) =>
            {
                if (chapter.Data.FirstStep == null)
                {
                    return;
                }

                TestableEditorElements.DisplayContextMenu(new List<TestableEditorElements.MenuOption>
                {
                    new TestableEditorElements.MenuItem(new GUIContent("Delete transition"), false, () =>
                    {
                        IStep firstStep = chapter.Data.FirstStep;

                        RevertableChangesHandler.Do(new CourseCommand(() =>
                            {
                                chapter.Data.FirstStep = null;
                                MarkToRefresh();
                            },
                            () =>
                            {
                                chapter.Data.FirstStep = firstStep;
                                MarkToRefresh();
                            }
                        ));
                    })
                });
            };

            joint.GraphicalEventHandler.PointerDrag += (sender, args) =>
            {
                joint.DragDelta = args.PointerPosition - joint.Position;
            };

            joint.GraphicalEventHandler.PointerUp += (sender, args) =>
            {
                joint.DragDelta = Vector2.zero;
                IStep oldStep = chapter.Data.FirstStep;

                if (TryGetStepForTransitionDrag(args.PointerPosition, out IStep target) == false)
                {
                    return;
                }

                RevertableChangesHandler.Do(new CourseCommand(() =>
                    {
                        chapter.Data.FirstStep = target;
                        MarkToRefresh();
                    },
                    () =>
                    {
                        chapter.Data.FirstStep = oldStep;
                        MarkToRefresh();
                    }
                ));

                joint.DragDelta = Vector2.zero;
            };

            SetupNode(entryNode, position => chapter.ChapterMetadata.EntryNodePosition = position);

            return entryNode;
        }

        private void SetupTransitions(IChapter chapter, EntryNode entryNode, IDictionary<IStep, StepNode> stepNodes)
        {
            if (chapter.Data.FirstStep != null)
            {
                CreateNewTransition(entryNode.ExitJoints.First(), stepNodes[chapter.Data.FirstStep].EntryJoints.First());
            }

            foreach (IStep step in stepNodes.Keys)
            {
                foreach (ITransition transition in step.Data.Transitions.Data.Transitions)
                {
                    ExitJoint joint = stepNodes[step].AddExitJoint();
                    if (transition.Data.TargetStep != null)
                    {
                        StepNode target = stepNodes[transition.Data.TargetStep];
                        CreateNewTransition(joint, target.EntryJoints.First());
                    }

                    IStep closuredStep = step;
                    ITransition closuredTransition = transition;
                    int transitionIndex = step.Data.Transitions.Data.Transitions.IndexOf(closuredTransition);

                    joint.GraphicalEventHandler.PointerDrag += (sender, args) =>
                    {
                        joint.DragDelta = args.PointerPosition - joint.Position;
                    };

                    joint.GraphicalEventHandler.PointerUp += (sender, args) =>
                    {
                        joint.DragDelta = Vector2.zero;
                        IStep oldStep = closuredTransition.Data.TargetStep;

                        if (TryGetStepForTransitionDrag(args.PointerPosition, out IStep targetStep) == false)
                        {
                            return;
                        }

                        RevertableChangesHandler.Do(new CourseCommand(() =>
                            {
                                closuredTransition.Data.TargetStep = targetStep;
                                SelectStepNode(stepNodes[closuredStep]);
                                MarkToRefresh();
                            },
                            () =>
                            {
                                closuredTransition.Data.TargetStep = oldStep;
                                SelectStepNode(stepNodes[closuredStep]);
                                MarkToRefresh();
                            }
                        ));
                    };

                    joint.GraphicalEventHandler.ContextClick += (sender, args) =>
                    {
                        TestableEditorElements.DisplayContextMenu(new List<TestableEditorElements.MenuOption>
                        {
                            new TestableEditorElements.MenuItem(new GUIContent("Delete transition"), false, () =>
                            {
                                bool isLast = closuredStep.Data.Transitions.Data.Transitions.Count == 1;
                                RevertableChangesHandler.Do(new CourseCommand(() =>
                                    {
                                        closuredStep.Data.Transitions.Data.Transitions.Remove(closuredTransition);
                                        if (isLast)
                                        {
                                            closuredStep.Data.Transitions.Data.Transitions.Add(EntityFactory.CreateTransition());
                                        }

                                        MarkToRefresh();
                                    },
                                    () =>
                                    {
                                        if (isLast)
                                        {
                                            closuredStep.Data.Transitions.Data.Transitions.RemoveAt(0);
                                        }

                                        closuredStep.Data.Transitions.Data.Transitions.Insert(transitionIndex, closuredTransition);
                                        MarkToRefresh();
                                    }
                                ));
                            })
                        });
                    };
                }
            }
        }

        private bool TryGetStepForTransitionDrag(Vector2 pointerPosition, out IStep step)
        {
            step = null;

            GraphicalElement elementUnderCursor = Graphics.GetGraphicalElementWithHandlerAtPoint(pointerPosition).FirstOrDefault();

            if (elementUnderCursor is EntryJoint endJoint)
            {
                if (endJoint.Parent is StepNode stepNode)
                {
                    step = stepNode.Step;
                }

                return true;
            }
            else if (elementUnderCursor is StepNode stepNode)
            {
                step = stepNode.Step;
                return true;
            }
            else
            {
                return elementUnderCursor == null;
            }
        }

        private IDictionary<IStep, StepNode> SetupSteps(IChapter chapter)
        {
            return chapter.Data.Steps.OrderBy(step => step == chapter.ChapterMetadata.LastSelectedStep).ToDictionary(step => step, CreateNewStepNode);
        }

        private void DeleteStep(IStep step)
        {
            if (currentChapter.ChapterMetadata.LastSelectedStep == step)
            {
                currentChapter.ChapterMetadata.LastSelectedStep = null;
                GlobalEditorHandler.ChangeCurrentStep(null);
            }

            currentChapter.Data.Steps.Remove(step);
            MarkToRefresh();
        }

        private void AddStep(IStep step)
        {
            currentChapter.Data.Steps.Add(step);

            MarkToRefresh();
        }

        private void CreateNewTransition(ExitJoint from, EntryJoint to)
        {
            TransitionElement transitionElement = new TransitionElement(Graphics, from, to);
            transitionElement.RelativePosition = Vector2.zero;
        }

        private void AddStepWithUndo(IStep step)
        {
            RevertableChangesHandler.Do(new CourseCommand(() =>
                {
                    AddStep(step);
                    currentChapter.ChapterMetadata.LastSelectedStep = step;
                },
                () =>
                {
                    DeleteStep(step);
                }
            ));
        }

        private void HandleCanvasContextClick(object sender, PointerGraphicalElementEventArgs e)
        {
            IList<TestableEditorElements.MenuOption> options = new List<TestableEditorElements.MenuOption>();

            options.Add(new TestableEditorElements.MenuItem(new GUIContent("Add step"), false, () =>
            {
                IStep step = EntityFactory.CreateStep("New Step");
                step.StepMetadata.Position = e.PointerPosition;
                AddStepWithUndo(step);
            }));

            if (SystemClipboard.IsStepInClipboard())
            {
                options.Add(new TestableEditorElements.MenuItem(new GUIContent("Paste step"), false, () =>
                {
                    Paste(e.PointerPosition);
                }));
            }
            else
            {
                options.Add(new TestableEditorElements.DisabledMenuItem(new GUIContent("Paste step")));
            }

            TestableEditorElements.DisplayContextMenu(options);
        }

        public void SetChapter(IChapter chapter)
        {
            currentChapter = chapter;

            Graphics.Reset();

            grid = new WorkflowEditorGrid(Graphics, gridCellSize);

            Graphics.Canvas.ContextClick += HandleCanvasContextClick;

            EntryNode entryNode = CreateEntryNode(chapter);
            IDictionary<IStep, StepNode> stepNodes = SetupSteps(chapter);
            SetupTransitions(chapter, entryNode, stepNodes);

            Graphics.CalculateBoundingBox();

            if (ValidationHandler.IsAllowedToValidate())
            {
                ValidationHandler.Instance.Validate(currentChapter.Data, GlobalEditorHandler.GetCurrentCourse(), null);
            }
        }

        public void HandleEvent(Event current, Rect controlRect)
        {
            if (isUpdated == false)
            {
                SetChapter(currentChapter);
                isUpdated = true;
            }

            grid.SetSize(controlRect);

            Graphics.HandleEvent(current, controlRect);
        }

        private bool CopyStep(IStep step)
        {
            if (step == null)
            {
                return false;
            }

            try
            {
                SystemClipboard.CopyStep(step);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Copies the selected step into the system's copy buffer.
        /// </summary>
        /// <returns>True if successful.</returns>
        public bool CopySelected()
        {
            IStep step = currentChapter.ChapterMetadata.LastSelectedStep;
            return CopyStep(step);
        }

        private bool CutStep(IStep step, StepNode owner)
        {
            if (CopyStep(step))
            {
                DeleteStepWithUndo(step, owner);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Cuts the selected step into the system's copy buffer from the chapter.
        /// </summary>
        /// <returns>True if successful.</returns>
        public bool CutSelected()
        {
            IStep step = currentChapter.ChapterMetadata.LastSelectedStep;
            return CutStep(step, lastSelectedStepNode);
        }

        /// <summary>
        /// Pastes the step from the system's copy buffer into the chapter at given <paramref name="position"/>.
        /// </summary>
        /// <returns>True if successful.</returns>
        public bool Paste(Vector2 position)
        {
            IStep step;
            try
            {
                step = SystemClipboard.PasteStep();

                if (step == null)
                {
                    return false;
                }

                step.Data.Name = "Copy of " + step.Data.Name;

                step.StepMetadata.Position = position - new Vector2(0f, step.Data.Transitions.Data.Transitions.Count * 20f / 2f);
            }
            catch
            {
                return false;
            }

            AddStepWithUndo(step);

            return true;
        }

        /// <summary>
        /// Deletes the selected step from the chapter.
        /// </summary>
        /// <returns>True if successful.</returns>
        public bool DeleteSelected()
        {
            IStep step = currentChapter.ChapterMetadata.LastSelectedStep;
            if (step == null)
            {
                return false;
            }

            DeleteStepWithUndo(step, lastSelectedStepNode);
            return true;
        }
    }
}
