using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    public static Action<Transform> OnEnemySpawnedEvent, OnEnemyDespawnEvent;
    [SerializeField] private float killAnimationSpeed;
    private bool _dead;

    private void OnEnable()
    {
        OnEnemySpawnedEvent?.Invoke(transform);
    }

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

    private void OnDestroy()
    {
        OnEnemyDespawnEvent?.Invoke(transform);
    }
}
