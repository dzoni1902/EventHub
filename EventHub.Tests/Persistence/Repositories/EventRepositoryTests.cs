using EventHub.Core;
using EventHub.Core.Models;
using EventHub.Persistence.Repositories;
using EventHub.Tests.Extensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace EventHub.Tests.Persistence.Repositories
{
    [TestClass]
    public class EventRepositoryTests
    {
        private EventRepository _repository;
        private Mock<DbSet<Event>> _mockEvents;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockEvents = new Mock<DbSet<Event>>();
            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.SetupGet(c => c.Events).Returns(_mockEvents.Object);

            _repository = new EventRepository(mockContext.Object);
        }

        [TestMethod]
        public void GetUpcomingEventsByArtist_EventIsInThePast_ShouldNotBeReturned()
        {
            var eventObject = new Event { DateTime = DateTime.Now.AddDays(-1), ArtistId = "1" };
            _mockEvents.SetSource(new List<Event> { eventObject });

            var events = _repository.GetUpcomingEventsByArtist("1");

            events.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingEventsByArtist_EventIsCanceled_ShouldNotBeReturned()
        {
            var eventObject = new Event { DateTime = DateTime.Now.AddDays(1), ArtistId = "1" };
            eventObject.CancelEvent();

            _mockEvents.SetSource(new List<Event> { eventObject });

            var events = _repository.GetUpcomingEventsByArtist("1");

            events.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingEventsByArtist_EventIsForADifferentArtist_ShouldNotBeReturned()
        {
            var eventObject = new Event { DateTime = DateTime.Now.AddDays(1), ArtistId = "1" };

            _mockEvents.SetSource(new List<Event> { eventObject });

            var events = _repository.GetUpcomingEventsByArtist("2");

            events.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingEventsByArtist_EventIsForAGivenArtist_ShouldBeReturned()
        {
            var eventObject = new Event { DateTime = DateTime.Now.AddDays(1), ArtistId = "1" };

            _mockEvents.SetSource(new List<Event> { eventObject });

            var events = _repository.GetUpcomingEventsByArtist("1");

            events.Should().Contain(eventObject);
        }
    }
}
