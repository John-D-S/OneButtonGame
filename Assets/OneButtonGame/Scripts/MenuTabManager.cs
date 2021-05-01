using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTabManager : MonoBehaviour
{
    private List<MenuTab> menuTabs = new List<MenuTab>();
    
    private List<int> selectedTabGroup = new List<int>();

    private int selectedTabDepth = 0;
    private int selectedTabIndex = 0;
    private int SelectedTabIndex 
    {
        set
        {
            //set the Tab's depth
            int lastSelectedTabDepth = selectedTabDepth;
            selectedTabDepth = menuTabs[value].depthInMenuHeighrachy;
            //set the Tab's index
            int lastSelectedTabIndex = selectedTabIndex;
            selectedTabIndex = value;

            //if we are going up a level
            if (selectedTabDepth > lastSelectedTabDepth)
            {
                UpdateSelectedTabGroup();
                foreach (int tabIndex in selectedTabGroup)
                    menuTabs[tabIndex].Expand();
            }
            //if we are going down a level
            else if (selectedTabDepth < lastSelectedTabDepth)
            {
                UpdateSelectedTabGroup();
                foreach (int tabIndex in selectedTabGroup)
                    menuTabs[tabIndex].CollapseChildren();
            }
            
            //visually select the tab with the selection boarder
            menuTabs[lastSelectedTabIndex].Deselect();
            menuTabs[value].Select();
        }
        get
        {
            return selectedTabIndex;
        }
    }
    
    [SerializeField] private int indentAmount = 100;

    // Start is called before the first frame update
    void Start()
    {
        //adding all the menu tab components in the children of this to the menuTabs list;
        foreach (MenuTab menuTab in transform.GetComponentsInChildren<MenuTab>())
            menuTabs.Add(menuTab);

        /*
        //the foreach loop will iterate over each menuTab in the list and will assign it to the int in the childAssignmentDict correlating to its depthInMenuHeighrachy
        //it will then add that menuTab into the childTabs list of the menuTab from the childAssignmentDict who's key is 1 less than that of the current menuTab.
        Dictionary<int, List<MenuTab>> childAssignmentDict = new Dictionary<int, List<MenuTab>>();
        */
        MenuTab LastTab = null;
        foreach (MenuTab menuTab in menuTabs)
        {
            int menuTabDepth = menuTab.depthInMenuHeighrachy;
            
            if (LastTab)
                if (menuTabDepth > LastTab.depthInMenuHeighrachy + 1)
                    menuTab.depthInMenuHeighrachy = LastTab.depthInMenuHeighrachy + 1;

            /* this is the busted code for assigning children to the menuTabs
            childAssignmentDict[menuTabDepth].Add(menuTab);

            if (menuTabDepth > 0 && childAssignmentDict.ContainsKey(menuTabDepth + 1))
                childAssignmentDict[menuTabDepth - 1].childTabs.Add(menuTab);
            */

            menuTab.ApplyIndent(indentAmount);
            menuTab.Deselect();
            
            LastTab = menuTab;
        }

        SelectedTabIndex = 0;
        UpdateSelectedTabGroup();
        Debug.Log("number of tabs in group: " + selectedTabGroup.Count);
        foreach (int tabIndex in selectedTabGroup)
        {
            AssignChildren(tabIndex);
        }

        foreach (int tabIndex in selectedTabGroup)
        {
            Debug.Log(tabIndex);
            menuTabs[tabIndex].CollapseChildren();
        }
    }

    void AssignChildren(int _tabIndex)
    {
        int tabIndexDepth = menuTabs[_tabIndex].depthInMenuHeighrachy;
        for (int i = _tabIndex + 1; i < menuTabs.Count; i++)
        {
            int iDepth = menuTabs[i].depthInMenuHeighrachy;
            if (iDepth == tabIndexDepth + 1)
            {
                menuTabs[_tabIndex].childTabs.Add(menuTabs[i]);
                AssignChildren(i);
            }
            else if (iDepth <= tabIndexDepth)
            {
                i = menuTabs.Count;
            }
        }

    }

    void UpdateSelectedTabGroup()
    {
        selectedTabGroup.Clear();
        int firstTabInTabGroup = 0;
        int lastTabInTabGroup = menuTabs.Count;

        //int i = selectedTab;
        for (int i = SelectedTabIndex; i >= 0; i--)
            if (i == 0 || menuTabs[i - 1].depthInMenuHeighrachy < selectedTabDepth)
            {
                firstTabInTabGroup = i;
                i = -1;
            }
        for (int i = SelectedTabIndex; i <= menuTabs.Count; i++)
            if (i == menuTabs.Count - 1 || menuTabs[i + 1].depthInMenuHeighrachy < selectedTabDepth)
            {
                lastTabInTabGroup = i;
                i = menuTabs.Count + 1;
            }
        for (int i = firstTabInTabGroup; i <= lastTabInTabGroup; i++)
            if (menuTabs[i].depthInMenuHeighrachy == selectedTabDepth)
                selectedTabGroup.Add(i);
    }

    void SelectNextTab()
    {
        int selectedTabIndexInTabGroup = selectedTabGroup.IndexOf(SelectedTabIndex);
        if (selectedTabIndexInTabGroup + 1 == selectedTabGroup.Count)
            SelectedTabIndex = selectedTabGroup[0];
        else
            SelectedTabIndex = selectedTabGroup[selectedTabIndexInTabGroup + 1];
    }
    
    void SelectPreviousTab()
    {
        int selectedTabIndexInTabGroup = selectedTabGroup.IndexOf(SelectedTabIndex);
        if (selectedTabIndexInTabGroup - 1 < 0)
            SelectedTabIndex = selectedTabGroup[selectedTabGroup.Count - 1];
        else
            SelectedTabIndex = selectedTabGroup[selectedTabIndexInTabGroup - 1];
    }

    void AscendTabGroup()
    {
        if (SelectedTabIndex + 1 < menuTabs.Count && menuTabs[SelectedTabIndex + 1].depthInMenuHeighrachy > selectedTabDepth)
        {
            SelectedTabIndex++;
        }
        else
        {
            //if the tab has a function, do it.
        }
    }

    // to decend is to go closer to zero
    void DecendTabGroup()
    {
        if (selectedTabDepth > 0)
        {
            SelectedTabIndex = selectedTabGroup[0] - 1;   
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Down"))
        {
            SelectNextTab();
        }
        if (Input.GetButtonDown("Up"))
        {
            SelectPreviousTab();
        }
        if (Input.GetButtonDown("Right"))
        {
            AscendTabGroup();
        }
        if (Input.GetButtonDown("Left"))
        {
            DecendTabGroup();
        }
    }
}
