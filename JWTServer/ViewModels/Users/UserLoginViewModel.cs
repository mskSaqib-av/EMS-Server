namespace JWTServer.ViewModels.Users
{
    public class UserLoginBaseModel
    {
        public string UserName { get; set; }
        public string Code { get; set; }
        public string HashPassword { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? DOB { get; set; }
        public bool Active { get; set; }
        public string? Action { get; set; }
    }

    public class UserLoginViewModel : UserLoginBaseModel
    {
        public Guid Id { get; set; }
    }

    public class UserLoginViewByIdModel : UserLoginBaseModel
    {
        public Guid Id { get; set; }
    }
    public class UserLoginAddModel : UserLoginBaseModel
    {
        public Guid Id { get; set; }
        public string HashPassword { get; set; }

    }

    public class UserLoginUpdateModel : UserLoginBaseModel
    {
        public Guid Id { get; set; }
        public string? HashPassword { get; set; }
    }

    public class UserLoginDeleteModel : UserLoginBaseModel
    {
        public Guid Id { get; set; }
    }
}
