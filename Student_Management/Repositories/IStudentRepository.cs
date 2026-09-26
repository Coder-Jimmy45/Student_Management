using Student_Management.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace Student_Management.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAll();
        Task<Student> GetById(int id);
        Task Add(Student student);
        Task Update(Student student);
        Task Delete(Student student);
    }
}
