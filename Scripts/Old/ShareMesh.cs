using System.Collections;
using UnityEngine;

public class ShareMesh : MonoBehaviour {

    //make var to get access from other files to it
    public MeshFilter meshFilter;

	// Use this for initialization
	void Start () {

        meshFilter = GetComponent<MeshFilter>();
        //Debug.Log(meshFilter.sharedMesh.vertices.Length);
	}
	
	// Update is called once per frame
	void Update () {
	  
	}
}
