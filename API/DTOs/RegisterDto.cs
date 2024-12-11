using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    [Required]
    [MaxLength(100)]
    [MinLength(5)]
    public required string UserName { get; set; }
    
    [Required]
    public required string Password { get; set; }
}
