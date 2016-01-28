using UnityEngine;
using System.Collections;

public class Fakamera : MonoBehaviour {

    private Transform boo;
    private Transform poo;

	void Start () {

        boo = GetComponentInParent<Transform>();
        poo = GetComponent<Transform>();
	}
	
	void Update () {

        poo.LookAt(boo.position);
	}
}
