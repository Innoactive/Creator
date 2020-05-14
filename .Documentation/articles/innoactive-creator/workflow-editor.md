# Workflow Editor

You can view and modify your training course in the Workflow Editor window. On the right, there is the workflow of the selected chapter, and you can select or add chapters in the menu on the left. New chapters have only a starting point and nothing else.

[![Empty Workflow Editor](../images/workflow-editor/empty-editor.png "")](../images/workflow-editor/empty-editor.png)

## Undo and Redo

You can undo and redo all changes in the Workflow Editor and [Step Inspector](step-inspector.md) by pressing `CTRL` + `Z` and `CTRL` + `Y` on the keyboard.

## Training Name

Click the pen icon on the top of the chapter menu to rename the current training course. When you do so, the Innoactive creator will save the current state of the course to the new file, using the new name as the filename. It will delete the old file.

## Saving

The Workflow Editor and [Step Inspector](step-inspector.md) automatically save all changes you make to the file of the currently opened training course.

## Hide and Show the Chapter Menu

To hide the left part of the Workflow Editor click on the `<<` icon above the name field. To show it again click on the `>>` icon on the left.

[![Change chapter](../images/workflow-editor/hide-and-open-left-part.gif "")](../images/workflow-editor/hide-and-open-left-part.gif)

## Chapters

You can separate your training course into multiple chapters. Each chapter has its own workflow and will start where the previous chapter has ended.

[![Change chapter](../images/workflow-editor/change-chapter.gif "")](../images/workflow-editor/change-chapter.gif)

### Add a Chapter

You can add a chapter by clicking on the `+Add Chapter` button under the list of chapters.

### Modify a Chapter

Next to each chapter are four different icons. These are buttons that can be used to modify the chapters.

[![Change chapter](../images/workflow-editor/chapter-buttons.png "")](../images/workflow-editor/chapter-buttons.png)

They offer the following functionality (from left to right):

- `Move up`: Move the chapter one step up.
- `Move down`: Move the chapter one step down.
- `Delete`: Delete the chapter.
- `Edit name`: Change the name of the chapter.

## Chapter Workflow

The workflow of a chapter at least consists of a small starting point. It can be extended by steps and transitions.

### Add Steps

The workflow of a new chapter has only a starting point. To add a step, click with the right mouse button anywhere on the empty area and choose the `Add step` option.  

[![Add a step](../images/workflow-editor/create-step.gif "")](../images/workflow-editor/create-step.gif)

### Remove Steps

To remove a step, click on the step with the right mouse button and choose the `Delete` option.

[![Remove a step](../images/workflow-editor/remove-step.gif "")](../images/workflow-editor/remove-step.gif)

### Move Steps Around

You can drag a step around the canvas with the left mouse button.

[![Move a step around](../images/workflow-editor/move-step.gif "")](../images/workflow-editor/move-step.gif)

### Copy, Cut, and Paste Steps

You can copy a step by clicking on it with the right mouse button and choosing the `Copy` option. In addition to this, you can cut it by choosing the `Cut` option. To paste a step, click anywhere in the Workflow Editor with the right mouse button and choose the `Paste step` option.
Alternatively, you can press `CTRL` + `C` to copy, `CTRL` + `X` to cut, and `CTRL` + `V` to paste steps.

It is possible to copy and paste steps in between various projects. You can even share steps with others. Copied steps are text snippets that can be pasted into any text file, chat message, or email. To paste one into your training course, simply copy the entire text snippet and paste it into the opened Workflow Editor.

### Add Transitions

To add a transition to a step, click on the white circle with the `+` sign right of the step. To connect a step to another, drag the transition origin (a white circle next to the origin object) to the transition target object.

[![Add and connect transitions](../images/workflow-editor/add-and-connect-transitions.gif "")](../images/workflow-editor/add-and-connect-transitions.gif)

### Remove Transitions

You can remove a transition by clicking with the right mouse button on the origin of the transition (a white circle next to the original object) and choosing the 'Remove transition' option.
