using UnityEngine;

public class CameraControl : MonoBehaviour
{

    [Header("References")]
    public Transform bodyTransform;

    [Header("Sensitivity")]
    public float sensitivityMultiplier = 1f;
    public float horizontalSensitivity = 1f;
    public float verticalSensitivity = 1f;

    [Header("Restrictions")]
    public float minYRotation = -90f;
    public float maxYRotation = 90f;

    //The real rotation of the camera without recoil
    private Vector3 realRotation;

    [Header("Aimpunch")]
    [Tooltip("bigger number makes the response more damped, smaller is less damped, currently the system will overshoot, with larger damping values it won't")]
    public float punchDamping = 9.0f;

    [Tooltip("bigger number increases the speed at which the view corrects")]
    public float punchSpringConstant = 65.0f;

    [HideInInspector]
    public Vector2 punchAngle;

    [HideInInspector]
    public Vector2 punchAngleVel;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Mathf.Abs(Time.timeScale) <= 0)
            return;

        DecayPunchAngle();

        float xMovement = Input.GetAxisRaw("Mouse X") * horizontalSensitivity * sensitivityMultiplier;
        float yMovement = -Input.GetAxisRaw("Mouse Y") * verticalSensitivity * sensitivityMultiplier;

        realRotation = new Vector3(Mathf.Clamp(realRotation.x + yMovement, minYRotation, maxYRotation), realRotation.y + xMovement, realRotation.z);
        realRotation.z = Mathf.Lerp(realRotation.z, 0f, Time.deltaTime * 3f);

        bodyTransform.eulerAngles = Vector3.Scale(realRotation, new Vector3(0f, 1f, 0f));

        Vector3 cameraEulerPunchApplied = realRotation;
        cameraEulerPunchApplied.x += punchAngle.x;
        cameraEulerPunchApplied.y += punchAngle.y;

        transform.eulerAngles = cameraEulerPunchApplied;
    }

    public void ViewPunch(Vector2 punchAmount)
    {
        //Remove previous recoil
        punchAngle = Vector2.zero;

        //Recoil go up
        punchAngleVel -= punchAmount * 20;
    }

    private void DecayPunchAngle()
    {
        if (punchAngle.sqrMagnitude > 0.001 || punchAngleVel.sqrMagnitude > 0.001)
        {
            punchAngle += punchAngleVel * Time.deltaTime;
            float damping = 1 - (punchDamping * Time.deltaTime);

            if (damping < 0)
                damping = 0;

            punchAngleVel *= damping;

            float springForceMagnitude = punchSpringConstant * Time.deltaTime;
            punchAngleVel -= punchAngle * springForceMagnitude;
        }
        else
        {
            punchAngle = Vector2.zero;
            punchAngleVel = Vector2.zero;
        }
    }
}
