using UnityEngine;
using System.Collections;

public class Ship_old : MonoBehaviour {

    Rigidbody rb;
    CharacterController charController;

    float speed = 0f;
    float rollSpeed = 0f;
    float pitchSpeed = 0f;

    float maxSpeed = 200f;
    float turnSpeed = 150f;
    
    float rollSpeedAccel = 500f;
    float rollSpeedMax = 40f;
    
    float pitchSpeedAccel = 400f;
    float pitchSpeedMax = 50f;

    float maxSpeedLimiter = .25f;

    float vertical;
    float horizontal;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        charController = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {

        // Get inputs
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        // Handle roll acceleration
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
            if (rollSpeed > rollSpeedAccel * Time.deltaTime)
                rollSpeed -= rollSpeedAccel * Time.deltaTime;
            else if (rollSpeed < -rollSpeedAccel * Time.deltaTime)
                rollSpeed += rollSpeedAccel * Time.deltaTime;
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

        float rollStrength = -Vector3.Dot(transform.up, Vector3.Cross(transform.forward, Vector3.up));

        // Apply movements
        speed = maxSpeed * (1 - (Mathf.Abs(rollStrength) * maxSpeedLimiter));
        transform.position += transform.forward * speed * Time.deltaTime; // forward movement
        transform.RotateAround(transform.position, transform.right, pitchSpeed * Time.deltaTime); //pitch
        transform.RotateAround(transform.position, transform.forward, rollSpeed * Time.deltaTime); // roll
        transform.Rotate(0, rollStrength * turnSpeed * Time.deltaTime, 0, Space.World); // turn into roll

        //AutoLevel();
    }

    public void AutoLevel()
    {
        if (horizontal == 0)
        {
            float rollAutoLevelSpeed = 100;
            Quaternion rollStableRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
            Quaternion fixingTilt = Quaternion.RotateTowards(transform.rotation, rollStableRotation, rollAutoLevelSpeed * Time.deltaTime);
            transform.rotation = fixingTilt;
        }

        // problem in here somwhere...
        //if (vertical == 0)
        //{
        //    Vector3 vec = Vector3.Cross(transform.right, Vector3.up);
        //    Quaternion pitchStableRotation = Quaternion.LookRotation(vec, transform.up);
        //    Quaternion fixingPitch = Quaternion.RotateTowards(transform.rotation, pitchStableRotation, pitchAutoLevelSpeed * Time.deltaTime);
        //    transform.rotation = fixingPitch;
        //}
    }

}
