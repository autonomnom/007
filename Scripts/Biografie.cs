using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.VR;

// to be attached to an root identity object
public class Biografie : NetworkBehaviour {

    [HideInInspector]
    public bool fipsi = true;
    [HideInInspector]
    public bool weare = true;

    [HideInInspector]
    public int mouzesenzitivity = 10;
    private GUIStyle mouz = new GUIStyle();

    private YoogooohUD huddi;
    private float thirdDelay = 0;

    void Awake() {

        if (!isLocalPlayer) {

            return;
        }
    }

    void Start() {

        if (!isLocalPlayer) {

            return;
        }

        // hide cursor on start
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // hide the network hud
        huddi = FindObjectOfType<YoogooohUD>();
        huddi.showGUI = false;

        // switch from third-person-camera (HUD) to first-person-camera         
        /** to prevent having 2 audiolistener active at the same time */
        FindObjectOfType<Festsitzen>().transform.GetChild(0).gameObject.SetActive(true);
    }

    public override void OnStartLocalPlayer() {

        base.OnStartLocalPlayer();
        /*
        // start the tuerwaechterin & tuerwaechterkinder scripte
        Tuerwaechterkinder[] knut = FindObjectsOfType<Tuerwaechterkinder>();
        for (var i = 0; i < knut.Length; i++) {

            knut[i].enabled = true;
        }

        Tuerwaechterin knutmama = FindObjectOfType<Tuerwaechterin>();
        knutmama.enabled = true;

        Aufwach lore = FindObjectOfType<Aufwach>();
        lore.enabled = true;*/
    }

    void FixedUpdate() {

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
        if (Input.GetKeyDown(KeyCode.Escape)) {

            Cursor.lockState = CursorLockMode.Locked;

            Cursor.visible = !Cursor.visible;
            huddi.showGUI = Cursor.visible;
        }

        // check for VR
        if (VRDevice.isPresent) { weare = true; }
        else { weare = false; }

        // changing the sensitivity without mouse
        if (Cursor.visible || !weare) {

            Cursor.lockState = CursorLockMode.None;

            if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus)) {

                mouzesenzitivity -= 1;
            }

            if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus)) {

                mouzesenzitivity += 1;
            }
        }
    }

    // extra class just for ui (unity intern)
    void OnGUI() {

        // don't draw if isnt menu button or if there is an oculus set up
        if (!Cursor.visible || weare)
            return;

        // GUIstyle
        mouz.font = (Font)Resources.Load("Fonts/tatuatu");
        mouz.fontSize = 80;
        mouz.fontStyle = FontStyle.Italic;
        // mouz.normal.textColor = new Color(81, 10, 122); // 0x510A7A; doesnt seem to work
        mouz.normal.textColor = new Color(230, 230, 250); 


        GUI.Label(new Rect(Screen.width / 8 * 4, Screen.height / 8 * 3, 200, 100), "" + mouzesenzitivity, mouz);
        GUI.Label(new Rect(Screen.width / 8 * 2, Screen.height / 8 * 2, 200, 100), "Mousesensitivity", mouz);


        if (GUI.Button(new Rect(Screen.width / 3, Screen.height / 8 * 3, 40, 40), "-")) {

            mouzesenzitivity -= 1;
        }

        if (GUI.Button(new Rect((Screen.width / 3) * 2, Screen.height / 8 * 3, 40, 40), "+")) {

            mouzesenzitivity += 1;
        }
    }
}
