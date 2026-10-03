using UnityEngine;

public class UIScript : MonoBehaviour
{
    private PlayerScript player;
    public RectTransform healthSlider;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        player.OnAttack += UpdateHealthBar;
    }

    // Called when the players health changes
    void UpdateHealthBar(object sender, int health)
    {
        healthSlider.localScale = new Vector3(health * 4, 1, 1);
    }
}
