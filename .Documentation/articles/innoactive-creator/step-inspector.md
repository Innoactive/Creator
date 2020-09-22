# Step Inspector

The Step Inspector is a window where you can view and modify steps of your training course.

## Undo and Redo

You can undo and redo all changes in the [Workflow window](workflow-window.md) and Step Inspector by pressing `CTRL` + `Z` and `CTRL` + `Y` on the keyboard.

## Open the Step Inspector

To open the Step Inspector, click on any step in the [Workflow window](workflow-window.md).

[![Open the Step Inspector](../images/step-inspector/open-inspector.gif "How to open the Step Inspector.")](../images/step-inspector/open-inspector.gif)

## Change Name of a Step

To rename a step, click on it in the [Workflow window](workflow-window.md) and change the name in the `Step Name` field at the top of the Step Inspector.

[![Change name of a step](../images/step-inspector/rename-step.gif "How to change the name of a step.")](../images/step-inspector/rename-step.gif)

## Change Description of a Step

To change the description of a step, click on the step in the [Workflow window](workflow-window.md) and change the description in the `Description` field at the top of the Step Inspector.

[![Change description of a step](../images/step-inspector/change-description.gif "How to change the description of a step.")](../images/step-inspector/change-description.gif)

## Add and Remove Behaviors

To add a behavior, click on the `Behaviors` tab and the `Add Behavior` button in the Step Inspector. To remove one, click on the <img src="../images/step-inspector/icon_delete_dark.png" alt="Trash Can Icon" height="16px"/> button in the upper right corner of it.

Learn more about the [default behaviors](default-behaviors.md).

[![Add a behavior to a step](../images/step-inspector/add-behavior.gif "How to add a behavior to a step.")](../images/step-inspector/add-behavior.gif)

## Add and Remove Transitions

To add a transition in the Step Inspector, click on the `Transitions` tab and the `Add Transition` button. It will point to the end of the chapter by default. To choose another destination see the [Workflow window](workflow-window.md#add-transitions). 
You can remove a transition by clicking on the <img src="../images/step-inspector/icon_delete_dark.png" alt="Trash Can Icon" height="16px"/> button in the upper right corner.

[![Add a transition to a step](../images/step-inspector/add-transition.gif "How to add a transition to a step.")](../images/step-inspector/add-transition.gif)

## Add and Remove Conditions

To add a condition, click on the `Transitions` tab in the Step Inspector, find the transition you want to add it to, and click on the `Add Condition` button. To remove a condition, click on the <img src="../images/step-inspector/icon_delete_dark.png" alt="Trash Can Icon" height="16px"/> button in the upper right corner of it.

Learn more about the [default conditions](default-conditions.md).

[![Add a condition to a step](../images/step-inspector/add-condition.gif "How to add a condition to a step.")](../images/step-inspector/add-condition.gif)

## Reorder Behaviors, Transitions, and Conditions

To reorder a behavior, transition, or condition, click on the <img src="../images/step-inspector/icon_arrow_down_dark.png" alt="Arrow Down Icon" height="16px"/> button to move it down or on the <img src="../images/step-inspector/icon_arrow_up_dark.png" alt="Arrow Up Icon" height="16px"/> button to move it up.

Tip: You can change the priority of transitions by sorting them from top to bottom. If two or more transitions are completed at the same time, the highest one in the list will be prioritized.

[![Reorder transitions](../images/step-inspector/reorder-entities.gif "How to reorder behaviors, transitions, and conditions.")](../images/step-inspector/reorder-entities.gif)

## Unlock Objects in Current Step

By default, every property (Touching, Grabbing, Using, ...) on a training scene object that is not needed by a condition is locked (non-interactable) during a step. In the `Unlocked Objects` tab you can see which object properties are active and which are not. You can also manually drag in other training scene objects from the scene to manually unlock their properties per step.

Learn more about [Training Scene Objects and their properties](training-scene-object.md).

[![Unlock Objects](../images/step-inspector/unlock-objects.gif "How to unlock Training Scene Objects.")](../images/step-inspector/unlock-objects.gif)
