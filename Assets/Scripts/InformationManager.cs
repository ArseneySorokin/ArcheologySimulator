using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InformationManager : MonoBehaviour
{
    public Toggle toggle;
    public TextMeshProUGUI tmpTitle;
    public TextMeshProUGUI tmp;
    public List<string> titles;
    [TextArea]
    public List<string> texts;

    private int index = 0;

    public void UpdateText()
    {
        if (texts.Count > index)
        {
            tmp.text = texts[index];
            tmpTitle.text = titles[index];
            index++;
            toggle.isOn = true;
        }
    }
}
