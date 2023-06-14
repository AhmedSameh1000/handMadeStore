using Identity.Models.DTOs;

namespace Identity.Models.AllRolesViewModel
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        public List<RolesViewModels> Roles { get; set; }
    }
}