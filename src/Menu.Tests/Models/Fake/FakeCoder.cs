using BL.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Menu.Tests.Models.Fake
{
    public class FakeCoder : ICoder
    {
        public string DecryptFromBytes(byte[] input, string key)
        {
            var s = Encoding.UTF8.GetString(input, 0, input.Length);
            if (s.EndsWith($"___{key}"))
            {
                return s[..^$"___{key}".Length];
                //return s.Replace($"___{key}","");
            }

            return string.Empty;
        }

        public string DecryptFromString(string input, string key)
        {
            if (input.EndsWith($"___{key}"))
            {
                return input[..^$"___{key}".Length];
                //return s.Replace($"___{key}","");
            }

            return string.Empty;
        }

        public byte[] EncryptWithByte(string input, string key)
        {
            var s = input + $"___{key}";
            return  Encoding.UTF8.GetBytes(s);

        }

        public string EncryptWithString(string input, string key)
        {
            return input + $"___{key}";
        }
    }
}
