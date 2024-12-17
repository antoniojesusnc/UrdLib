namespace Urd.Services
{
    public interface IErrorService : IBaseService
    {
        void LogError(string warning);
    }
}