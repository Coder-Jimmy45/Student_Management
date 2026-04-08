using Microsoft.EntityFrameworkCore;
using Student_Management.Data;
using Student_Management.Models;

namespace Student_Management.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Students>> GetAll()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Students> GetById(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task Add(Students student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Students student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Students student)
        {
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }
    }
}
