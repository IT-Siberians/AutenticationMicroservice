﻿namespace Services.Contracts;

public class CreateUserDto
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
}