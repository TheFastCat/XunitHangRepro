namespace Api
{
    using System.Web.Http;
    using System.Web.Http.Cors;

    /// <summary>
    /// Abstract class hiding internals of relay functionality to the Azure Service Bus
    /// </summary>
    [EnableCors("*", "*", "*")]
    public abstract class ClientApiBase :  ApiController
    {
    }
} 