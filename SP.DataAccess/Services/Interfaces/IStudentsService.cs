using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SP.DataAccess.Entities;

namespace SP.DataAccess.Services.Interfaces
{
    public interface IStudentsService
    {
        Task<IEnumerable<Student>> GetAllAsync();

        Task<Student> GetOneAsync(string id);

        Task AddAsync(Student student);

        Task UpdateAsync(Student student);

        Task DeleteAsync(string id);
    }
}