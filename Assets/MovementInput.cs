using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MovementInput : MonoBehaviour {

    bool Tracking = false;

    private RectTransform tf;

    public Vector2 Value;
    private Vector2 origin;
    private Vector2 centered;

    private int finger;

    // Use this for initialization
    void Start () {
        tf = GetComponent<RectTransform>();
        Value = Vector2.zero;
        centered = tf.localPosition;
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
                        origin = touch.position;
                        finger = touch.fingerId;
                        eventSystem.SetSelectedGameObject(tf.gameObject);
                    }
                }
                if (Tracking) {
                    if (touch.fingerId == finger) {
                        Value = Vector2.ClampMagnitude(touch.position - origin, tf.rect.width / 2);
                        tf.localPosition = centered + Value;
                        Value /= tf.rect.width / 2;
                    }
                }
                if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && touch.fingerId == finger) {
                    Tracking = false;
                    tf.localPosition = centered;
                    Value = Vector2.zero;
                }
            }
        }
#endif
    }
}
