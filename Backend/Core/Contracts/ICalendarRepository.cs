using Core.Entities;

namespace Core.Contracts
{
    public interface ICalendarRepository
    {
        void Add(Calendar calendar);

        Task Delete(int calendarId);

        Task<List<Calendar>> GetAllAsync();

    }
}