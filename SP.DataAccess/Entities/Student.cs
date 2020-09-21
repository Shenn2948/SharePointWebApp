namespace SP.DataAccess.Entities
{
    public class Student
    {
        public Student()
        {
            __metadata = new { type = "SP.Data.StudentsListItem" };
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public object __metadata { get; }
    }
}