using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeRecords
{
    internal class MainEntry
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter number of employee");
            int nums = Convert.ToInt32(Console.ReadLine());
            EmployeeInfo[] em = new EmployeeInfo[nums];
            for (int i = 0; i < nums; i++)
            {
                em[i] = new EmployeeInfo();
                //salary
                Console.WriteLine("Enter Employee name :");
                em[i].Name = Console.ReadLine();
                Console.WriteLine("Enter Employee Id :");
                em[i].ID = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter Employee HRA:");
                em[i].HRA = Convert.ToDecimal(Console.ReadLine());
                Console.WriteLine("Enter Employee DA :");
                em[i].DA = Convert.ToDecimal(Console.ReadLine());
                Console.WriteLine("Enter Employee SpecialAllowance:");
                em[i].SpecialAllowance = Convert.ToDecimal(Console.ReadLine());
                Console.WriteLine("Enter Employee bonus :");
                em[i].Bonus = Convert.ToDecimal(Console.ReadLine());
                Console.WriteLine("Enter Employee overtime salary :");
                em[i].OvertimePay = Convert.ToDecimal(Console.ReadLine());
                //deduction salary
                Console.WriteLine("Enter Employee PF :");
                em[i].PF = Convert.ToDecimal(Console.ReadLine());
                Console.WriteLine("Enter Employee ProfessionalTax :");
                em[i].ProfessionalTax = Convert.ToDecimal(Console.ReadLine());
                Console.WriteLine("Enter Employee IncomeTax :");
                em[i].IncomeTax = Convert.ToDecimal(Console.ReadLine());
                Console.WriteLine("Enter Employee ESI :");
                em[i].ESI = Convert.ToDecimal(Console.ReadLine());
                Console.WriteLine("Enter Employee OtherDeductions :");
                em[i].OtherDeductions = Convert.ToDecimal(Console.ReadLine());

            }
            //Employee record
            Console.WriteLine("-------------Employee Records---------------------");
            for (int i = 0; i < nums; i++)

            {
                Console.WriteLine("Employee Name: {0}", em[i].Name);
                Console.WriteLine("Employee ID: {0}", em[i].ID);
                Console.WriteLine("Employee Salary: {0}", em[i].BasicSalary);
                
            }

        }
    }
}
