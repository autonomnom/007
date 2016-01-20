using UnityEngine;
using System.Collections;
	
[RequireComponent(typeof(CheckMeshesAndFindTheNearest))]
[RequireComponent(typeof(Rigidbody))]

public class DownLogicOLD : MonoBehaviour {

    private Vector3 chooseGooseNorm;
    private Vector3 chooseGoosePoint;
    private Vector3 chooseGoose;

    //script for finding the right normal
    CheckMeshesAndFindTheNearest chekMesh;

    public float gravity = -250f;
    Rigidbody body;

    void Awake() {

        chekMesh = GetComponent<CheckMeshesAndFindTheNearest>();

        body = GetComponent<Rigidbody>();
        body.constraints = RigidbodyConstraints.FreezeRotation;
        body.useGravity = false;
    }

	void Start () {

	}
	
	void Update () {
	
	}

    void FixedUpdate() {

        //applying rotation based on the triangles normal below
        chooseGooseNorm = chekMesh.theNorm;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, chooseGooseNorm) * transform.rotation, .5f);
        body.AddForce(chooseGooseNorm * gravity);
    }
}
