using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class MenuTab : MonoBehaviour
{
    public int depthInMenuHeighrachy;
    [System.NonSerialized] public List<MenuTab> childTabs = new List<MenuTab>();
    public Image selectionBoarder;

    private HorizontalLayoutGroup TabLayout;

    private void Awake()
    {
        TabLayout = gameObject.GetComponent<HorizontalLayoutGroup>();
        
    }

    public void ApplyIndent(int unitsPerIndent)
    {
        TabLayout.padding.left = depthInMenuHeighrachy * unitsPerIndent;
    }

    public void CollapseChildren()
    {
        if (childTabs.Count > 0)
            foreach (MenuTab tab in childTabs)
                tab.Collapse();
    }

    public void ExpandChildren()
    {
        if (childTabs.Count > 0)
            foreach (MenuTab tab in childTabs)
                tab.gameObject.SetActive(true);
    }

    public void Collapse()
    {
        CollapseChildren();
        gameObject.SetActive(false);
        Debug.Log("Child collapsed");
    }

    public void ExpandCompletely()
    {
        gameObject.SetActive(true);
        ExpandChildren();
    }

    public void Expand()
    {
        gameObject.SetActive(true);
    }

    public void Select() => selectionBoarder.enabled = true;
    public void Deselect() => selectionBoarder.enabled = false;
}
