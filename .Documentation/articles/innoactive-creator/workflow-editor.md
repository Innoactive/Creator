# Workflow Editor

You can view and modify your training course in the Workflow Editor window. On the right, there is the workflow of the selected chapter, and you can select or add chapters in the menu on the left. New chapters have only a starting point and nothing else.

[![Empty Workflow Editor](../images/workflow-editor/empty-editor.png "")](../images/workflow-editor/empty-editor.png)

## Undo and redo

You can undo and redo all changes in the Workflow Editor and [Step Inspector](step-inspector.md) by pressing `CTRL` + `Z` and `CTRL` + `Y` on the keyboard.

## Training Name

You can change the displayed name of the training course on the top of the chapter menu. Changing this name does not change the filename of the training course.

## Save button

You can save any changes you made in the Workflow Editor by clicking the save button in the upper left corner. They will be saved in the file of the currently opened training course.

## Unsaved changes

If you have unsaved changes, then the red text in the upper right corner appears. It disappears as soon as you save your changes. Make sure you have saved your changes before testing them in Unity's `Play Mode`.

## Hide and show the chapter menu

To hide the left part of the Workflow Editor click on the `<<` icon above the name field. To show it again click on the `>>` icon on the left.

[![Change chapter](../images/workflow-editor/hide-and-open-left-part.gif "")](../images/workflow-editor/hide-and-open-left-part.gif)

## Chapters

You can separate your training course into multiple chapters. Each chapter has its own workflow and will start where the previous chapter has ended.

[![Change chapter](../images/workflow-editor/change-chapter.gif "")](../images/workflow-editor/change-chapter.gif)

### Add a chapter

You can add a chapter by clicking on the `+Add Chapter` button under the list of chapters.

### Modify a chapter

Next to each chapter are four different icons. These are buttons that can be used to modify the chapters.

[![Change chapter](../images/workflow-editor/chapter-buttons.png "")](../images/workflow-editor/chapter-buttons.png)

They offer the following functionality (from left to right):

- `Move up`: Move the chapter one step up.
- `Move down`: Move the chapter one step down.
- `Delete`: Delete the chapter.
- `Edit name`: Change the name of the chapter.

## Chapter Workflow

The workflow of a chapter at least consists of a small starting point. It can be extended by steps and transitions.

### Add steps

The workflow of a new chapter has only a starting point. To add a step, click with the right mouse button anywhere on the empty area and choose the `Add Step` option.  

[![Add a step](../images/workflow-editor/create-step.gif "")](../images/workflow-editor/create-step.gif)

### Remove steps

To remove a step, click on the step with the right mouse button and choose the `Delete Step` option.

[![Remove a step](../images/workflow-editor/remove-step.gif "")](../images/workflow-editor/remove-step.gif)

### Move steps around

You can drag a step around the canvas with the left mouse button.

[![Move a step around](../images/workflow-editor/move-step.gif "")](../images/workflow-editor/move-step.gif)

### Add transitions

To add a transition to a step, click on the white circle with the `+` sign right of the step. To connect a step to another or to the starting point, drag the transition origin (a white circle next to the origin object) to the transition target (the white circle with a `>` sign on the left of the target object).

> Make sure the last step has an outgoing transition with no target step. As it has no target step, it will lead to the end of the chapter.

[![Add and connect transitions](../images/workflow-editor/add-and-connect-transitions.gif "")](../images/workflow-editor/add-and-connect-transitions.gif)
