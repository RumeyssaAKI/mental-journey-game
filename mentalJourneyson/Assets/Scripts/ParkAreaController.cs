using UnityEngine;

public class ParkAreaController : MonoBehaviour
{
    public KeyCollector keyCollector;      // Player �zerindeki KeyCollector referans�
    public Transform playerTransform;      // Player objesinin Transform referans�
    public float pushBackForce = 5f;       // Geri itme kuvveti
    public GameObject parkBarrier;         // Fiziksel engel objesi (duvar vb.)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (keyCollector != null && keyCollector.keyCount >= 5)
            {
                Debug.Log("Park alan�na giri� izni verildi.");

                // Fiziksel engeli kald�r (engeli g�r�nmez ve ge�ilebilir yap)
                if (parkBarrier != null)
                {
                    parkBarrier.SetActive(false);
                }
            }
            else
            {
                Debug.Log("5 anahtar toplanmadan park alan�na girilemez!");

                // Oyuncuyu park alan�ndan geri it (�rne�in player'� geriye do�ru ta��)
                Vector3 pushDirection = (playerTransform.position - transform.position).normalized;
                playerTransform.position += pushDirection * pushBackForce;
            }
        }
    }
}
