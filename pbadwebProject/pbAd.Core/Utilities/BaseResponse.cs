using System;
using System.Collections.Generic;
using System.Text;

namespace pbAd.Core.Utilities
{
    public class Response<T> : BaseResponse
    {
        public T Result { get; set; }
    }

    public class BaseResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }          
    }
}
