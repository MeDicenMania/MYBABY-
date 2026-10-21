using UnityEngine;

public class BrokenGlassTrap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerController>(out PlayerController player))
            {
                if (!player.isKnockedOut)
                {
                    player.Die();
                }
            }
        }
    }
}