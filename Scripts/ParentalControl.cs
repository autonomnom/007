using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class ParentalControl : MonoBehaviour {

    public Material theLove;
    public UnityEngine.Rendering.ShadowCastingMode ofYou;
    public PhysicMaterial hurtsAndHeals;

    void Awake() {

        int allMyKidz = this.transform.childCount;

        // change certain attributes for all children
        for(int i = 0; i < allMyKidz; i++) {

            if (this.transform.GetChild(i) != null) {

                // ShadowCastingMode
                this.transform.GetChild(i).GetComponent<MeshRenderer>().shadowCastingMode = ofYou;

                // Material Renderer
                this.transform.GetChild(i).GetComponent<MeshRenderer>().material = theLove;

                // Material Collider
                this.transform.GetChild(i).GetComponent<MeshCollider>().material = hurtsAndHeals;
            }
        }
    }

	void Start () {
	
	}
	
	void Update () {
	

	}
}
