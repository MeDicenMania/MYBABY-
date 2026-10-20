using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Configuracion del impacto")]
    public float bounceForce; 

    private void OnCollisionEnter(Collision collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (player != null && !player.isKnockedOut)
        {
            Vector3 pushDirection = (collision.transform.position - transform.position).normalized;

            pushDirection.y = 0.5f;
            pushDirection.Normalize();

            player.HitAndFall(pushDirection, bounceForce);
        }
    }
}