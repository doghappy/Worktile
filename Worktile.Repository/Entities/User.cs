namespace Worktile.Repository.Entities
{
    public class User
    {
        public string TeamId { get; set; }
        public string Uid { get; set; }
        public string ShortCode { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Mobile { get; set; }
        public string Avatar { get; set; }
        public string DepartmentId { get; set; }
        public bool IsDeleted { get; set; }

        public Team Team { get; set; }
    }

    public enum UserStatus
    {
        Ok = 1,
        Disabled,
        Init,
        Pending
    }

    public enum UserRoleType
    {
        Member = 1,
        Admin,
        Guest,
        Owner,
        Bot
    }
}
