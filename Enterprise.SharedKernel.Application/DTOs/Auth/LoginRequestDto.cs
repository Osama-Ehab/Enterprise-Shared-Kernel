namespace SharedKernel.Application.DTOs.Auth;

// Record is perfect for DTOs: Lightweight, Immutable, and Value-based equality
public record LoginRequestDto(string Username, string Password);