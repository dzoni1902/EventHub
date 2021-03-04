using EventHub.Controllers.WebAPI;
using EventHub.Core;
using EventHub.Core.Models;
using EventHub.Core.RepositoryInterfaces;
using EventHub.Tests.Extensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Http.Results;

namespace EventHub.Tests.Controllers.Api
{
    [TestClass]
    public class EventsControllerTests
    {
        private static EventsController _controller;
        private Mock<IEventRepository> _mockRepository;
        private string _userId;

        public EventsControllerTests()
        {
            _mockRepository = new Mock<IEventRepository>();

            var mockUOW = new Mock<IUnitOfWork>();
            mockUOW.SetupGet(u => u.Events).Returns(_mockRepository.Object);

            _controller = new EventsController(mockUOW.Object);
            _userId = "1";
            _controller.MockCurrentUser(_userId, "user1@domain.com");
        }

        [TestMethod]
        public void Cancel_NoEventWithGivenIdExists_ShouldReturnNotFound()
        {
            var result = _controller.Cancel(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void Cancel_EventIsCanceled_ShouldReturnNotFound()
        {
            var eventObject = new Event();
            eventObject.CancelEvent();

            _mockRepository.Setup(r => r.GetEventWithAtendees(1)).Returns(eventObject);

            var result = _controller.Cancel(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void Cancel_UserCancelingAnotherUserEvent_ShouldReturnUnauthorized()
        {
            var eventObject = new Event { ArtistId = _userId + "-" };

            _mockRepository.Setup(r => r.GetEventWithAtendees(1)).Returns(eventObject);

            var result = _controller.Cancel(1);

            result.Should().BeOfType<UnauthorizedResult>();
        }

        [TestMethod]
        public void Cancel_ValidRequest_ShouldReturnOk()
        {
            var eventObject = new Event { ArtistId = _userId };

            _mockRepository.Setup(r => r.GetEventWithAtendees(1)).Returns(eventObject);

            var result = _controller.Cancel(1);

            result.Should().BeOfType<OkResult>();
        }
    }
}
