using Assets.Scripts.Configs;
using Assets.Scripts.State_Machine;
using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Scripts
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private LevelSettings _levelSettings;

        private PlayerData _playerData;
        private StateMachine _stateMachine;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_levelSettings.GameAreaCenter, 1);
            CustomGizmos.DrawCircle(_levelSettings.GameAreaCenter, _levelSettings.GameAreaRadius, Color.red);
        }

        private void Awake()
        {
            _playerData = new(new(), new(_levelSettings));

            FactoryService factoryService = new(_levelSettings, _playerData);
            _stateMachine = new();

            _stateMachine.AddState(new MenuState(_stateMachine))
                .AddState(new GameState(_stateMachine, _levelSettings, factoryService, _playerData));

            _stateMachine.SetState<MenuState>();
        }

        private void Update()
        {
            _stateMachine.Update();
        }
    }
}
