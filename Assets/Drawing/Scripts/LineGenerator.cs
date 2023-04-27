using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineGenerator : MonoBehaviour
{
    #region Draw
    public GameObject linePrefab, drawable;
    Line activeLine;
    #endregion

    #region Undo
    List<Line> undoList = new List<Line>();
    List<Line> redoList = new List<Line>();
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
                // z = -1 to make line appear on top
                mousePos = new Vector3(mousePos.x, mousePos.y, -1);
                activeLine.updateLine(mousePos);
            }
        }
        #endregion
        #region Left Click Release
        if(Input.GetMouseButtonUp(0)) {
            activeLine.complete = true;
            undoList.Add(activeLine);
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
            Line toRemove;
            if(undoList[undoList.Count - 1].complete)
                toRemove = undoList[undoList.Count - 1];
            else
                toRemove = undoList[undoList.Count - 2];
            
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
        Vector2 center = transform.position;
        float lineOffset = (linePrefab.GetComponent<LineRenderer>().startWidth / 2);

        // two separate ifs instead of if/else chain because
        // corners are edge cases

        // mouse off right side
        if(mousePos.x > extents.x + center.x - lineOffset) {
            mousePos.x = extents.x + center.x - lineOffset;
        }
        // mouse off top side
        if(mousePos.y > extents.y + center.y - lineOffset) {
            mousePos.y = extents.y + center.y - lineOffset;
        }
        // mouse off left side
        if(mousePos.x < -extents.x + center.x + lineOffset) {
            mousePos.x = -extents.x + center.x + lineOffset;
        }
        // mouse off top side
        if(mousePos.y < -extents.y + center.y + lineOffset) {
            mousePos.y = -extents.y + center.y + lineOffset;
        }

        return mousePos;
    }
}
