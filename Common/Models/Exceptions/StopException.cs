﻿using System;

namespace Common.Models.Exceptions
{
    /// <summary>
    /// stop handle request, 
    /// exc for fast return
    /// dont error
    /// </summary>
    public class StopException : Exception
    {
        public StopException() : base()
        {

        }
    }
}
