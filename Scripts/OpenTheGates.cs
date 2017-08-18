using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class OpenTheGates : NetworkBehaviour {

	void Start () {
	
	}

    public override void OnStartLocalPlayer() {

        base.OnStartLocalPlayer();

        // start the tuerwaechterin & tuerwaechterkinder scripte
        Tuerwaechterkinder[] knut = FindObjectsOfType<Tuerwaechterkinder>();
        for(var i = 0; i < knut.Length; i++) {

            knut[i].enabled = true;
        }

        Tuerwaechterin knutmama = FindObjectOfType<Tuerwaechterin>();
        knutmama.enabled = true;

        Aufwach lore = FindObjectOfType<Aufwach>();
        lore.enabled = true;
    }

    void Update () {
	
	}
}
