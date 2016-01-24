using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// The off
public class Puppenspiel : MonoBehaviour {

    public int staffelung = 18;
    public float test = 0;
    private bool check = false;

    void Awake() {

    }

	void Start () {

	}
	
	void Update () {
	
        // switch the identities
        if (Input.GetKeyDown(KeyCode.T)) {

            if (G.identitaet != G.Who.Adam) { G.identitaet++; }
            else G.identitaet = G.Who.Igor;
        }

        ////////////////////// E:X:P:E:R:I:M:E:N:T:E &&&//////////////////////////
        // kugelkoordinaten checks
        else if (Input.GetKeyDown(KeyCode.L)) {

            check = !check;
        }

        if (check) { 

            drawTheRays(); 
        }
	}

    void drawTheRays() {

        Vector3[] test = letsTry(staffelung);

        for (int i = 0; i < test.Length; i++) {

            Debug.DrawRay(GetComponent<Transform>().transform.position, test[i], Color.red);
        }
    }

    public Vector3[] letsTry(int amount) {

        // with that code 12 for camera and 18 for player
        Vector3[] rayt = new Vector3[amount];

        for (int i = 0; i < rayt.Length; i++) {

            float abstandY = Mathf.PI / amount;
            float y = Mathf.Cos(i * abstandY);
            float phi = i * ((10f * Mathf.PI) / amount);
            float x = Mathf.Sin(i * abstandY) * Mathf.Sin(phi);
            float z = Mathf.Sin(i * abstandY) * Mathf.Cos(phi);
            rayt[i] = new Vector3(x, y, z);      
        }

        return rayt;
    }

}
