using UnityEngine;

public class StickyGumTrap : MonoBehaviour
{
    [Header("Configuración del Chicle")]
    public float slowedSpeed = 2f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerController>(out PlayerController player))
            {
                if (!player.isKnockedOut)
                {
                    player.ChangeSpeed(slowedSpeed);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerController>(out PlayerController player))
            {
                player.RestoreSpeed();
            }
        }
    }
}