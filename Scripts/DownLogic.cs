using UnityEngine;
using System.Collections;
	
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Bewegungskraefte))]

// gravity 
public class DownLogic : MonoBehaviour {

    private Vector3 chooseGooseNorm;

    public float gravity = -250f;
    Rigidbody body;
    Bewegungskraefte fuese;
    Xray rray;

    void Awake() {

        fuese = GetComponent<Bewegungskraefte>();

        body = GetComponent<Rigidbody>();
        body.constraints = RigidbodyConstraints.FreezeRotation;
        body.useGravity = false;
        
        rray = new Xray(body);
    }

	void Start () {

        chooseGooseNorm = body.transform.up;
	}
	
	void Update () {

        // get normal while on ground..
        if (fuese.grounded) {

            Ray rey = new Ray(body.position, -body.transform.up);
            RaycastHit bey;

            if (Physics.Raycast(rey, out bey, 1.5f, fuese.groundMask)) {

                chooseGooseNorm = bey.normal;
            }
            else chooseGooseNorm = body.transform.up;
        }
        // and while in air.
        else {

            if (rray != null) { 
             
                Ray[] sonne = rray.letTheRaysRain(18);
                chooseGooseNorm = rray.findTheNearestNormal(sonne, fuese.groundMask); 
            }
            else chooseGooseNorm = body.transform.up;
        }
	}

    void FixedUpdate() {

        // applying rotation based on the triangles normal below
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, chooseGooseNorm) * transform.rotation, .5f);
        body.AddForce(chooseGooseNorm * gravity);
    }
}
