using UnityEngine;
using System.Collections;

public class Festsitzen : MonoBehaviour {

    private Transform folow;
    private float off = 2f;
    private float smooty = 8.5f;

	void Start () {

        findFollow();
	}
	
	void FixedUpdate () {
	
        findFollow();
        
        // place the camera always above the player about "off"
        this.gameObject.transform.position = folow.transform.position + Vector3.Scale(folow.up, new Vector3(off, off, off));

        // get his rotation
        this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, folow.transform.rotation, Time.fixedDeltaTime * smooty);
	}


    /// <summary>
    /// Find the identity to follow.
    /// </summary>
    void findFollow() {

        folow = GameObject.Find(G.identitaet.ToString()).transform;
    }
}
