using Microsoft.AspNetCore.Identity;

namespace TaskTracker.Common.Exceptions;

public class BadRequestException : Exception
{
    public IEnumerable<IdentityError>? IdentityErrors { get; }

    public BadRequestException(string message, IEnumerable<IdentityError>? identityErrors = null)
        : base(message) => IdentityErrors = identityErrors;
}