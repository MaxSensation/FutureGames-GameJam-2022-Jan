using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float killAnimationSpeed;
    private bool _dead;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (_dead) return;
        if (col.transform.CompareTag("Player")) col.transform.GetComponent<IDamageable>().TakeDamage();
    }

    public void TakeDamage()
    {
        _dead = true;
        Destroy(gameObject, killAnimationSpeed);
    }
}
