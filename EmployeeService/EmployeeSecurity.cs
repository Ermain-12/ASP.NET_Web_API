using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmployeeDataModel;

namespace EmployeeService
{
    public class EmployeeSecurity
    {

        public static Boolean Login(string username, string password)
        {
            using(StoreDBEntities entities = new StoreDBEntities())
            {
                return entities.Users.Any(user => user.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
                                                        && user.Password == password);
            }
        }
    }
}