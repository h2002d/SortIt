using SortItResearch.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SortItResearch.Models
{
    public class Answer
    {
        public int? Id { get; set; }
          [Required]
        public string Name { get; set; }
        [Required]
        public int QuestionId { get; set; }

        public bool IsRight { get; set; } = false;
        static QuestionDAO DAO = new QuestionDAO();

        public static void Delete(int id)
        {
            DAO.deleteAnswer(id);
        }
    }
}