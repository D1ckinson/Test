using Assets.Scripts;
using UnityEngine;

namespace YG
{
    public partial class SavesYG
    {
        private string _playerDataJson;

        public void Save(PlayerData data)
        {
            _playerDataJson = JsonUtility.ToJson(data);
        }

        public PlayerData Load()
        {
            return JsonUtility.FromJson<PlayerData>(_playerDataJson) ?? new();
        }
    }
}
