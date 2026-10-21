using UnityEngine;

[CreateAssetMenu(fileName = "PasserbyData", menuName = "Scriptable Objects/PasserbyData")]
public class PasserbyData : ScriptableObject
{
   public float bounceForce;
   public float walkSpeed;
   public GameObject modelPrefab;
}
