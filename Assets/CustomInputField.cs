using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomInputField : MonoBehaviour
{
    public GameObject placeholder;

    // Technically I could use Unity's GameObject.SetActive thing, but I
    // like this for organization purposes so we're keeping it.
    public void onSelect() {
        placeholder.SetActive(false);
    }

    public void onDeselect() {
        if(GetComponent<TMP_InputField>().text == "")
            placeholder.SetActive(true);
    }
}
