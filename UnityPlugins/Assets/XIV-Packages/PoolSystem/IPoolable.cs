namespace XIV_Packages.PoolSystem
{
    public interface IPoolable
    {
        void OnPoolCreate(IPool pool);
        void OnPoolReturn();
    }
}