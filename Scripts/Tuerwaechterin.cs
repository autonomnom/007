using UnityEngine;
using System.Collections;

public class Tuerwaechterin : MonoBehaviour {

    public enum tuerposi {

        innen,
        außen,
        wasanderes,
    }

    private Aufwach schnarzz;
    [HideInInspector] public bool hello = false;
    [HideInInspector] public bool baba = false;

    [HideInInspector] public tuerposi knolle;

    void Awake() {

        schnarzz = FindObjectOfType<Aufwach>();
        knolle = tuerposi.wasanderes;
    }

	void Start () {
	
	}
	
	void Update () {
	
	}

    public void licht (bool night) {

        schnarzz.intransit = true;
        schnarzz.nighttime = night;
    }
}
