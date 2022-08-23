﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Addresses
{
    public class AddressAddRequest
    {
        [Required]
        public string LineOne { get; set; }
        public int SuiteNumber { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string PostalCode { get; set; }
        public Boolean IsActive { get; set; }
        [Required]
        [Range(-90, 90)]
        public double Lat { get; set; }
        [Required]
        [Range(-180, 180)]
        public double Long { get; set; }
    }
}
