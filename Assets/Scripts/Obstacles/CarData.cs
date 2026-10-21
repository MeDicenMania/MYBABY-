using UnityEngine;

[CreateAssetMenu(fileName = "CarData", menuName = "Scriptable Objects/CarData")]
public class CarData : ScriptableObject
{
   public float bounceForce;
   public float walkSpeed;
   public GameObject modelPrefab;

   
}
