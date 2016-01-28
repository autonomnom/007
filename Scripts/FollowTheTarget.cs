using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// camera, who to follow and staying smooth
public class FollowTheTarget : MonoBehaviour {

    public enum Polizei {

        nah,
        fern,
        reisendzu,
        reisendweg
    }
    [HideInInspector] public Polizei status = Polizei.fern;

    public LayerMask groundMask;
   
    [SerializeField] private float camDistance = 42f;
    [SerializeField] private float camUp = 10f;
    [SerializeField] private float smoothy1 = 1.2f;
    private float camDistanceZoom = 1f;
    private float camUpZoom = 2f;
    private float lerpBeschleunigung;

    private float _camDistance;
    private float _camUp;

    private float statDelay = 0f;
    private float statDelayMax = 0.2f;

    private Xray rray;
    private float rayWeiteAmPo = 10f;

    private Vector3 targetPosition;
    private Vector3 _targetPosition;
    private Transform foollow;
    private Rigidbody body;


    void Awake() {

        findFollow();
        body = GetComponent<Rigidbody>();
        rray = new Xray(body);

        _camDistance = camDistance;
        _camUp = camUp;

        rray.activateDebug = false;
    }

	void Start() {

	}

    void Update() {

        findFollow();
    }

	void FixedUpdate () {

        // ckecking for state, zoom in or out
        zoomy();

        // getting in position
        targetPosition = foollow.position + foollow.up * camUp - foollow.forward * camDistance;

        // applying the targetposition to the camera's body position
        body.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothy1);

        // adjust rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(foollow.position - transform.position, foollow.up), 0.1f);
	}


    /// <summary>
    /// Find the identity to follow.
    /// </summary>
    void findFollow() {

        foollow = GameObject.Find(G.identitaet.ToString()).GetComponentInChildren<Seele>().followMePlz;
    }


    /// <summary>
    /// Check for any obstacles between the camera and the desired identity.
    /// </summary>
    /// <returns>True if something is between.</returns>
    bool obstacleDetected() {

        GameObject ida = GameObject.Find(G.identitaet.ToString());
        float distance = Vector3.Distance(ida.transform.position, body.position);
        RaycastHit bam;

        // straight "camera <-> identity" check
        Ray augenKontakt = new Ray(body.position, -(body.position - ida.transform.position));
        if(Physics.Raycast(augenKontakt, out bam, distance, groundMask)) {

            Debug.Log("auge");
            return true;
        }

        // cylinder check around the identity
        Ray[] zyli = rray.tanzTanzRingeltanz(ida.transform, 8, 3f, .8f, .15f);
        for (int i = 0; i < zyli.Length; i++) {

            if (Physics.Raycast(zyli[i], out bam, distance - 5f, groundMask)) {
                
                Debug.Log("zylinder");
                return true;
            }
        }

        // side & back check + backward check while zooming
        Transform kampi = GameObject.Find(G.identitaet.ToString()).GetComponentInChildren<Fakamera>().transform;
        Vector3 kampiRueckwaerts = (body.position - ida.transform.position).normalized;
        Vector3[] position = new Vector3[3];
            position[0] = kampi.position + Vector3.Scale(kampiRueckwaerts, new Vector3(rayWeiteAmPo, rayWeiteAmPo, rayWeiteAmPo));  // süd
            position[1] = kampi.position + Vector3.Scale(-kampi.right, new Vector3(rayWeiteAmPo, rayWeiteAmPo, rayWeiteAmPo));      // west
            position[2] = kampi.position + Vector3.Scale(kampi.right, new Vector3(rayWeiteAmPo, rayWeiteAmPo, rayWeiteAmPo));       // ost
        for (int i = 0; i < position.Length; i++) {

            if(Physics.Raycast(new Ray(body.position, (body.position - position[i]).normalized), out bam, rayWeiteAmPo, groundMask)) {

                Debug.Log("kompass");
                return true;
            }
        }

        return false;
    }


    /// <summary>
    /// Adjusting camera orientation depending on its state (Polizei).
    /// </summary>
    void zoomy() {

        // no eye contact to the identity
        if (obstacleDetected()) {

            kameraHin();
        }
        else {

            kameraWeg();
        }
    }

    void kameraHin() {

        // if the camera is at max distance to the identity
        if (status == Polizei.fern) {

            statDelay = 0;
            status = Polizei.reisendzu;
        }
        // if it is already traveling
        else if (status == Polizei.reisendzu) {

            // to the target
            if (camDistance != camDistanceZoom || camUp != camUpZoom) {

                if (camDistance > camDistanceZoom) { camDistance = Mathf.SmoothDamp(camDistance, camDistanceZoom, ref lerpBeschleunigung, 0.01f, 300, Time.fixedDeltaTime); }
                if (camUp > camUpZoom) { camUp = Mathf.SmoothDamp(camUp, camUpZoom, ref lerpBeschleunigung, 0.05f, 25, Time.fixedDeltaTime); }
                statDelay = 0;
            }
            else { status = Polizei.nah; statDelay = 0; }
        }
        // away from it
        else if (status == Polizei.reisendweg) {

            status = Polizei.reisendzu;
            statDelay = 0;
        }
    }

    void kameraWeg() {

        // delay while camera is close, after the delay camera is drving back
        if (status == Polizei.nah) {

            if (statDelay < statDelayMax) { statDelay += Time.fixedDeltaTime; }
            else { statDelay = 0; status = Polizei.reisendweg; }
        }
        // if camera is in move while the identity is seen
        else if (status == Polizei.reisendzu) {

            // towards the target although now there is eye contact
            if (statDelay < statDelayMax) { statDelay += Time.fixedDeltaTime; }
            else { statDelay = 0; status = Polizei.reisendweg; }
        }
        // away to the original camera position
        else if (status == Polizei.reisendweg) {

            if (camDistance != _camDistance || camUp != _camUp) {

                if (camDistance < _camDistance) { camDistance = Mathf.SmoothDamp(camDistance, _camDistance, ref lerpBeschleunigung, 1f, 36, Time.fixedDeltaTime); }
                if (camUp < _camUp) { camUp = Mathf.SmoothDamp(camUp, _camUp, ref lerpBeschleunigung, 1f, 25, Time.fixedDeltaTime); }
                statDelay = 0;
            }
            else {

                statDelay = 0;
                status = Polizei.fern;
            }
        }
    }
}
