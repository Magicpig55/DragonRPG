using UnityEngine;
using System.Collections;

public class Targettable : MonoBehaviour {
    void OnMouseDown() {
        Toolbox.Instance.Player.Target = this;
    }
}
