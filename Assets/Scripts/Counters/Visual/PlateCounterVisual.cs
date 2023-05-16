using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounterVisual : MonoBehaviour
{
    [SerializeField] private Transform counterToPoint;
    [SerializeField] private Transform plateVisualPrefab;
    [SerializeField] private PlateCounter plateCounter;

    private List<GameObject> plateVisualGameObjectList;

    private void Awake()
    {
        plateVisualGameObjectList = new List<GameObject>();
    }
    // Start is called before the first frame update
    void Start()
    {
        plateCounter.OnPlateSpawned += PlateCounter_OnPlateSpawned;
        plateCounter.OnPlateRemoved += PlateCounter_OnPlateRemoved;
    }

    private void PlateCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        GameObject plateGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];
        plateVisualGameObjectList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }

    private void PlateCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
        Transform plateTransform = Instantiate(plateVisualPrefab, counterToPoint);

        float plateOffsetY = .1f;
        plateTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualGameObjectList.Count, 0);

        plateVisualGameObjectList.Add(plateTransform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
