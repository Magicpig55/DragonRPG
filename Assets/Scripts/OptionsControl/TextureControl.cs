using UnityEngine;
using System.Collections;

public class TextureControl : OptionControl {

    private string[] Values = { "Full Resolution", "Half Resolution", "Quarter Resolution" , "Eighth Resolution"};

	// Use this for initialization
	void Start () {
        Slider.maxValue = 3;
        Slider.minValue = 0;
	}

    void Update () {
        Slider.value = QualitySettings.masterTextureLimit;
        ValueText.text = Values[QualitySettings.masterTextureLimit];
    }

    protected override void ValueChanged() {
        base.ValueChanged();
        QualitySettings.masterTextureLimit = (int)Slider.value;
    }
}
