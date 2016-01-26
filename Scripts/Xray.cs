﻿using UnityEngine;
using System.Collections;

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

    // if it works with bool, consider changing to unity intern function SphereCollision()
    public bool checkForCollisionNearBy(Ray[] allrayz, LayerMask layerMask, float castRange) {

        for (int i = 0; i < allrayz.Length; i++) {

            RaycastHit piu;
            if (Physics.Raycast(allrayz[i], out piu, castRange, layerMask)) {

                return true;
            }
        }

        return false;
    }
}

