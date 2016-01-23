using UnityEngine;
using System.Collections;
	
[RequireComponent(typeof(Xray))]
[RequireComponent(typeof(Rigidbody))]

// gravity 
public class DownLogic : MonoBehaviour {

    private Vector3 chooseGooseNorm;
    private Vector3 chooseGoosePoint;
    private Vector3 chooseGoose;

    // script for finding the right normal
    Xray rray;

    public float gravity = -250f;
    Rigidbody body;

    void Awake() {

        rray = GetComponent<Xray>();

        body = GetComponent<Rigidbody>();
        body.constraints = RigidbodyConstraints.FreezeRotation;
        body.useGravity = false;
    }

	void Start () {

	}
	
	void Update () {
	
	}

    void FixedUpdate() {

        // applying rotation based on the triangles normal below
        chooseGooseNorm = rray.theNorm;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, chooseGooseNorm) * transform.rotation, .5f);
        body.AddForce(chooseGooseNorm * gravity);
    }
}
