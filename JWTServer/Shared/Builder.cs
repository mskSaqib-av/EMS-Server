using JWTServer.Utilities;
using JWTServer.Manager;
using Microsoft.AspNetCore.Identity;
using JWTServer.Manager.Configuration;


namespace JWTServer.Shared
{
    public static class Builder
    {
        public static IManager? MakeManagerClass(Enums.ModuleClassName ClassName, AppDbContext _context)
        {
            switch (ClassName)
            {
                case Enums.ModuleClassName.Users:
                    return new UserManager(_context);
                
                default:
                    return null;
            }
        }
    }
}
