﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class PasswordReset
    {
        public Guid ID { get; set; }
        public long LoginID { get; set; }
        public byte[] UsernameSalt { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public string Url { get; set; }
    }
}