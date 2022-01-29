using MaxHelpers;
using UnityEngine;

public class InkSpawner : MonoBehaviour
{
    [SerializeField] private GameObject inkPrefab;
    [SerializeField] private float range;
    private void Start()
    {
        transform.Rotate(0.0f, 0.0f, 180.0f);
        RotateToClosestEnemy();
        Instantiate(inkPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void RotateToClosestEnemy()
    {
        var shortestDistance = float.MaxValue;
        Transform enemyTrans = null;
        foreach (var enemy in GameManager.GetAllEnemies())
        {
            var distance = Vector2.Distance(transform.position, enemy.position);
            if (!(distance < shortestDistance)) continue;
            shortestDistance = distance;
            enemyTrans = enemy;
        }

        if (enemyTrans == null || !(shortestDistance <= range)) return;
        var dir = enemyTrans.position - transform.position;
        transform.rotation = Quaternion.AngleAxis((Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) -90f, Vector3.forward);
    }
}