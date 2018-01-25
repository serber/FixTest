using FixTest.Entities.Base;

namespace FixTest.Entities
{
    public class User : Entity<long>
    {
        public string Login { get; set; }

        public string Password { get; set; }
    }
}