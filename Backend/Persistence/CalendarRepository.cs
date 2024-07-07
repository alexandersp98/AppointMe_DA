using Core.Contracts;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    internal class CalendarRepository : ICalendarRepository
    {
        private ApplicationDbContext _context;

        public CalendarRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Add(Calendar calendar)
        {
            _context.Calendars.Add(calendar);
        }

        public async Task Delete(int calendarId)
        {
            Calendar? calendarToRemove = await _context.Calendars.Where(c => c.Id == calendarId).FirstOrDefaultAsync();

            if(calendarToRemove != null)
            {
                _context.Calendars.Remove(calendarToRemove);
            }

        }

        public async Task<List<Calendar>> GetAllAsync()
        {
            return await _context.Calendars.OrderBy(c => c.Appointment).ToListAsync();
        }
    }
}