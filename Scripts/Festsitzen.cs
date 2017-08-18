using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class Festsitzen : MonoBehaviour {

    public Transform folow;           // getting set by Biografie (on the peeps prefab)
    private float off = 1f;
    public float smooty = 8.5f;

    void Start() {

        if (folow == null) {

            return;
        }

        if (FindObjectOfType<Biografie>().weare) {

            this.gameObject.transform.rotation = folow.rotation;
        }
    }

    void FixedUpdate() {

        if (folow == null) {

            return;
        }

        // place the camera always above the player about "off"
        this.gameObject.transform.position = folow.transform.position + Vector3.Scale(folow.up, new Vector3(off, off, off));

        // get his rotation
        if (!FindObjectOfType<Biografie>().weare) {

            this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, folow.transform.rotation, Time.fixedDeltaTime * smooty);
        }
        else {

            Bewegungskraefte meow = folow.GetComponent<Bewegungskraefte>();
            DownLogic down = folow.GetComponent<DownLogic>();

            // for more crisp horizontal movement remove the slerp but then the gravity is more hard as well

            //  --  // this line was used for the sensor focused / spinning version // DEPRECATED?
            //  --  // this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, Quaternion.AngleAxis( -meow.angliene / meow.schmus, folow.up) * folow.rotation, Time.fixedDeltaTime * smooty);


            // first copy the downlogic (check Downlogic class for more insights)
            this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, Quaternion.FromToRotation(transform.up, down.chooseGooseNorm) * this.gameObject.transform.rotation, Time.fixedDeltaTime * 1.95f * 5); 

            // SOMEHOW its working without the following line Ô_Ô"

            // then get the rotation logic of the VR Glasses
            // this.gameObject.transform.rotation = Quaternion.AngleAxis(-meow.angliene, folow.up) * this.gameObject.transform.rotation;
        }
    }
}
