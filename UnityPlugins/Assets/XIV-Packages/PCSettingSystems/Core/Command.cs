namespace XIV_Packages.PCSettingsSystem
{
    public abstract class Command
    {
        public abstract void Apply(object value);
    }

    public abstract class Command<T> : Command
    {
        public abstract void Apply(T value);

        public override void Apply(object value)
        {
            this.Apply((T)value);
        }
    }
}