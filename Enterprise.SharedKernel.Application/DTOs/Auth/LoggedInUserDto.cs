
namespace Enterprise.SharedKernel.Application.DTOs.Auth
{
    public record LoggedInUserDto(
      int Id,
      string FullName,
      string Username,
      long Permissions,
      string LanguagePreference
  );
}
