using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class RealCanvasSize : MonoBehaviour {

    private RectTransform rt;

    public float RealWidth;
    public float RealHeight;

	// Use this for initialization
	void Start () {
        rt = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        rt.localScale = new Vector3(RealWidth / rt.rect.width, RealHeight / rt.rect.height, 1f);
    }
}
