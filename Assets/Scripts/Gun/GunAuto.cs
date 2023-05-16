using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAuto : Gun
{
    [SerializeField] float searchRadius;
    [SerializeField] bool onDrawGizmos = true;
    [SerializeField] float searchTimeMax = 1.0f;
    float searchTime = 1.0f;
    List<Enemy> listEnermy;
    private const  string targetTag = "Enermy";
    private Transform currentTarget;
    private Vector3 lookPositionTransform;


    void Start()
    {
        listEnermy = new List<Enemy>();
         lookPositionTransform = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        searchTime -= Time.deltaTime;
        if (searchTime<=0)
        {
            listEnermy.Clear();
            Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius);
            float closestDistance = Mathf.Infinity;
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag(targetTag))
                {
                    listEnermy.Add(collider.transform.GetComponent<Enemy>());
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        currentTarget = collider.transform;
                    }
                }
            }
            searchTime = searchTimeMax;
        }
       
        if (currentTarget != null)
        {
            Vector3 lookAtPosition = new Vector3(currentTarget.transform.position.x, transform.position.y, currentTarget.transform.position.z);
            transform.LookAt(lookAtPosition);
            lookPositionTransform = currentTarget.position;
        }
        else if(listEnermy.Count != 0)
        {
            //if (listEnermy[0].transform != null)
            //{
            //  lookPositionTransform = listEnermy[0].transform.position;
            //}
            transform.LookAt(lookPositionTransform);
        }

        if (listEnermy.Count != 0)
        {
            this.Shoot();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (onDrawGizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, searchRadius);
        }
    }

    private List<Enemy> FindAllEnemies()
    {
        listEnermy.Clear();
        Enemy[] temp = FindObjectsOfType<Enemy>();

        foreach (Enemy e in temp)
        {
            listEnermy.Add(e);
        }
        Debug.Log("listEnermy.size(): " + listEnermy.Count);
        return listEnermy;
    }
}
