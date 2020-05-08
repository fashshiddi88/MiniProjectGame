using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusuhterbangController : Enemy
{
    private float speed = 5f;
    public bool moveUp;


    // Update is called once per frame
    void Update()
    {
        if (moveUp)
        {
            transform.localPosition = new Vector2(this.transform.localPosition.x, this.transform.localPosition.y + speed * Time.deltaTime);
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            transform.localPosition = new Vector2(this.transform.localPosition.x, this.transform.localPosition.y - speed * Time.deltaTime);
            transform.localScale = new Vector2(-1, 1);
        }
    }

     private void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.CompareTag("Turn"))
        {
            if (moveUp)
            {
                moveUp = false;
            }
            else
            {
                moveUp = true;
            }
        }
    }
   
}