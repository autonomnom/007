using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class Festsitzen : MonoBehaviour {

    public Transform folow;           // getting set by Biografie (on the peeps prefab)
    private float off = 1f;
    public float smooty = 8.5f;

	void Start () {

	}
	
	void FixedUpdate () {
	
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

            // for more crisp horizontal movement remove the slerp but then the gravity is more hard as well

                            // -- wiz spinning
                            // this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, Quaternion.AngleAxis( -meow.angliene / meow.schmus, folow.up) * folow.rotation, Time.fixedDeltaTime * smooty);

                            // -- this works for the player and parasit, but fipsi is over-turning
                            // this.gameObject.transform.rotation = Quaternion.AngleAxis(-meow.anglieForX * meow.schmus, folow.up) * folow.rotation;

            Debug.Log("festsitzen" + meow.angliene);

            // - need to find a way to rotate this gameobjects forward rotation 
            // - into the negative of the folow's to make this' child, the fipsi, 
            // - be on point with the forward rotation of folow. T____________T
            // - RN all 3 are on point but that way fipsi is accelerating:

               this.gameObject.transform.rotation = Quaternion.AngleAxis( -meow.angliene, -folow.up) * folow.rotation;

            //this.gameObject.transform.rotation = folow.rotation;
            //this.gameObject.transform.rotation = Quaternion.AngleAxis( meow.angliene + 180, folow.up) * this.gameObject.transform.rotation;
        }
    }
}
