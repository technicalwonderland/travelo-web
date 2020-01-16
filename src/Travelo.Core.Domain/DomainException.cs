using System;

namespace Travelo.Core.Domain
{
    public class DomainException : Exception
    {
        public DomainErrorCodes ErrorCode { get; private set; }
        public string Description { get; private set; }

        public DomainException(DomainErrorCodes errorCode, Exception innerException, string description,
            params object[] args) : base(FormatDescription(description, args))
        {
            ErrorCode = errorCode;
            Description = FormatDescription(description, args);
        }

        private static string FormatDescription(string description, params object[] args)
        {
            return string.Format(description, args);
        }
    }
}