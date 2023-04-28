using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    public Color color;
    public GameObject linePreview;
    public DrawingTool drawingTool;
    public FlexibleColorPicker fcp;

    public void setColor(Color color) {
        this.color = color;
    }

    public void setColorFromImage(Image image) {
        this.color = image.color;
    }

    public void updateLinePreview() {
        if(color != null)
            linePreview.GetComponent<LineRenderer>().material.SetColor("_EmissionColor", color);
    }

    public void updatePicker() {
        fcp.startingColor = color;
        fcp.color = color;
        // Debug.Log("Update Picker: " + color);
    }

    public void updateLine() {
        drawingTool.color = color;
    }

}
