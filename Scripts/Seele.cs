using UnityEngine;
using System.Collections;

// has to be attached to one identity's soul.
// mainly to enable GetComponentInChildren() in FollowTheTarget.cs
// that is used to find the right Seele.
public class Seele : MonoBehaviour {

    FollowTheTarget cameraScript;
    [HideInInspector] public Transform followMePlz; 

	void Awake() {

        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowTheTarget>();
        followMePlz = this.transform;
	}
	
	void Update() {

        if (cameraScript.status != FollowTheTarget.Polizei.fern) {

            followMePlz = this.transform.parent;
        }
        else { followMePlz = this.transform; }
	}
}
