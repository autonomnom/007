using UnityEngine;
using System.Collections;


public class FollowTheTarget : MonoBehaviour {

    GameObject igor; 

    [SerializeField] private float camDistance = 42f;
    [SerializeField] private float camUp = 10f;
    [SerializeField] private float smoothy1 = 1.2f;
//  [SerializeField] private float smoothy2 = 5f;
    [SerializeField] private Transform foollow;

    private Vector3 targetPosition;

    void Awake() {

        //finding the soul to follow
        foollow = GameObject.Find("Soul").transform;
    }

	void Start () {
	    
	}
	
	void FixedUpdate () {

        // Maybe got to split this values up to get independent rotation/adjusting speed 
        // for vertical and horizontal.

        //getting in position
        /*  classic  */
        targetPosition = foollow.position + foollow.up * camUp - foollow.forward * camDistance;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothy1);
        
        /*  2.0  // too much jittering cause of the vertex positions
        targetPosition = foollow.position + foollow.up * camUp;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothy1);
        targetPosition = foollow.position - foollow.forward * camDistance;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothy2);
        */

        //adjust rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(foollow.position - transform.position, foollow.up), 0.15f);
	}
}
