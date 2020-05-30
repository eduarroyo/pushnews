using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PushNews.Dominio.Entidades;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace PushNews.WebApp.Services
{
    /// <summary>
    ///     Validates users before they are saved
    /// </summary>
    public class CustomUserValidator : UserValidator<Asociado, long>
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="manager"></param>
        public CustomUserValidator(AsociadosUserManager manager): base(manager)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }
            AllowOnlyAlphanumericUserNames = true;
            Manager = manager;
        }

        private AsociadosUserManager Manager { get; set; }

        /// <summary>
        ///     Validates a user before saving
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override async Task<IdentityResult> ValidateAsync(Asociado item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            var errors = new List<string>();
            await ValidateUserName(item, errors);
            if (errors.Count > 0)
            {
                return IdentityResult.Failed(errors.ToArray());
            }
            return IdentityResult.Success;
        }

        private async Task ValidateUserName(Asociado user, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                errors.Add(string.Format(CultureInfo.CurrentCulture, Resources.PropertyTooShort, "Name"));
            }
            else if (AllowOnlyAlphanumericUserNames && !Regex.IsMatch(user.UserName, @"^[A-Za-z0-9@_\.]+$"))
            {
                // If any characters are not letters or digits, its an illegal user name
                errors.Add(string.Format(CultureInfo.CurrentCulture, Resources.InvalidUserName, user.UserName));
            }
            else
            {
                var owner = await Manager.FindByNameAsync(user.Codigo, user.AplicacionID);
                if (owner != null && !EqualityComparer<long>.Default.Equals(owner.Id, user.Id))
                {
                    errors.Add(string.Format(CultureInfo.CurrentCulture, Resources.DuplicateName, user.UserName));
                }
            }
        }
    }
}
