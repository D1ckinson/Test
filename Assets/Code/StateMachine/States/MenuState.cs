namespace Assets.Scripts.State_Machine
{
    public class MenuState : State
    {
        public MenuState(StateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            SetState<GameState>();
        }

        public override void Exit()
        {
        }

        public override void Update()
        {
        }
    }
}
