using UnityEngine;
using System.Collections;

public class Targettable : MonoBehaviour {
    void OnMouseDown() {
        if (Toolbox.Instance.Player.Targetting) {
            Toolbox.Instance.Player.Targetting = false;
        } else {
            Toolbox.Instance.Player.Target = this;
        }
    }
}
