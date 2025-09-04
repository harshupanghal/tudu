using Tudu.Application.DTOs;
using Tudu.Domain.Entities;

namespace Tudu.Application.Mappers;

public static class UserMapper
    {
    public static AuthResponse ToAuthResponse(this User user, string message = "Success")
        {
        return new AuthResponse
            {
            Id = user.Id,
            UserName = user.UserName,
            ProfilePicturePath = user.ProfilePicturePath,
            Success = true,
            message = message
            };
        }
    }
