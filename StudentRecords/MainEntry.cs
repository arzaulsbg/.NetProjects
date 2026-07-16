using System;

namespace StudentRecords
{
    internal class MainEntry
    {
        static void Main(string[] args)
        {
            Console.Write("Enter Number of Students: ");
            int num = Convert.ToInt32(Console.ReadLine());

            StudentInfo[] students = new StudentInfo[num];

            StudentInfo.CollegeName = "Chandigarh University";

            // Input
            for (int i = 0; i < num; i++)
            {
                students[i] = new StudentInfo();

                Console.WriteLine();
                Console.WriteLine($"===== Student {i + 1} =====");

                Console.Write("Enter Name: ");
                students[i].Name = Console.ReadLine();

                Console.Write("Enter UID: ");
                students[i].UID = Console.ReadLine();

                for (int j = 0; j < 5; j++)
                {
                    Console.Write($"Enter Subject {j + 1} Name: ");
                    students[i].Subjects[j].SubjectName = Console.ReadLine();

                    Console.Write($"Enter Subject {j + 1} Marks: ");
                    students[i].Subjects[j].Marks = Convert.ToInt32(Console.ReadLine());
                }
            }

            Console.WriteLine("      STUDENT RECORDS");

            int highest = 0;
            string topper = "";

            // Display
            for (int i = 0; i < num; i++)
            {
                Console.WriteLine();
                Console.WriteLine("--------------------------------");
                Console.WriteLine($"Student {i + 1}");
                Console.WriteLine("--------------------------------");

                Console.WriteLine("College : " + StudentInfo.CollegeName);
                Console.WriteLine("Name    : " + students[i].Name);
                Console.WriteLine("UID     : " + students[i].UID);

                Console.WriteLine("\nSubjects");

                for (int j = 0; j < 5; j++)
                {
                    Console.WriteLine(
                        $"{students[i].Subjects[j].SubjectName} : {students[i].Subjects[j].Marks}");
                }

                Console.WriteLine();

                Console.WriteLine("Total      : " + students[i].TotalMarks());
                Console.WriteLine("Percentage : " + students[i].Percentage() + "%");
                Console.WriteLine("Grade      : " + students[i].Grade());

                if (students[i].TotalMarks() > highest)
                {
                    highest = students[i].TotalMarks();
                    topper = students[i].Name;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Topper : " + topper);
            Console.WriteLine("Marks  : " + highest);
        }
    }
}