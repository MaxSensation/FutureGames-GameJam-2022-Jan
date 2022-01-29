using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) other.GetComponent<IDamageable>().TakeDamage();
    }

    public void TakeDamage() { }
}
