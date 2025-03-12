namespace OnlineExamSystem.PL.ViewModels.Identity
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        public string RoleName { get; set; } = null!;
        public RoleViewModel()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
