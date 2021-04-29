using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTabManager : MonoBehaviour
{
    private List<MenuTab> menuTabs = new List<MenuTab>();
    private int activeTab = 0;

    [SerializeField] private int indentAmount = 100;

    // Start is called before the first frame update
    void Start()
    {
        //adding all the menu tab components in the children of this to the menuTabs list;
        foreach (MenuTab menuTab in transform.GetComponentsInChildren<MenuTab>())
            menuTabs.Add(menuTab);


        //the foreach loop will iterate over each menuTab in the list and will assign it to the int in the childAssignmentDict correlating to its depthInMenuHeighrachy
        //it will then add that menuTab into the childTabs list of the menuTab from the childAssignmentDict who's key is 1 less than that of the current menuTab.
        Dictionary<int, MenuTab> childAssignmentDict = new Dictionary<int, MenuTab>();
        MenuTab LastTab = null;
        foreach (MenuTab menuTab in menuTabs)
        {
            int menuTabDepth = menuTab.depthInMenuHeighrachy;
            
            if (LastTab)
                if (menuTabDepth > LastTab.depthInMenuHeighrachy + 1)
                    menuTab.depthInMenuHeighrachy = LastTab.depthInMenuHeighrachy + 1;

            childAssignmentDict[menuTabDepth] = menuTab;

            if (menuTabDepth > 0 && childAssignmentDict.ContainsKey(menuTabDepth + 1))
                childAssignmentDict[menuTabDepth - 1].childTabs.Add(menuTab);
            
            LastTab = menuTab;
        }

        //apply indents to all the tabs
        foreach (MenuTab tab in menuTabs)
        {
            tab.ApplyIndent(indentAmount);
        }

        //collapse all the highest level menu tabs to start out
        //foreach (MenuTab tab in menuTabs)
        //    if (tab.depthInMenuHeighrachy = 0 &&)
                
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
