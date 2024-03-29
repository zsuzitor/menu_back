﻿using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Web;

namespace Common.Models.Validators
{
    public interface IStringValidator
    {
        string Validate(string str);
    }

    public sealed class StringValidator : IStringValidator
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
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            string res = str;

            //var jsParam = new TextEncoderSettings(UnicodeRanges.All);
            //jsParam.AllowCharacter('\n');
            //var jsEncoder = JavaScriptEncoder.Create(jsParam);
            //res = jsEncoder.Encode(res);
            //res = _javaScriptEncoder.Encode(res);
            //res = _htmlEncoder.Encode(res);
            res = HttpUtility.HtmlEncode(res);
            //res = _urlEncoder.Encode(res);//todo зачем это?
            return res;
        }
    }
}
