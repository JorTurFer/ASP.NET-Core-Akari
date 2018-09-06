using Akari_Net.Core.Areas.Pacientes.Controllers;
using Akari_Net.Core.Areas.Pacientes.Hubs;
using Akari_Net.Core.Areas.Pacientes.Models.Data;
using Akari_Net.Core.Areas.Pacientes.Models.Entities;
using Akari_Net.Core.Areas.Pacientes.Models.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitaryTests
{
    public class CalendarioControllerTest
    {
        [Fact]
        public async void GetEvents()
        {
            // Arrange
            var serviceMock = new Mock<ICalendarioServices>();
            serviceMock.Setup(x => x.GetCalendarEventsAsync(DateTime.Now.Date, "month")).ReturnsAsync(() => new List<CalendarEvent>
              {
                new CalendarEvent{EventID=1, IsFullDay=true, Subject = "test",Start = DateTime.Now},
                new CalendarEvent{EventID=2, IsFullDay=true, Subject = "test2",Start = DateTime.Now.AddDays(7)},
                new CalendarEvent{EventID=3, IsFullDay=true, Subject = "skip",Start = DateTime.Now.AddDays(1)},
              });
            var mockHub = new Mock<IHubContext<CalendarioHub>>();
            var controller = new CalendarioController(serviceMock.Object, mockHub.Object);

            // Act
            var result = await controller.GetEvents(DateTime.Now.Date,"month");

            // Assert
            var okResult = result.Should().BeOfType<JsonResult>().Subject;
            var citas = okResult.Value.Should().BeAssignableTo<IEnumerable<CalendarEvent>>().Subject;

            citas.Count().Should().Be(3);
        }
    }
}
