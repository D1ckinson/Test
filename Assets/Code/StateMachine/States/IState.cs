namespace Assets.Scripts.State_Machine
{
    public interface IState
    {
        public void Enter();

        public void Update();

        public void Exit();
    }
}
