using Student_Management.DTOs;
using Student_Management.Models;
using System.Threading.Tasks;

namespace Student_Management.Services
{
    public interface IStudentService
    {
        Task<List<Student>> GetAll();
        Task<Student> Get(int id);
        Task Add(StudentDTOs dto);
        Task Update(int id, StudentDTOs dto);
        Task Delete(int id);
    }
}
