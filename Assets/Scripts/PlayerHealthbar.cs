using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthbar : MonoBehaviour
{
    public Player character;
    public Image healthFill;
    public float maxHealth = 100f;

    void Start() {
        
        if (character == null) {
            Debug.LogError("Player component not found!");
        }

        if (healthFill == null) {
            Debug.LogError("HealthFill image not assigned");
        }
        
    }

    void Update() {
        if (character != null && healthFill != null) {
            healthFill.fillAmount = character.health / maxHealth;
        }
    }
}
