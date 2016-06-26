using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TurningInput : MonoBehaviour {

    bool Tracking = false;

    private RectTransform tf;

    public CanvasScaler canvas;
    public Vector2 Delta;

    private int finger;
    private float wscale;
    private float hscale;

    // Use this for initialization
    void Start () {
        tf = GetComponent<RectTransform>();
        Delta = Vector2.zero;
        wscale = canvas.referenceResolution.x / Screen.width;
        hscale = canvas.referenceResolution.y / Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
        EventSystem eventSystem = EventSystem.current;

#if UNITY_STANDALONE || UNITY_WEBPLAYER
        Value = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

        if (!eventSystem.alreadySelecting) {
            for (int i = 0; i < Input.touchCount; i++) {
                Touch touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Began) {
                    if (Helper.RectTransformToScreenSpace(tf).Contains(touch.position)) {
                        Tracking = true;
                        finger = touch.fingerId;
                        eventSystem.SetSelectedGameObject(tf.gameObject);
                    }
                }
                if (Tracking) {
                    if (touch.fingerId == finger) {
                        Vector2 t = touch.deltaPosition;
                        t /= (wscale * hscale) / 2;
                        t.x /= tf.rect.width;
                        t.y /= tf.rect.height;
                        Delta = t;
                    }
                }
                if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && touch.fingerId == finger) {
                    Tracking = false;
                    Delta = Vector2.zero;
                }
            }
        }
#endif
    }
}
