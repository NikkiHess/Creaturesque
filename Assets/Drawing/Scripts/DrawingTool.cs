using System.Collections.Generic;
using UnityEngine;

public class DrawingTool : MonoBehaviour
{
    #region Draw
    public GameObject linePrefab, drawable;
    public Color color = Color.black;
    public float size = 0.08f;
    Line activeLine;
    #endregion

    #region Undo/Redo
    List<Line> undoList = new List<Line>(), 
               redoList = new List<Line>();
    #endregion

    void Update()
    {
        bool pointerInBounds = isPointerInBounds();
        // Debug.Log("pointerInBounds: " + pointerInBounds);

        #region Left Click Down
        if(Input.GetMouseButtonDown(0)) {
            if(pointerInBounds) {
                GameObject newLine = Instantiate(linePrefab);
                activeLine = newLine.GetComponent<Line>();
                // recolor the line accordingly
                activeLine.lineRenderer.material.SetColor("_EmissionColor", color);
                // resize the line accordingly
                activeLine.lineRenderer.startWidth = size;
            }
        }
        #endregion
        #region Left Click Held
        if(Input.GetMouseButton(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // force line inside drawable if necessary
            mousePos = forceWithinBounds(mousePos);

            // Debug.Log("drawing at " + mousePos);
            if(activeLine != null) {
                // z = -(Count + 1) to make line appear on top
                mousePos = new Vector3(mousePos.x, mousePos.y, -(undoList.Count * 0.01f));
                activeLine.updateLine(mousePos);
            }
        }
        #endregion
        #region Left Click Released
        if(Input.GetMouseButtonUp(0) && activeLine != null) {
            // only lines that show up should be in lists
            if(activeLine.GetComponent<LineRenderer>().positionCount <= 1)
                GameObject.Destroy(activeLine.gameObject);
            else {
                // add to undoList and clear redoList (for consistency with other apps)
                undoList.Add(activeLine);
                clearRedoList();
            }

            // either way, stop drawing
            activeLine = null;
        }
        #endregion
        #region Undo (Ctrl+Z OR Z in Editor)
        bool con;
        #if UNITY_EDITOR
        con = Input.GetKeyDown(KeyCode.Z);
        #else
        con = Input.GetKey(KeyCode.CTRL) && Input.GetKeyDown(KeyCode.Z);
        #endif
        if(undoList.Count > 0 && con) {
            Line toRemove = undoList[undoList.Count - 1];
            
            // destroy and remove from list
            toRemove.gameObject.SetActive(false);
            undoList.Remove(toRemove);
            redoList.Add(toRemove);
        }
        #endregion
        #region Redo (Ctrl+Y OR Y in Editor)
        #if UNITY_EDITOR
        con = Input.GetKeyDown(KeyCode.Y);
        #else
        con = Input.GetKey(KeyCode.CTRL) && Input.GetKeyDown(KeyCode.Y);
        #endif
        if(redoList.Count > 0 && con) {
            Line toAdd = redoList[redoList.Count - 1];
            
            // destroy and remove from list
            toAdd.gameObject.SetActive(true);
            redoList.Remove(toAdd);
            undoList.Add(toAdd);
        }
        #endregion

    }

    bool isPointerInBounds() {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

        return hit && hit.collider.CompareTag("Drawable");
        // return results.Count > 0 && results[0].gameObject.CompareTag("Drawable");
    }

    /// Returns the drawable edge nearest the mouse pointer
    Vector2 forceWithinBounds(Vector2 mousePos) {
        Vector2 extents = drawable.GetComponent<Renderer>().bounds.extents;
        Vector2 center = drawable.transform.position;
        float lineOffset = (linePrefab.GetComponent<LineRenderer>().startWidth / 2);

        // two separate ifs instead of if/else chain because
        // corners are edge cases

        // mouse off right side
        if(mousePos.x > extents.x + center.x - lineOffset)
            mousePos.x = extents.x + center.x - lineOffset;
        // mouse off top side
        if(mousePos.y > extents.y + center.y - lineOffset)
            mousePos.y = extents.y + center.y - lineOffset;
        // mouse off left side
        if(mousePos.x < -extents.x + center.x + lineOffset)
            mousePos.x = -extents.x + center.x + lineOffset;
        // mouse off top side
        if(mousePos.y < -extents.y + center.y + lineOffset)
            mousePos.y = -extents.y + center.y + lineOffset;

        return mousePos;
    }

    /// Destroy all lines in redo list, and clear it
    void clearRedoList() {
        foreach(Line line in redoList)
            GameObject.Destroy(line.gameObject);
        redoList.Clear();
    }

}
