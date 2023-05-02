using System;
using XIV.Core.Utils;

namespace XIV.TweenSystem
{
    public interface ITween
    {
        void Update(float deltaTime);
        bool IsDone();
        void Complete();
        void Cancel();
        ITween AddEasing(EasingFunction.Function easingFunction);
        ITween UseUnscaledTime();
        ITween OnComplete(Action action);
        ITween OnCanceled(Action action);
    }
}