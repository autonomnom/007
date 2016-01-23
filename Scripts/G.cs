using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// globalization
public static class G {

    // enum for all playable identities
    public enum Who : int {
        Igor = 1,
        James = 2,
        Frida = 3,
        Adam = 4
    }

    // the global container for the actual identity
    public static Who identitaet = Who.Igor;
}
