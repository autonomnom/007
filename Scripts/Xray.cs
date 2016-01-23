﻿using UnityEngine;
using System.Collections;

// for casting rays, checking for nearest normal & 
public class Xray {

    private Rigidbody _body;

    public Xray(Rigidbody body) {

        _body = body;
    }

    /// <summary>
    /// Gathering an array of Rays with spherical directions.
    /// </summary>
    /// <param name="rayz">The amount of rays to be gathered.</param>
    /// <returns>a Ray[]</returns>
    public Ray[] letTheRaysRain(int rayz) {

        // old code
        Ray[] allrayz = new Ray[rayz];

        for (int i = 0; i < allrayz.Length; i++) {

            if (i == 0) { allrayz[i] = new Ray(_body.position, -_body.transform.up); }
            else if (i == 1) { allrayz[i] = new Ray(_body.position, _body.transform.up); }
            else if (i == 2) { allrayz[i] = new Ray(_body.position, _body.transform.right); }
            else if (i == 3) { allrayz[i] = new Ray(_body.position, -_body.transform.right); }
            else if (i == 4) { allrayz[i] = new Ray(_body.position, _body.transform.forward); }
            else if (i == 5) { allrayz[i] = new Ray(_body.position, -_body.transform.forward); }
            else if (i == 6) { allrayz[i] = new Ray(_body.position, _body.transform.up + _body.transform.forward + _body.transform.right); }
            else if (i == 7) { allrayz[i] = new Ray(_body.position, _body.transform.up + _body.transform.forward - _body.transform.right); }
            else if (i == 8) { allrayz[i] = new Ray(_body.position, _body.transform.up - _body.transform.forward + _body.transform.right); }
            else if (i == 9) { allrayz[i] = new Ray(_body.position, _body.transform.up - _body.transform.forward - _body.transform.right); }
            else if (i == 10) { allrayz[i] = new Ray(_body.position, -_body.transform.up + _body.transform.forward + _body.transform.right); }
            else if (i == 11) { allrayz[i] = new Ray(_body.position, -_body.transform.up + _body.transform.forward - _body.transform.right); }
            else if (i == 12) { allrayz[i] = new Ray(_body.position, -_body.transform.up - _body.transform.forward + _body.transform.right); }
            else if (i == 13) { allrayz[i] = new Ray(_body.position, -_body.transform.up - _body.transform.forward - _body.transform.right); }
            else if (i == 14) { allrayz[i] = new Ray(_body.position, _body.transform.forward + _body.transform.right); }
            else if (i == 15) { allrayz[i] = new Ray(_body.position, _body.transform.forward - _body.transform.right); }
            else if (i == 16) { allrayz[i] = new Ray(_body.position, -_body.transform.forward + _body.transform.right); }
            else if (i == 17) { allrayz[i] = new Ray(_body.position, -_body.transform.forward - _body.transform.right); }
            else if (i == 18) { allrayz[i] = new Ray(_body.position, _body.transform.up + _body.transform.forward); }
            else if (i == 19) { allrayz[i] = new Ray(_body.position, _body.transform.up - _body.transform.forward); }
            else if (i == 20) { allrayz[i] = new Ray(_body.position, _body.transform.up + _body.transform.right); }
            else if (i == 21) { allrayz[i] = new Ray(_body.position, _body.transform.up - _body.transform.right); }
            else if (i == 22) { allrayz[i] = new Ray(_body.position, -_body.transform.up + _body.transform.forward); }
            else if (i == 23) { allrayz[i] = new Ray(_body.position, -_body.transform.up - _body.transform.forward); }
            else if (i == 24) { allrayz[i] = new Ray(_body.position, -_body.transform.up + _body.transform.right); }
            else if (i == 25) { allrayz[i] = new Ray(_body.position, -_body.transform.up - _body.transform.right); }
        }

        return allrayz;
    }

    /// <summary>
    /// Checking for the nearest normal in a distance of 100f.
    /// </summary>
    /// <param name="allrayz">An array populated with rays.</param>
    /// <param name="layerMask">The layerMask type to be checked.</param>
    /// <returns>The closest normal.</returns>
    public Vector3 findTheNearestNormal(Ray[] allrayz, LayerMask layerMask) {

        float distance = 100f;
        Vector3 normal = new Vector3();

        for (int i = 0; i < allrayz.Length; i++) {

            RaycastHit piu;
            if (Physics.Raycast(allrayz[i], out piu, layerMask)) {

                if (piu.distance < distance) {

                    distance = piu.distance;
                    normal = piu.normal;
                }
            }
        }

        return normal;
    }
}

