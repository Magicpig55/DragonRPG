using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Toolbox : Singleton<Toolbox> {
    protected Toolbox () {}

    private Canvas canvas;

    public InputControlBase InputControl;

    private GameObject MessagePrefab;

    void Awake () {
        MessagePrefab = Resources.Load("prefabs/MessagePanel", typeof(GameObject)) as GameObject;
        canvas = FindObjectOfType<Canvas>();
        InputControl = FindObjectOfType<InputControlBase>();
    }

    public void ShowMessage(string message) {
        InputControl.Enabled = false;
        GameObject newMsg = Instantiate(MessagePrefab) as GameObject;
        newMsg.transform.parent = canvas.transform;
        Text text = newMsg.GetComponentInChildren<Text>();
        text.text = message;
        Animator anim = newMsg.GetComponent<Animator>();
        anim.SetTrigger("show");
    }

    public static T RegisterComponent<T> () where T: Component {
        return Instance.GetOrAddComponent<T>();
    }
}
