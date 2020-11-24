using System;

namespace Web.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public Exception Exception { get; set; }
    }

    [Serializable]
    public class UnauthorizedApiException : Exception
    {
        public UnauthorizedApiException()
        {

        }

        public UnauthorizedApiException(string message) : base(message)
        {
        }
    }
}