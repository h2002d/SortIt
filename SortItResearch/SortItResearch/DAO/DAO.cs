using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SortItResearch.DAO
{
    public class DAO
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }
}