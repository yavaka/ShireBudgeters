using Microsoft.AspNetCore.Identity;

namespace ShireBudgeters.DA.Models;

/// <summary>
/// User model that extends IdentityUser with audit properties.
/// </summary>
public class UserModel : IdentityUser, IAuditable
{
    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    public required string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the user.
    /// </summary>
    public required string LastName { get; set; }

    /// <summary>
    /// Gets or sets the status of the user.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the last login date of the user.
    /// </summary>
    public DateTime? LastLoginDate { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who created the record.
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the record was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who last modified the record.
    /// </summary>
    public string? ModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the record was last modified.
    /// </summary>
    public DateTime? ModifiedDate { get; set; }
}
