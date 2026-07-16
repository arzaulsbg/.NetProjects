using System;

namespace StudentRecords
{
    internal class StudentInfo
    {
        // Static variable (same for all students)
        public static string CollegeName;

        // Student Details
        public string Name { get; set; }
        public string UID { get; set; }

        // Every student has 5 subjects
        public Subject[] Subjects { get; set; }

        public StudentInfo()
        {
            Subjects = new Subject[5];

            for (int i = 0; i < 5; i++)
            {
                Subjects[i] = new Subject();
            }
        }

        // Calculate Total Marks
        public int TotalMarks()
        {
            int total = 0;

            foreach (Subject s in Subjects)
            {
                total += s.Marks;
            }

            return total;
        }

        // Calculate Percentage
        public double Percentage()
        {
            return TotalMarks() / 5.0;
        }

        // Calculate Grade
        public string Grade()
        {
            double per = Percentage();

            if (per >= 90)
                return "A+";
            else if (per >= 80)
                return "A";
            else if (per >= 70)
                return "B";
            else if (per >= 60)
                return "C";
            else if (per >= 50)
                return "D";
            else
                return "Fail";
        }
    }

    class Subject
    {
        public string SubjectName { get; set; }
        public int Marks { get; set; }
    }
}