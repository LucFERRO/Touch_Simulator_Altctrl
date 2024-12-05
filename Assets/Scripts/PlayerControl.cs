using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Utility
{
    public static void Invoke(this MonoBehaviour mb, Action f, float delay)
    {
        mb.StartCoroutine(InvokeRoutine(f, delay));
    }

    private static IEnumerator InvokeRoutine(System.Action f, float delay)
    {
        yield return new WaitForSeconds(delay);
        f();
    }
}
public class PlayerControl : MonoBehaviour
{
    public int hp;
    public int maxHp;
    public int tiltAngle;
    public int crouchAngle;
    public float attackDuration;
    public GameObject leftAttack;
    public GameObject rightAttack;
    public GameObject shield;
    private Vector3 startingPos;
    void Start()
    {
        hp = maxHp;
        startingPos = transform.position;
    }

    void Update()
    {
        SawDodgeHandler();
        CrouchHandler();
        AttackHandler();
        ShieldHandler();
        if (hp <= 0)
        {
            Debug.Log("YOU DIED");
        }
    }

    void AttackHandler()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            leftAttack.SetActive(true);
            this.Invoke(() => leftAttack.SetActive(false), attackDuration);            
        }        

        if (Input.GetKeyDown(KeyCode.Y))
        {
            rightAttack.SetActive(true);
            this.Invoke(() => rightAttack.SetActive(false), attackDuration);
        }
    }

    void ShieldHandler()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            shield.SetActive(true);
            this.Invoke(() => shield.SetActive(false), attackDuration);
        }
    }

    void CrouchHandler()
    {
        if (Input.GetKey(KeyCode.E))
        {
            Vector3 newPos = transform.position;
            newPos.y = startingPos.y - 1;
            Quaternion newRotation = Quaternion.AngleAxis(crouchAngle, Vector3.right);
            transform.position = Vector3.Lerp(transform.position, newPos, .05f);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, .05f);
        } else
        {
            transform.position = Vector3.Lerp(transform.position, startingPos, .05f);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, .05f); ;
        }
    }

    void SawDodgeHandler()
    {
        if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.W))
        {
            transform.position = Vector3.Lerp(transform.position, startingPos, .05f);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, .05f); ;
        }
        if (!Input.GetKey(KeyCode.Q))
        {
            Vector3 newPos = transform.position;
            newPos.x = startingPos.x + 1;
            Quaternion newRotation = Quaternion.AngleAxis(-tiltAngle, Vector3.forward);
            transform.position = Vector3.Lerp(transform.position, newPos, .05f);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, .05f);
        }
        if (!Input.GetKey(KeyCode.W))
        {
            Vector3 newPos = transform.position;
            newPos.x = startingPos.x - 1;
            Quaternion newRotation = Quaternion.AngleAxis(tiltAngle, Vector3.forward);
            transform.position = Vector3.Lerp(transform.position, newPos, .05f);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, .05f);
        }
        if (!Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.W))
        {
            Vector3 newPos = transform.position;
            newPos.y = startingPos.y + 1;
            transform.position = Vector3.Lerp(transform.position, newPos, .05f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT!");
        hp -= 1;
    }
}
