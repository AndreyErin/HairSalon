﻿using HairSalon.Model.Employees;

namespace HairSalon.Model.Records.Admin
{
    public class RecordsForEmployeeOfDayModel
    {
        public Employee Employee { get; set; }
        public TimeTable RecordsOfDay { get; set; }
    }
}
