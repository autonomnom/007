using UnityEngine;
using System.Collections;

public class Festsitzen : MonoBehaviour {

    private Transform folow;
    private float off = 2f;

	void Start () {

        findFollow();
	}
	
	void FixedUpdate () {
	
        findFollow();
        
        // place the camera always above the player about "off"
        this.gameObject.transform.position = folow.transform.position + Vector3.Scale(folow.up, new Vector3(off, off, off));

        // get his rotation
        this.gameObject.transform.rotation = folow.transform.rotation;
	}


    /// <summary>
    /// Find the identity to follow.
    /// </summary>
    void findFollow() {

        folow = GameObject.Find(G.identitaet.ToString()).transform;
    }
}
