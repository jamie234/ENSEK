using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Account
{
    [Key]
    public int AccountId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}
