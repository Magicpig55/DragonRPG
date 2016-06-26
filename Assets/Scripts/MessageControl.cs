using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Animator))]
public class MessageControl : MonoBehaviour {

    private Button butt;
    private Animator anim;

	// Use this for initialization
	void Start () {
        butt = GetComponent<Button>();
        anim = GetComponent<Animator>();
        butt.onClick.AddListener(() => Clicked());
	}

    void Clicked () {
        anim.SetTrigger("close");
        Destroy(gameObject, 1);
        Toolbox.Instance.InputControl.Enabled = true;
    }
}
