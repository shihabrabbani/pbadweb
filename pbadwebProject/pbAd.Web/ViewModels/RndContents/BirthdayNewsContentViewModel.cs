using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pbAd.Web.ViewModels.RndContents
{
    public class BirthdayNewsContentViewModel
    {
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "News Content")]
        public string NewsContent { get; set; }

        [Display(Name = "Image Content")]
        public IFormFile ImageContent { get; set; }
    }

    public class FileUploadViewModel
    {
        public FileUploadViewModel()
        {
            //this.RemoveUploadedFiles = new List<RemoveUploadedFileModel>();
        }
        public string Username { get; set; }
        public List<IFormFile> ImageContents { get; set; }
        public string RemoveUploadedFiles { get; set; }
    }   
}
