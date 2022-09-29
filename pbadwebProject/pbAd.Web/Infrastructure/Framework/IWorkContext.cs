using pbAd.Data.Models;

namespace pbAd.Web.Infrastructure.Framework
{
    public interface IWorkContext
    {        
        /// <summary>
        /// Gets or sets the current logged in user.
        /// </summary>
        /// <value>
        /// The current logged in user.
        /// </value>
        User CurrentLoggedInUser { get; set; }       
    }
}
