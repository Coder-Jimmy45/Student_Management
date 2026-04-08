using Student_Management.DoTs;
using Student_Management.Models;
using System.Threading.Tasks;

namespace Student_Management.Services
{
    public interface IStudentService
    {
        Task<List<Students>> GetAll();
        Task<Students> Get(int id);
        Task Add(StudentDTOs dto);
        Task Update(int id, StudentDTOs dto);
        Task Delete(int id);
    }
}
