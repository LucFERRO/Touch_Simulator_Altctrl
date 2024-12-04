using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform playerTransform;
    public float speed;

    void Start()
    {
        
    }
    void Update()
    {
        if (transform.position.z > playerTransform.position.z - 3) {
            Vector3 itemPos = transform.position;
            itemPos.z -= speed * Time.deltaTime;
            transform.position = itemPos;
        } else
        {
            Destroy(gameObject);
        }
    }
}
