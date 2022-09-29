using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.EmailSenders
{
    public interface IEmailSenderService
    {
        Task<BaseResponse> SendEmail(EmailSenderModel model);
    }
}
