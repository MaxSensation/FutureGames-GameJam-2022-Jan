using UnityEngine;

namespace MaxHelpers
{
    public abstract class DataHandler<TC, TD> : StaticInstance<TC> where TC : MonoBehaviour where TD : ISave
    {
        [SerializeField] private TD defaultData;
        private string _saveKey;
        protected TD Data;

        protected void InitData(string saveKey)
        {
            _saveKey = saveKey;
            Data = DataManager.Instance.Load<TD>(saveKey) ? DataManager.Instance.GetData<TD>(_saveKey) : defaultData;
        }

        public void SaveData() => DataManager.Instance.SaveData(_saveKey, Data);
    }
}