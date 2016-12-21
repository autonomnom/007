using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Yoohooho : NetworkManager {

    private Vector3[] roundround;

    void Start() {

        // gather all possible start positions
        NetworkStartPosition[] allchilds = FindObjectsOfType<NetworkStartPosition>();
        roundround = new Vector3[allchilds.Length];

        for (int i = 0; i < allchilds.Length; i++) {

            roundround[i] = allchilds[i].GetComponent<Transform>().position;
        }
    }

    void Update() {

    }
      

    // registering the prefabs on the client
    public override void OnClientConnect(NetworkConnection conn) {
        
        // gathering an array with all prefabs from folder "peeps"
        GameObject[] peeps = Resources.LoadAll<GameObject>("Peeps");
        for(int i = 0; i < peeps.Length; i++) {

            // registering
            ClientScene.RegisterPrefab(peeps[i]);
        }

        base.OnClientConnect(conn);
    }

    public override void OnClientDisconnect(NetworkConnection conn) {

        base.OnClientDisconnect(conn);
    }

    // changing the player prefab depending on it's connection id
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {

        // add player to server with a specific avatar
        var ididi = (GameObject)GameObject.Instantiate(robin(conn.connectionId));
        NetworkServer.AddPlayerForConnection(conn, ididi, playerControllerId);
    }

    /// <summary>
    /// Find the right avatar.
    /// </summary>
    /// <returns> Avatar of "choice" </returns>
    private GameObject robin(int idnummer) {

        GameObject prefalala;
        int id = idnummer;

        if (id == 0)      { prefalala = Resources.Load("Peeps/KAKA") as GameObject; }
        else if (id == 1) { prefalala = Resources.Load("Peeps/IGOR") as GameObject; }
        else if (id == 2) { prefalala = Resources.Load("Peeps/BRUNI") as GameObject; }
        else if (id == 3) { prefalala = Resources.Load("Peeps/SABELL") as GameObject; }
        else if (id == 4) { prefalala = Resources.Load("Peeps/JIGGLI") as GameObject; }
        else if (id == 5) { prefalala = Resources.Load("Peeps/CLEMENTINE") as GameObject; }
        else if (id == 6) { prefalala = Resources.Load("Peeps/RINGO") as GameObject; }
        else if (id == 7) { prefalala = Resources.Load("Peeps/ZAUBERIN") as GameObject; }
        else if (id == 8) { prefalala = Resources.Load("Peeps/FLUMILI") as GameObject; }
        else if (id == 9) { prefalala = Resources.Load("Peeps/KOK") as GameObject; }
        else prefalala = Resources.Load("Peeps/KAKA") as GameObject;

        prefalala.transform.position = roundround[id];

        return prefalala;
    }
}
