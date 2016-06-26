using UnityEngine;
using System.Collections;

public class QuickResolution : MonoBehaviour {

    public int Width = 1920;
    public int Height = 1080;

	// Use this for initialization
	void Start () {
        Screen.SetResolution(Width, Height, true);
	}
}
