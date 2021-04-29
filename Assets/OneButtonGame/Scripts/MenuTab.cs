using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class MenuTab : MonoBehaviour
{
    public int depthInMenuHeighrachy;
    [System.NonSerialized] public List<MenuTab> childTabs = new List<MenuTab>();

    private HorizontalLayoutGroup TabLayout;

    private void Awake()
    {
        TabLayout = gameObject.GetComponent<HorizontalLayoutGroup>();
    }

    public void ApplyIndent(int unitsPerIndent)
    {
        TabLayout.padding.left = depthInMenuHeighrachy * unitsPerIndent;
    }

    public void Collapse()
    {
        if (childTabs.Count > 0)
            foreach (MenuTab tab in childTabs)
                tab.Deactivate();
    }

    public void Expand()
    {
        if (childTabs.Count > 0)
            foreach (MenuTab tab in childTabs)
                tab.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        Collapse();
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        Expand();
    }
}
