﻿using EventHub.Models;
using EventHub.Repositories;

namespace EventHub.Persistence
{
    public class UnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public EventRepository Events { get; private set; }
        public AttendanceRepository Attendances { get; private set; }
        public GenreRepository Genres { get; private set; }
        public FollowingRepository Followings { get; private set; }
        public NotificationRepository Notifications { get; private set; }

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