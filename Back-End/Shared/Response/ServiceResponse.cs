using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shared.Response
{
    public class ServiceResponse<T>
    {
        public int ReturnValue { get; set; }
        public string ReturnContent { get; set; }
        public Stream ReturnStream { get; set; }
        public List<ServiceError> Errors { get; set; }
        public T Data { get; set; }
        public string MessageTitle { get; set; }
        public string MessageText { get; set; }
        public bool Reload { get; set; }
        public string RedirectUrl { get; set; }
        public bool Status => !Errors.Any();
        public List<string> HtmlViews { get; set; }
        public ServiceResponse()
        {
            Errors = new List<ServiceError>();
            HtmlViews = new List<string>();
        }

        public void AddError(Exception ex)
        {
            Errors.Add(new ServiceError(ex));
        }
        public void AddError(string errorCode)
        {
            Errors.Add(new ServiceError(errorCode));
        }
        public void AddError(string errorCode, string errorMessage)
        {
            Errors.Add(new ServiceError(errorCode, errorMessage));
        }
        public void AddError(string errorCode, string errorMessage, string errorDetail)
        {
            Errors.Add(new ServiceError(errorCode, errorMessage, errorDetail));
        }
        public void AddError(string errorCode, string errorMessage, string errorDetail, ServiceErrorLevel errorLevel)
        {
            Errors.Add(new ServiceError(errorCode, errorMessage, errorDetail, errorLevel));
        }
        public void AddError(ServiceError serviceError)
        {
            Errors.Add(serviceError);
        }
        public void AddErrors(List<ServiceError> serviceErrorList)
        {
            foreach (var e in serviceErrorList)
                Errors.Add(e);
        }
    }

    public class ServiceResponse : ServiceResponse<object> { }
}