using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class OptionControl : MonoBehaviour {

    protected Slider Slider;
    protected Text LabelText;
    protected Text ValueText;

    public string Label = "Setting";

	void Awake () {
        Slider = GetComponentInChildren<Slider>();
        LabelText = transform.Find("LabelText").GetComponent<Text>();
        ValueText = transform.Find("ValueText").GetComponent<Text>();

        Slider.onValueChanged.AddListener(delegate { ValueChanged(); });
        LabelText.text = Label;
	}

    protected virtual void ValueChanged() {}
	
}
