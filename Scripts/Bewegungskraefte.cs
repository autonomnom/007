using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.VR;

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
    public Quaternion rotationAmount;

    // for goundcheck, needed in Xray.cs
    [HideInInspector] public bool grounded;
    public LayerMask groundMask;

    // oculus as rotation controller
    [HideInInspector] public float angliene;
    [HideInInspector] public float anglieForX;
    public float schmus = 10f;
    private bool left;
    private List<float> schmusingList = new List<float>();
    private int maxSchmusing = 15;
    private float lastAngliene = 0;

    Rigidbody body;
    Biografie bio;

	void Start () {

        body = GetComponent<Rigidbody>();
        bio = GetComponent<Biografie>();

        rotationAmount = new Quaternion(0, 0, 0, 1);

        for (int i = 0; i < schmusingList.Count; i++) {
            schmusingList.Add(0);
        }
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
        
        // 3rd Person View
        if (!bio.fipsi) {   rotationAmount = MTPV(); }
        // 1st Person View / Oculus
        else {              rotationAmount = !bio.weare ? MFPV() : OFPV(); }
    }
	

	void FixedUpdate() {

        if(!isLocalPlayer) {

            return;
        }

        //applying movement
        body.MovePosition(body.position + GetComponent<Transform>().TransformDirection(moveAmount) * Time.fixedDeltaTime);


        //applying mouse rotation
        if (bio.fipsi) {

            if (!bio.weare) {
                //body.MoveRotation(rotationAmount * body.rotation);                                                                                // jitterless, dafür sind die kanten härter + er hängt an diesen etwas
                body.MoveRotation(Quaternion.Slerp( body.rotation, rotationAmount * body.rotation, Time.fixedDeltaTime * turnSpeed * 10 ));        //  THE ORIGINAL MOUSELOOK
            }
            else {
                //body.transform.rotation =  rotationAmount * body.transform.rotation; 
                body.MoveRotation(Quaternion.Slerp( body.rotation, rotationAmount * body.rotation, Time.fixedDeltaTime * turnSpeed * 5 ));
            } 
        }
        else { 
            body.MoveRotation(Quaternion.Slerp(body.rotation, rotationAmount * body.rotation, Time.fixedDeltaTime * turnSpeed));
        }        
	}


    /// <summary>
    /// Have slight mouse rotation on the UP-axis for 3d person view.
    /// </summary>
    Quaternion MTPV() {

        Vector3 horizontalInput = new Vector3(Input.GetAxis("Mouse X"), 0, 0).normalized;
        float desiredAngle = 49f;
        desiredAngle *= horizontalInput.x;
        return Quaternion.AngleAxis(desiredAngle, body.transform.up);
    }


    /// <summary>
    /// First person mouse view.
    /// Thanks to ben esponito's First Person Drifter Controller - @torahhorse
    /// </summary>
    //  X - AXIS
    Quaternion MFPV () {

        float rotAverageX = 0f;
        List<float> rotArrayX = new List<float>();              // ?? Doesnt it has to be in the top so it doesnt create a new list every frame ??
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
        return Quaternion.AngleAxis(rotAverageX, body.transform.up);
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

    /// <summary>
    /// Oculus First Person Controller
    /// </summary>
    /// <returns></returns>
    Quaternion OFPV() {

        // get the camera
        Transform fipsi = GameObject.Find("Fipsi").transform;

        // recalculating the forward vector of the camera to make it orthogonal to the body.transform.up vector
        Vector3 fipsoff = fipsi.forward - (Vector3.Dot(fipsi.forward, body.transform.up) * body.transform.up);

        // checking for the angle between orthoganlised fipsi.forward and body.forward - should be 0 on mouse and changing while on oculus
        angliene =  Vector3.Distance(fipsi.forward, body.transform.right) <= Vector3.Distance(fipsi.forward, -body.transform.right)
                    ? Vector3.Angle(body.transform.forward, fipsoff)    //Mathf.Round(Vector3.Angle(body.transform.forward, fipsoff))
                    : -Vector3.Angle(body.transform.forward, fipsoff);  //Mathf.Round(-Vector3.Angle(body.transform.forward, fipsoff));

        // wiz spinning /remove applyfriction
        // ------------------------- // anglieForX = angliene;
        // ------------------------- // schmus = 10f;
        // ------------------------- // Quaternion.AngleAxis(anglieForX / schmus, body.transform.up);
        // ------------------------- // has to be added to the rotation of Parasit +

        // applyFriction();
        // Quaternion.AngleAxis(anglieForX  * schmus, body.transform.up);
        // does move the body according to the oculus 
        // but overmoves the fipsi

        Debug.Log("bewegung" + angliene);

        return Quaternion.AngleAxis(angliene, body.transform.up);
    }

    // deprecated
    void applyFriction() {

        float averageDifference = 0;
        float anglieneDifference = 0;

        // get velocity
        if (lastAngliene != angliene) {
            anglieneDifference = angliene - lastAngliene;
        }
        else anglieneDifference = 0;

        lastAngliene = angliene;

        // adding difference to the list
        schmusingList.Add(anglieneDifference);

        // checking if list is full, remove first entry
        if (schmusingList.Count >= maxSchmusing) {
            schmusingList.RemoveAt(0);
        }

        // get average value of schmusingList
        for (int i = 0; i < schmusingList.Count; i++) {
            averageDifference += schmusingList[i];
        }
        averageDifference /= schmusingList.Count;

        // and round it to 2 decimal places
        averageDifference = Mathf.Round(averageDifference * 100f) / 100f;

        // --------------

        anglieForX = averageDifference;
    }
}
