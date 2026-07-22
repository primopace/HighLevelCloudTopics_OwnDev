using System;
using System.Collections.Generic;
using System.Text;

namespace Assessment.Domain.Common
{
    public class BusinessException : Exception
    {
        public string ErrorCode { get; }

        public BusinessException(string message, string errorCode = "BUSINESS_RULE_VIOLATION")
            : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
