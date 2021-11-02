using MUSMModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUSMWebApplication.Objects
{
    public class LoggedInUser
    {
        public LoginModel Login { get; set; } = new LoginModel() /*{ UserType = MUSMModelsLibrary.Enums.UserType.Student }*/;

    }
}
