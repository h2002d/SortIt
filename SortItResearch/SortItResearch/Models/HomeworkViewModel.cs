using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SortItResearch.Models
{
    public class HomeworkViewModel : Progress
    {
        public UserProfile Student { get { return new UserProfile(StudentId); } }
        public Lesson Lesson { get { return Lesson.GetLesson(LessonId, null).First(); } }
    }
}