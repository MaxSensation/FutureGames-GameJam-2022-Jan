using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Action<Vector2> OnCheckPointReachedEvent;
    private bool _activated;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!_activated && col.CompareTag("Player"))
        {
            
            _activated = true;
            OnCheckPointReachedEvent?.Invoke(transform.position);
        }        
    }
}
