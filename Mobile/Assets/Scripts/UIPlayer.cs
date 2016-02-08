using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPlayer : MonoBehaviour {

    public Text pName;
    public Image pPortrait;

    public Image HealthBar;
    public Text HealthText;
    public Vector2 HealthSize;

    public Image MoveBar;
    public Text MoveText;
    public Vector2 MoveSize;


    public void setUI(Player p)
    {
        pName.text = p.Name;

        MoveText.text = "Moves Left: " + p.MoveLeft.ToString() + "/" + p.Move.ToString();
        HealthText.text = "Health: " + p.Health.ToString() + "/" + p.HealthTotal.ToString();
        HealthSize = HealthBar.rectTransform.sizeDelta;
        MoveSize = MoveBar.rectTransform.sizeDelta;
    }

    public void setHealthBar(Player p)
    {
        HealthText.text = "Health: " + p.Health.ToString() + "/" + p.HealthTotal.ToString();
        float hRatio = (float)p.Health / (float)p.HealthTotal;
        HealthBar.rectTransform.sizeDelta = new Vector2(HealthSize.x * hRatio, HealthSize.y);
    }

    public void setMoveBar(Player p)
    {
        MoveText.text = "Moves Left: " + p.MoveLeft.ToString() + "/" + p.Move.ToString();
        float hRatio = (float)p.MoveLeft / (float)p.Move;
        MoveBar.rectTransform.sizeDelta = new Vector2(HealthSize.x * hRatio, HealthSize.y);
    }
}
