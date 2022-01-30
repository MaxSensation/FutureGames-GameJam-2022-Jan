using System;
using System.Collections.Generic;
using UnityEngine;

namespace MaxHelpers
{
    public class GameManager : StaticInstance<GameManager>
    {
        public Action<float> OnWaterLevelChanged;
        public Action<int> OnInksChanged;
        public PlayerInputs Inputs { get; private set; }
        private readonly List<Transform> _enemies = new();

        protected override void Awake()
        {
            base.Awake();
            Inputs = new();
        }

        private void Start() => Inputs.Player.RestartLevel.performed += _ => LevelManager.Instance.LoadLastLevel();

        public List<Transform> GetAllEnemies() => _enemies;
        public void RegisterEnemy(Transform enemy) => _enemies.Add(enemy);
        public void DeregisterEnemy(Transform enemy)
        {
            if (_enemies.Contains(enemy)) _enemies.Remove(enemy);
        }
    }
}