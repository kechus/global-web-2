namespace GlobalWeb.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RegisterNumber { get; set; }
        public string Carrer { get; set; }
        public string ProfilePic { get; set; }
        public bool IsDeleted { get; set; } = false; 
    }
}
