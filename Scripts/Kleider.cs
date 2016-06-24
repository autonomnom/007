using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Kleider : NetworkBehaviour {

    private NetworkManager netzwe;
    [SyncVar] public int count = 0;

	void Start () {

        if (isServer) {

            // get the current playercount
            count = FindObjectOfType<NetworkManager>().numPlayers;
        }

        if (!isLocalPlayer) {

            return;
        }

        // gather materials from certain folders.
        // be aware of having enough materials for the maximum of players joining
        Material[] matzen = Resources.LoadAll<Material>("Materials");       //new Material[9];
        GameObject[] gamen = Resources.LoadAll<GameObject>("Meshes");       //new Mesh[matzen.Length];
        Mesh[] meshen = new Mesh[gamen.Length];

        for(int i=0; i < gamen.Length; i++) {

            meshen[i] = gamen[i].GetComponent<MeshFilter>().sharedMesh;
        }

        // assign the desired mesh and material
        GetComponent<MeshRenderer>().material = matzen[count - 1];
        GetComponent<MeshFilter>().mesh = meshen[count - 1];
    }
	
	void Update () {

        if (isServer) {

            // get the current playercount
            count = FindObjectOfType<NetworkManager>().numPlayers;
        }
    }
}
