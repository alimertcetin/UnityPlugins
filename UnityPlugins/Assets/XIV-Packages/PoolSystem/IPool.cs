using System;

namespace XIV_Packages.PoolSystem
{
    public interface IPool
    {
        Type StoredType { get; }
        void Return(IPoolable item);
    }

    public interface IPool<out T> : IPool where T : IPoolable
    {
        T GetItem();
    }
}