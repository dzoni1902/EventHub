using EventHub.Core;
using EventHub.Core.RepositoryInterfaces;
using EventHub.Persistence.Repositories;

namespace EventHub.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IEventRepository Events { get; private set; }
        public IAttendanceRepository Attendances { get; private set; }
        public IGenreRepository Genres { get; private set; }
        public IFollowingRepository Followings { get; private set; }
        public INotificationRepository Notifications { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Events = new EventRepository(context);
            Attendances = new AttendanceRepository(context);
            Genres = new GenreRepository(context);
            Followings = new FollowingRepository(context);
            Notifications = new NotificationRepository(context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}