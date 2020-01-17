using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFollow : MonoBehaviour
{
    public Transform target;
    public float maxMoveSpeed = 2;
    public float maxRotateSpeed = 2;

    Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, target.position, maxMoveSpeed);
            Quaternion rot = Quaternion.RotateTowards(transform.rotation, target.rotation, maxRotateSpeed);
            rigid.MovePosition(pos);
            rigid.MoveRotation(rot);
        }
    }
}
