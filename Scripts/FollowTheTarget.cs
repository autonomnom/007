using UnityEngine;
using System.Collections;

// camera, who to follow and staying smooth
public class FollowTheTarget : MonoBehaviour {

    [SerializeField] private float camDistance = 42f;
    [SerializeField] private float camUp = 10f;
    [SerializeField] private float smoothy1 = 1.2f;
    [SerializeField] private int rayAmount = 8;
    [SerializeField] private float lerpFactor = 4;
    private float _camDistance;
    private float _camUp;

    private Vector3 targetPosition;
    private Vector3 _targetPosition;
    private Transform foollow;
    private Rigidbody body;

    private Ray kontakt;

    private Ray[] blicke;
    private Xray rundumblick;
    private bool zoom;
    public LayerMask groundMask;


    void Awake() {

        findFollow();
        body = GetComponent<Rigidbody>();
        rundumblick = new Xray(body);

        _camDistance = camDistance;
        _camUp = camUp;

        rundumblick.activateDebug = false;
    }

	void Start() {
	    
	}

    void Update() {
        
        findFollow();
        checkEyeContact();
        Debug.Log(checkEyeContact());

        blicke = rundumblick.letTheRaysRain(rayAmount);
        zoom = rundumblick.checkForCollisionNearBy(blicke, groundMask, 2f);
        
    }

	void FixedUpdate () {

        Debug.Log(zoom);
 
        // getting in position
        // using body.postion instead of transform.position enables camera collision, got to be adjusted though
        adjustUpDist();
        targetPosition = foollow.position + foollow.up * camUp - foollow.forward * camDistance;
        if (zoom) { targetPosition = Vector3.Lerp(targetPosition, targetPosition - foollow.up * 5f, 1.1f); }
        body.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothy1);

        // adjust rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(foollow.position - transform.position, foollow.up), 0.15f);
	}

    /// <summary>
    /// Find the identity to follow.
    /// </summary>
    void findFollow() {

        Transform hihi = GameObject.Find(G.identitaet.ToString()).GetComponentInChildren<Seele>().transform;
        foollow = GameObject.Find(hihi.name).transform;
    }

    /// <summary>
    /// Check for any obstacles between the camera and the desired identity.
    /// </summary>
    /// <returns>True if something is between.</returns>
    bool checkEyeContact() {

        GameObject ida = GameObject.Find(G.identitaet.ToString());
        float distance = Vector3.Distance(ida.transform.position, body.position);
        RaycastHit bam;

        kontakt = new Ray(body.position, -(body.position - ida.transform.position));

        if(Physics.Raycast(kontakt,out bam,distance,groundMask)) {

            return true;
        }
        else return false;
    }

    void adjustUpDist() {

        if (checkEyeContact() || zoom) {

            camDistance = Mathf.Lerp(camDistance, 11, lerpFactor); ;
            camUp = Mathf.Lerp(camUp, 10, lerpFactor / 3);
        }
        else {
            camDistance = Mathf.Lerp(camDistance, _camDistance, lerpFactor);
            camUp = Mathf.Lerp(camUp, _camUp, lerpFactor);
        }
    }
}
