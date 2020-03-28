
using DDHCenter.Core.Models;
namespace DDHCenter.Core.Utils
{
    public class ViewMessage
    {
        public static void Send(string message)
        {
            Mediator.Mediator.NotifyColleagues("APageHashErrors", new PageErrorsAndMessage
            {
                HasErrors = true,
                Message = message
            });
        }
    }
}
