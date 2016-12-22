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
   
    [SerializeField] private float camDistance = 12f;
    [SerializeField] private float camUp = 2f;
    [SerializeField] private float smoothy1 = 1.2f;
    private float camDistanceZoom = 4f;
    private float camUpZoom = 1.1f;
    private float lerpBeschleunigung;

    private float _camDistance;
    private float _camUp;

    private float statDelay = 0f;
    private float statDelayMax = 0.2f;

    private Xray rray;
    private float rayWeiteAmPo = 10f;

    private Vector3 targetPosition;
    private Vector3 _targetPosition;
    public Transform foollow;
    private Biografie bio;
    private Rigidbody body;
    private SphereCollider sphere;
    private Camera vogelkamera;
    private float _sr;

    void Awake() {

        findFollow();
        body = GetComponent<Rigidbody>();
        rray = new Xray(body);
        vogelkamera = GetComponent<Camera>();

        _camDistance = camDistance;
        _camUp = camUp;
    }

	void Start() {

        sphere = GetComponent<SphereCollider>();
        _sr = sphere.radius;
	}

    void Update() {

    }

	void FixedUpdate () {

        if (foollow == null) {

            // the syntax is called bitshifting and adds layer 5 to the cullingMask of the camera
            int layerMaskel = this.GetComponent<Camera>().cullingMask;
            this.GetComponent<Camera>().cullingMask = layerMaskel | (1 << 5);

            // to cancel fixedupdate, since there is no one to follow
            return;
        }
        else {

            // removes the layer 5
            int layerMaskel = this.GetComponent<Camera>().cullingMask;
            this.GetComponent<Camera>().cullingMask = layerMaskel & ~(1 << 5);
        }

        findFollow();

        // changing between the cameras - maybe use tags instead of active
        if (bio.fipsi) {

            vogelkamera.enabled = false;
            vogelkamera.GetComponent<AudioListener>().enabled = false;
        }
        else {

            vogelkamera.enabled = true;
            vogelkamera.GetComponent<AudioListener>().enabled = true;
        }

        // ckecking for state, zoom in or out
        zoomy();

        // getting in position
        targetPosition = foollow.position + foollow.up * camUp - foollow.forward * camDistance;

        // applying the targetposition to the camera's body position
        body.position = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * smoothy1);

        // adjust rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(foollow.position - transform.position, foollow.up), 0.1f);
	}


    /// <summary>
    /// Find the identity to follow.
    /// </summary>
    void findFollow() {

        if (foollow != null) {

            bio = foollow.GetComponentInParent<Biografie>();
        }
    }


    /// <summary>
    /// Check for any obstacles between the camera and the desired identity.
    /// </summary>
    /// <returns>True if something is between.</returns>
    bool obstacleDetected() {

        Transform ida = foollow.parent;
        float distance = Vector3.Distance(ida.position, body.position);  // ~ 44f by default
        RaycastHit bam;

        sphereResize(distance, 30, 12);

        // straight "camera <-> identity" check
        Ray augenKontakt = new Ray(body.position, -(body.position - ida.position));
                    
                //Debug.DrawLine(body.position, body.position + Vector3.Scale(augenKontakt.direction, new Vector3(distance, distance, distance)), Color.red);

        if(Physics.Raycast(augenKontakt, out bam, distance, groundMask)) {

            return true;
        }

        // cylinder check around the identity
        Ray[] zyli = rray.tanzTanzRingeltanz(ida, 8, 2f, .8f, .15f);
        for (int i = 0; i < zyli.Length; i++) {

                    //Debug.DrawLine(body.position, body.position + Vector3.Scale(zyli[i].direction, new Vector3(distance - .5f, distance - .5f, distance - .5f)), Color.green);

            if (Physics.Raycast(zyli[i], out bam, distance - .5f, groundMask)) {

                return true;
            }
        }

        // side & back check + backward check while zooming
        Transform kampi = foollow.parent.GetComponentInChildren<Fakamera>().transform;
        Vector3 kampiRueckwaerts = (body.position - ida.position).normalized;
        Vector3[] position = new Vector3[3];
            position[0] = kampi.position + Vector3.Scale(kampiRueckwaerts, new Vector3(rayWeiteAmPo, rayWeiteAmPo, rayWeiteAmPo));  // süd
            position[1] = kampi.position + Vector3.Scale(-kampi.right, new Vector3(rayWeiteAmPo, rayWeiteAmPo, rayWeiteAmPo));      // west
            position[2] = kampi.position + Vector3.Scale(kampi.right, new Vector3(rayWeiteAmPo, rayWeiteAmPo, rayWeiteAmPo));       // ost
        for (int i = 0; i < position.Length; i++) {

                    //Debug.DrawLine(body.position, position[i], Color.yellow);

            if(Physics.Raycast(new Ray(body.position, -(body.position - position[i]).normalized), out bam, rayWeiteAmPo, groundMask)) {

                return true;
            }
        }

        return false;
    }

    // resizing the sphere to get more moveing possibilities while being near to the target
    void sphereResize(float distanz, float start = 44f, float ende = 0f) {

        if (distanz < start) {

            sphere.radius = (distanz > ende) ? (_sr / (start / distanz)) / (start / distanz) : 1f;
        }
        else {

            sphere.radius = _sr;
        }
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
            if (!Mathf.Approximately(camDistance, camDistanceZoom) || !Mathf.Approximately(camUp, camUpZoom)) {

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

            if (!Mathf.Approximately(camDistance, _camDistance) || !Mathf.Approximately(camUp, _camUp)) {

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
