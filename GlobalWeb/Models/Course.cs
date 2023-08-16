namespace GlobalWeb.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Period { get; set; }
        public string TeacherName { get; set; }
        public int PartialsName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
