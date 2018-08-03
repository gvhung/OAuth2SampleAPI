using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http.Filters;
using PatientJourney.GlobalConstants;
using Serilog;

namespace PatientSpectrum.WebAPI.Exception_Handler
{
    public class PatientSpectrumExceptionFilter : ExceptionFilterAttribute, IExceptionFilter
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                //Set the response status code to 500
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

                //construct response message based on the exception type
                if (actionExecutedContext.Exception is DbUpdateException)
                {
                    MethodBase method = actionExecutedContext.Exception.TargetSite;
                    string methodName = method == null ? string.Empty : method.Name;

                    response = new HttpResponseMessage()
                    {
                        Content = new StringContent(OAuthPJConstants.InvalidRequestMessage),
                        ReasonPhrase = string.Format(OAuthPJConstants.InvalidRequestReasonPharse, methodName)
                    };

                    actionExecutedContext.Response = response;
                    actionExecutedContext.ActionContext.Response = response;
                }
                else if (actionExecutedContext.Exception is DbEntityValidationException)
                {
                    MethodBase method = actionExecutedContext.Exception.TargetSite;
                    string methodName = method == null ? string.Empty : method.Name;

                    response = new HttpResponseMessage()
                    {
                        Content = new StringContent(OAuthPJConstants.InvalidRequestMessage),
                        ReasonPhrase = string.Format(OAuthPJConstants.InvalidRequestReasonPharse, methodName)
                    };

                    actionExecutedContext.Response = response;
                    actionExecutedContext.ActionContext.Response = response;
                }
                else if (actionExecutedContext.Exception is System.Web.Services.Protocols.SoapException)
                {
                    MethodBase method = actionExecutedContext.Exception.TargetSite;
                    string methodName = method == null ? string.Empty : method.Name;

                    response = new HttpResponseMessage()
                    {
                        Content = new StringContent(OAuthPJConstants.InvalidRequestMessage),
                        ReasonPhrase = string.Format(OAuthPJConstants.InvalidRequestReasonPharse, methodName)
                    };

                    actionExecutedContext.Response = response;
                    actionExecutedContext.ActionContext.Response = response;
                }
                else if (actionExecutedContext.Exception is NullReferenceException)
                {
                    MethodBase method = actionExecutedContext.Exception.TargetSite;
                    string methodName = method == null ? string.Empty : method.Name;

                    response = new HttpResponseMessage()
                    {
                        Content = new StringContent(OAuthPJConstants.NullReferenceExceptionMessage),
                        ReasonPhrase = string.Format(OAuthPJConstants.InvalidRequestReasonPharse, methodName)
                    };

                    actionExecutedContext.Response = response;
                    actionExecutedContext.ActionContext.Response = response;
                }
                else if (actionExecutedContext.Exception is FormatException)
                {
                    MethodBase method = actionExecutedContext.Exception.TargetSite;
                    string methodName = method == null ? string.Empty : method.Name;

                    response = new HttpResponseMessage()
                    {
                        Content = new StringContent(OAuthPJConstants.InvalidCastMessage),
                        ReasonPhrase = string.Format(OAuthPJConstants.InvalidCastReasonPharse, methodName)
                    };

                    actionExecutedContext.Response = response;
                    actionExecutedContext.ActionContext.Response = response;
                }
                else
                {
                    MethodBase method = actionExecutedContext.Exception.TargetSite;
                    string methodName = method == null ? string.Empty : method.Name;

                    response = new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent(OAuthPJConstants.UnknownExceptionMessage),
                        ReasonPhrase = string.Format(OAuthPJConstants.UnknownExceptionReasonPharse, methodName)
                    };

                    actionExecutedContext.Response = response;
                    actionExecutedContext.ActionContext.Response = response;
                }
            }
            catch(Exception ex)
            {
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message + ex.StackTrace.ToString()),
                    ReasonPhrase = ex.Message.Replace(Environment.NewLine, "")
                };
                actionExecutedContext.Response = response;
                actionExecutedContext.ActionContext.Response = response;
            }
            finally
            {
                //log error finally
                Log.Fatal(actionExecutedContext.Exception, OAuthPJConstants.UnhandledErrorMessage);
            }
        }
    }
}