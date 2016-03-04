using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu_Char : MenuBase {

    public MenuManager manager;
    public Text infoText;

    public void Logout()
    {
        manager.openView(MenuManager.CurrentScreen.Login);
    }

    public override void Init(params object[] args)
    {
        infoText.text = "Logged in as: " + args[0];
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
