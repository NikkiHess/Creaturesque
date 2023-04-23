using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineGenerator : MonoBehaviour
{
    public GameObject linePrefab;
    Line activeLine;

    Ray ray;
    RaycastHit hit;

    void Update()
    {
        // left click down
        if(Input.GetMouseButtonDown(0)) {
            GameObject newLine = Instantiate(linePrefab);
            activeLine = newLine.GetComponent<Line>();
        }
        // left click up
        if(Input.GetMouseButtonUp(0)) {
            activeLine = null;
        }

        if(activeLine != null) {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            activeLine.updateLine(mousePosition);
        }
    }
}
