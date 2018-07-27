using Akari_Net.Core.Areas.Usuarios.Models.Entities;
using Akari_Net.Core.Areas.Usuarios.Models.ViewModels.ManageViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Usuarios.Extensions
{
    public static class UserManagerExtensions
    {
        public static UsersPageDataViewModel GetUserPageAsync(this UserManager<ApplicationUser> _manager, string text, int page, int pageSize, string sort, bool @ascending)
        {
            var usersQuery = _manager.Users;
            switch (sort.ToLower())
            {
                case "email":
                    usersQuery = ascending
                        ? usersQuery.OrderBy(p => p.Email)
                        : usersQuery.OrderByDescending(p => p.Email);
                    break;
                case "name":
                    usersQuery = ascending
                        ? usersQuery.OrderBy(p => p.NombreCompleto)
                        : usersQuery.OrderByDescending(p => p.NombreCompleto);
                    break;
                default:
                    usersQuery = ascending
                        ? usersQuery.OrderBy(p => p.UserName)
                        : usersQuery.OrderByDescending(p => p.UserName);
                    break;
            }

            if (!string.IsNullOrWhiteSpace(text))
                usersQuery = usersQuery.Where(u => u.UserName.Contains(text) || u.NombreCompleto.Contains(text) || u.Email.Contains(text) || u.PhoneNumber.Contains(text));

            var count = usersQuery.Count();

            var data = usersQuery.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new UserViewModel
            {
                UserName = x.UserName,
                Name = x.NombreCompleto,
                Email = x.Email,
                Id = x.Id
            }).ToList();
            var result = new UsersPageDataViewModel
            {
                TotalUsers = count,
                Users = data,
            };
            return result;
        }
    }
}
