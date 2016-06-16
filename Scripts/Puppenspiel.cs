using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// The off
public class Puppenspiel : MonoBehaviour {

    void Awake() {

    }

	void Start () {

	}
	
	void Update () {
	
        // switch the identities
        if (Input.GetKeyDown(KeyCode.T)) {

            if (G.identitaet != G.Who.ADAM) { G.identitaet++; }
            else G.identitaet = G.Who.IGOR;
        }

        // switch the camera view
        if (Input.GetKeyDown(KeyCode.F)) {

            if (G.sicht == G.cam.FIRST) { G.sicht = G.cam.THIRD; }
            else { G.sicht = G.cam.FIRST; }
        }
	}
}
