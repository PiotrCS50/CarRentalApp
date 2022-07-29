using CarRental.App_Start;
using CarRental.Domain.Abstract;
using CarRental.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Extentions
{
    public class AdminUsers
    {
        public static string GetAdmins()
        {
            DataContext data = new DataContext();
            var admins = data.Users.Where(u => u.AccountType == Role.Administrator).ToList();
            string toReturn = "";
            foreach (var admin in admins)
            {
                toReturn += admin.UserName + ",";
            }
            return toReturn;
        }
    }
}
