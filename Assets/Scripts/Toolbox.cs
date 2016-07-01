using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Toolbox : Singleton<Toolbox> {
    protected Toolbox () {}

    private Canvas canvas;

    public InputControlBase InputControl;

    private GameObject MessagePrefab;

    public PlayerControl Player;

    void Awake () {
        MessagePrefab = Resources.Load("prefabs/ui/MessagePanel", typeof(GameObject)) as GameObject;
        canvas = GameObject.Find("canvas_ui").GetComponent<Canvas>();
        InputControl = FindObjectOfType<InputControlBase>();
        Player = FindObjectOfType<PlayerControl>();
    }

    public void ShowMessage(string message, PointOfInterest p = null, bool Focus = false) {
        InputControl.Enabled = false;
        Player.ShowHealth = false;
        if (p != null) Player.Interest = p;
        if (Focus) Player.Interacting = true;
        GameObject newMsg = Instantiate(MessagePrefab) as GameObject;
        newMsg.transform.SetParent(canvas.transform, false);
        newMsg.GetComponent<MessageControl>().Text = message;
        Animator anim = newMsg.GetComponent<Animator>();
        anim.SetTrigger("open");
    }

    public static T RegisterComponent<T> () where T: Component {
        return Instance.GetOrAddComponent<T>();
    }
}
