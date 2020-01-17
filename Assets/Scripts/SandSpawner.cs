using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandSpawner : MonoBehaviour
{
    public InformationManager informationManager;
    public RotateToSpin rotateToSpin;

    public int amount;

    public Transform particleParent;
    public int particlesPerAngle = 100;
    public ParticleSystem sandParticles;

    public float finishCount = 100;

    public GameObject prefab;
    public Transform corner1;
    public Transform corner2;

    public Renderer sandRenderer;

    private List<GameObject> balls;

    public static bool done = false;

    private bool localDone;

    private bool open = true;
    private float startingCount;
    private float startingScale;

    public void OpenTool(bool open)
    {
        this.open = open;
    }

    // Start is called before the first frame update
    void Start()
    {
        startingScale = prefab.transform.localScale.x;
        startingCount = finishCount;
        prvsRot = transform.rotation;
        balls = new List<GameObject>();
        for (int i = 0; i < amount; i++)
        {
            Vector3 pos = new Vector3(Mathf.Lerp(corner1.position.x, corner2.position.x, Random.Range(0, 1f)),
                                      Mathf.Lerp(corner1.position.y, corner2.position.y, Random.Range(0, 1f)),
                                      Mathf.Lerp(corner1.position.z, corner2.position.z, Random.Range(0, 1f)));
            balls.Add(Instantiate(prefab, pos, transform.rotation));
        }
    }

    private Quaternion prvsRot;
    private float val = 0;

    private void FixedUpdate()
    {
        if (localDone || !open)
            return;

        Vector3 downVector = transform.TransformDirection(Vector3.down);
        float angle = Mathf.Atan2(downVector.x, downVector.y) * Mathf.Rad2Deg;
        particleParent.localEulerAngles = new Vector3(0, 0, angle + 180);

        float rotAmount = Quaternion.Angle(transform.rotation, prvsRot);
        var em = sandParticles.emission; 

        if(particlesPerAngle * rotAmount > em.rateOverTime.constant)
            em.rateOverTime = particlesPerAngle * rotAmount;
        else
        {
            em.rateOverTime = Mathf.Lerp(em.rateOverTime.constant, particlesPerAngle * rotAmount, 0.5f);
        }

        if (rotAmount > 1)
        {
            finishCount -= rotAmount;
            if (finishCount > 0)
            {
                foreach (GameObject go in balls)
                {
                    go.transform.localScale = Vector3.Lerp(Vector3.one * startingScale, Vector3.zero, Mathf.InverseLerp(startingCount, 0, finishCount));
                    sandRenderer.material.color = new Color(1, 1, 1, Mathf.Lerp(1,0.75f,Mathf.InverseLerp(startingCount,0,finishCount))); 
                }
            }
            else
            {
                foreach (GameObject go in balls)
                {
                    go.SetActive(false);
                }
                em.rateOverTime = 0;
                rotateToSpin.SwitchToFocus();
                informationManager.UpdateText();
                localDone = true;
            }
        }
        prvsRot = transform.rotation;
    }
}
