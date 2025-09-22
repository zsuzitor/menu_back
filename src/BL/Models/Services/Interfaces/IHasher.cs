using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Models.Services.Interfaces
{
    public interface IHasher
    {
        string GetHash(string str);

    }
}
