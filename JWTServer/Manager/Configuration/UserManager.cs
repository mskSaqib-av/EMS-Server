using JWTServer.Model;
using JWTServer.Shared;
using JWTServer.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JWTServer.Manager.Configuration
{
    public class UserManager : IManager
    {
        private readonly AppDbContext _context;
        public UserManager(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse> GetDataAsync(ClaimsPrincipal _User)
        {
            var apiResponse = new ApiResponse();
            try
            {
                var _Table = await _context.Users
                .Where(a => a.Action != Enums.Operations.D.ToString()).OrderBy(o => o.UserName).ToListAsync();

                if (_Table == null || _Table.Count == 0)
                {
                    apiResponse.statusCode = StatusCodes.Status404NotFound.ToString(); ;
                    apiResponse.message = "Record not found";
                    return apiResponse;
                }

                apiResponse.statusCode = StatusCodes.Status200OK.ToString();
                apiResponse.data = _Table;
                return apiResponse;
            }
            catch (Exception e)
            {
                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = innerexp;
                return apiResponse;
            }
        }

        public async Task<ApiResponse> GetDataByIdAsync(Guid _Id, ClaimsPrincipal _User)
        {
            var apiResponse = new ApiResponse();
            try
            {

                var _Table = await _context.Users
                .Where(a => a.Id == _Id && a.Action != Enums.Operations.D.ToString()).FirstOrDefaultAsync();

                if (_Table == null)
                {
                    apiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    apiResponse.message = "Record not found";
                    return apiResponse;
                }
                apiResponse.statusCode = StatusCodes.Status200OK.ToString();
                apiResponse.data = _Table;
                return apiResponse;
            }
            catch (Exception e)
            {
                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = innerexp;
                return apiResponse;
            }
        }
        public async Task<ApiResponse> AddAsync(object model, ClaimsPrincipal _User)
        {
            var apiResponse = new ApiResponse();
            try
            {

                var _UserId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserId.ToString())?.Value.ToString();
                var _model = (UserLogin)model;
                string error = "";
                bool _EmailExists = _context.Users.Any(rec => rec.Email.Trim().ToLower().Equals(_model.Email.Trim().ToLower()) && rec.Action != Enums.Operations.D.ToString());
                bool _ContactExists = _context.Users.Any(rec => rec.Phone.Trim().ToLower().Equals(_model.Phone.Trim().ToLower()) && rec.Action != Enums.Operations.D.ToString());
                bool _Code = _context.Users.Any(rec => rec.Code.Trim().ToLower().Equals(_model.Code.Trim().ToLower()) && rec.Action != Enums.Operations.D.ToString());

                if (_Code)
                {
                    error = "Applogoies, This Code is Already Generated, Please Regenrate Code";
                    apiResponse.statusCode = StatusCodes.Status409Conflict.ToString();
                    apiResponse.message = error;
                    return apiResponse;
                }

                if (_EmailExists)
                {
                    error = error + "Email";
                }

                if (_ContactExists)
                {
                    error = error + "Phone Number";
                }

                if (_EmailExists || _ContactExists)
                {
                    apiResponse.statusCode = StatusCodes.Status409Conflict.ToString();
                    apiResponse.message = error + " already exist";
                    return apiResponse;
                }

                _model.created_by = Guid.Parse(_UserId);
                _model.created_at = DateTime.Now;
                _model.Action = Enums.Operations.A.ToString();

                await _context.Users.AddAsync(_model);
                _context.SaveChanges();

                apiResponse.statusCode = StatusCodes.Status200OK.ToString();
                apiResponse.message = _model.UserName + " has been added successfully";
                return apiResponse;

            }
            catch (DbUpdateException _exceptionDb)
            {

                string innerexp = _exceptionDb.InnerException == null ? _exceptionDb.Message : _exceptionDb.Message + " Inner Error : " + _exceptionDb.InnerException.ToString();
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = innerexp;
                return apiResponse;

            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = innerexp;
                return apiResponse;
            }
        }
        public async Task<ApiResponse> UpdateAsync(object model, ClaimsPrincipal _User)
        {
            var apiResponse = new ApiResponse();
            try
            {

                var _UserId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserId.ToString())?.Value.ToString();
                var _model = (UserLogin)model;
                string error = "";
                bool _EmailExists = _context.Users.Any(rec => rec.Email.Trim().ToLower().Equals(_model.Email.Trim().ToLower()) && rec.Id != _model.Id && rec.Action != Enums.Operations.D.ToString());
                bool _ContactExists = _context.Users.Any(rec => rec.Phone.Trim().ToLower().Equals(_model.Phone.Trim().ToLower()) && rec.Id != _model.Id && rec.Action != Enums.Operations.D.ToString());
                bool _Code = _context.Users.Any(rec => rec.Code.Trim().ToLower().Equals(_model.Code.Trim().ToLower()) && rec.Id != _model.Id && rec.Action != Enums.Operations.D.ToString());


                if (_Code)
                {
                    error = "Applogoies, This Code is Already Generated, Please Regenrate Code";
                    apiResponse.statusCode = StatusCodes.Status409Conflict.ToString();
                    apiResponse.message = error;
                    return apiResponse;
                }

                if (_EmailExists)
                {
                    error = error + "Email";
                }

                if (_ContactExists)
                {
                    error = error + "Phone Number";
                }

                if (_EmailExists || _ContactExists)
                {
                    apiResponse.statusCode = StatusCodes.Status409Conflict.ToString();
                    apiResponse.message = error + " already exist";
                    return apiResponse;
                }
                var result = _context.Users.Where(a => a.Id == _model.Id && a.Action != Enums.Operations.D.ToString()).FirstOrDefault();
                if (result == null)
                {
                    apiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    apiResponse.message = "Record not found ";
                    return apiResponse;
                }

                result.Code = _model.Code;
                result.UserName = _model.UserName;
                result.Phone= _model.Phone;
                result.Email = _model.Email;
                result.HashPassword = _model.HashPassword;
                result.Active = _model.Active;
                result.updated_by = Guid.Parse(_UserId);
                result.Action = Enums.Operations.E.ToString();
                result.updated_at = DateTime.Now;

                await _context.SaveChangesAsync();

                apiResponse.statusCode = StatusCodes.Status200OK.ToString();
                apiResponse.message = result.UserName + " has been updated successfully";
                return apiResponse;

            }
            catch (DbUpdateException _exceptionDb)
            {

                string innerexp = _exceptionDb.InnerException == null ? _exceptionDb.Message : _exceptionDb.Message + " Inner Error : " + _exceptionDb.InnerException.ToString();
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = innerexp;
                return apiResponse;

            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = innerexp;
                return apiResponse;
            }
        }
        public async Task<ApiResponse> DeleteAsync(Guid _Id, ClaimsPrincipal _User)
        {
            var apiResponse = new ApiResponse();
            try
            {

                var _UserId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserId.ToString())?.Value.ToString();
                var result = _context.Users.Where(a => a.Id == _Id && a.Action != Enums.Operations.D.ToString()).FirstOrDefault();
                if (result == null)
                {
                    apiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    apiResponse.message = "Record not found ";
                    return apiResponse;
                }

                result.deleted_by = Guid.Parse(_UserId);
                result.Action = Enums.Operations.D.ToString();
                result.deleted_at = DateTime.Now;

                await _context.SaveChangesAsync();

                apiResponse.statusCode = StatusCodes.Status200OK.ToString();
                apiResponse.message = result.UserName + " has been deleted successfully";
                return apiResponse;

            }
            catch (DbUpdateException _exceptionDb)
            {

                string innerexp = _exceptionDb.InnerException == null ? _exceptionDb.Message : _exceptionDb.Message + " Inner Error : " + _exceptionDb.InnerException.ToString();
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = innerexp;
                return apiResponse;

            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = innerexp;
                return apiResponse;
            }
        }

        
    }
}
