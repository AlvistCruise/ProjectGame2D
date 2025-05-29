using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppossumController : Enemy
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    [SerializeField] private float walkSpeed = 3f;

    private Collider2D coll;
    private bool facingLeft = true;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.deltaTime != 0)
        {
            Move();
        }
    }

    private void Move()
    {
        if (facingLeft)
        {
            if (transform.position.x > leftCap)
            {
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }

                rb.velocity = new Vector2(-walkSpeed, rb.velocity.y);
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightCap)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }

                rb.velocity = new Vector2(walkSpeed, rb.velocity.y);
            }
            else
            {
                facingLeft = true;
            }
        }
    }
    
}
