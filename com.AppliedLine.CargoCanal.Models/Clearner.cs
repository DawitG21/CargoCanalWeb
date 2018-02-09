using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Clearner
/// </summary>
/// 
namespace com.AppliedLine.Marilog.Models
{
    public static class Clearner
    {
        public static bool isClean(string data)
        {
            bool isClean = true;
            if (
                data.Contains("'")
                || data.Contains("-")
                || data.Contains("~")
                || data.Contains("*")
                || data.Contains("`")
                || data.Contains("&")
                || data.Contains("<")
                || data.Contains(">")
                || data.Contains("=")
                || data.Contains("%")
                || data.Contains("#")
                || data.Contains("{")
                || data.Contains("(")
                || data.Contains("!")
                || data.Contains("|")
                || data.Contains("?")
                || data.Contains("/")
                || data.Contains("$")
                || data.Contains("^")
                )
            {
                isClean = false;
            }
            return isClean;
        }
    }
}