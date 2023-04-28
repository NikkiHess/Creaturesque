using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PresetColorButton : MonoBehaviour
{
    public void onPressed() {
        ColorManager cm = GameObject.FindGameObjectsWithTag("EventSystem")[0].GetComponent<ColorManager>();
        cm.setColorFromImage(GetComponent<Image>());
        cm.updateLinePreview();
        cm.updateLine();
        cm.updatePicker();
    }
}
