using UnityEngine;
using System.Collections;

// to be attached to an root identity object
public class Biografie : MonoBehaviour {

    [HideInInspector] public bool aktiv = false;

    void Awake() {

    }

	void Start () {
	
	}
	
	void Update () {

        // sets to active if the one to be played, otherwise sets to !active
        if (G.identitaet.ToString() == gameObject.name) {

            aktiv = true;
        }
        else aktiv = false;
	}
}
