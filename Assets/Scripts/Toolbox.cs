using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Toolbox : Singleton<Toolbox> {
    protected Toolbox () {}

    private Canvas canvas;
    private InputControlBase inputControl;
    private GameObject MessagePrefab;
    private PlayerControl player;
    private Options options;

    private Animator optionsPanelAnimator;
    private Transform optionsPanelContents;
    private Hashtable optionsPanelObjects = new Hashtable();

    public static PlayerControl Player {
        get {
            return Instance.player;
        }
    }
    public static InputControlBase InputControl {
        get {
            return Instance.inputControl;
        }
    }
    public static Options Options {
        get {
            return Instance.options;
        }
    }

    public void OpenOptionsDialogue(string dialogue) {
        optionsPanelAnimator.SetTrigger("OpenPanel");
        GameObject obj = null;
        if(optionsPanelObjects.Contains(dialogue)) {
            obj = optionsPanelObjects[dialogue] as GameObject;
        } else {
            obj = Resources.Load("prefabs/ui/options/" + dialogue, typeof(GameObject)) as GameObject;
            optionsPanelObjects[dialogue] = obj;
        }
        if (obj == null) {
            throw new System.Exception("[Toolbox] Prefab doesn't exist: \"Resources/prefabs/ui/options/" + dialogue + "\"");
        } else {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in optionsPanelContents) children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));
            GameObject go = Instantiate(obj);
            go.transform.SetParent(optionsPanelContents, false);
            inputControl.DisableTurningInput();
        }
    }

    void Start () {
        options = new Options();
		// Oh, look, a change!
    }

    void Awake () {
        MessagePrefab = Resources.Load("prefabs/ui/MessagePanel", typeof(GameObject)) as GameObject;
        canvas = GameObject.Find("canvas_ui").GetComponent<Canvas>();
        optionsPanelAnimator = canvas.transform.Find("Options").GetComponent<Animator>();
        inputControl = FindObjectOfType<InputControlBase>();
        player = FindObjectOfType<PlayerControl>();
        optionsPanelContents = (canvas.transform.Find("Options/OptionsArea/Scroll View").gameObject.GetComponent<ScrollRect>()).content.transform; 
    }

    public void ShowMessage(string message, PointOfInterest p = null, bool Focus = false) {
        inputControl.Enabled = false;
        player.ShowHealth = false;
        if (p != null) player.Interest = p;
        if (Focus) player.Interacting = true;
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
