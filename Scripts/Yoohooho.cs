using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Yoohooho : NetworkManager {

    private Vector3[] roundround;

    void Start() {

        NetworkStartPosition[] allchilds = FindObjectsOfType<NetworkStartPosition>();
        roundround = new Vector3[allchilds.Length];

        for (int i = 0; i < allchilds.Length; i++) {

            roundround[i] = allchilds[i].GetComponent<Transform>().position;
        }
    }

    void Update() {

    }

    public override void OnClientConnect(NetworkConnection conn) {

        GameObject[] peeps = Resources.LoadAll<GameObject>("Peeps");
        for(int i = 0; i < peeps.Length; i++) {

            ClientScene.RegisterPrefab(peeps[i]);
        }

        base.OnClientConnect(conn);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {

        var ididi = (GameObject)GameObject.Instantiate(robin());
        NetworkServer.AddPlayerForConnection(conn, ididi, playerControllerId);
    }

    private GameObject robin() {

        GameObject prefalala;

        if (numPlayers == 0)        { prefalala = Resources.Load("Peeps/IGOR") as GameObject; }
        else if (numPlayers == 1)   { prefalala = Resources.Load("Peeps/BRUNI") as GameObject; }
        else if (numPlayers == 2)   { prefalala = Resources.Load("Peeps/SABELL") as GameObject; }
        else if (numPlayers == 3)   { prefalala = Resources.Load("Peeps/JIGGLI") as GameObject; }
        else                          prefalala = Resources.Load("Peeps/JIGGLI") as GameObject;

        prefalala.transform.position = roundround[numPlayers];

        return prefalala;
    }
}
