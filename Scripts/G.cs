using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// globalization
public static class G {

    // enum for all playable identities
    public enum Who : int {
        IGOR = 1,
        JAMES = 2,
        FRIDA = 3,
        ADAM = 4
    }

    // the global container for the actual identity
    public static Who identitaet = Who.IGOR;


    // camera state
    public enum cam : int {
        FIRST = 1,
        THIRD = 2
    }

    public static cam sicht = cam.THIRD;
}
