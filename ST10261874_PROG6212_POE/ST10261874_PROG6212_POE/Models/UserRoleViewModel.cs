namespace ST10261874_PROG6212_POE.Models
{
    public class UserRoleViewModel
    {
        public string Email { get; set; }
        public IList<string> Roles { get; set; }

        public List<string> AvailableRoles { get; set; }
    }
}


