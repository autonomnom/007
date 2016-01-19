using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Bewegungskraefte))]

public class Xray : MonoBehaviour {

    Rigidbody body;

    Bewegungskraefte muskel;
    [Range(0, 26)] public int rayAmount = 18;
    [HideInInspector] public Vector3 theNorm;

	void Start () {

        body = GetComponent<Rigidbody>();
        muskel = GetComponent<Bewegungskraefte>();
	}
	
	void Update () {

        //check for the normal you standing on
        if (muskel.grounded) {

            Ray rey = new Ray(body.position, -body.transform.up);
            RaycastHit bey;

            if (Physics.Raycast(rey, out bey, 1.5f, muskel.groundMask)) {

                theNorm = bey.normal;
            }
            else theNorm = body.transform.up;
        }

        //check for normals all around you while flying
        else {

            if (rayAmount != 0) {

                //gather the desired rays
                Ray[] allRayz = new Ray[rayAmount];

                for (int i = 0; i < allRayz.Length; i++) {

                    if (i == 0) { allRayz[i] = new Ray(body.position, -body.transform.up); }
                    else if (i == 1) { allRayz[i] = new Ray(body.position, body.transform.up); }
                    else if (i == 2) { allRayz[i] = new Ray(body.position, body.transform.right); }
                    else if (i == 3) { allRayz[i] = new Ray(body.position, -body.transform.right); }
                    else if (i == 4) { allRayz[i] = new Ray(body.position, body.transform.forward); }
                    else if (i == 5) { allRayz[i] = new Ray(body.position, -body.transform.forward); }
                    else if (i == 6) { allRayz[i] = new Ray(body.position, body.transform.up + body.transform.forward + body.transform.right); }
                    else if (i == 7) { allRayz[i] = new Ray(body.position, body.transform.up + body.transform.forward - body.transform.right); }
                    else if (i == 8) { allRayz[i] = new Ray(body.position, body.transform.up - body.transform.forward + body.transform.right); }
                    else if (i == 9) { allRayz[i] = new Ray(body.position, body.transform.up - body.transform.forward - body.transform.right); }
                    else if (i == 10) { allRayz[i] = new Ray(body.position, -body.transform.up + body.transform.forward + body.transform.right); }
                    else if (i == 11) { allRayz[i] = new Ray(body.position, -body.transform.up + body.transform.forward - body.transform.right); }
                    else if (i == 12) { allRayz[i] = new Ray(body.position, -body.transform.up - body.transform.forward + body.transform.right); }
                    else if (i == 13) { allRayz[i] = new Ray(body.position, -body.transform.up - body.transform.forward - body.transform.right); }
                    else if (i == 14) { allRayz[i] = new Ray(body.position, body.transform.forward + body.transform.right); }
                    else if (i == 15) { allRayz[i] = new Ray(body.position, body.transform.forward - body.transform.right); }
                    else if (i == 16) { allRayz[i] = new Ray(body.position, -body.transform.forward + body.transform.right); }
                    else if (i == 17) { allRayz[i] = new Ray(body.position, -body.transform.forward - body.transform.right); }
                    else if (i == 18) { allRayz[i] = new Ray(body.position, body.transform.up + body.transform.forward); }
                    else if (i == 19) { allRayz[i] = new Ray(body.position, body.transform.up - body.transform.forward); }
                    else if (i == 20) { allRayz[i] = new Ray(body.position, body.transform.up + body.transform.right); }
                    else if (i == 21) { allRayz[i] = new Ray(body.position, body.transform.up - body.transform.right); }
                    else if (i == 22) { allRayz[i] = new Ray(body.position, -body.transform.up + body.transform.forward); }
                    else if (i == 23) { allRayz[i] = new Ray(body.position, -body.transform.up - body.transform.forward); }
                    else if (i == 24) { allRayz[i] = new Ray(body.position, -body.transform.up + body.transform.right); }
                    else if (i == 25) { allRayz[i] = new Ray(body.position, -body.transform.up - body.transform.right); }
                }

                //find the nearest ground
                float distance = 100f;

                for (int i = 0; i < allRayz.Length; i++) {

                    RaycastHit piu;
                    if (Physics.Raycast(allRayz[i], out piu, muskel.groundMask)) {

                        if (piu.distance < distance) {

                            distance = piu.distance;
                            theNorm = piu.normal;
                        }
                    }
                }

                //Hint for setting the right amount of casted rays
                if (rayAmount != 2 && rayAmount != 6 && rayAmount != 14 && rayAmount != 18 && rayAmount != 26) {

                    Debug.LogAssertion("Rayamount should be 2, 6, 14, 18 or 26 to secure consistent sphercial check.");
                }
            }
            else {

                theNorm = body.transform.up;
                Debug.LogAssertion("No special Raycasts while not being grounded.");
            }
        }	
	}
}
