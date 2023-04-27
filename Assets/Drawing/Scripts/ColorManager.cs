using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    public Color color;
    public GameObject colorPickerButton;
    public DrawingTool drawingTool;
    public FlexibleColorPicker fcp;

    public void setColor(Color color) {
        this.color = color;
    }

    public void setColorFromImage(Image image) {
        this.color = image.color;
    }

    public void updateImage() {
        if(color != null)
            colorPickerButton.GetComponent<Image>().color = color;
    }

    public void updatePicker() {
        fcp.startingColor = color;
        fcp.color = color;
    }

    public void updateLine() {
        drawingTool.color = color;
    }

}
