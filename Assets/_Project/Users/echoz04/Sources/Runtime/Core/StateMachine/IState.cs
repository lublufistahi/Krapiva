namespace Sources.Runtime.Core.StateMachine
{
    public interface IState
    {
        void Enter();
        void Exit();
    }
}