using MUSMModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUSMWebApplication.Objects
{
    public class LoggedInUser
    {
        public LoginModel Login { get; set; }
        public StudentModel Student { get; set; }
        public StaffModel Staff { get; set; }

        public LoggedInUser()
        {
            Login = new LoginModel();
            Student = new StudentModel();
            Staff = new StaffModel();
        }
    }
}
