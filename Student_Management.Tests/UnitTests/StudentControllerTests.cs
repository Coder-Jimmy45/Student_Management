using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Moq;
using Student_Management.Controllers;
using Student_Management.DTOs;
using Student_Management.Models;
using Student_Management.Services;

namespace Student_Management.Tests.UnitTests
{
    public class StudentControllerTests
    {
        private readonly Mock<IStudentService> _mockService;
        private readonly Mock<ILogger<StudentController>> _mockLogger;
        private readonly StudentController _controller;

        public StudentControllerTests()
        {
            _mockService = new Mock<IStudentService>();
            _mockLogger = new Mock<ILogger<StudentController>>();
            _controller = new StudentController(_mockService.Object, _mockLogger.Object);
        }

        #region GetAll Tests
        [Fact]
        public async Task GetAll_ReturnsOkWithStudentsList()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student { Id = 1, Name = "John Doe", Email = "john@example.com", Age = 20, Course = "CS" },
                new Student { Id = 2, Name = "Jane Smith", Email = "jane@example.com", Age = 21, Course = "IT" }
            };
            _mockService.Setup(s => s.GetAll()).ReturnsAsync(students);

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult.Value);
            
            var returnedStudents = okResult.Value as List<Student>;
            Assert.Equal(2, returnedStudents.Count);
            _mockService.Verify(s => s.GetAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithEmptyList_WhenNoStudents()
        {
            // Arrange
            _mockService.Setup(s => s.GetAll()).ReturnsAsync(new List<Student>());

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            
            var okResult = result as OkObjectResult;
            var students = okResult.Value as List<Student>;
            Assert.Empty(students);
        }
        #endregion

        #region Add Tests
        [Fact]
        public async Task Add_WithValidData_ReturnsOk()
        {
            // Arrange
            var studentDto = new StudentDTOs 
            { 
                Name = "Test Student", 
                Email = "test@example.com", 
                Age = 20, 
                Course = "CS" 
            };
            _mockService.Setup(s => s.Add(It.IsAny<StudentDTOs>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Add(studentDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            
            var okResult = result as OkObjectResult;
            Assert.Equal("Student added", okResult.Value);
            _mockService.Verify(s => s.Add(studentDto), Times.Once);
        }

        [Fact]
        public async Task Add_CallsServiceWithProvidedData()
        {
            // Arrange
            var studentDto = new StudentDTOs 
            { 
                Name = "Alice Johnson", 
                Email = "alice@example.com", 
                Age = 22, 
                Course = "Mathematics" 
            };
            _mockService.Setup(s => s.Add(It.IsAny<StudentDTOs>())).Returns(Task.CompletedTask);

            // Act
            await _controller.Add(studentDto);

            // Assert
            _mockService.Verify(s => s.Add(It.Is<StudentDTOs>(dto => 
                dto.Name == "Alice Johnson" && 
                dto.Email == "alice@example.com" && 
                dto.Age == 22 && 
                dto.Course == "Mathematics"
            )), Times.Once);
        }
        #endregion

        #region Update Tests
        [Fact]
        public async Task Update_WithValidData_ReturnsOk()
        {
            // Arrange
            var studentId = 1;
            var studentDto = new StudentDTOs 
            { 
                Name = "Updated Student", 
                Email = "updated@example.com", 
                Age = 21, 
                Course = "IT" 
            };
            _mockService.Setup(s => s.Update(It.IsAny<int>(), It.IsAny<StudentDTOs>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(studentId, studentDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            
            var okResult = result as OkObjectResult;
            Assert.Equal("Updated", okResult.Value);
            _mockService.Verify(s => s.Update(studentId, studentDto), Times.Once);
        }

        [Fact]
        public async Task Update_PassesCorrectIdAndData()
        {
            // Arrange
            var studentId = 5;
            var studentDto = new StudentDTOs 
            { 
                Name = "Bob Smith", 
                Email = "bob@example.com", 
                Age = 23, 
                Course = "Physics" 
            };
            _mockService.Setup(s => s.Update(It.IsAny<int>(), It.IsAny<StudentDTOs>())).Returns(Task.CompletedTask);

            // Act
            await _controller.Update(studentId, studentDto);

            // Assert
            _mockService.Verify(s => s.Update(5, It.Is<StudentDTOs>(dto => 
                dto.Name == "Bob Smith" && 
                dto.Email == "bob@example.com" && 
                dto.Age == 23 && 
                dto.Course == "Physics"
            )), Times.Once);
        }

        [Fact]
        public async Task Update_ServiceThrowsException_ExceptionPropagates()
        {
            // Arrange
            var studentId = 999;
            var studentDto = new StudentDTOs 
            { 
                Name = "Test", 
                Email = "test@example.com", 
                Age = 20, 
                Course = "CS" 
            };
            _mockService.Setup(s => s.Update(It.IsAny<int>(), It.IsAny<StudentDTOs>()))
                .ThrowsAsync(new Exception("Student not found"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _controller.Update(studentId, studentDto));
        }
        #endregion

        #region Delete Tests
        [Fact]
        public async Task Delete_WithValidId_ReturnsOk()
        {
            // Arrange
            var studentId = 1;
            _mockService.Setup(s => s.Delete(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(studentId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            
            var okResult = result as OkObjectResult;
            Assert.Equal("Deleted", okResult.Value);
            _mockService.Verify(s => s.Delete(studentId), Times.Once);
        }

        [Fact]
        public async Task Delete_PassesCorrectIdToService()
        {
            // Arrange
            var studentId = 7;
            _mockService.Setup(s => s.Delete(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            await _controller.Delete(studentId);

            // Assert
            _mockService.Verify(s => s.Delete(7), Times.Once);
        }

        [Fact]
        public async Task Delete_ServiceThrowsException_ExceptionPropagates()
        {
            // Arrange
            var studentId = 999;
            _mockService.Setup(s => s.Delete(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Student not found"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _controller.Delete(studentId));
        }
        #endregion

        #region Logging Tests
        [Fact]
        public async Task GetAll_LogsInformationMessage()
        {
            // Arrange
            _mockService.Setup(s => s.GetAll()).ReturnsAsync(new List<Student>());

            // Act
            await _controller.GetAll();

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Getting all students")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Add_LogsInformationMessage()
        {
            // Arrange
            var studentDto = new StudentDTOs { Name = "Test", Email = "test@example.com", Age = 20, Course = "CS" };
            _mockService.Setup(s => s.Add(It.IsAny<StudentDTOs>())).Returns(Task.CompletedTask);

            // Act
            await _controller.Add(studentDto);

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Adding student")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
        #endregion
    }
}
