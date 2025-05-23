using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FIreball : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        FrogController frog = other.gameObject.GetComponent<FrogController>();
        if (other.gameObject.CompareTag("Enemy"))
        {
            frog.JumpedOn();
            Destroy(gameObject);
        }
        if(other.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

    }
}
