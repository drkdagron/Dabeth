using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UIManager : MonoBehaviour {

    public Text turnCounter;
    public UIPlayer uiPlayer;
    public GameObject[] UIPlayerOnly;
    public GameManager game;

    public UIEnemy enemy;

    public EventSystem eSystem;

    public void PlayerUI(bool on)
    {
        foreach (GameObject go in UIPlayerOnly)
        {
            go.SetActive(on);
        }
    }

    public void NextTurn()
    {
        SetCombatUI(true);
        SetMoveUI(true);
    }

    public void SetCombatUI(bool act)
    {
        GameObject obj = transform.FindChild("ButtonParent").FindChild("Attack").gameObject;
        if (act == true)
        {
            obj.GetComponent<Button>().interactable = true;
        }
        else
        {
            obj.GetComponent<Button>().interactable = false;
        }
    }
    public void SetMoveUI(bool act)
    {
        GameObject obj = transform.FindChild("ButtonParent").FindChild("Move").gameObject;
        if (act == true)
        {
            obj.GetComponent<Button>().interactable = true;
        }
        else
        {
            obj.GetComponent<Button>().interactable = false;
        }
    }

    public void setTurnCounter()
    {
        turnCounter.text = "Turn " + game.currentTurn.ToString();
    }

    public void setUI(Player p)
    {
        uiPlayer.pName.text = p.Name;

        uiPlayer.MoveText.text = "Moves Left: " + p.MoveLeft.ToString() + "/" + p.Move.ToString();
        uiPlayer.HealthText.text = "Health: " + p.Health.ToString() + "/" + p.HealthTotal.ToString();
        uiPlayer.HealthSize = uiPlayer.HealthBar.rectTransform.sizeDelta;
        uiPlayer.MoveSize = uiPlayer.MoveBar.rectTransform.sizeDelta;

        setTurnCounter();
    }

    public void setEnemyUI(Enemy e)
    {
        enemy.gameObject.SetActive(true);
        enemy.setUI(e);
    }
    public void hideEnemyUI()
    {
        enemy.gameObject.SetActive(false);
    }

    public void setHealthBar(Player p)
    {
        uiPlayer.HealthText.text = "Health: " + p.Health.ToString() + "/" + p.HealthTotal.ToString();
        float hRatio = (float)p.Health / (float)p.HealthTotal;
        uiPlayer.HealthBar.rectTransform.sizeDelta = new Vector2(uiPlayer.HealthSize.x * hRatio, uiPlayer.HealthSize.y);
    }

    public void PlayerMove(Player p)
    {
        uiPlayer.MoveText.text = "Moves Left: " + p.MoveLeft.ToString() + "/" + p.Move.ToString();
        float hRatio = (float)p.MoveLeft / (float)p.Move;
        uiPlayer.MoveBar.rectTransform.sizeDelta = new Vector2(uiPlayer.HealthSize.x * hRatio, uiPlayer.HealthSize.y);
    }
}
