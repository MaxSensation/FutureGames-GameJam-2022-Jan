using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MaxHelpers
{
    public class GameManager : StaticInstance<GameManager>
    {
        public Action<float> OnWaterLevelChanged;
        public Action<int> OnInksChanged;
        public PlayerInputs Inputs { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Inputs = new();
        }

        private void Start() => Inputs.Player.RestartLevel.performed += _ => LevelManager.Instance.LoadLastLevel();

        public static List<Transform> GetAllEnemies()
        {
            return GameObject.FindGameObjectsWithTag("Enemy").Select(enemy => enemy.transform).ToList();
        }
    }
}