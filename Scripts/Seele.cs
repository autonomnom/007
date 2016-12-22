using UnityEngine;
using System.Collections;

// has to be attached to one identity's soul.
// mainly to enable GetComponentInChildren() in FollowTheTarget.cs
// that is used to find the right Seele.
public class Seele : MonoBehaviour {

    private FollowTheTarget cameraScript;
    private Vector3 smoothVelo;

	void Awake() {

        cameraScript = GameObject.Find("Vogel").GetComponent<FollowTheTarget>();
	}
	
	void Update() {

        if (cameraScript.status == FollowTheTarget.Polizei.nah || cameraScript.status == FollowTheTarget.Polizei.reisendzu) {

            moveCloser(true);
        }
        else {

            moveCloser(false);
        }
	}

    /// <summary>
    /// Moving the soul on its parent up-axis.
    /// </summary>
    /// <param name="which">Goes towards the parent if true, otherwise away from it.</param>
    void moveCloser(bool which) {

        Transform papa = transform.parent.transform;
        Vector3 desire = which ? papa.position : papa.position + Vector3.Scale(papa.up, new Vector3(3, 3, 3));
        float smooth = which ? 0.5f : 1.5f;
        float max = which ? 70f : 40f;

        transform.position = Vector3.SmoothDamp(transform.position, desire, ref smoothVelo, smooth, max, Time.deltaTime);
    }
}
