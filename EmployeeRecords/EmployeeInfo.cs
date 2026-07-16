using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeRecords
{
    internal class EmployeeInfo
    {
        public string Name{ set; get; }
        public int ID{ set; get; }
        // Salary Components
        public decimal BasicSalary { get; set; }
        public decimal HRA { get; set; }
        public decimal DA { get; set; }
        public decimal SpecialAllowance { get; set; }
        public decimal Bonus { get; set; }
        public decimal OvertimePay { get; set; }

        // Deductions
        public decimal PF { get; set; }
        public decimal ESI { get; set; }
        public decimal ProfessionalTax { get; set; }
        public decimal IncomeTax { get; set; }
        public decimal OtherDeductions { get; set; }

        // Calculate Gross Salary
        public decimal CalculateGrossSalary()
        {
            return BasicSalary +
                   HRA +
                   DA +
                   SpecialAllowance +
                   Bonus +
                   OvertimePay;
        }

        // Calculate Total Deductions
        public decimal CalculateDeductions()
        {
            return PF +
                   ESI +
                   ProfessionalTax +
                   IncomeTax +
                   OtherDeductions;
        }

        // Calculate Net Salary
        public decimal CalculateNetSalary()
        {
            return CalculateGrossSalary() - CalculateDeductions();
        }
    }
}

