using Core.Entities;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace Core.Contracts
{
    public interface IAppointmentRepository
    {
        void Add(Appointment calendar);

        void Delete(Appointment calendarToDelete);

        Task<List<Appointment>> GetAllAsync();

        Task<Appointment?> GetById(int calendarId);

    }
}