using CineTest.Service.Enums;
namespace CineTest.Service.Interface
{
    public interface IWebClientService
    {
        T HttpRequest<T>(
            string controller,
            string action,
            object body,
            HttpResquestMethod method = HttpResquestMethod.GET,
            AuthorizationType authType = AuthorizationType.NoAuth,
            string token = "",
           int timeoutMin = 0);
        string HttpRequest(
            string controller,
            string action,
            object body,
            HttpResquestMethod method = HttpResquestMethod.GET,
            AuthorizationType authType = AuthorizationType.NoAuth,
            string token = "",
           int timeoutMin = 0);
    }
}