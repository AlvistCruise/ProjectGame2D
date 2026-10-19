using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttributes : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Collider2D coll;
    protected Animator anim;

    [SerializeField] protected float speed = 8f;
    [SerializeField] protected float jumpForce = 10f;
    [SerializeField] protected LayerMask ground;
    protected bool isJumping = false;

    [SerializeField] protected int hurtForce = 10;

    [SerializeField] protected int cherries = 0;
    [SerializeField] protected int health = 5;

    [SerializeField] protected Text cherriesTXT;
    [SerializeField] protected Text healthTXT;

    [SerializeField] protected GameObject fireballPrefab;
    [SerializeField] protected Transform fireballSpawnPoint;
    [SerializeField] protected float fireballSpeed = 10f;
    [SerializeField] protected float fireCooldown = 3;
    [SerializeField] protected Text fireCooldownTXT;

    protected virtual void Awake()
    {
        InitAttributes();
    }

    protected void InitAttributes()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        if (healthTXT != null)
        {
            healthTXT.text = health.ToString();
        }
        if (cherriesTXT != null)
        {
            cherriesTXT.text = cherries.ToString();
        }
        if (fireCooldownTXT != null)
        {
            fireCooldownTXT.text = fireCooldown.ToString();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;
        if (healthTXT != null)
        {
            healthTXT.text = health.ToString();
        }
    }

    public void AddCherry(int amount)
    {
        cherries += amount;
        if (cherriesTXT != null)
        {
            cherriesTXT.text = cherries.ToString();
        }
    }

    protected void UpdateFireCooldown()
    {
        if (fireCooldown > 0)
        {
            if (fireCooldownTXT != null)
            {
                fireCooldownTXT.text = fireCooldown.ToString();
            }
            fireCooldown -= Time.deltaTime;
        }
        else
        {
            fireCooldown = 0;
            if (fireCooldownTXT != null)
            {
                fireCooldownTXT.text = fireCooldown.ToString();
            }
        }
    }

    protected void ResetFireCooldown(float cooldown = 3f)
    {
        fireCooldown = cooldown;
        if (fireCooldownTXT != null)
        {
            fireCooldownTXT.text = fireCooldown.ToString();
        }
    }
}

