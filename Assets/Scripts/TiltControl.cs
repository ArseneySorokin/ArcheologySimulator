using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TiltControl : MonoBehaviour
{
    public Text text;
    Gyroscope gyro;
    Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        gyro = Input.gyro;
        if (!gyro.enabled)
        {
            gyro.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion revisedRotation = Quaternion.Euler(-gyro.attitude.eulerAngles.x, -gyro.attitude.eulerAngles.z, gyro.attitude.eulerAngles.y);
        rigid.MoveRotation(revisedRotation);
        text.text = revisedRotation.eulerAngles + " " + transform.rotation.eulerAngles;
    }
}
