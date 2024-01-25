﻿using System.Net;

namespace Bit.Tutorial08.Shared.Exceptions;

public class ConflictException : RestException
{
    public ConflictException()
        : this(nameof(AppStrings.ConflictException))
    {
    }

    public ConflictException(string message)
        : base(message)
    {
    }

    public ConflictException(string message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public ConflictException(LocalizedString message)
        : base(message)
    {
    }

    public ConflictException(LocalizedString message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
}
