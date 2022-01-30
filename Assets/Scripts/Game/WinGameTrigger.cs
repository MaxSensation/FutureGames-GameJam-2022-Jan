using MaxHelpers;
using UnityEngine;

public class WinGameTrigger : MonoBehaviour
{
    private bool _activated;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !_activated)
        {
            _activated = true;
            GameManager.Instance.OnWinEvent?.Invoke();
        }
    }
}
