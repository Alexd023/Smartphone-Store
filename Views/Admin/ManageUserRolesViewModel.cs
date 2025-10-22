namespace Smartphone_Store.ViewModels
{
    public class ManageUserRolesViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public List<UserRoleViewModel> Roles { get; set; } = new List<UserRoleViewModel>();
    }

    public class UserRoleViewModel
    {
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}