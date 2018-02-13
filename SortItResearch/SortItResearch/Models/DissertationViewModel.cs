using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SortItResearch.Models
{
    public class DissertationViewModel:Dissertation
    {
        public UserProfile Student { get { return new UserProfile(StudentId); } }

        //public DissertationViewModel(Dissertation parent)
        //{
        //    StudentId = parent.StudentId;
        //    Attachement = parent.Attachement;
        //    SubjectId = parent.SubjectId;
        //    Id = parent.Id;
        //    CreateDate = parent.CreateDate;
        //    Accepted = parent.Accepted;
        //}
    }
}