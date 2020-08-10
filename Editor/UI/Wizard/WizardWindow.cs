using System.Collections.Generic;
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
    private int pagePosition = 0;

    [SerializeField]
    public Vector2 Size = new Vector2(800, 500);

    protected float BottomBarHeight = 40f;
    protected float NavigationBarRatio = 0.25f;

    protected float ButtonPadding = 8f;
    protected Vector2 ButtonSize;

    [SerializeField]
    protected List<WizardPage> pages;

    protected WizardNavigation navigation;

    public WizardWindow()
    {
        minSize = Size;
        maxSize = Size;

        ButtonSize = new Vector2(BottomBarHeight * 2.5f, BottomBarHeight - 8);
    }

    public void Setup(string name, List<WizardPage> pages)
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
        entries[pagePosition].Selected = true;
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
        EditorGUI.DrawRect(new Rect(window.x, window.y, window.width, 1), LineColor);

        Vector2 buttonPosition = new Vector2(window.width - (ButtonSize.x + 4), window.y + 4);

        buttonPosition = DrawFinishButton(buttonPosition);
        buttonPosition = DrawNextButton(buttonPosition);
        buttonPosition = DrawSkipButton(buttonPosition);
        buttonPosition = DrawPreviousButton(buttonPosition);
        buttonPosition = DrawCloseButton(buttonPosition);
    }

    private Vector2 DrawFinishButton(Vector2 position)
    {
        if (pagePosition == pages.Count - 1)
        {
            EditorGUI.BeginDisabledGroup(GetActivePage().CanProceed == false);
            if (GUI.Button(new Rect(position, ButtonSize), "Finish"))
            {
                FinishButtonPressed();
            }
            EditorGUI.EndDisabledGroup();
            return new Vector2(position.x - (ButtonSize.x + ButtonPadding), position.y);
        }

        return position;
    }

    private Vector2 DrawNextButton(Vector2 position)
    {
        if (pagePosition < pages.Count - 1)
        {
            EditorGUI.BeginDisabledGroup(GetActivePage().CanProceed == false);
            if (GUI.Button(new Rect(position, ButtonSize), "Next"))
            {
                NextButtonPressed();
            }
            EditorGUI.EndDisabledGroup();
            return new Vector2(position.x - (ButtonSize.x + ButtonPadding), position.y);
        }

        return position;
    }

    private Vector2 DrawSkipButton(Vector2 position)
    {
        if (pagePosition < pages.Count - 1 && GetActivePage().AllowSkip)
        {
            if (GUI.Button(new Rect(position, ButtonSize), "Skip this Step"))
            {
                SkipButtonPressed();
            }
            return new Vector2(position.x - (ButtonSize.x + ButtonPadding), position.y);
        }

        return position;
    }

    private Vector2 DrawPreviousButton(Vector2 position)
    {
        if (pagePosition > 0)
        {
            if (GUI.Button(new Rect(position, ButtonSize), "Previous"))
            {
                BackButtonPressed();
            }
            EditorGUI.EndDisabledGroup();
            return new Vector2(position.x - (ButtonSize.x + ButtonPadding), position.y);
        }
        return position;
    }


    private Vector2 DrawCloseButton(Vector2 position)
    {
        if (pagePosition == 0)
        {
            if (GUI.Button(new Rect(position, ButtonSize), "Close Wizard"))
            {
                Close();
            }
            EditorGUI.EndDisabledGroup();
            return new Vector2(position.x - (ButtonSize.x + ButtonPadding), position.y);
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
        GetActivePage().Cancel();
        pagePosition--;
        navigation.SetSelected(pagePosition);
    }

    protected virtual  void SkipButtonPressed()
    {
        GetActivePage().Skip();
        pagePosition++;
        navigation.SetSelected(pagePosition);
    }

    protected virtual  void NextButtonPressed()
    {
        GetActivePage().Apply();
        pagePosition++;
        navigation.SetSelected(pagePosition);
    }

    protected WizardPage GetActivePage()
    {
        return pages[pagePosition];
    }

    protected Rect GetNavigationRect()
    {
        return new Rect(0, 0, Size.x * NavigationBarRatio, Size.y - BottomBarHeight);
    }

    protected Rect GetContentRect()
    {
        return new Rect(Size.x * NavigationBarRatio, 0, Size.x - (Size.x * NavigationBarRatio), Size.y - BottomBarHeight);
    }

    protected Rect GetBottomBarRect()
    {
        return new Rect(0, Size.y - BottomBarHeight, Size.x, BottomBarHeight);
    }
}
