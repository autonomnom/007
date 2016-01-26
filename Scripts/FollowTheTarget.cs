using UnityEngine;
using System.Collections;

// camera, who to follow and staying smooth
public class FollowTheTarget : MonoBehaviour {

    [SerializeField] private float camDistance = 42f;
    [SerializeField] private float camUp = 10f;
    [SerializeField] private float smoothy1 = 1.2f;
    [SerializeField] private int rayAmount = 8;
    [SerializeField] private float lerpZeit = 0.8f;
    private float camDistanceZoom = 1f;
    private float camUpZoom = 2f;
    private float lerpBeschleunigung;

    private float _camDistance;
    private float _camUp;

    public enum Polizei {

        nah,
        fern,
        reisendzu,
        reisendweg
    }
    [HideInInspector] public Polizei status = Polizei.fern;

    private float statDelay = 0f;
    private float statDelayMax = 0.5f;

    private Ray kontakt;
    private Ray[] blicke;
    private Xray rundumblick;
    private float blickdistanz = 2f;
    private bool zoom;
    public LayerMask groundMask;

    private Vector3 targetPosition;
    private Vector3 _targetPosition;
    private Transform foollow;
    private Rigidbody body;

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
    }

	void FixedUpdate () {

        // recalculating ray origns & directions 
        blicke = rundumblick.letTheRaysRain(rayAmount);

        // spherical ray cast around the camera
        zoom = rundumblick.checkForCollisionNearBy(blicke, groundMask, blickdistanz);

        // special case when no spherical detection is positive but would be when zooming out
        if (status != Polizei.fern) {

            Ray rueckwarts = new Ray(body.position, body.position - foollow.position);
            if (Physics.SphereCast(rueckwarts, blickdistanz * 2, 10, groundMask)) {

                zoom = true;
            }
        }

        // ckecking for state, zoom in or out
        cameraOrientation();


        // getting in position
        targetPosition = foollow.position + foollow.up * camUp - foollow.forward * camDistance;
        body.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothy1);

        // adjust rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(foollow.position - transform.position, foollow.up), 0.15f);
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
    bool augenKontakt() {

        GameObject ida = GameObject.Find(G.identitaet.ToString());
        float distance = Vector3.Distance(ida.transform.position, body.position);
        RaycastHit bam;

        kontakt = new Ray(body.position, -(body.position - ida.transform.position));

        if(Physics.Raycast(kontakt,out bam,distance,groundMask)) {

            return false;
        }
        else return true;
    }

    /// <summary>
    /// Adjusting camera orientation depending on its state (Polizei).
    /// </summary>
    void cameraOrientation() {

        // no eye contact to the identity
        if (!augenKontakt() || zoom) {

            // if the camera is at max distance to the identity
            if (status == Polizei.fern) {

                statDelay = 0;
                status = Polizei.reisendzu;
            }
            // if it is already traveling
            else if (status == Polizei.reisendzu) {

                // to the target
                if (camDistance != camDistanceZoom && camUp != camUpZoom) {

                    camDistance = Mathf.SmoothDamp(camDistance, camDistanceZoom, ref lerpBeschleunigung, Time.fixedDeltaTime / lerpZeit); ;
                    camUp = Mathf.SmoothDamp(camUp, camUpZoom, ref lerpBeschleunigung, Time.fixedDeltaTime / lerpZeit);
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
        else if (augenKontakt() || !zoom) {

            // delay while camera is close, after the delay camera is drving back
            if (status == Polizei.nah) {

                if (!zoom) {

                    if (statDelay < statDelayMax) { statDelay += Time.fixedDeltaTime; }
                    else { statDelay = 0; status = Polizei.reisendweg; }
                }
                else statDelay = 0;

            }
            // if camera is in move while the identity is seen
            else if (status == Polizei.reisendzu) {

                // towards the target although now there is eye contact
                if (statDelay < statDelayMax) { statDelay += Time.fixedDeltaTime; }
                else { statDelay = 0; status = Polizei.reisendweg; }
            }
            // away to the original camera position
            else if(status == Polizei.reisendweg) {

                if (camDistance != _camDistance && camUp != _camUp) {

                    camDistance = Mathf.SmoothDamp(camDistance, _camDistance, ref lerpBeschleunigung, Time.fixedDeltaTime / lerpZeit);
                    camUp = Mathf.SmoothDamp(camUp, _camUp, ref lerpBeschleunigung, Time.fixedDeltaTime / lerpZeit);
                } 
                else {

                    statDelay = 0;
                    status = Polizei.fern;
                 }
            }
        }
    }
}
