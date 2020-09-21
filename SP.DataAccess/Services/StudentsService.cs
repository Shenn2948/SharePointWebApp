using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SP.DataAccess.Entities;
using SP.DataAccess.Services.Interfaces;

namespace SP.DataAccess.Services
{
    public class StudentsService : IStudentsService
    {
        private readonly Uri _uri;
        private const string SharePointStudentsListName = "Students";

        public StudentsService()
        {
            _uri = new Uri("https://epam.sharepoint.com/sites/epm-sp-projects/");
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            using (var client = new SharePointRestClient(_uri))
            {
                JToken studentsListItems = await client.GetListItemsAsync(SharePointStudentsListName);
                return studentsListItems.Select(x => new Student { Id = x["Id"]?.ToString(), Title = x["Title"]?.ToString() });
            }
        }

        public async Task<Student> GetOneAsync(string id)
        {
            using (var client = new SharePointRestClient(_uri))
            {
                JToken studentListItem = await client.GetListItemAsync(SharePointStudentsListName, id);
                return new Student { Id = studentListItem["Id"]?.ToString(), Title = studentListItem["Title"]?.ToString() };
            }
        }

        public async Task AddAsync(Student student)
        {
            using (var client = new SharePointRestClient(_uri))
            {
                await client.InsertListItemAsync(SharePointStudentsListName, new { student.__metadata, student.Title });
            }
        }

        public async Task UpdateAsync(Student student)
        {
            using (var client = new SharePointRestClient(_uri))
            {
                await client.UpdateListItemAsync(SharePointStudentsListName, student.Id, student);
            }
        }

        public async Task DeleteAsync(string id)
        {
            using (var client = new SharePointRestClient(_uri))
            {
                await client.DeleteListItemAsync(SharePointStudentsListName, id);
            }
        }
    }
}