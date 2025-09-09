namespace JWTServer.Shared
{
    public class Enums {
        public enum Operations {
                A,
                E,
                D,
                S,
                U,
                I,
                O,
                M

            }
        
        public enum ModuleClassName
        {
            //Configiration
            
            Company,
            Branch,
            MenuModule,
            MenuCategory,
            MenuSubCategory,
            UserRole,
            Users,

            // PAYROLL

            // ACCOUNTS
            Dashboard,
        }

        public enum UserRoles{
            Crew,
            Manager,
            Admin,
            Requester,

        }

        public enum Status{
            Pending,
            Completed,
            Assigned,
            Cancel,

        }

        public enum Roles
        {
            Role,
            SuperAdmin

        }

        public enum Voucher
        {
            G,
            NG,
            GD,

        }
        public enum Approved
        {
            A,
            NA,

        }

        public enum Transaction
        {
            C,
            D,

        }

        public enum Misc
        {
            UserId,
            UserName,
            CompanyId,
            CompanyName,
            BranchId,
            BranchName,
            Email,
            Key,
            Role,

        }

        public enum SSBTYPE
        {
            FRESHER,
        }
    }
}