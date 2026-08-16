using bankingapp.API.Entities;

namespace bankingapp.API.Services
{
    public interface IRequestLogService
    {
        Task LogAsync(RequestLog log);
    }
}
