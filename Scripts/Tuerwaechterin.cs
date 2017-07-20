using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Tuerwaechterin : MonoBehaviour {

    public enum tuerposi {

        innen,
        außen,
        wasanderes,
    }

    public GameObject schnwarzzPAPA;
    public Aufwach schnarzz;
    [HideInInspector] public bool hello = false;
    [HideInInspector] public bool baba = false;

    [HideInInspector] public tuerposi knolle;

    void Awake() {

        knolle = tuerposi.wasanderes;
    }

	void Start () {

        if (schnwarzzPAPA == null) {

            Debug.LogError("No light-system connected.");
        }
        else {

            schnarzz = schnwarzzPAPA.GetComponent<Aufwach>();
        }
    }

    void Update () {

	}

    public void licht (bool night) {

        schnarzz.intransit = true;
        schnarzz.nighttime = night;
    }
}
