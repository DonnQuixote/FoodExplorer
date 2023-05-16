using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FireManager : MonoBehaviour
{

    [SerializeField] private Transform spawnFire;
    [SerializeField] private ParticleSystem prefab;
    [SerializeField] private Transform parent;
    [SerializeField] private int fireCount;
    [SerializeField] private int fireCountMax = 25;
    [SerializeField] private float extinguishFlameSpeed =  20;
    [SerializeField] private List<StokeCounters> listStokeCounters;
    public AnimationCurve lifetimeCurve;
    //private ParticleSystem ps;

    public bool Spawn;
    public bool Destroy;

    public event EventHandler<OnSpawnFireArgs> OnSpawnFireManager; 
    public class OnSpawnFireArgs : EventArgs
    {
        public Node node;
    }

    public event EventHandler<OnPSDestroyArgs> OnPSDestroy;
    public class OnPSDestroyArgs : EventArgs
    {
        public Node node;
    }


    public float duration = 6f; //time length (seconds£©
    private float startTime;

    private Dictionary<Node, ParticleSystem> hasExistedFire;
    private int extinguishFireCount = 0;
    private int extinguishFireCountMax = 3;
    private void Awake()
    {
        hasExistedFire = new Dictionary<Node, ParticleSystem>();
        //listStokeCounters = new List<StokeCounters>();
    }

    private void Start()
    {
        Grid.Instance.OnAnyNodeBeFired += Instance_OnAnyNodeBeFired;
        Player.Instance.OnExtinguishFire += Player_OnExtinguishFire;
        foreach(StokeCounters st in listStokeCounters)
        {
            st.OnSpawnFire += St_OnSpawnFire;
        }
    }

    private void Player_OnExtinguishFire(object sender, Player.OnExtinguishFireArgs e)
    {
        Node node = e.node;
        ExtinguishFlame(node);
    }

    private void St_OnSpawnFire(object sender, StokeCounters.OnSpawnFireArgs e)
    {
        StokeCounters sctt = sender as StokeCounters;
        Vector3 position = e.positionSpawnFire;
        InitialFire(position);
    }

    private void Instance_OnAnyNodeBeFired(object sender, Grid.OnAnyNodeBeFiredArgs e)
    {
        ParticleSystem ps = Instantiate(prefab, e.node.worldPostion, prefab.transform.rotation, parent);
        ps.Play();

        hasExistedFire.Add(e.node, ps);
    }

    private void Update()
    {
        //if (Spawn)
        //{
        //    Spawn = false;
        //    InitialFire(spawnFire.position);
        //    startTime = Time.time;
        //}

        //if (Destroy)
        //{
        //    ExtinguishFlame( );
        //    //DestroyFire();
        //    Destroy = false;
        //}
    }

    private void InitialFire(Vector3 position)
    {
        Node node = Grid.Instance.NodeFromWorldPoint(position);
        node.state = Node.State.Ignited;

        //ps = Instantiate(prefab);
        ParticleSystem ps = Instantiate(prefab,node.worldPostion,prefab.transform.rotation,parent);
        ps.Play();
        hasExistedFire.Add(node, ps);

        OnSpawnFireManager?.Invoke(this, new OnSpawnFireArgs
        {
            node = node
        });
    }

    private void ExtinguishFlame(Node node)
    {
        startTime = Time.time;
        float tDuration = duration;

        ParticleSystem ps = null;
        if (hasExistedFire.ContainsKey(node))
        {
             ps = hasExistedFire[node];
        }
        else
        {
            //Debug.LogError("there has nothing burned node");
            return;
        }
        int childCount = ps.transform.childCount;
        Debug.Log("childCount:"+childCount);
        foreach (Transform child in ps.transform)
        {
            ParticleSystem childParticleSystem = child.GetComponent<ParticleSystem>();
            if (childParticleSystem != null)
            {
                ParticleSystem.EmissionModule emission = childParticleSystem.emission;
                ParticleSystem.MinMaxCurve rateOverTime = emission.rateOverTime;
                rateOverTime.constant *= 0.5f;
                emission.rateOverTime = rateOverTime;
            }
        }
        extinguishFireCount++;

        if (extinguishFireCount == extinguishFireCountMax)
        {
            extinguishFireCount = 0;
            Destroy(ps.gameObject);
            hasExistedFire.Remove(node);
            OnPSDestroy?.Invoke(this, new OnPSDestroyArgs
            {
                node = node
            });
        }

    }

    //private void DestroyFire()
    //{
    //    ps.Stop();
    //    Destroy(ps.gameObject);
    //}

}
