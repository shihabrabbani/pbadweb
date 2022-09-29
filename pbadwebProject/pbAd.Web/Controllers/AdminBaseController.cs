#region Usings

using pbAd.Core.Utilities;
using pbAd.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

#endregion

namespace pbAd.Web.Controllers
{
    [Authorize]
    public class AdminBaseController : Controller
    {
        public User CurrentLoginUser
        {
            get
            {
                var userModel = new User();

                if (!User.Identity.IsAuthenticated)
                    return userModel;

                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userClaims = claimsIdentity.Claims;

                if (userClaims == null)
                    return null;

                var userGroup = Convert.ToInt32(claimsIdentity.Claims.FirstOrDefault(f => f.Type == "GroupId").Value);

                userModel.UserId = Convert.ToInt32(claimsIdentity.Claims.FirstOrDefault(f => f.Type == "UserId").Value);
                userModel.RoleId = Convert.ToInt32(claimsIdentity.Claims.FirstOrDefault(f => f.Type == "RoleId").Value);
                userModel.UserName = claimsIdentity.Claims.FirstOrDefault(f => f.Type == "UserName").Value;
                userModel.FullName = claimsIdentity.Claims.FirstOrDefault(f => f.Type == "FullName").Value;
                userModel.MobileNo = claimsIdentity.Claims.FirstOrDefault(f => f.Type == "MobileNo").Value;
                userModel.Email = claimsIdentity.Claims.FirstOrDefault(f => f.Type == "Email").Value;
                userModel.EditionId = Convert.ToInt32(claimsIdentity.Claims.FirstOrDefault(f => f.Type == "EditionId").Value);
                userModel.DistrictId = Convert.ToInt32(claimsIdentity.Claims.FirstOrDefault(f => f.Type == "DistrictId").Value);
                userModel.UpazillaId = Convert.ToInt32(claimsIdentity.Claims.FirstOrDefault(f => f.Type == "UpazillaId").Value);
                userModel.DefaultCommission = Convert.ToDecimal(claimsIdentity.Claims.FirstOrDefault(f => f.Type == "DefaultCommission").Value);
                userModel.UserGroup = userGroup;
                userModel.IsCRMUser = userGroup == UserGroupConstants.CRM_User;
                userModel.IsCorrespondentUser = userGroup == UserGroupConstants.Correspondent;

                return userModel;
            }
        }
    }
}
