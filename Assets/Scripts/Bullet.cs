using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
Rigidbody2D myRigidBody;
[SerializeField] float bulletspeed = 15f;
PlayerMovement player;
float xSpeed;


    void Start()
    {
      myRigidBody= GetComponent<Rigidbody2D>();
      player=FindAnyObjectByType<PlayerMovement>();
      xSpeed=player.transform.localScale.x*bulletspeed;
      transform.localScale = new Vector2((Mathf.Sign(xSpeed)) * transform.localScale.x, 1f);

    }

    
    void Update()
    {
     myRigidBody.velocity= new Vector2(xSpeed,0f);
    }



void OnCollisionEnter2D(Collision2D other) 
{
    if (other.gameObject.CompareTag("Enemy")) 
    {
        Destroy(other.gameObject); 
        Destroy(gameObject);
    }
    else if (!other.gameObject.CompareTag("Bullet")) 
    {
        Destroy(gameObject); 
    }
}


 }

    
    


    

 
   
