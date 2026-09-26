using Moq;
using Student_Management.DTOs;
using Student_Management.Models;
using Student_Management.Repositories;
using Student_Management.Services;

namespace Student_Management.Tests.UnitTests
{
    public class StudentServiceTests
    {
        private readonly Mock<IStudentRepository> _mockRepository;
        private readonly StudentService _service;

        public StudentServiceTests()
        {
            _mockRepository = new Mock<IStudentRepository>();
            _service = new StudentService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsAllStudents()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student { Id = 1, Name = "John Doe", Email = "john@example.com", Age = 20, Course = "CS" },
                new Student { Id = 2, Name = "Jane Smith", Email = "jane@example.com", Age = 21, Course = "IT" }
            };
            _mockRepository.Setup(r => r.GetAll()).ReturnsAsync(students);

            // Act
            var result = await _service.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("John Doe", result[0].Name);
            Assert.Equal("Jane Smith", result[1].Name);
            _mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenNoStudents()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAll()).ReturnsAsync(new List<Student>());

            // Act
            var result = await _service.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Get_ReturnsSingleStudent()
        {
            // Arrange
            var student = new Student { Id = 1, Name = "John Doe", Email = "john@example.com", Age = 20, Course = "CS" };
            _mockRepository.Setup(r => r.GetById(1)).ReturnsAsync(student);

            // Act
            var result = await _service.Get(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("John Doe", result.Name);
        }

        [Fact]
        public async Task Get_ReturnsNull_WhenStudentNotFound()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetById(It.IsAny<int>())).ReturnsAsync((Student)null);

            // Act
            var result = await _service.Get(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Add_CreatesNewStudent()
        {
            // Arrange
            var dto = new StudentDTOs { Name = "John Doe", Email = "john@example.com", Age = 20, Course = "CS" };
            _mockRepository.Setup(r => r.Add(It.IsAny<Student>())).Returns(Task.CompletedTask);

            // Act
            await _service.Add(dto);

            // Assert
            _mockRepository.Verify(r => r.Add(It.Is<Student>(s => 
                s.Name == "John Doe" && s.Email == "john@example.com" && s.Age == 20 && s.Course == "CS"
            )), Times.Once);
        }

        [Fact]
        public async Task Update_UpdatesExistingStudent()
        {
            // Arrange
            var student = new Student { Id = 1, Name = "John Doe", Email = "john@example.com", Age = 20, Course = "CS" };
            var dto = new StudentDTOs { Name = "John Updated", Email = "john.updated@example.com", Age = 21, Course = "IT" };
            
            _mockRepository.Setup(r => r.GetById(1)).ReturnsAsync(student);
            _mockRepository.Setup(r => r.Update(It.IsAny<Student>())).Returns(Task.CompletedTask);

            // Act
            await _service.Update(1, dto);

            // Assert
            Assert.Equal("John Updated", student.Name);
            Assert.Equal("john.updated@example.com", student.Email);
            Assert.Equal(21, student.Age);
            Assert.Equal("IT", student.Course);
            _mockRepository.Verify(r => r.Update(It.IsAny<Student>()), Times.Once);
        }

        [Fact]
        public async Task Update_ThrowsException_WhenStudentNotFound()
        {
            // Arrange
            var dto = new StudentDTOs { Name = "John", Email = "john@example.com", Age = 20, Course = "CS" };
            _mockRepository.Setup(r => r.GetById(It.IsAny<int>())).ReturnsAsync((Student)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.Update(999, dto));
            Assert.Equal("Student not found", ex.Message);
        }

        [Fact]
        public async Task Delete_DeletesStudent()
        {
            // Arrange
            var student = new Student { Id = 1, Name = "John Doe", Email = "john@example.com", Age = 20, Course = "CS" };
            _mockRepository.Setup(r => r.GetById(1)).ReturnsAsync(student);
            _mockRepository.Setup(r => r.Delete(It.IsAny<Student>())).Returns(Task.CompletedTask);

            // Act
            await _service.Delete(1);

            // Assert
            _mockRepository.Verify(r => r.Delete(student), Times.Once);
        }

        [Fact]
        public async Task Delete_ThrowsException_WhenStudentNotFound()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetById(It.IsAny<int>())).ReturnsAsync((Student)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.Delete(999));
            Assert.Equal("Student not found", ex.Message);
        }
    }
}
