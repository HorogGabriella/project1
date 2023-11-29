using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    //public float minspeed = 5;
    //public float maxspeed = 10;
    public float speed = 10;
    private bool move = true;
    void Start()
    {
        //speed = Random.Range(minspeed, maxspeed);
    }
    void Update()
    {
       if(move)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        if (transform.position.x > 20f) 
        {
            move = false;
        }
       else if (transform.position.x < -20f)
        {
            move = true;
        }
    }
}
