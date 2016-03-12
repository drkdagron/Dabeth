using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu_Inventory : MenuBase {

    public MenuManager manager;

    public void Inventory()
    {
        manager.openView(MenuManager.CurrentScreen.Inventory);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
