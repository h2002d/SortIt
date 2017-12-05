﻿using SortItResearch.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SortItResearch.Models
{
    public class InviteModel
    {
        public int Id { get; set; }
        public string TeacherId { get; set; }
        public string StudentId { get; set; }
        public bool Accepted { get; set; }
        public DateTime CreateDate { get; set; }
        public int Type { get; set; }
        public int SubjectId { get; set; }

        static InviteDAO DAO = new InviteDAO();

        public static InviteModel GetInvite(int token)
        {
            return DAO.getInvite(token);
        }

        public int Save()
        {
            return DAO.saveInvite(this);
        }

        public static void SaveStatus(int token,bool accepted)
        {
            DAO.saveInviteStatus(token, accepted);
        }

    }
}