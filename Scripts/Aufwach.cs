using UnityEngine;
using System.Collections;

public class Aufwach : MonoBehaviour {

    /**
     *      Color-keys
     *      "innen" is referring to the inner-body-ambient color
     *      "außen" for being on the body surface
     **/
    public Color innen = new Color(0.05490196f, 0.02352941f, 0.03137255f, 1f);
    public Color ausen = new Color(0.9411765f, 0.7529412f, 0.7529412f, 1f);
    private Color bday = new Color(0.02352941f, 0.03529412f, 0.5294118f, 1f);

    private bool nighttime = false;
    private bool intransit = false;

    void Awake() {

        // setting the außen color as default (spawn positions outside the body)
        RenderSettings.ambientLight = ausen;
    }


    void Start () {
	
	}
	

	void Update () {
	    
        // for testing, got to be replaced with triggers
        if(Input.GetKeyDown(KeyCode.L)) {

            // turnaround in case we already transitioning
            if (intransit) { nighttime = !nighttime; }

            // start the transition
            intransit = true;
        }
        
        if (intransit) {

            RenderSettings.ambientLight = TheNewerCycleOfLifeAndDeath(nighttime);
        }

        // easter egg
        if (Input.GetKeyDown(KeyCode.Numlock)) {

            RenderSettings.ambientLight = bday;
        }
	}

    /// <summary>
    /// Te Changing process of the RenderSettings.ambientLight value.
    /// </summary>
    /// <param name="old"> If true, we are in the body, if false, we are on the surface.</param>
    /// <returns>A Color between innen & ausen.</returns>
    private Color TheNewerCycleOfLifeAndDeath(bool old) {

        // AmbientLight rn
        Color jetzt = RenderSettings.ambientLight;

        // if it's dark and we want it to be lighten
        if (old) {

            // we r already there
            if (jetzt == ausen) {

                intransit = false;
                nighttime = false;
                return jetzt;
            }

            // or on the way
            return Color.Lerp(jetzt, ausen, Time.fixedDeltaTime);
        }
        // otherwise we want a darker ambientLight
        else {
            
            // we r already there
            if (jetzt == innen) {

                intransit = false;
                nighttime = true;
                return jetzt;
            }

            // or on the way
            return Color.Lerp(jetzt, innen, Time.fixedDeltaTime);
        }
    }
}
