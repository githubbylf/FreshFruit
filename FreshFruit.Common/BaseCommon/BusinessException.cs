using System;

namespace FreshFruit.Common.BaseCommon
{
    public class BusinessException : SystemException
    {
        public BusinessException(string message) : base(message)
        {
        }
    }
}
