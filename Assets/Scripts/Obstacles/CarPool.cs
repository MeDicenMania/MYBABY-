using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class CarPool : MonoBehaviour
{
    [Tooltip("Prefab del transeunte")]
    [SerializeField] private CarScript CarPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField]private float cooldownSpawn;
    [SerializeField]private float spawnRate = 1f;

    private IObjectPool<CarScript> _pool;

    [SerializeField] private bool colectionCheck = true;//Tira error si intentamos devolver un objeto ya existente dentro del pool

    private int defaultCapacity = 5;
    private int maxCapacity = 20;

    void Awake()
    {
        _pool = new ObjectPool<CarScript>(CreatePasser, OnGet, OnRelease, OnDestroyPooledObject, colectionCheck, defaultCapacity, maxCapacity);
    }

    private void FixedUpdate()
    {
        
        cooldownSpawn -= Time.fixedDeltaTime;
        
        if (cooldownSpawn > 0f) return;
        

        cooldownSpawn = spawnRate;

        
        int index = UnityEngine.Random.Range(0, spawnPoints.Length);
        Transform point = spawnPoints[index];

        CarScript CarObj = _pool.Get();
        CarObj.transform.SetPositionAndRotation(point.position, point.rotation);
       
            
        if (index==0)
        {
            CarObj.Foward();
        }
        else
        {
            CarObj.Backward();
        }

        
    }

    public void OnDestroyPooledObject(CarScript prefab)
    {
       Destroy(prefab.gameObject);
    }

    public void OnRelease(CarScript prefab)
    {
        prefab.gameObject.SetActive(false);
    }

    public void OnGet(CarScript prefab)
    {
         prefab.gameObject.SetActive(true);
    }

    public CarScript CreatePasser()
    {
        CarScript CarInstance = Instantiate(CarPrefab);
        CarInstance._ObjectPool = _pool;//Hace que tenga acceso al pool
        return CarInstance;
    }

    

}
