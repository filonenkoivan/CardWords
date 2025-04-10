﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPasswordHasherService
    {
        string HashPassword(string password);
        bool HashVerify(string password, string hashedPassword);
    }
}
