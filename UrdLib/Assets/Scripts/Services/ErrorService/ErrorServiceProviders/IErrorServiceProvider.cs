namespace Urd.Services
{
    public interface IErrorServiceProvider
    {
        void Init();
        void LogError(string message);
    }
}