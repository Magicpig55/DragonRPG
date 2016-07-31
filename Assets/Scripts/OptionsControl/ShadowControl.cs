using UnityEngine;
using System.Collections;

public class ShadowControl : OptionControl {

    private string[] values = { "Disabled", "Low", "Medium", "High", "Very High" };

    void Start() {
        Slider.maxValue = 4;
        Slider.minValue = 0;
    }

    void Update() {
        Slider.value = QualitySettings.GetQualityLevel();
        ValueText.text = values[(int)Slider.value];
    }

    protected override void ValueChanged() {
        base.ValueChanged();
        Toolbox.Options.SetShadowQuality((int)Slider.value);
    }
}
