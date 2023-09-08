namespace Assets.XIV
{
    public interface ICommand
    {
        void Execute();

        void Unexecute();
    }
}