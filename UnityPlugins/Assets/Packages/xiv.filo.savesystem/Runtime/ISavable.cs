namespace XIV.Packages.SaveSystems
{
    public interface ISavable
    {
        object GetSaveData();
        void LoadSaveData(object data);
    }
}