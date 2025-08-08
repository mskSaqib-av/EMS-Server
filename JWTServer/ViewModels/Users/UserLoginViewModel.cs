namespace JWTServer.ViewModels.Users
{
    public class UserLoginBaseModel
    {
        public string UserName { get; set; }
        public string? Code { get; set; }
        public string HashPassword { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? DOB { get; set; }
    }

    public class UserLoginViewModel : UserLoginBaseModel
    {
        public string UserName { get; set; }
        public string? Code { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? DOB { get; set; }
    }

    public class UserLoginViewByIdModel : UserLoginBaseModel
    {
        public string UserName { get; set; }
        public string? Code { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? DOB { get; set; }
    }
}
