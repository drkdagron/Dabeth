using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu_Login : MenuBase {

    public MenuManager manager;

    public InputField userName;
    public InputField passWord;
    public string UserName;
    public string Password;

	private string url;

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
		url = "dabeth.x10host.com/game/retrieve.php?";
		StartCoroutine(CheckLogin());
	}

	public void Register()
	{
		url = "dabeth.x10host.com/game/upload.php?";
		StartCoroutine(CheckRegister());
	}

	IEnumerator CheckLogin()
	{
		url = url + "username=" + UserName + "&password=" + Password;
		WWW www = new WWW(url);
		yield return www;

		if (www.text == "Success") {
			Debug.Log(www.text);
			manager.openView (MenuManager.CurrentScreen.CharSelect, UserName);

		} 
		else 
		{
			Debug.Log(www.text);

		}
			
    }

	IEnumerator CheckRegister()
	{
		url = url + "username=" + UserName + "&password=" + Password;
		WWW www = new WWW(url);
		yield return www;

		if (www.text == "Success") {
			Debug.Log(www.text);
			//manager.openView (MenuManager.CurrentScreen.CharSelect, UserName);

		} 
		else 
		{
			Debug.Log(www.text);

		}

	}
}
