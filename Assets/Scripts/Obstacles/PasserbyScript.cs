using System;
using UnityEngine;
using UnityEngine.Pool;



public class PasserbyScript : MonoBehaviour
{
    private IObjectPool<PasserbyScript> _objectPool;

    public IObjectPool<PasserbyScript> _ObjectPool { set => _objectPool = value; }

    [SerializeField] public Transform[] spawnPoint;
    

    [SerializeField] PasserbyData data;

    private int direction = 1;
    public bool spawnCheck;

    private void Start()
    {
        //if (_objectPool != null) return;

        //Transform point = spawnPoint[UnityEngine.Random.Range(0,spawnPoint.Length)];

        //transform.position = point.position;
        //transform.rotation = point.rotation;

        //_objectPool.Get();
    }

    private void Update()
    {
        transform.Translate(0, 0, direction * data.walkSpeed * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (player != null && !player.isKnockedOut)
        {
            Vector3 pushDirection = (collision.transform.position - transform.position).normalized;

            pushDirection.y = 0.5f;
            pushDirection.Normalize();

            player.HitAndFall(pushDirection, data.bounceForce);
        }

        
       // if (collision.gameObject.CompareTag("Finish"))
            //Spawn();
        //else
            //NoSpawn();
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (other.gameObject.tag == "Back")
        {
            Desactivate();
        }
    }
    public void Desactivate ()
    {
        _objectPool.Release(this);
    }

    public void Foward()
    {
        direction = 1;
    }

    public void Backward()
    {
        direction = -1;
    }

   // public void Spawn()
    //{
      //  spawnCheck = true;

    //}

   // public void NoSpawn()
    //{
       // spawnCheck = false;
       // _objectPool.Release(this);
    //}

}
