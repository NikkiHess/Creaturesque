using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject menu, eventSystem;
    public void toggleMenu() {
        // if the menu will be opened, close all the others
        if(!menu.activeSelf)
            foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Menu"))
                obj.SetActive(false);

        menu.SetActive(!menu.activeSelf);

        // If this button is our color picker button
        if(menu.GetComponent<FlexibleColorPicker>() != null) {
            Color color = GetComponent<Image>().color;
            // Update FlexibleColorPicker start color
            eventSystem.GetComponent<ColorManager>().updatePicker();
        }
    }
}
