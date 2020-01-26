using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocksBreaker : MonoBehaviour
{
    public InformationManager informationManager;
    public List<AudioClip> clips;
    public AudioSource breakingAudio;

    //public DustCleaner dustCleaner;
    public int rocksAmount;
    public LayerMask raycastLayers;
    public GameObject prefab;
    public GameObject particlesPrefab;
    public int rocksHP = 5;

    Camera mainCamera;

    private List<Collider> rocksHit;
    private List<int> rocksHitCount;

    private int destroyed;
    private bool open;

    void Start()
    {
        mainCamera = Camera.main;
        rocksHit = new List<Collider>();
        rocksHitCount = new List<int>();
    }
    public void OpenTool(bool open)
    {
        this.open = open;
    }


    // Update is called once per frame
    void Update()
    {
        if (!open)
            return;
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit ,10, raycastLayers))
            {
                if(hit.collider.gameObject.tag == "Breakable")
                {
                    TutorialManager.instance.RockBroken();
                    breakingAudio.clip = clips[Random.Range(0, clips.Count)];
                    breakingAudio.Play();
                    if (rocksHit.Contains(hit.collider))
                    {
                        int index = rocksHit.FindIndex(a => a == hit.collider);
                        rocksHitCount[index]++;
                        if (rocksHitCount[index] >= 5)
                        {
                            GameObject spawnedDecal = GameObject.Instantiate(particlesPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                            destroyed++;
                            hit.collider.gameObject.SetActive(false);
                            if (destroyed == rocksAmount)
                            {
                                TutorialManager.instance.AllRocksBroken();
                                //dustCleaner.enabled = true;
                                informationManager.UpdateText();
                            }
                        }
                        else
                        {
                            GameObject spawnedDecal = GameObject.Instantiate(prefab, hit.point, Quaternion.LookRotation(hit.normal));
                            spawnedDecal.transform.SetParent(hit.collider.transform);
                        }
                    }
                    else
                    {
                        GameObject spawnedDecal = GameObject.Instantiate(prefab, hit.point, Quaternion.LookRotation(hit.normal));
                        spawnedDecal.transform.SetParent(hit.collider.transform);
                        rocksHit.Add(hit.collider);
                        rocksHitCount.Add(0);
                    }
                }
            }
        }
    }
}
