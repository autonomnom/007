using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// for casting rays, checking for nearest normal & 
public class Xray {

    public bool activateDebug = false;
    private Rigidbody _body;

    public Xray(Rigidbody body) {

        _body = body;
    }

    /// <summary>
    /// Gathering an array of Rays with spherical directions.
    /// </summary>
    /// <param name="rayz">The amount of rays to be gathered. 12 for cam, 18 for identity.</param>
    /// <returns>a Ray[]</returns>
    public Ray[] letTheRaysRain(int rayz) {

        Ray[] rayt = new Ray[rayz];

        for (int i = 0; i < rayt.Length; i++) {

            float abstand = Mathf.PI / rayz;
            float y = Mathf.Cos(i * abstand);
            float phi = i * ((10f * Mathf.PI) / rayz);
            float x = Mathf.Sin(i * abstand) * Mathf.Sin(phi);
            float z = Mathf.Sin(i * abstand) * Mathf.Cos(phi);
            rayt[i] = new Ray(_body.position, new Vector3(x, y, z));      
        }

        // for debug
        if (activateDebug) {

            for (int i = 0; i < rayt.Length; i++) {

                Debug.DrawRay(rayt[i].origin, rayt[i].direction);
            }
        }

        return rayt;
    }

    /// <summary>
    /// Gathering rays organized in a circle around a specific entity.
    /// </summary>
    /// <param name="target">Entity to raycast around.</param>
    /// <param name="rayz">Amount of rayz == frequency of rays on that circle.</param>
    /// <param name="radius">Distance between the raycircle and the entity.</param>
    /// <param name="length">Length of the circle (multiplied with PI).</param>
    /// <param name="startPoint">Starting Point of the circle (multiplied with PI).</param>
    /// <returns></returns>
    public Ray[] tanzTanzRingeltanz(Transform target, int rayz, float radius, float length = 2f, float startPoint = 2f) {

        Ray[] rayt = new Ray[rayz];
        float bogenmaß = (Mathf.PI * length) / rayz; // Pi * 2 for full fircle, everything less give just rays on a shorter radiantsize

        for (int i = 0; i < rayt.Length; i++) {

            float z = radius * Mathf.Cos(Mathf.PI / 2 + target.up.z);
            float x = radius * Mathf.Sin(Mathf.PI / 2) * Mathf.Cos(i * bogenmaß + Mathf.PI * startPoint);
            float y = radius * Mathf.Sin(Mathf.PI / 2) * Mathf.Sin(i * bogenmaß + Mathf.PI * startPoint);
            // applying locally
            Vector3 local = target.InverseTransformPoint(target.position) + new Vector3(x, y, z);
            // converting it to global
            Vector3 global = target.TransformPoint(local);
            
            rayt[i] = new Ray(_body.position, global - _body.position);
        }

        return rayt;
    }

    /// <summary>
    /// Gathering rays within the four cardianl points.
    /// </summary>
    /// <param name="n">Foward.</param>
    /// <param name="o">Right.</param>
    /// <param name="s">Back.</param>
    /// <param name="w">Left.</param>
    /// <returns>An array ordered NOSW.</returns>
    public Ray[] cardianlDirections(bool n, bool o, bool s, bool w) {

        int count = 0;
        count = n ? count + 1 : count;
        count = o ? count + 1 : count;
        count = s ? count + 1 : count;
        count = w ? count + 1 : count;
        Ray[] directions = new Ray[count];

        for (int i = 0; i < count; i++) {

            if (n) { directions[i] = new Ray(_body.position, _body.transform.forward); n = false; continue; }
            else if (o) { directions[i] = new Ray(_body.position, _body.transform.right); o = false; continue; }
            else if (s) { directions[i] = new Ray(_body.position, -_body.transform.forward); s = false; continue; }
            else if (w) { directions[i] = new Ray(_body.position, -_body.transform.right); w = false; continue; }
        }

        return directions;
    }

    /// <summary>
    /// Checking for the nearest normal in a distance of 100f.
    /// </summary>
    /// <param name="allrayz">An array populated with rays.</param>
    /// <param name="layerMask">The layerMask type to be checked.</param>
    /// <param name="range">The max range of the ray.</param>
    /// <returns>The closest normal.</returns>
    public Vector3 findTheNearestNormal(Ray[] allrayz, LayerMask layerMask, float range) {

        float distance = 100f;
        Vector3 normal = _body.transform.up;

        for (int i = 0; i < allrayz.Length; i++) {

            RaycastHit piu;
            if (Physics.Raycast(allrayz[i], out piu, range, layerMask)) {

                if (piu.distance < distance) {

                    distance = piu.distance;
                    normal = piu.normal;
                }
            }
        }

        return normal;
    }

    /// <summary>
    /// Looking for nearby obstacles that has to be dodged for keeping vision of the identity.
    /// </summary>
    /// <param name="allrayz">An array popilted with rays.</param>
    /// <param name="layerMask">LayerMask type to be checked.</param>
    /// <param name="castRange">Max range of the raycasts.</param>
    /// <returns>True if hit anything.</returns>
    public bool checkForCollisionNearBy(Ray[] allrayz, LayerMask layerMask, float castRange) {

        for (int i = 0; i < allrayz.Length; i++) {

            RaycastHit piu;
            if (Physics.Raycast(allrayz[i], out piu, castRange, layerMask)) {

                return true;
            }
        }

        return false;
    }






























    //////////////////////// CAN BE ABADONED (!?)
    public Dictionary<Vector3, float> findCameraMoveDirections(Ray[] allrayz, LayerMask layerMask, float castRange) {

        Dictionary<Vector3, float> dirDis = new Dictionary<Vector3, float>();

        for (int i = 0; i < allrayz.Length; i++) {

            RaycastHit hit;

            if (Physics.Raycast(allrayz[i], out hit, castRange, layerMask)) {

                dirDis.Add(_body.position - hit.point, hit.distance);
            }
        }

        return dirDis;
    }
}

