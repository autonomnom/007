using UnityEngine;
using System.Collections;

// The off
public class Puppenspiel : MonoBehaviour {

	void Start () {
	
	}
	
	void Update () {
	
        //switch the identities
        if (Input.GetKeyDown(KeyCode.T)) {

            if (G.identitaet != G.Who.Adam) { G.identitaet++; }
            else G.identitaet = G.Who.Igor;
        }
	}
}
