using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    public float stayTime = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(stayTime > 0)
        {
            stayTime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != gameObject.tag)
        {
            other.SendMessage("TakeDamage", 1);
        }
    }

    public void TakeDamage(int damage = 1)
    {
        Destroy(gameObject);
    }
}
