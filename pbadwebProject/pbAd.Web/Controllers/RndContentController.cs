using CoreHtmlToImage;
using pbAd.Web.ViewModels.RndContents;
using Microsoft.AspNetCore.Mvc;
using SelectPdf;
using System;
using System.IO;
using System.Threading.Tasks;

namespace pbAd.Web.Controllers
{
    public class RndContentController : Controller
    {
        
        public RndContentController()
        {
           
        }

        public IActionResult UploadFiles()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadFiles(FileUploadViewModel model)
        {
            return View();
        }


        public async Task<IActionResult> BirthdayNews()
        {
            var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}";

            return View();
        }

        [HttpPost]
        public IActionResult BirthdayNews(BirthdayNewsContentViewModel model)
        {
            return View();
        }
        [HttpGet]

        public IActionResult CreatePDF(string html)
        {
            return View();           
        }

        public IActionResult Invoice(string html)
        {
            return View();
        }

        public IActionResult HtmlContent()
        {
            var myHmlt = @"
                        <div class='row' style='margin-top:30px;'>
                            <div class='col-md-10 offset-md-2'>
                                <div class='row'>
                                    <div class='col-sm-6' style='align-self:center;font-family:cursive'>
                                        <div class='logo'>
                                            <h1>Shorobor</h1>
                                            <img src='http://localhost:50147/img/Classified-Text-6.jpg'>
                                        </div>
                                    </div>
                                    <div class='col-sm-6'>
                                        <p style='margin-bottom:3px;'>Shorobor</p>
                                        <p style='margin-bottom:3px;'>+8801786545375</p>
                                        <p style='margin-bottom:3px;'>abcd@gmail.com</p>
                                        <p style='margin-bottom:3px;'>www.pbAd.com</p>
                                    </div>
<img src='http://localhost:50147/img/ad.jpg'>
                                </div>
                            </div>
                        </div>


                        <div class='row' style='margin-top:50px;'>
                            <div class='col-md-10 offset-md-1'>
                                <h2 style='margin-bottom:30px;'>Invoice</h2>
                            </div>
    
    
                            <div class='col-md-10 offset-md-1'>
                                <div class='row'>
                                    <div class='col-sm-6'>
                                        <p style='margin-bottom:3px;'>Rejwanul Reja</p>
                                        <p style='margin-bottom:3px;'>House No.</p>
                                        <p style='margin-bottom:3px;'>Mirpur 1</p>
                                        <p style='margin-bottom:3px;'>Dhaka</p>
                                        <p style='margin-bottom:3px;'>reja@gmai.com</p>
                                        <p style='margin-bottom:3px;'>+8801786545453</p>
                                    </div>
                                    <div class='col-sm-6' style='display:flex;justify-content:space-around;'>
                                        <div>
                                            <p style='margin-bottom:3px;'>Invoice Number:</p>
                                            <p style='margin-bottom:3px;'>Invoice Date:</p>
                                            <p style='margin-bottom:3px;'>Order number:</p>
                                            <p style='margin-bottom:3px;'>Order Date:</p>
                                            <p style='margin-bottom:3px;'>Payment Mathod:</p>
                                        </div>
                                        <div>
                                            <p style='margin-bottom:3px;'>378675</p>
                                            <p style='margin-bottom:3px;'>02-feb-2021</p>
                                            <p style='margin-bottom:3px;'>262572</p>
                                            <p style='margin-bottom:3px;'>02-feb-2021</p>
                                            <p style='margin-bottom:3px;'>Cash on Delivery</p>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>

                        <div class='row' style='margin-top:20px;'>
                            <div class='col-md-10 offset-md-1'>
                                <div class='row'>
                                    <table class='table'>
                                        <thead class='thead-dark'>
                                            <tr>
                                                <th scope='col'>Product</th>
                                                <th scope='col'>Quantity</th>
                                                <th scope='col'>Price</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <th style='font-weight:normal;'>Zaitun - 225g <br />.225kg</th>
                                                <td>1</td>
                                                <td>BDT 300.000</td>
                                            </tr>
                                            <tr>
                                                <th style='font-weight:normal;'>Zaitun - 225g <br />.225kg</th>
                                                <td>1</td>
                                                <td>BDT 300.000</td>
                                            </tr>
                                            <tr>
                                                <th  style='font-weight:normal;'>Samna (pure cow gee) - 225g<br /> .225kg</th>
                                                <td>1</td>
                                                <td>BDT 300.000</td>
                                            </tr>
                                        </tbody>
                                    </table>

                                </div>
                            </div>
                        </div>

                        <div class='row' style='margin-top:10px;'>
                            <div class='col-md-10 offset-md-1'>
                                <div class='row'>           
                                    <div class='col-md-4 offset-md-7'>
                                        <div style='display: inline-flex;margin-top: 10px;width: 100%;'>
                                            <p style='margin-bottom:3px;width:120px;'>Sub total:</p>
                                            <p style='margin-bottom:3px;'>BDT 900</p>

                                        </div>
                                    </div>

                                    <div class='col-md-4 offset-md-7'>
                                        <div style='display: inline-flex;width: 100%;'>
                                            <p style='margin-bottom:3px;width:120px;'>Shipping:</p>
                                            <p style='margin-bottom:3px;'>BDT 900</p>

                                        </div>
                                    </div>

                                    <div class='col-md-4 offset-md-7'>
                                        <div style='padding: 4px 3px;border-top: 2px solid black;display: inline-flex;margin-top: 10px;width: 100%;border-bottom: 2px solid black;'>
                                            <p style='margin-bottom:3px;font-weight:bold;width:120px;'>total:</p>
                                            <p style='margin-bottom:3px;font-weight:bold;'>BDT 900</p>

                                        </div>
                                    </div>


                                </div>
                                </div>
                        </div>
                        ";

            myHmlt = @"
                <!DOCTYPE html>
                <html>
                <body style='font-family: sans-serif;'>
                  <div style='max-width:940px;width: 100%;padding-right: 15px;padding-left: 15px;margin-right: auto;margin-left: auto;'>
                    <div style='width: 100%;display: flex;'>
                      <div style='width: 60%;padding:0px 15px;align-self: center;'>
                        <h1 style='font-family: cursive;font-size: 35px;font-weight: normal;'>Shorobor
                            <img src='http://localhost:50147/img/Classified-Text-6.jpg'>
                        </h1>
                      </div>
                      <div style='width: 40%;padding: 0px 15px;'>
                        <h3 style='margin-bottom: 8px;'>Shorobor</h3>
                        <p style='margin-bottom: 5px;margin-top:0px;'>01768767654</p>
                        <p style='margin-bottom: 5px;margin-top:0px;'>abc@gmail.com</p>
                        <p style='margin-bottom: 5px;margin-top:0px;'>www.shorobor.com</p>
                      </div>
                    </div>

                    <div>
                      <h2 style='margin-top: 70px;font-family: sans-serif;padding: 0px 15px;'>INVOICE</h2>
                    </div>

                    <div style='width: 100%;display: flex;'>
                      <div style='width: 60%;padding:0px 15px;align-self: center;'>
                        <p style='margin-bottom:5px;'>Rejwanul Reja</p>
                        <p style='margin-bottom:5px;'>House No.</p>
                        <p style='margin-bottom:5px;'>Mirpur 1</p>
                        <p style='margin-bottom:5px;'>Dhaka</p>
                        <p style='margin-bottom:5px;'>reja@gmai.com</p>
                        <p style='margin-bottom:5px;'>+8801786545453</p>
                      </div>
                      <div style='width: 40%;padding: 0px 15px;display: flex;justify-content: space-between;'>
                        <div>
                          <p style='margin-bottom:5px;'>Invoice Number:</p>
                          <p style='margin-bottom:5px;'>Invoice Date:</p>
                          <p style='margin-bottom:5px;'>Order number:</p>
                          <p style='margin-bottom:5px;'>Order Date:</p>
                          <p style='margin-bottom:5px;'>Payment Mathod:</p>
                        </div>

                        <div>
                          <p style='margin-bottom:5px;'>378675</p>
                          <p style='margin-bottom:5px;'>02-feb-2021</p>
                          <p style='margin-bottom:5px;'>262572</p>
                          <p style='margin-bottom:5px;'>02-feb-2021</p>
                          <p style='margin-bottom:5px;'>Cash on Delivery</p>
                        </div>
                      </div>
                    </div>

                    <table style='width:100%;margin-top: 40px;border-collapse: collapse;'>
                      <thead>
                        <tr style='background-color: #343a40;'>
                          <th style='padding:15px 5px;text-align:left;color:white;'>Product</th>
                          <th style='padding:15px 5px;text-align:left;color:white;'>Quantity</th>
                          <th style='padding:15px 5px;text-align:left;color:white;'>price</th>
                        </tr>
                      </thead>
                      <tr>
                        <td style='padding: 12px 5px;'>Samna (pure cow gee) - 225g<br /> .225kg</td>
                        <td style='padding: 12px 5px;'>1</td>
                        <td style='padding: 12px 5px;'>BDT 300.000</td>
                      </tr>
                      <tr>
                        <td style='padding: 12px 5px;'>Samna (pure cow gee) - 225g<br /> .225kg</td>
                        <td style='padding: 12px 5px;'>1</td>
                        <td style='padding: 12px 5px;'>BDT 300.000</td>
                      </tr>
                      <tr>
                        <td style='padding: 12px 5px;'>Zaitun - 225g <br />.225kg</td>
                        <td style='padding: 12px 5px;'>1</td>
                        <td style='padding: 12px 5px;'>BDT 300.000</td>
                      </tr>
                    </table>

                    <div style='width: 100%;display: flex;'>
                      <div style='width: 60%;padding:0px 15px;align-self: center;'>
                      </div>

                      <div style='width: 40%;padding: 0px 15px;'>

                        <div style='display: inline-flex;margin-top: 10px;width: 100%;'>
                          <p style='margin-bottom:3px;width:120px;'>Sub total:</p>
                          <p style='margin-bottom:3px;'>BDT 900</p>
                        </div>

                        <div style='display: inline-flex;width: 100%;'>
                          <p style='margin-bottom:3px;width:120px;'>Shipping:</p>
                          <p style='margin-bottom:3px;'>BDT 900</p>
                        </div>

                        <div style='padding: 1px 3px;border-top: 2px solid black;display: inline-flex;margin-top: 15px;width: 100%;border-bottom: 2px solid black;'>
                          <p style='margin:10px 0px;font-weight:bold;width:120px;'>TOTAL:</p>
                          <p style='margin:10px 0px;font-weight:bold;'>BDT 900</p>
                          <p style='margin:10px 0px;font-weight:bold;'>শুভ জন্মদিন রাইফ রাইদ!</p>
                        </div>

                      </div>
                    </div>


                  </div>
                </body>
                </html>
                ";
            
            var converter = new HtmlConverter();
            var html = @"
            <div style='margin: auto;width: 300px;'>
	            <meta charset='utf-8' > 
	            <div style='width: 300px;'>                          
	            <p class='classified-display-title text-center font-weight-bolder' style='font-weight: bold;text-align: center;margin-bottom: 10px;'>
		            শুভ জন্মদিন রাইফ রাইদ!
	            </p>
	            <div class='cls-dis-photoframe image-content-in-preview' style='width: 45px;height: 50px;float: left;
                margin-right: 7px;padding: .25rem;background-color: #fff;border: 1px solid #dee2e6;border-radius: .25rem;max-width: 100%;'>

		            <img style='width:43px;height: 46px;margin-top: 3px;float: left;' src='http://localhost:50147/img/display-add-default-image.jpg ' />
	            </div>
	            <span class='classified-display-content'>
                    রাষ্ট্রপতি মো. আবদুল হামিদের ৭৮তম জন্মদিন আজ (১ জানুয়ারি)। প্রতিবছর জন্মদিন উপলক্ষে বঙ্গভবনে রাষ্ট্রপতির কার্যালয়ের কর্মকর্তা-কর্মচারীরা অনাড়ম্বর অনুষ্ঠানে রাষ্ট্রপতিকে শুভেচ্ছা।
		            রাষ্ট্রপতি মো. আবদুল হামিদের ৭৮তম জন্মদিন আজ (১ জানুয়ারি)। প্রতিবছর জন্মদিন উপলক্ষে বঙ্গভবনে রাষ্ট্রপতির কার্যালয়ের কর্মকর্তা-কর্মচারীরা অনাড়ম্বর অনুষ্ঠানে রাষ্ট্রপতিকে শুভেচ্ছা।
	            </span>  
	            </div>                         
            </div>
            ";
            var htmlToImagebytes = converter.FromHtmlString(html, 300, ImageFormat.Jpg, 100);

            System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(htmlToImagebytes));

            var height = image.Height;
            var width = image.Width;

            HtmlToPdf htmlToPdf = new HtmlToPdf();

            PdfDocument pdfDocument = htmlToPdf.ConvertHtmlString(myHmlt);
            byte[] pdf = pdfDocument.Save(); ;
            pdfDocument.Close();

            var baseDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\webshared\\uploads\\rnd-invoices");
            var guid = new Guid();
            var imagePath = $@"{baseDirectory}\{guid}-html-image.Jpg";
            var fullPath = $@"{baseDirectory}\{guid}invoice.pdf";

            System.IO.File.WriteAllBytes(imagePath, htmlToImagebytes);

            System.IO.File.WriteAllBytes(fullPath, pdf);
            
            return File(
                pdf,
                "application/pdf",
                "birthday.pdf"
                );
        }
    }


}
