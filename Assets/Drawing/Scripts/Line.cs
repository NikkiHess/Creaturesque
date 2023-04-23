using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using UnityEditor;

public class Line : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float threshold;
    List<Vector2> points;

    public void updateLine(Vector2 position) {
        if(points == null) {
            points = new List<Vector2>();
            addPoint(position);
        }

        if(Vector2.Distance(points.Last(), position) > threshold) {
            addPoint(position);
        }
    }

    void addPoint(Vector2 point) {
        // add to list
        points.Add(point);

        // set count to list size
        lineRenderer.positionCount = points.Count;
        // add the new point
        lineRenderer.SetPosition(points.Count - 1, point);
    }
}
