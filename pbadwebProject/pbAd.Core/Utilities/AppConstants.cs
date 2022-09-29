using System;
using System.Collections.Generic;
using System.Text;

namespace pbAd.Core.Utilities
{
    #region Action Result Type

    public static class ActionResultTypeConstants
    {
        public const string Message = "message";
        public const string Url = "url";

    }

    #endregion

    #region User Role

    public static class UserRoleConstants
    {
        public const string Admin = "Admin";

    }

    #endregion

    #region Booking Step

    public static class BookingStepConstants
    {
        public const int Booked = 1;
        public const int Checkout = 2;
        public const int Completed = 3;
    }

    #endregion

    #region Add Enhancement Type

    public static class AddEnhancementTypeConstants
    {
        public const string BigBulletPointSingle = "Big Bullet Point (Single)";
        public const string BigBulletPointDouble = "Big Bullet Point (Double)";

        public const string Bold = "Bold";
        public const string BoldInScreen = "Bold in Screen";
        public const string BoldInScreenAndSingleBox = "Bold in screen and single box";
        public const string BoldInScreenAndDoubleBoxes = "Bold in screen and double boxes";
    }

    #endregion

    #region Classified Display Size

    public static class ClassifiedDisplayColumnInchConstants
    {
        public const decimal Inch_1 = 1;
        public const decimal Inch_1_5 = 1.50M;
        public const decimal Inch_2 = 2;
        public const decimal Inch_2_5 = 2.5M;
        public const decimal Inch_3 = 3;
        public const decimal Inch_3_5 = 3.5M;
        public const decimal Inch_4 = 4;
    }

    #endregion

    #region Classified Display Ad Widh

    public static class ClassifiedDisplayAdWidthConstants
    {
        public const decimal Inch_1 = 1;
        public const decimal Inch_1_5 = 1.50M;
        public const decimal Inch_2 = 2;
        public const decimal Inch_2_5 = 2.5M;
        public const decimal Inch_3 = 3;
        public const decimal Inch_3_5 = 3.5M;
        public const decimal Inch_4 = 4;
    }

    #endregion

    #region File Icon

    public static class FileIconConstants
    {
        public const string XLSX = "xlsx";
        public const string XLS = "xls";

        public const string Docx = "docx";
        public const string Doc = "doc";

        public const string PDF = "pdf";

        public const string JPG = "jpg";
        public const string JPEG = "jpeg";
        public const string PNG = "png";
        public const string GIF = "gif";
        public const string JFIF = "jfif";

        public const string TXT = "txt";
    }

    #endregion

    #region Payment Mode

    public static class PaymentModeConstants
    {
        public const int SSL = 1;
        public const int Cash = 2;
        public const int Credit = 3;

        public static string GetText(int value)
        {
            string constantText = "SSL";

            if (SSL == value)
                constantText = "SSL";

            if (Cash == value)
                constantText = "Cash";

            if (Credit == value)
                constantText = "Credit";

            return constantText;
        }
    }

    #endregion

    #region Payment Transaction Status

    public static class PaymentTransactionStatusConstants
    {
        public const int InProgress = 1;
        public const int Completed = 2;
        public const int Failed = 3;
        public const int Cancelled = 3;
    }

    #endregion

    #region Payment Status

    public static class PaymentStatusConstants
    {
        public const int Not_Paid = 1;
        public const int Paid = 2;
    }

    #endregion

    #region Ad Status

    public static class AdStatusConstants
    {
        public const int Booked = 1;
        public const int Confirmed_But_Not_Paid = 2;
        public const int Confirmed_And_Paid = 3;
    }

    #endregion

    #region User Group

    public static class UserGroupConstants
    {
        public const int Correspondent = 1;
        public const int CRM_User = 2;
        public const int Agency = 3;
    }

    #endregion

    #region Screen Type

    public static class ScreenTypeConstants
    {
        public const string Desktop = "Desktop";
        public const string Mobile = "Mobile";
    }

    #endregion

    #region Classified Type

