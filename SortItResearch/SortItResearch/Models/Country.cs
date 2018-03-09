﻿using SortItResearch.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SortItResearch.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static List<Country> GetCountries()
        {
            return UserProfileDAO.getCountries();
        }
    }
}