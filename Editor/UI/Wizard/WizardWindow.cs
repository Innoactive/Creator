using System;
using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.Core.Editor.UI.Wizard;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Wizard base class which allows you to implement a new, awesome wizard!
/// </summary>
internal class WizardWindow : EditorWindow
{
    public static readonly Color LineColor = new Color(0, 0, 0, 0.25f);

    [SerializeField]
    private int selectedPage = 0;

    [SerializeField]
    public Vector2 Size = new Vector2(800, 500);

    protected float bottomBarHeight = 40f;
    protected float navigationBarRatio = 0.25f;

    protected float buttonPadding = 8f;
    protected Vector2 buttonSize;

    [SerializeField]
    protected List<WizardPage> pages;

    protected WizardNavigation navigation;

    public WizardWindow()
    {
        minSize = Size;
        maxSize = Size;

        buttonSize = new Vector2(bottomBarHeight * 2.5f, bottomBarHeight - 8);
    }

    public virtual void Setup(string name, List<WizardPage> pages)
    {
        titleContent = new GUIContent(name);
        this.pages = pages;
    }

    protected virtual WizardNavigation CreateNavigation()
    {
        List<IWizardNavigationEntry> entries = new List<IWizardNavigationEntry>();
        foreach (WizardPage page in pages)
        {
            entries.Add(new WizardNavigation.Entry(page.Name));
        }
        entries[selectedPage].Selected = true;
        return new WizardNavigation(entries);
    }

    private void OnGUI()
    {
        if (navigation == null)
        {
            navigation = CreateNavigation();
        }

        navigation.Draw(GetNavigationRect());
        GetActivePage().Draw(GetContentRect());
        DrawBottomBar(GetBottomBarRect());
    }

    protected void DrawBottomBar(Rect window)
    {
        EditorGUI.DrawRect(new Rect(0, window.y, window.width, 1), LineColor);

        Vector2 buttonPosition = new Vector2(window.width - (buttonSize.x + 4), window.y + 4);

        buttonPosition = DrawFinishButton(buttonPosition);
        buttonPosition = DrawNextButton(buttonPosition);
        buttonPosition = DrawPreviousButton(buttonPosition);
        buttonPosition = DrawSkipButton(buttonPosition);
        buttonPosition = DrawCloseButton(buttonPosition);
    }

    private Vector2 DrawFinishButton(Vector2 position)
    {
        if (selectedPage == pages.Count - 1)
        {
            EditorGUI.BeginDisabledGroup(GetActivePage().CanProceed == false);
            if (GUI.Button(new Rect(position, buttonSize), "Finish"))
            {
                FinishButtonPressed();
            }
            EditorGUI.EndDisabledGroup();
            return new Vector2(position.x - (buttonSize.x + buttonPadding), position.y);
        }

        return position;
    }

    private Vector2 DrawNextButton(Vector2 position)
    {
        if (selectedPage < pages.Count - 1)
        {
            EditorGUI.BeginDisabledGroup(GetActivePage().CanProceed == false);
            if (GUI.Button(new Rect(position, buttonSize), "Next"))
            {
                NextButtonPressed();
            }
            EditorGUI.EndDisabledGroup();
            return new Vector2(position.x - (buttonSize.x + buttonPadding), position.y);
        }

        return position;
    }

    private Vector2 DrawSkipButton(Vector2 position)
    {
        if (selectedPage < pages.Count - 1 && GetActivePage().AllowSkip)
        {
            position = new Vector2(position.x - buttonPadding * 6, position.y);
            if (GUI.Button(new Rect(new Vector2(GetNavigationRect().width + 4, position.y), buttonSize), "Skip this Step"))
            {
                SkipButtonPressed();
            }
        }

        return position;
    }

    private Vector2 DrawPreviousButton(Vector2 position)
    {
        if (selectedPage > 0)
        {
            if (GUI.Button(new Rect(position, buttonSize), "Previous"))
            {
                BackButtonPressed();
            }
            EditorGUI.EndDisabledGroup();
            return new Vector2(position.x - (buttonSize.x + buttonPadding), position.y);
        }
        return position;
    }


    private Vector2 DrawCloseButton(Vector2 position)
    {
        if (selectedPage == 0)
        {
            if (GUI.Button(new Rect(position, buttonSize), "Close Wizard"))
            {
                Close();
            }
            EditorGUI.EndDisabledGroup();
            return new Vector2(position.x - (buttonSize.x + buttonPadding), position.y);
        }
        return position;
    }

    protected virtual void FinishButtonPressed()
    {
        GetActivePage().Apply();
        Close();
    }

    protected virtual void BackButtonPressed()
    {
        GetActivePage().Back();
        selectedPage--;
        navigation.SetSelected(selectedPage);
    }

    protected virtual void SkipButtonPressed()
    {
        GetActivePage().Skip();
        selectedPage++;
        navigation.SetSelected(selectedPage);
    }

    protected virtual void NextButtonPressed()
    {
        GetActivePage().Apply();
        selectedPage++;
        navigation.SetSelected(selectedPage);
    }

    protected void OnDestroy()
    {
        bool cancelled = pages.GetRange(selectedPage + 1, pages.Count - selectedPage - 1).Any(page => page.Mandatory);
        pages.ForEach(page => page.Closing(!cancelled));
    }

    protected WizardPage GetActivePage()
    {
        return pages[selectedPage];
    }

    protected Rect GetNavigationRect()
    {
        return new Rect(0, 0, Size.x * navigationBarRatio, Size.y);
    }

    protected Rect GetContentRect()
    {
        return new Rect(Size.x * navigationBarRatio, 0, Size.x - (Size.x * navigationBarRatio), Size.y - bottomBarHeight);
    }

    protected Rect GetBottomBarRect()
    {
        return new Rect(Size.x * navigationBarRatio, Size.y - bottomBarHeight, Size.x, bottomBarHeight);
    }
}
