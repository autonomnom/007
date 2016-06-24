using UnityEngine;
using System.Collections;

public class Festsitzen : MonoBehaviour {

    public Transform folow;
    private float off = 2f;
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
        this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, folow.transform.rotation, Time.fixedDeltaTime * smooty);
	}
}
