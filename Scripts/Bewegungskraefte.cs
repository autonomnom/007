using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Biografie))]

// let the identities dance!
public class Bewegungskraefte : NetworkBehaviour {

    public float walkSpeed = 13f;
    public float turnSpeed = 1.95f;
    public float jumpForce = 25f;
    public float colliderHalf = 1;

    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
    Quaternion rotationAmount;

    // for goundcheck, needed in Xray.cs
    [HideInInspector] public bool grounded;
    public LayerMask groundMask;

    Rigidbody body;
    Biografie bio;

	void Start () {

        body = GetComponent<Rigidbody>();
        bio = GetComponent<Biografie>();

        rotationAmount = new Quaternion(0, 0, 0, 1);
	}

    void Update() {

        if (!isLocalPlayer) {

            return;
        }

        // WASD
        // forward + backwards + left + right
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        Vector3 plusWalk = dir * walkSpeed;
        moveAmount = Vector3.SmoothDamp(moveAmount, plusWalk, ref smoothMoveVelocity, .15f);

        // up
        if (Input.GetButtonDown("Jump")) {

            if (grounded) {

                body.AddForce(body.transform.up * jumpForce, ForceMode.VelocityChange);
            }
        }

        grounded = false;
        Ray charles = new Ray(transform.position, -transform.up);
        RaycastHit wonder;
        if (Physics.Raycast(charles, out wonder, colliderHalf + .1f, groundMask)) {

            grounded = true;
        }


        // MOUSE
        // 3rd person
        if (!bio.fipsi) {

            MTPV();
        }
        // 1st person
        else {
       
            MFPV();
        }
    }
	
	void FixedUpdate() {

        if(!isLocalPlayer) {

            return;
        }

        //applying movement
        body.MovePosition(body.position + GetComponent<Transform>().TransformDirection(moveAmount) * Time.fixedDeltaTime);

        //applying mouse rotation
        if (bio.fipsi) { 

            //body.MoveRotation(rotationAmount * body.rotation);  // jitterless, dafür sind die kanten härter + er hängt an diesen etwas
            body.MoveRotation(Quaternion.Slerp(body.rotation, rotationAmount * body.rotation, Time.fixedDeltaTime * turnSpeed * 10));
        }
        else {

            body.MoveRotation(Quaternion.Slerp(body.rotation, rotationAmount * body.rotation, Time.fixedDeltaTime * turnSpeed));
        }        
	}

    /// <summary>
    /// Have slight mouse rotation on the UP-axis for 3d person view.
    /// </summary>
    void MTPV() {

        Vector3 horizontalInput = new Vector3(Input.GetAxis("Mouse X"), 0, 0).normalized;
        float desiredAngle = 49f;
        desiredAngle *= horizontalInput.x;
        rotationAmount = Quaternion.AngleAxis(desiredAngle, body.transform.up);
    }

    /// <summary>
    /// First person mouse view.
    /// Thanks to ben esponito's First Person Drifter Controller - @torahhorse
    /// </summary>
    void MFPV () {

        float rotAverageX = 0f;
        List<float> rotArrayX = new List<float>();
        float sensitivityX = 7f;
        float rotationX = 0f;
        float framesOfSmoothing = 25f;
        float minimumX = -360F;
	    float maximumX = 360F;

        // collect the mouse data
        rotationX += Input.GetAxis("Mouse X") * sensitivityX;

        // process a smooth average value of the mouse moving
        rotArrayX.Add(rotationX);

        if (rotArrayX.Count >= framesOfSmoothing) {
            rotArrayX.RemoveAt(0);
        }
        for (int i = 0; i < rotArrayX.Count; i++) {
            rotAverageX += rotArrayX[i];
        }
        rotAverageX /= rotArrayX.Count;
        rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);

        // save the desired amount
        rotationAmount = Quaternion.AngleAxis(rotAverageX, body.transform.up);
    }

    /// <summary>
    /// Set a fixed angle of moving with the mouse.
    /// </summary>
    float ClampAngle(float angle, float min, float max) {
        angle = angle % 360;
        if ((angle >= -360F) && (angle <= 360F)) {
            if (angle < -360F) {
                angle += 360F;
            }
            if (angle > 360F) {
                angle -= 360F;
            }
        }
        return Mathf.Clamp(angle, min, max);
    }
}
