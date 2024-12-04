using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public int hp;
    public int maxHp;
    public int tiltAngle;
    public int crouchAngle;
    public GameObject leftAttack;
    public GameObject rightAttack;
    private Vector3 startingPos;
    void Start()
    {
        hp = maxHp;
        startingPos = transform.position;
    }

    void Update()
    {
        SideDodgeHandler();
        CrouchHandler();
        AttackHandler(leftAttack, Input.GetKey(KeyCode.R));
        AttackHandler(rightAttack, Input.GetKey(KeyCode.T));
        if (hp <= 0)
        {
            Debug.Log("YOU DIED");
        }
    }

    void AttackHandler(GameObject attackSide, bool attackCommand)
    {
        attackSide.SetActive(attackCommand);
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

    void SideDodgeHandler()
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
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT!");
        hp -= 1;
    }
}
