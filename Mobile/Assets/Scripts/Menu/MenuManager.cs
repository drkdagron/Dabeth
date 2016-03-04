using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

    public bool popupOpen;

    public enum CurrentScreen
    {
        Login = 0,
        CharSelect = 1,
        MainMenu = 2,
    }

    public MenuBase[] Screens;
    public CurrentScreen current;

    // Use this for initialization
    void Start()
    {
        openView(CurrentScreen.Login);
    }

    public void openView(CurrentScreen scr, params object[] objs)
    {
        if (scr == current)
            return;

        current = scr;
        int num = (int)current;
        for (int i= 0; i < Screens.Length; i++)
        {
            if (i == num)
            {
                Screens[i].gameObject.SetActive(true);
                Screens[i].Init(objs);
            }
            else
            {
                Screens[i].gameObject.SetActive(false);
            }
        }
    }
}
