using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToSpin : MonoBehaviour
{
    public GhostFollow ghostFollow;

    public MeshCollider vaseCollider;
    public MeshCollider dustCollider;

    public float focusTime = 1;
    public Rigidbody artifact;

    //public Rigidbody box;
    public Transform boxExitPoint;
    //public float boxExitTime = 2;

    public Transform focusPoint;

    public float dragSensitivity;

    private bool open;

    public void SwitchToFocus()
    {
        StartCoroutine(_Focus());
    }

    IEnumerator _Focus()
    {
        Vector3 startingPosition = artifact.transform.position;
        Quaternion startingRotation = artifact.transform.rotation;
        artifact.isKinematic = true;
        //set non-convex for artifact + dust
        vaseCollider.convex = false;
        dustCollider.convex = false;

        artifact.transform.SetParent(focusPoint);
        Quaternion targetRotation = Quaternion.Euler(90, 0, 0);
        for (float i = 0; i < 1; i += Time.deltaTime / focusTime)
        {
            artifact.transform.rotation = (Quaternion.Lerp(startingRotation, focusPoint.rotation, i));
            artifact.transform.position = (Vector3.Lerp(startingPosition, focusPoint.position, i)); 
            yield return null;
        }
        artifact.transform.rotation = (focusPoint.rotation);
        artifact.transform.position = (focusPoint.position);
        ghostFollow.enabled = false;
        /*Vector3 boxSP = box.transform.position;
        box.transform.SetParent(boxExitPoint);
        for (float i = 0; i < 1; i += Time.deltaTime / boxExitTime)
        {
            box.transform.position = (Vector3.Lerp(boxSP, boxExitPoint.position, i));
            yield return null;
        }
        box.transform.position =(boxExitPoint.position);*/
        SandSpawner.done = true;
    }

    public void OpenTool(bool open)
    {
        this.open = open;
    }

    Vector3 mousePoint;
    Vector3 startingEuler;
    private void Update()
    {
        if (!SandSpawner.done || !open)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            mousePoint = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 cameraUp = Camera.main.transform.TransformDirection(Vector3.up);
            Vector3 cameraRight = Camera.main.transform.TransformDirection(Vector3.right);

            Vector2 rotationOffset = ((Input.mousePosition - mousePoint) / Screen.width) * dragSensitivity;

            artifact.transform.Rotate(cameraRight, rotationOffset.y, Space.World);
            artifact.transform.Rotate(cameraUp, - rotationOffset.x, Space.World);
            mousePoint = Input.mousePosition;
        }
    }
}