    public static class ClassifiedTypeConstants
    {
        public const string DigitalDisplay = "DigitalDisplay";
        public const string ClassifiedDisplay = "ClassifiedDisplay";
        public const string ClassifiedText = "ClassifiedText";
        public const string PrivateDisplay = "PrivateDisplay";
        public const string SpotDisplay = "SpotDisplay";
        public const string GovtDisplay = "GovtDisplay";
    }

    #endregion

    #region Ad Type

    public static class AdTypeConstants
    {
        public const string DigitalDisplay = "DigitalDisplay";
        public const string ClassifiedDisplay = "ClassifiedDisplay";
        public const string ClassifiedText = "ClassifiedText";
        public const string PrivateDisplay = "Private";
        public const string SpotDisplay = "Spot";
        public const string GovtDisplay = "Govt";
        public const string EAR_Panel = "EAR Panel";
    }

    #endregion

    #region Payment Status

    public static class ApproveStatusConstants
    {
        public const int Not_Approved = 1;
        public const int Pending_Approval_Layer1 = 2;
        public const int Pending_Approval_Layer2 = 3;
        public const int Pending_Approval_Layer3 = 4;
        public const int Approved = 5;
    }

    #endregion

    #region One Time Password

    public static class OneTimePasswordConstants
    {
        public const string Password = "ONE_time_PASSWORD";

    }

    #endregion

    #region Login Provider

    public static class LoginProviderConstants
    {
        public const string Internal = "Internal";
        public const string Facebook = "Facebook";
        public const string Google = "Google";
    }

    #endregion

    #region Social Login Provider

    public static class SocialLoginProviderConstants
    {
        public const string Facebook = "Facebook";
        public const string Google = "Google";
    }

    #endregion

    #region Checkout Payment Methods

    public static class CheckoutPaymentMethodConstants
    {
        public const string CashOnDelivery = "Cash On Delivery";
        public const string MobileBankingOrBankPayment = "Mobile Banking or Bank Payment";
    }

    #endregion

    #region Action From

    public static class ActionFromConstants
    {
        public const string Login = "Login";
        public const string Signup = "Signup";
    }

    #endregion

    #region Payment Type

    public static class PaymentTypeConstants
    {
        public const string Cash_On_Lab = "Cash On Lab";
        public const string Mobile_Banking_Or_Credit_Debit = "Mobile Banking Or Credit/Debit Card";
    }

    #endregion

    #region Checkout Payment Type

    public static class CheckoutPaymentTypeConstants
    {
        public const int Direct = 1;
        public const int Card = 2;
        public const int Check_Or_Payorder = 3;

        public static string GetText(int value)
        {
            string constantText = "Card";

            if (Direct == value)
                constantText = "Direct";

            if (Check_Or_Payorder == value)
                constantText = "Check/Payorder";

            return constantText;
        }
    }

    #endregion

    #region Payment Method

    public static class PaymentMethodConstants
    {
        public const int bKash = 1;
        public const int Rocket = 2;
        public const int Nogod = 3;
        public const int Check_Or_Payorder = 4;
    }

    #endregion

    #region Edition

    public static class EditionConstants
    {
        public const int National = 20;
        public const int Rajshahi = 9;
        public const int Rangpur = 10;

        public static string GetText(int value)
        {
            string constantText = "All Edition";

            if (National == value)
                constantText = "All Edition";

            if (Rajshahi == value)
                constantText = "Rajshahi";

            if (Rangpur == value)
                constantText = "Rangpur";

            return constantText;
        }
    }

    #endregion

    #region Cache Key

    public static class CacheKeyConstants
    {
        public const string UserProfile = "CACHEMemory::USER-PROFILE-KEY";
        public const string SSLCacheKey = "CACHEMemory::SSL-CACHE-KEY";

    }

    #endregion

    #region Session Key

    public static class SessionKeyConstants
    {
        //classified text
        public const string ABPrintClassifiedText = "SessionKey::ADPRO::AB-PRINT-CLASSIFIED-TEXT-KEY";
        public const string RatePrintClassifiedTextByEditions = "SessionKey::ADPRO::RATE-PRINT-CLASSIFIED-TEXT-BY-EDITIONS-KEY";
        public const string RatePrintClassifiedText = "SessionKey::ADPRO::RATE-PRINT-CLASSIFIED-TEXT-KEY";
        public const string ClassifiedTextTotalPublishDates = "SessionKey::ADPRO::CLASSIFIED-TEXT-TOTAL-PUBLISH-DATES-KEY";

