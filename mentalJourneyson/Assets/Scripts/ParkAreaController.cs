using UnityEngine;

public class ParkAreaController : MonoBehaviour
{
    public KeyCollector keyCollector;      // Player üzerindeki KeyCollector referansý
    public Transform playerTransform;      // Player objesinin Transform referansý
    public float pushBackForce = 5f;       // Geri itme kuvveti
    public GameObject parkBarrier;         // Fiziksel engel objesi (duvar vb.)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (keyCollector != null && keyCollector.keyCount >= 5)
            {
                Debug.Log("Park alanýna giriþ izni verildi.");

                // Fiziksel engeli kaldýr (engeli görünmez ve geçilebilir yap)
                if (parkBarrier != null)
                {
                    parkBarrier.SetActive(false);
                }
            }
            else
            {
                Debug.Log("5 anahtar toplanmadan park alanýna girilemez!");

                // Oyuncuyu park alanýndan geri it (örneðin player'ý geriye doðru taþý)
                Vector3 pushDirection = (playerTransform.position - transform.position).normalized;
                playerTransform.position += pushDirection * pushBackForce;
            }
        }
    }
}
