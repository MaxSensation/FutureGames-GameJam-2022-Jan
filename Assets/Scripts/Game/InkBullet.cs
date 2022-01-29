using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class InkBullet : MonoBehaviour
    {
        [SerializeField] private float force;
        [SerializeField] private float lifeTime;
        [SerializeField] private float maxOffset;
        [SerializeField] private LayerMask enemyMask;
        [SerializeField] private float increaseSizeSpeed;
        private void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        private void FixedUpdate()
        {
            transform.position += transform.up * force * Time.deltaTime;
            transform.Rotate(0.0f, 0.0f, Random.Range(-maxOffset, maxOffset));
            CheckForEnemy();
            IncreaseSize();
        }

        private void IncreaseSize()
        {
            transform.localScale += Vector3.one * (increaseSizeSpeed * Time.deltaTime);
        }

        private void CheckForEnemy()
        {
            var result = Physics2D.OverlapCircle(transform.position, 1f, enemyMask);
            if (result == null) return;
            result.GetComponent<IDamageable>().TakeDamage();
            Destroy(gameObject);
        }
    }
}