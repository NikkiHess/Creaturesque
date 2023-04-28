using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeManager : MonoBehaviour
{
    public float size = 0.08f;
    public GameObject linePreview;
    public DrawingTool drawingTool;

    public void setSizeFromSlider(float sliderValue) {
        this.size = sliderValue * 0.08f;
    }

    public void updateLinePreview() {
        linePreview.GetComponent<LineRenderer>().startWidth = size;
    }

    public void updateLine() {
        drawingTool.size = size;
    }
}
