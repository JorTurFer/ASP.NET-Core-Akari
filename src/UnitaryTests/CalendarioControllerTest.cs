using Akari_Net.Core.Areas.Pacientes.Models.Entities;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace UnitaryTests
{
    public class CalendarioControllerTest
    {
        [Fact]
        public void GetEvents()
        {
            // Arrange
            var serviceMock = new Mock<PacientesDbContext>();
            serviceMock.Setup(x => x.CalendarEvents()).Returns(() => new List<CalendarEvent>
  {
    new CalendarEvent{EventID=1, IsFullDay=true, Subject = "test"},
    new CalendarEvent{EventID=1, IsFullDay=true, Subject = "test"},
    new CalendarEvent{EventID=1, IsFullDay=true, Subject = "test"},
  });
            var controller = new PersonsController(serviceMock.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var persons = okResult.Value.Should().BeAssignableTo<IEnumerable<Person>>().Subject;

            persons.Count().Should().Be(3);
        }
    }
}
