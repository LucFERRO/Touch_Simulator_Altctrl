using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSaw : MonoBehaviour
{
    public float rotatingSpeed;
    public bool spinDirection;

    private void Start()
    {
        rotatingSpeed = spinDirection ? rotatingSpeed : -rotatingSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotatingSpeed * Time.deltaTime, Space.Self); 
    }
}
