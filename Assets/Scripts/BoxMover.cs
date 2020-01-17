using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMover : MonoBehaviour
{
    public GhostFollow ghostFollow;
    public Transform boxExitPoint;
    public GameObject box;

    public float boxExitTime;
    public void ChangeBox(bool bring)
    {
        if (bring)
            Bring();
        else
            Remove();
    }
    private void Remove()
    {
        StartCoroutine(RemoveBox());
    }

    private void Bring()
    {
        ghostFollow.enabled = true;
        StopAllCoroutines();
    }
    
    IEnumerator RemoveBox()
    {
        ghostFollow.enabled = false;
        Vector3 boxSP = box.transform.position;
        box.transform.SetParent(boxExitPoint);
        for (float i = 0; i < 1; i += Time.deltaTime / boxExitTime)
        {
            box.transform.position = (Vector3.Lerp(boxSP, boxExitPoint.position, i));
            yield return null;
        }
        box.transform.position = (boxExitPoint.position);
    }
}
