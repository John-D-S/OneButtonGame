using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTabManager : MonoBehaviour
{
    private List<MenuTab> menuTabs = new List<MenuTab>();
    private MenuTab activeTab;

    private int highestNumberInHeighrachy = 0;

    // Start is called before the first frame update
    void Start()
    {
        //adding all the menu tab components in the children of this to the menuTabs list;
        foreach (MenuTab menuTab in transform.GetComponentsInChildren<MenuTab>())
            menuTabs.Add(menuTab);

        foreach (MenuTab menuTab in menuTabs)
        {
            if menuTab.
        }

        
    }

    private void IndentTab()
    {
        for
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
