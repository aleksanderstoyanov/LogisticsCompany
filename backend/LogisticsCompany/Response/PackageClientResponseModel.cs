﻿namespace LogisticsCompany.Response
{
    public class PackageClientResponseModel
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string PackageStatusName { get; set; }
        public int Weight { get; set; }
        public bool ToOffice { get; set; }
    }
}