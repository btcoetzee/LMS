﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.LoggerClient.Interface
{
    public interface ILoggerClientObject
    {
       // Name of Soluction
        string SolutionContext { get; set; }

        // Name of Process
        string ProcessContext { get; set; }

        // Operation being logged
        string OperationContext { get; set; }

    }
}
