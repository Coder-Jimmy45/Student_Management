using Student_Management.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace Student_Management.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Students>> GetAll();
        Task<Students> GetById(int id);
        Task Add(Students student);
        Task Update(Students student);
        Task Delete(Students student);
    }
}
