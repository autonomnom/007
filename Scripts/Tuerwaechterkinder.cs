using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Tuerwaechterkinder : NetworkBehaviour {

    // has to be innen or außen
    [SerializeField] private Tuerwaechterin.tuerposi choreo;

    private Tuerwaechterin mama;

    void Awake() {

        mama = GetComponentInParent<Tuerwaechterin>();
    }

	void Start () {
	
	}
	
	void Update () {

	}

    void OnTriggerEnter (Collider other) {

        // only check for avatars
        // have to find something like if NetworkClientId = other.Id
        if (other.tag != "Igor" || !other.GetComponent<NetworkIdentity>().isLocalPlayer) { return; }
        Debug.Log(other.GetComponent<NetworkIdentity>().isLocalPlayer + "  " + other.GetComponent<NetworkIdentity>().playerControllerId);

        if (choreo == Tuerwaechterin.tuerposi.außen) { mama.hello = true; }
        if (choreo == Tuerwaechterin.tuerposi.innen) { mama.baba = true; }

        // if mama.knolle is wasanderes, she is choreo now, otherwise she stays what she is
        // ~ finding the first trigger
        mama.knolle = (mama.knolle == Tuerwaechterin.tuerposi.wasanderes) ? choreo : mama.knolle;

        if (mama.hello && mama.baba) {

            // beginning of a confusing true-false-drama
            bool nighttime = (mama.knolle == Tuerwaechterin.tuerposi.außen) ? false : true;
            mama.licht(nighttime);
        }
    }

    void OnTriggerStay (Collider other) {

        // only check for avatars
        if (other.tag != "Igor" || !other.GetComponent<NetworkIdentity>().isLocalPlayer) { return; }
    }

    void OnTriggerExit (Collider other) {

        // only check for avatars
        if (other.tag != "Igor" || !other.GetComponent<NetworkIdentity>().isLocalPlayer) { return; }

        if (choreo == Tuerwaechterin.tuerposi.außen) { mama.hello = false; }
        if (choreo == Tuerwaechterin.tuerposi.innen) { mama.baba = false; }

        if(!mama.hello && !mama.baba) {

            mama.knolle = Tuerwaechterin.tuerposi.wasanderes;
        }
    }
}
