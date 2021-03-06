﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;

namespace Common.Models.Validators
{
    public interface IStringValidator
    {
        string Validate(string str);
    }

    public class StringValidator : IStringValidator
    {
        private readonly HtmlEncoder _htmlEncoder;
        private readonly JavaScriptEncoder _javaScriptEncoder;
        private readonly UrlEncoder _urlEncoder;

        public StringValidator(
            HtmlEncoder htmlEncoder,
            JavaScriptEncoder javascriptEncoder,
            UrlEncoder urlEncoder)
        {

            _htmlEncoder = htmlEncoder;
            _javaScriptEncoder = javascriptEncoder;
            _urlEncoder = urlEncoder;
        }

        public string Validate(string str)
        {
            string res = _htmlEncoder.Encode(str);
            res = _javaScriptEncoder.Encode(res);
            res = _urlEncoder.Encode(res);
            return res;
        }
    }
}
