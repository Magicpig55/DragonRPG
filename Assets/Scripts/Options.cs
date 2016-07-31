using UnityEngine;
using System.Collections;

public class Options {
    public void SetShadowQuality(int level) {
        var prevTextQuality = QualitySettings.masterTextureLimit;
        QualitySettings.SetQualityLevel(level);
        QualitySettings.masterTextureLimit = prevTextQuality;
    }

    public float TextDispDelay = 0.05f;
}
