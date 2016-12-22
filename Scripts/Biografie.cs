using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

// to be attached to an root identity object
public class Biografie : NetworkBehaviour {

    [HideInInspector] public bool fipsi = true;

    private YoogooohUD huddi;
    private float thirdDelay = 0;

    void Start () {

        if (!isLocalPlayer) {

            return;
        }

        // hide cursor on start
        Cursor.visible = false;

        // hide the network hud
        huddi = FindObjectOfType<YoogooohUD>();
        huddi.showGUI = false;

        // switch from third-person-camera (HUD) to first-person-camera         
        /** to prevent having 2 audiolistener active at the same time */
        FindObjectOfType<Festsitzen>().transform.GetChild(0).gameObject.SetActive(true); 
    }

    void FixedUpdate () {

        if (!isLocalPlayer) {

            GetComponent<MeshRenderer>().enabled = true;
            return;
        }
        else {

            if (fipsi) {

                GetComponent<MeshRenderer>().enabled = false;
            }
            else {

                GetComponent<MeshRenderer>().enabled = true;
            }
        }

        FindObjectOfType<Festsitzen>().folow = this.transform;
        FindObjectOfType<FollowTheTarget>().foollow = this.transform.GetComponentInChildren<Seele>().transform;
        FindObjectOfType<ImKopf>().fooloow = this.transform;

        // switch between first and third person view
        if (thirdDelay < 10) {

            thirdDelay += Time.fixedDeltaTime;
        }
        else if (Input.GetKeyDown(KeyCode.H)) {

            fipsi = !fipsi;
        }
        
        // hide cursor
        if(Input.GetKeyDown(KeyCode.Escape)) {

            Cursor.visible = !Cursor.visible;
            huddi.showGUI = Cursor.visible;
        }
	}
}
