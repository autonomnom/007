using UnityEngine;
using System.Collections;

// camera, who to follow and staying smooth
public class FollowTheTarget : MonoBehaviour {

    [SerializeField] private float camDistance = 42f;
    [SerializeField] private float camUp = 10f;
    [SerializeField] private float smoothy1 = 1.2f;
    private Transform foollow;
    private Vector3 targetPosition;

    void Awake() {

        //starts with..
        GameObject hihi = GameObject.Find(G.identitaet.ToString()); 
        Transform child = hihi.transform.GetChild(0);
        foollow = GameObject.Find(child.name).transform; 
    }

	void Start() {
	    
	}

    void Update() {

        //finding the soul to follow while changing
        switch (G.identitaet) {
            case G.Who.Igor: foollow = GameObject.Find("Soul").transform; break;     
            case G.Who.James: foollow = GameObject.Find("Seele").transform; break;    
            case G.Who.Frida: foollow = GameObject.Find("Spirit").transform; break;    
            case G.Who.Adam: foollow = GameObject.Find("Ghost").transform; break;       
            default: foollow = GameObject.Find("Soul").transform; break;
        }
    }

	void FixedUpdate () {

        // Maybe got to split this values up to get independent rotation/adjusting speed 
        // for vertical and horizontal.

        //getting in position
        targetPosition = foollow.position + foollow.up * camUp - foollow.forward * camDistance;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothy1);

        //adjust rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(foollow.position - transform.position, foollow.up), 0.15f);
	}
}
