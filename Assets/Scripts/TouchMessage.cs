using UnityEngine;
using System.Collections;

public class TouchMessage : MonoBehaviour {

    public string Message = "";

	void OnMouseDown() {
        Toolbox.Instance.ShowMessage(Message);
    }
}
