﻿using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10;
    public float lifetime = 10;
    public float damage = 5;
    public GameObject shooter;
    Rigidbody2D rb;
    public GameObject bloodEffect;

    void Start()
    {
        Destroy(gameObject, lifetime);
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * speed, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == shooter) return;
        if (col.isTrigger) return;
        Entity e = col.GetComponent<Entity>();

        if (e != null)
        {
            e.TakeDamage(damage);
            if (bloodEffect)
            {
                Instantiate(bloodEffect, transform.position, transform.rotation);
            }
        }

        if (col.GetComponent<Bullet>()) return;
        Destroy(gameObject);
    }
}