        //classified display
        public const string ABPrintClassifiedDisplay = "SessionKey::ADPRO::AB-PRINT-CLASSIFIED-DISPLAY-KEY";
        public const string RatePrintClassifiedDisplayByEditions = "SessionKey::ADPRO::RATE-PRINT-CLASSIFIED-DISPLAY-BY-EDITIONS-KEY";
        public const string RatePrintClassifiedDisplay = "SessionKey::ADPRO::RATE-PRINT-CLASSIFIED-DISPLAY-KEY";
        public const string ClassifiedDisplayAdHeight = "SessionKey::ADPRO::CLASSIFIED-DISPLAY-AD-HEIGHT-KEY";
        public const string ClassifiedDisplayTotalPublishDates = "SessionKey::ADPRO::CLASSIFIED-DISPLAY-TOTAL-PUBLISH-DATES-KEY";

        //private display
        public const string ABPrintPrivateDisplay = "SessionKey::ADPRO::AB-PRINT-PRIVATE-DISPLAY-KEY";
        public const string RatePrintPrivateDisplay = "SessionKey::ADPRO::RATE-PRINT-PRIVATE-DISPLAY-KEY";
        public const string RatePrintPrivateDisplayByEditions = "SessionKey::ADPRO::RATE-PRINT-CLASSIFIED-DISPLAY-BY-EDITIONS-KEY";
        public const string PrivateDisplayTotalPublishDates = "SessionKey::ADPRO::PRIVATE-DISPLAY-TOTAL-PUBLISH-DATES-KEY";

        //Rate Print SpotAd
        public const string RatePrintSpotAd = "SessionKey::ADPRO::RATE-PRINT-SPOTAD-KEY";
        public const string RatePrintSpotAdByEditions = "SessionKey::ADPRO::RATE-PRINT-SPOT-AD-BY-EDITIONS-KEY";

        //Govt Ad
        public const string ABPrintPrivateGovtDisplay = "SessionKey::ADPRO::AB-PRINT-PRIVATE-DISPLAY-GOVT-KEY";
        public const string RatePrintGovtAd = "SessionKey::ADPRO::RATE-PRINT-GOVT-AD-KEY";
        public const string PrivateDisplayGovtTotalPublishDates = "SessionKey::ADPRO::CLASSIFIED-TEXT-GOVT-TOTAL-PUBLISH-DATES-KEY";

        //digital display
        public const string ABDigitalDisplay = "SessionKey::ADPRO::AB-PRINT-PRIVATE-DISPLAY-KEY";
        public const string RateABDigitalDisplay = "SessionKey::ADPRO::RATE-PRINT-PRIVATE-DISPLAY-KEY";

    }

    #endregion

    #region  PrivateAdTypes

    public static class PrivateAdTypesConstants
    {
        public const string Private = "Private";
        public const string Spot = "Spot";
        public const string EARPanel = "EAR Panel";
        public const string Govt = "Govt";
        public const string Inhouse = "Inhouse";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Private", Value = Private, Selected = true},
                    new ConstantDropdownItem {Text = "Spot", Value = Spot, Selected = false},
                    new ConstantDropdownItem {Text = "EAR Panal", Value = EARPanel, Selected = false},
                    new ConstantDropdownItem {Text = "Inhouse", Value = Inhouse, Selected = false}
                };
            }
        }
    }

    #endregion

    #region  Edition Page

    public static class EditionPagesConstants
    {
        public const int Page_1 = 1;
        public const int Page_2 = 2;
        public const int Page_3 = 3;
        public const int Page_4 = 4;
        public const int Page_5 = 5;
        public const int Page_6 = 6;
        public const int Page_7 = 7;
        public const int Page_8 = 8;
        public const int Page_9 = 9;
        public const int Page_10 = 10;
        public const int Page_11 = 11;
        public const int Page_12 = 12;
    }

    #endregion    

    #region  Brand

    public static class BrandsConstants
    {
        public const int Others = 9200;
    }

    #endregion    
}
