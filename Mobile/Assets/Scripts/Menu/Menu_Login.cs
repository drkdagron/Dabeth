using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu_Login : MenuBase {

    public MenuManager manager;

    public InputField userName;
    public InputField passWord;
    public string UserName;
    public string Password;

    public void updateUserField()
    {
        UserName = userName.text;
    }
    public void updatePasswordField()
    {
        Password = passWord.text;
    }

    public override void Init(params object[] args)
    {
        userName.text = "";
        passWord.text = "";
        UserName = "";
        Password = "";
    }

    public void Login()
    {
        if (UserName == "Success")
        {
            manager.openView(MenuManager.CurrentScreen.CharSelect, UserName);
        }
        else
        {
            Debug.Log("Invalid Login...Try Again");
        }
    }
}
