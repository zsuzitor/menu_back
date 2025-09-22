using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Models.Services.Interfaces
{
    public interface ICoder
    {
        byte[] EncryptWithByte(string input, string key);
        string EncryptWithString(string input, string key);

        string DecryptFromString(string input, string key);
        string DecryptFromBytes(byte[] input, string key);

    }
}
