using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// for the first person camera, fipsi
public class ImKopf : MonoBehaviour {

    private Transform fooloow;
    private Biografie bio;
    private GameObject camera;

    // the amount the camera is above the player while in first-person-view
    private int off = 2;

	void Start () {

        camera = this.gameObject;
        findFollow();
	}
	
	void Update () {
        
	}

    void FixedUpdate() {

        findFollow();

        if (bio.fipsi) { 
            
            camera.GetComponent<Camera>().enabled = true;
            fooloow.GetComponent<MeshRenderer>().enabled = false;
        }
        else { 
            
            camera.GetComponent<Camera>().enabled = true;
            fooloow.GetComponent<MeshRenderer>().enabled = true;
        }

        // look above and beneath
        mouseYview();
    }


    /// <summary>
    /// Find the identity to follow.
    /// </summary>
    void findFollow() {

        fooloow = GameObject.Find(G.identitaet.ToString()).transform;
        bio = fooloow.GetComponent<Biografie>();
    }

    /// <summary>
    /// Mouseview on the Y-Axis of the local view
    /// </summary>
    void mouseYview() {

        // variables has to be function-exluded to make them publicly available
        float rotAverageY = 0f;
        List<float> rotArrayY = new List<float>();
        float sensitivityY = 5.5f;
        float rotationY = 0f;
        float framesOfSmoothing = 25f;
        float minimumY = -55f;
        float maximumY = 55f;
        bool invertY = true;

        Quaternion rotationAmount;

        // optional inversion
        float invertFlag = 1f;
        if (invertY) {
            invertFlag = -1f;
        }

        // collect the mouse data
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY * invertFlag * Time.timeScale;

        // process a smooth average value of the mouse moving
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        rotArrayY.Add(rotationY);

        if (rotArrayY.Count >= framesOfSmoothing) {
            rotArrayY.RemoveAt(0);
        }
        for (int j = 0; j < rotArrayY.Count; j++) {
            rotAverageY += rotArrayY[j];
        }
        rotAverageY /= rotArrayY.Count;

        // save the desired amount
        rotationAmount = Quaternion.AngleAxis(rotAverageY, camera.transform.right);

        // apply it
        camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, rotationAmount * camera.transform.rotation, Time.fixedDeltaTime * 5); //rotationAmount * camera.transform.rotation
    }
}
