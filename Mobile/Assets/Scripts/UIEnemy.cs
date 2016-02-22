using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIEnemy : MonoBehaviour
{

    public Text pName;
    public Image pPortrait;

    public Image HealthBar;
    public Text HealthText;
    public Vector2 HealthSize;

    public void setUI(Enemy e)
    {
        pName.text = e.Name;
        HealthText.text = "Health: " + e.Health.ToString() + "/" + e.HealthTotal.ToString();
        float hRatio = (float)e.Health / (float)e.HealthTotal;
        HealthBar.rectTransform.sizeDelta = new Vector2(HealthSize.x * hRatio, HealthSize.y);
    }
}
