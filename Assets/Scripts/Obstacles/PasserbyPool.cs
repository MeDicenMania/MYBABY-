using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class PasserbyPool : MonoBehaviour
{
    [Tooltip("Prefab del transeunte")]
    [SerializeField] private PasserbyScript passerbyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField]private float cooldownSpawn;
    [SerializeField]private float spawnRate = 1f;

    private IObjectPool<PasserbyScript> _pool;

    [SerializeField] private bool colectionCheck = true;//Tira error si intentamos devolver un objeto ya existente dentro del pool

    private int defaultCapacity = 5;
    private int maxCapacity = 20;

    void Awake()
    {
        _pool = new ObjectPool<PasserbyScript>(CreatePasser, OnGet, OnRelease, OnDestroyPooledObject, colectionCheck, defaultCapacity, maxCapacity);
    }

    private void FixedUpdate()
    {
        cooldownSpawn -= Time.fixedDeltaTime;
        if (cooldownSpawn > 0f) return;

        cooldownSpawn = spawnRate;

        Transform point = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

        PasserbyScript passerObj = _pool.Get();
        passerObj.transform.SetPositionAndRotation(point.position, point.rotation);
    }

    public void OnDestroyPooledObject(PasserbyScript prefab)
    {
       Destroy(prefab.gameObject);
    }

    public void OnRelease(PasserbyScript prefab)
    {
        prefab.gameObject.SetActive(false);
    }

    public void OnGet(PasserbyScript prefab)
    {
         prefab.gameObject.SetActive(true);
    }

    public PasserbyScript CreatePasser()
    {
        PasserbyScript passerbyInstance = Instantiate(passerbyPrefab);
        passerbyInstance._ObjectPool = _pool;//Hace que tenga acceso al pool
        return passerbyInstance;
    }



}
