using UnityEngine;
using System.Collections;

public class Targettable : MonoBehaviour {
    void OnMouseDown() {
        if (Toolbox.Player.Targetting) {
            Toolbox.Player.Targetting = false;
        } else {
            Toolbox.Player.Target = this;
        }
    }
}
