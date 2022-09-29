using pbAd.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbAd.Web.ViewModels.Account
{
    public class AuthenticationSchemeModel
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }

        public List<AuthenticationSchemeModel> GetAuthenticationSchemes()
        {
            return new List<AuthenticationSchemeModel>() { new AuthenticationSchemeModel { DisplayName = SocialLoginProviderConstants.Google, Name = SocialLoginProviderConstants.Google }, new AuthenticationSchemeModel { DisplayName = SocialLoginProviderConstants.Facebook, Name = SocialLoginProviderConstants.Facebook } };
        }
    }
}
