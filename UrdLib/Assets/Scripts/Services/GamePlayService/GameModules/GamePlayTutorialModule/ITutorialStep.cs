using System;

namespace Urd.Tutorial
{
    public interface ITutorialStep : IDisposable
    {
        string Id { get; set; }
        bool IsActive { get; }
        int ExecutionOrder { get; }
        void Init();
        void Begin();
        void Finish();
        
        TutorialDependency[] Dependencies { get; }
        GamePlayTutorialStepConfig[] CompleteTogether { get; }
        bool HasPlayerCompleted { get; }
    }
    
    public static class TutorialStepExtensions
    {
        public static bool AreMet(this TutorialDependency[] dependencies)
        {
            foreach (var dependency in dependencies)
            {
                if (!dependency.IsMet)
                    return false;
            }

            return true;
        }
    }
}