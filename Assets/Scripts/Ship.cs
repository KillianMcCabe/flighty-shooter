using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {

    Rigidbody rb;
    //CharacterController charController;

    [HideInInspector]
    public Quaternion rotation;

    float speed = 0f;
    float rollSpeed = 0f;
    float pitchSpeed = 0f;

    float mouseSpeed = 50.0f;
    //float mouseSensitivity;

    float maxSpeed = 200f;
    float turningRollAmount = 10f;
    
    float rollSpeedAccel = 140f;
    float rollSpeedDeaccel = 200f;
    float rollSpeedMax = 100f;
    
    float pitchSpeedAccel = 400f;
    float pitchSpeedMax = 50f;

    float maxSpeedLimiter = .25f;

    float rollingEffectFromTurn = 0f;
    float rollingEffectFromTurnAccel = 20f;
    float rollingEffectFromTurnDeaccel = 60f;
    float rollingEffectFromTurnMax = 50.0f;

    float mouseX;
    float mouseY;
    float vertical;
    float horizontal;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        //charController = GetComponent<CharacterController>();

        rotation = transform.rotation;
        Cursor.lockState = CursorLockMode.Locked;
    }
	
	// Update is called once per frame
	void Update () {

        // Get inputs
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        // Handle yaw acceleration
        if (mouseX < 0 && rollingEffectFromTurn > -rollingEffectFromTurnMax)
        {
            rollingEffectFromTurn += mouseX * rollingEffectFromTurnAccel * Time.deltaTime;
            if (rollingEffectFromTurn > rollingEffectFromTurnDeaccel * Time.deltaTime)
                rollingEffectFromTurn -= rollingEffectFromTurnDeaccel * Time.deltaTime;
            
        }
        else if (mouseX > 0 && rollingEffectFromTurn < rollingEffectFromTurnMax)
        {
            rollingEffectFromTurn += mouseX * rollingEffectFromTurnAccel * Time.deltaTime;
            if (rollingEffectFromTurn < -rollingEffectFromTurnDeaccel * Time.deltaTime)
                rollingEffectFromTurn += rollingEffectFromTurnDeaccel * Time.deltaTime;
        }
        else
        {
            if (rollingEffectFromTurn > rollingEffectFromTurnDeaccel * Time.deltaTime)
                rollingEffectFromTurn -= rollingEffectFromTurnDeaccel * Time.deltaTime;
            else if (rollingEffectFromTurn < -rollingEffectFromTurnDeaccel * Time.deltaTime)
                rollingEffectFromTurn += rollingEffectFromTurnDeaccel * Time.deltaTime;
            else
                rollingEffectFromTurn = 0;
        }

        // Handle rolling acceleration
        if (horizontal > 0 && rollSpeed > -rollSpeedMax)
        {
            rollSpeed -= rollSpeedAccel * Time.deltaTime;
        }
        else if (horizontal < 0 && rollSpeed < rollSpeedMax)
        {
            rollSpeed += rollSpeedAccel * Time.deltaTime;
        }
        else
        {
            if (rollSpeed > rollSpeedDeaccel * Time.deltaTime)
                rollSpeed -= rollSpeedDeaccel * Time.deltaTime;
            else if (rollSpeed < -rollSpeedDeaccel * Time.deltaTime)
                rollSpeed += rollSpeedDeaccel * Time.deltaTime;
            else
                rollSpeed = 0;
        }
        // Handle pitch acceleration
        if (vertical > 0 && pitchSpeed > -pitchSpeedMax)
        {
            pitchSpeed -= pitchSpeedAccel * Time.deltaTime;
        }
        else if (vertical < 0 && pitchSpeed < pitchSpeedMax)
        {
            pitchSpeed += pitchSpeedAccel * Time.deltaTime;
        }
        else
        {
            if (pitchSpeed > pitchSpeedAccel * Time.deltaTime)
                pitchSpeed -= pitchSpeedAccel * Time.deltaTime;
            else if (pitchSpeed < -pitchSpeedAccel * Time.deltaTime)
                pitchSpeed += pitchSpeedAccel * Time.deltaTime;
            else
                pitchSpeed = 0;
        }

        
        //
        // Apply movements
        //
        rotation *= Quaternion.Euler(-mouseY * mouseSpeed * Time.deltaTime, 0, 0); // pitch
        rotation *= Quaternion.Euler(0, mouseX * mouseSpeed * Time.deltaTime, 0); // yaw
        rotation *= Quaternion.Euler(0, 0, rollSpeed * Time.deltaTime); // roll

        transform.rotation = rotation;
        transform.RotateAround(transform.position, transform.forward, -rollingEffectFromTurn); // roll caused by yaw (mouseX turning)

        float rollStrength = -Vector3.Dot(transform.up, Vector3.Cross(transform.forward, Vector3.up)); // used for slowing forward motion when turning
        speed = maxSpeed * (1 - (Mathf.Abs(rollStrength) * maxSpeedLimiter));

        transform.position += transform.forward * speed * Time.deltaTime; // forward movement
    }

}
