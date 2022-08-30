using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;
    public float playerHealth;
    [SerializeField] private Slider playerHealthBar;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        playerHealthBar.value = playerHealth / 100;
    }

    public void TakeDamage(int enemyDamage)
    {
        playerHealth -= enemyDamage;
        playerHealthBar.value = playerHealth / 100;
    }
}
