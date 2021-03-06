﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    public int MaxHealth;
    public int CurrentHealth;
    private HealthBar HealthBar;

    SpriteRenderer player_spriteRenderer;
    public bool hurt = false;

    // Start is called before the first frame update
    void Start()
    {
        SetMaxHealth();
        player_spriteRenderer = gameObject.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
    }
    private void OnEnable()
    {
        HealthBar = FindObjectOfType<HealthBar>();
        SetMaxHealth();
    }
    // Update is called once per frame
    void Update()
    {
        
        if (CurrentHealth <= 0)
        {
            GameManager.instance.SetCurrentGameState(GameManager.GameState.GAME_OVER);
            gameObject.SetActive(false);
        }
        if (!hurt) player_spriteRenderer.color = Color.white;
        else if (hurt)
        {
            player_spriteRenderer.color = Color.red;
            hurt = false;
        }
    }

    void SetMaxHealth()
    {
        CurrentHealth = MaxHealth;
    }

    public void HurtPlayer(int DamageToGive)
    {
        CurrentHealth -= DamageToGive;
        if (HealthBar != null)
        {
            HealthBar.SetHealthBar(CurrentHealth);
        }
        else
        {
            HealthBar = FindObjectOfType<HealthBar>();
        }
    }

    public void HealPlayer(int heal)
    {
        if (CurrentHealth != MaxHealth)
        {
            CurrentHealth += heal;
            if (CurrentHealth > 100) CurrentHealth = 100;
            HealthBar.SetHealthBar(CurrentHealth);
        }
        else return;
        
    }
}
