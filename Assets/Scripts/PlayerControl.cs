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
    [Header("Health Manager")]
    [Range(3, 10)] public int hp;
    public int maxHp;
    public GameObject[] hpBars;
    [Header("Controls")]
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
        //CreateHpBar(maxHp-1);
        startingPos = transform.position;
    }

    void Update()
    {
        SawDodgeHandler();
        CrouchHandler();
        AttackHandler();
        ShieldHandler();
    }

    void CreateHpBar(int maximumHpIndex)
    {
        for (int i = 0; i < 10; i++)
        {
            if (i < maximumHpIndex)
            {
                hpBars[i].SetActive(true);
            } else
            {
                hpBars[i].SetActive(false);
            }
        }
    }
    void HealthManager()
    {
        hp -= 1;
        hpBars[hp].SetActive(false);
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
        HealthManager();
    }
}
