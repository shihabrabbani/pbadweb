using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace pbAd.Core.Utilities
{
    public class CoreHelper
    {
        public static string GenerateRandomToken(int length)
        {
            var r = new Random(Environment.TickCount);
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyz";
            var builder = new StringBuilder(length);

            for (var i = 0; i < length; ++i)
                builder.Append(chars[r.Next(chars.Length)]);

            return builder.ToString();
        }

        public static string RandomCodeGenerateNumber(int length)
        {
            Random random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static int TotalWordCounts(string sentance)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sentance))
                    return 0;

                MatchCollection collection = Regex.Matches(sentance, @"[\S]+");

                return collection.Count;
            }
            catch
            {
                return 0;
            }
        }

    }

    public static class CorePixelInchHelper
    {
        public static double ToInch(this int pixel)
        {
            var inch = (double)pixel / ((double)96);
            return inch;
        }
        public static int ToPixel(this double inch)
        {
            var actualPixel = (inch / (double)96);
            int pixel = Convert.ToInt32(Math.Ceiling(actualPixel));
            return pixel;
        }

        public static bool IsImageFile(this string extension)
        {
            bool isImageFile = true;
            switch (extension)
            {
                case FileIconConstants.XLSX:
                    isImageFile = false;
                    break;

                case FileIconConstants.XLS:
                    isImageFile = false;
                    break;

                case FileIconConstants.Docx:
                    isImageFile = false;
                    break;

                case FileIconConstants.Doc:
                    isImageFile = false;
                    break;

                case FileIconConstants.PDF:
                    isImageFile = false;
                    break;

                case FileIconConstants.TXT:
                    isImageFile = false;
                    break;

                case FileIconConstants.PNG:
                    isImageFile = true;
                    break;

                case FileIconConstants.JPG:
                    isImageFile = true;
                    break;

                case FileIconConstants.JPEG:
                    isImageFile = true;
                    break;

                case FileIconConstants.GIF:
                    isImageFile = true;
                    break;
                case FileIconConstants.JFIF:
                    isImageFile = true;
                    break;

                default:
                    isImageFile = false;
                    break;
            }

            return isImageFile;
        }
    }

    public class FileIconHelper
    {
        public static string GetFileIcon(string extension, string scale)
        {
            string fileIcon = $"<i class='fa fa-file-o {scale}' aria-hidden='true'></i>";
            switch (extension)
            {
                case FileIconConstants.XLSX:
                    fileIcon = $"<i class='fa fa-file-excel-o {scale}' aria-hidden='true'></i>";
                    break;

                case FileIconConstants.XLS:
                    fileIcon = $"<i class='fa fa-file-excel-o {scale}' aria-hidden='true'></i>";
                    break;

                case FileIconConstants.Docx:
                    fileIcon = $"<i class='fa fa-file-word-o {scale}' aria-hidden='true'></i>";
                    break;

                case FileIconConstants.Doc:
                    fileIcon = $"<i class='fa fa-file-word-o {scale}' aria-hidden='true'></i>";
                    break;

                case FileIconConstants.PDF:
                    fileIcon = $"<i class='fa fa-file-pdf-o {scale}' aria-hidden='true'></i>";
                    break;

                case FileIconConstants.TXT:
                    fileIcon = $"<i class='fa fa-file-o {scale}' aria-hidden='true'></i>";
                    break;

                case FileIconConstants.PNG:
                    fileIcon = $"<i class='fa fa-file-image-o {scale}' aria-hidden='true'></i>";
                    break;

                case FileIconConstants.JPG:
                    fileIcon = $"<i class='fa fa-file-image-o {scale}' aria-hidden='true'></i>";
                    break;

                case FileIconConstants.JPEG:
                    fileIcon = $"<i class='fa fa-file-image-o {scale}' aria-hidden='true'></i>";
                    break;

                case FileIconConstants.GIF:
                    fileIcon = $"<i class='fa fa-file-image-o {scale}' aria-hidden='true'></i>";
                    break;

                default:
                    fileIcon = $"<i class='fa fa-file-o {scale}' aria-hidden='true'></i>";
                    break;
            }

            return fileIcon;
        }

    }
}
