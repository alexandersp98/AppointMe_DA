using Core.Contracts;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    internal class AppointmentRepository : IAppointmentRepository
    {
        private ApplicationDbContext _context;

        public AppointmentRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Add(Appointment calendar)
        {
            _context.Appointments.Add(calendar);
        }

        public void Delete(Appointment calendarToDelete)
        {
            _context.Appointments.Remove(calendarToDelete);
        }

        public async Task<List<Appointment>> GetAllAsync()
        {
            return await _context.Appointments
                .OrderBy(c => c.Appointment_Date)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsByUserName(string userName)
        {
            return await _context.Appointments
                .Include(a => a.Customer)
                .Where(a => a.Business!.UserName == userName)
                .ToListAsync();
        }

        public async Task<Appointment?> GetById(int calendarId)
        {
            return await _context.Appointments.Where(c => c.Id == calendarId).FirstOrDefaultAsync();
        }

        
    }
}