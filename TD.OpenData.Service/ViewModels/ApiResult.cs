using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD.OpenData.Service.ViewModels
{
    public class ApiResult
    {
        public object data { get; set; }
        public int total { get; set; }
        public ErrorResult error { get; set; }

        public ApiResult()
        {
            data = null;
            total = 0;
            error = new ErrorResult();
        }
    }

    public class ErrorResult
    {
        public string userMessage { get; set; }
        public string internalMessage { get; set; }
        public int code { get; set; }

        public ErrorResult()
        {
            userMessage = string.Empty;
            internalMessage = string.Empty;
            code = 200;
        }
    }
}
