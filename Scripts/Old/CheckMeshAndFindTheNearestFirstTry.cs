/**using System.Collections;
using UnityEngine;

public class CheckMeshesAndFindTheNearest : MonoBehaviour
{

    [HideInInspector]
    public Vector3[] polygonCenters;
    [HideInInspector]
    public Vector3[] polygonNorms;

    [HideInInspector]
    public Vector3 theChosen;

    public float distSqrd = 1f;
    private float tempDistSqrd = 5f; // for logging, has to be put completly into the fixupdate function

    void Awake()
    {

        GameObject go0 = GameObject.Find("MeshPart0");
        MeshFilter mf0 = go0.GetComponent<MeshFilter>();

        polygonCenters = new Vector3[mf0.sharedMesh.vertices.Length / 3];
        polygonNorms = new Vector3[mf0.sharedMesh.triangles.Length / 3];

        for (int i = 0; i < polygonCenters.Length; i++)
        {

            polygonCenters[i] = (mf0.sharedMesh.vertices[i * 3] + mf0.sharedMesh.vertices[i * 3 + 1] + mf0.sharedMesh.vertices[i * 3 + 2]);
            //polygonNorms[i] = (mf0.sharedMesh.normals[mf0.sharedMesh.triangles[i * 3]] + mf0.sharedMesh.normals[mf0.sharedMesh.triangles[i * 3 + 1]] + mf0.sharedMesh.normals[mf0.sharedMesh.triangles[i * 3 + 2]]) / 3;

            polygonCenters[i] = transform.TransformPoint(polygonCenters[i]);
            // polygonNorms[i] = transform.TransformPoint(polygonNorms[i]);
        }
    }

    void Start()
    {

    }

    void FixedUpdate()
    {

        Vector3 localPosition = transform.TransformPoint(transform.position);
        //float tempDistSqrd;

        for (int i = 0; i < polygonCenters.Length; i++)
        {

            tempDistSqrd = Vector3.Distance(polygonCenters[i], transform.position);

            if (tempDistSqrd < distSqrd)
            {

                theChosen = polygonCenters[i];
                break;
            }
        }

        // Debug.Log("transform.position: " + transform.position + "   localPosition: " + localPosition + "   theChosen: " + theChosen + "   tempDistSqrd: " + tempDistSqrd + "     LENGTH: "+ polygonCenters.Length);
        Debug.Log(polygonCenters[0] + "   " + polygonCenters[0] + "   " + polygonCenters[0] + "   " + polygonCenters[0] + polygonCenters.Length);
    }
}*/
