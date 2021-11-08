using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;
using MUSMModelsLibrary.Enums;

namespace MUSMModelsLibrary
{
    public class LoginModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }  // 0 = Unset, 1 = Admin, 2 = Staff, 3 = Student
    }
}
