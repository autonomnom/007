using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

// for the first person camera, fipsi
public class ImKopf : MonoBehaviour {

    public Transform fooloow;
    private GameObject calamera;
    private Vector3 localEu;

	void Start () {

        calamera = this.gameObject;
        findFollow();
	}
	
	void Update () {

	}

    void FixedUpdate() {

        if (fooloow == null) {

            return;
        }
            
        // getLocalEuler for looping protection
        localEu = calamera.transform.localEulerAngles;

        // look above and beneath
        mouseYview();
    }


    /// <summary>
    /// Find the identity to follow.
    /// </summary>
    void findFollow() {

    }

    /// <summary>
    /// Mouseview on the Y-Axis of the local view
    /// </summary>
    void mouseYview() {

        // variables has to be function-exluded to make them publicly available
        float rotAverageY = 0f;
        List<float> rotArrayY = new List<float>();
        float sensitivityY = 3.5f;
        float rotationY = 0f;
        float framesOfSmoothing = 25f;
        float minimumY = -55f;
        float maximumY = 55f;
        bool invertY = true;

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

        // set a fixed angle range for up & downview
        rotAverageY = fixedAngelSpectrum(rotAverageY);

        //apply it
        calamera.transform.Rotate(new Vector3(rotAverageY, 0, 0));
    }


    /// <summary>
    /// Reset the given angle based on a fixed angle width.
    /// </summary>
    /// <param name="ay"> the given angle of the mouse input </param>
    /// <returns> a decreased angle to block an over-movement </returns>
    float fixedAngelSpectrum(float ay) {

        float avg = ay;

        if (localEu.x >= 0f && localEu.x < 85f) {

            if (localEu.x + avg > 85f) {

                avg = 85 - localEu.x - 1;
            }

            return avg;
        }
        else if (localEu.x <= 360 && localEu.x > 280) {

            if (localEu.x + avg < 280) {

                avg = 280 - localEu.x + 1;
            }

            return avg;
        }
        else return avg;
    }
}
