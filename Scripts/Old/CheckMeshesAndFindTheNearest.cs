using System.Collections;
using UnityEngine;

public class CheckMeshesAndFindTheNearest : MonoBehaviour {

    [HideInInspector] public Vector3[] polygonNorms;
    [HideInInspector] public Vector3[] polygonCenters;

    [HideInInspector] public Vector3 theNorm;
    [HideInInspector] public Vector3 thePoint;

    void Awake()
    {

        //get all meshparts
        GameObject[] skeletons = GameObject.FindGameObjectsWithTag("Skelett");
        int tempVerts = 0;

        //find the amount of tris in it
        for (int i = 0; i < skeletons.Length; i++)
        {

            GameObject go = skeletons[i];
            Mesh mesch = go.GetComponent<MeshFilter>().mesh;

            tempVerts += mesch.triangles.Length / 3;
        }

        //create containers for the vertecies and norms
        polygonCenters = new Vector3[tempVerts];
        polygonNorms = new Vector3[tempVerts];

        //recycle tempVerts
        tempVerts = 0;

        //fill em
        for (int i = 0; i < skeletons.Length; i++)
        {

            GameObject go = skeletons[i];
            Mesh mesch = go.GetComponent<MeshFilter>().mesh;

            for (int p = 0; p < mesch.triangles.Length / 3; p++)
            {

                Debug.Log("tempVerts: " + tempVerts + "   p: " + p + "   mesch.triangles/3: " + mesch.triangles.Length / 3 + "   polygonCenters: " + polygonCenters);

                polygonNorms[tempVerts] = (mesch.normals[mesch.triangles[p * 3]] + mesch.normals[mesch.triangles[p * 3 + 1]] + mesch.normals[mesch.triangles[p * 3 + 2]]) / 3;
                polygonCenters[tempVerts] = (mesch.vertices[mesch.triangles[p * 3]] + mesch.vertices[mesch.triangles[p * 3 + 1]] + mesch.vertices[mesch.triangles[p * 3 + 2]]) / 3;
                tempVerts++;
            }
        }

        /*  classic  /  for 1 mesh  /
        //get the vertex and norm positions out of the mesh
        GameObject go0 = GameObject.Find("MeshPart0");
        Mesh mf0 = go0.GetComponent<MeshFilter>().mesh;

        polygonNorms = new Vector3[mf0.triangles.Length/3];
        polygonCenters = new Vector3[mf0.triangles.Length/3];
  
        for (int i = 0; i < polygonCenters.Length; i++) {

            //get the norms and centers together
            polygonNorms[i] = (mf0.normals[mf0.triangles[i * 3]] + mf0.normals[mf0.triangles[i * 3 + 1]] + mf0.normals[mf0.triangles[i * 3 + 2]]) / 3;
            polygonCenters[i] = (mf0.vertices[mf0.triangles[i * 3]] + mf0.vertices[mf0.triangles[i * 3 + 1]] + mf0.vertices[mf0.triangles[i * 3 + 2]]) / 3;
        }
        */
    }

    void Start()
    {

    }

    void FixedUpdate()
    {

        //find the distance and put the closest vertex into theChosen
        float distSqrd = 1000;    //has to be higher than the maximum distance between player and the closest vertex
        float tempDistSqrd;
        int polyMoly = 50;

        for (int i = 0; i < polygonCenters.Length; i++)
        {

            tempDistSqrd = Vector3.SqrMagnitude(polygonCenters[i] - transform.position);

            if (tempDistSqrd < distSqrd)
            {
                distSqrd = tempDistSqrd;
                polyMoly = i;
            }

            // Draws a line from vertex into normal direction
            //Debug.DrawLine(polygonCenters[i], polygonCenters[i] + Vector3.Scale(polygonNorms[i], new Vector3(4f, 4f, 4f)));
            //Debug.DrawRay(polygonCenters[i], polygonNorms[i]);

            // Draws the Normaldrections
            //Debug.DrawLine(polygonNorms[i], Vector3.Scale(polygonNorms[i], new Vector3(100,100,100)));

            // Draws the Verticies to the origin of the world
            //Debug.DrawLine(polygonCenters[i], new Vector3(0, 0, 0));  
        }

        theNorm = polygonNorms[polyMoly];
        thePoint = polygonCenters[polyMoly];
    }
}
