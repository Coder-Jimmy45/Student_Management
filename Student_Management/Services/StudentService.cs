using Student_Management.DTOs;
using Student_Management.Models;
using Student_Management.Repositories;

namespace Student_Management.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repo;

        public StudentService(IStudentRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Student>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<Student> Get(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task Add(StudentDTOs dto)
        {
            var student = new Student
            {
                Name = dto.Name,
                Email = dto.Email,
                Age = dto.Age,
                Course = dto.Course,
                CreatedDate = DateTime.Now
            };

            await _repo.Add(student);
        }

        public async Task Update(int id, StudentDTOs dto)
        {
            var student = await _repo.GetById(id);
            if (student == null) throw new Exception("Student not found");

            student.Name = dto.Name;
            student.Email = dto.Email;
            student.Age = dto.Age;
            student.Course = dto.Course;

            await _repo.Update(student);
        }

        public async Task Delete(int id)
        {
            var student = await _repo.GetById(id);
            if (student == null) throw new Exception("Student not found");

            await _repo.Delete(student);
        }
    }
}
