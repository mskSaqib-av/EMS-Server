using JWTServer.Manager;
using JWTServer.Model;
using JWTServer.Shared;
using JWTServer.Utilities;
using JWTServer.ViewModels.Users;
using System.Security.Claims;

namespace JWTServer.Processor.Configuration
{
    public class UserProcessor : IProcessor<UserLoginBaseModel>
    {
        private AppDbContext _context;
        private IManager? _manager;
        public UserProcessor(AppDbContext context)
        {
            _context = context;
            _manager = Builder.MakeManagerClass(Enums.ModuleClassName.Users, _context);
        }

        public async Task<ApiResponse> ProcessGet(Guid MenuId, ClaimsPrincipal _User)
        {
            ApiResponse apiResponse = new ApiResponse();
            if (_manager != null)
            {
                var response = await _manager.GetDataAsync(_User);
                var _Table = response.data as IEnumerable<UserLogin>;
                var result = (from ViewTable in _Table
                              select new UserLoginViewModel
                              {
                                  Id = ViewTable.Id,
                                  Code = ViewTable.Code,
                                  UserName = ViewTable.UserName,
                                  Phone = ViewTable.Phone,
                                  Email = ViewTable.Email,
                                  Active = ViewTable.Active,
                              }).ToList();
                response.data = result;
                return response;
            }
            apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
            apiResponse.message = "Invalid Class";
            return apiResponse;
        }

        public async Task<ApiResponse> ProcessGetById(Guid _Id, Guid _MenuId, ClaimsPrincipal _User)
        {
            ApiResponse apiResponse = new ApiResponse();
            if (_manager != null)
            {
                var response = await _manager.GetDataByIdAsync(_Id, _User);
                if (Convert.ToInt32(response.statusCode) == 200)
                {
                    var _Table = response.data as UserLogin;
                    var _ViewModel = new UserLoginViewByIdModel
                    {
                        Id = _Table.Id,
                        Code = _Table.Code,
                        UserName = _Table.UserName,
                        Phone = _Table.Phone,
                        //HashPassword = SecurityHelper.DecryptString("1234567890123456", _Table.HashPassword),
                        Email = _Table.Email,
                        Active = _Table.Active
                    };
                    response.data = _ViewModel;
                }
                return response;
            }
            apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
            apiResponse.message = "Invalid Class";
            return apiResponse;
        }

        public async Task<ApiResponse> ProcessPost(object _Usermodel, ClaimsPrincipal _User)
        {
            ApiResponse apiResponse = new ApiResponse();
            var User = (UserLoginAddModel)_Usermodel;

            if (_manager != null)
            {
                var _Table = new UserLogin
                {
                    Code = User.Code,
                    UserName = User.UserName,
                    Phone = User.Phone,
                    //HashPassword = SecurityHelper.EncryptString("1234567890123456", User.HashPassword),
                    Email = User.Email,
                    Active = User.Active,
                };
                return await _manager.AddAsync(_Table, _User);
            }
            apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
            apiResponse.message = "Invalid Class";
            return apiResponse;
        }

        public async Task<ApiResponse> ProcessPut(object _Usermodel, ClaimsPrincipal _User)
        {
            ApiResponse apiResponse = new ApiResponse();
            var User = (UserLoginUpdateModel)_Usermodel;
            if (_manager != null)
            {
                var _Table = new UserLogin
                {
                    Id = User.Id,
                    Code = User.Code,
                    UserName = User.UserName,
                    Phone = User.Phone,
                    //HashPassword = SecurityHelper.EncryptString("1234567890123456", User.HashPassword),
                    Email = User.Email,
                    Active = User.Active,
                };
                return await _manager.UpdateAsync(_Table, _User);

            }
            apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
            apiResponse.message = "Invalid Class";
            return apiResponse;
        }

        public async Task<ApiResponse> ProcessDelete(Guid _Id, ClaimsPrincipal _User)
        {
            ApiResponse apiResponse = new ApiResponse();
            if (_manager != null)
            {
                return await _manager.DeleteAsync(_Id, _User);
            }
            apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
            apiResponse.message = "Invalid Class";
            return apiResponse;
        }
    }
}
