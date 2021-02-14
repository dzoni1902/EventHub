using EventHub.Core.RepositoryInterfaces;

namespace EventHub.Core
{
    public interface IUnitOfWork
    {
        IEventRepository Events { get; }
        IAttendanceRepository Attendances { get; }
        IGenreRepository Genres { get; }
        IFollowingRepository Followings { get; }
        INotificationRepository Notifications { get; }
        void Complete();
    }
}