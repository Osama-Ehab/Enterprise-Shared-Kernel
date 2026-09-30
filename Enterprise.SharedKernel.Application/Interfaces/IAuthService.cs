
using Enterprise.SharedKernel.Application.DTOs.Auth;
using Enterprise.SharedKernel.Models;
using SharedKernel.Application.DTOs.Auth;

namespace Enterprise.SharedKernel.Application.Interfaces;
public interface IAuthService
{
   Task<Result<LoggedInUserDto>> LoginAsync(LoginRequestDto request);
}