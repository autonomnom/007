using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class Festsitzen : MonoBehaviour {

    public Transform folow;
    private float off = 1f;
    private float smooty = 8.5f;

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
            this.gameObject.transform.rotation = Quaternion.AngleAxis(-meow.angliene / meow.schmus, folow.up) * folow.rotation;
        }
    }
}
