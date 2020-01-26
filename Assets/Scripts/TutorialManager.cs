using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    public GameObject panel1;
    public bool p2Played;
    public GameObject panel2;
    public bool p3Played;
    public GameObject panel3;
    public bool p4Played;
    public GameObject panel4;
    public bool p5Played;
    public GameObject panel5;
    public bool p6Played;
    public GameObject panel6;
    public bool p7Played;
    public GameObject panel7;

    private void Awake()
    {
        instance = this;
    }

    public void SieveRotated()
    {
        panel1.SetActive(false);
    }
    public void SieveDone()
    {
        if(!p2Played)
            panel2.SetActive(true);
        p2Played = true;
    }
    public void HandSelected()
    {
        panel2.SetActive(false);
        if (!p3Played)
            panel3.SetActive(true);
        p3Played = true;
    }
    public void ObjectRotated()
    {
        panel3.SetActive(false);
        if (!p4Played)
            panel4.SetActive(true);
        p4Played = true;
    }
    public void PickSelected()
    {
        panel4.SetActive(false);
        if (!p5Played)
            panel5.SetActive(true);
        p5Played = true;
    }
    public void RockBroken()
    {
        panel5.SetActive(false);
    }
    public void AllRocksBroken()
    {
        if (!p6Played)
            panel6.SetActive(true);
        p6Played = true;
    }
    public void BrushSelected()
    {
        panel6.SetActive(false);
        if (!p7Played)
            panel7.SetActive(true);
        p7Played = true;
    }
    public void DustBrushed()
    {
        panel7.SetActive(false);
    }
}
