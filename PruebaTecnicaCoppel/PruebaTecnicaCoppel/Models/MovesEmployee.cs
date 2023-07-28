using System.ComponentModel.DataAnnotations;

namespace PruebaTecnicaCoppel.Models
{
    public class MovesEmployee
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public long EmployeeId { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateMove { get; set; }
        public string DateMoveSTR { get; set; } = "";
        public int Role { get; set; }
        public bool Status { get; set; }
        public int Deliver { get; set; }
    }

    public class MovesEmployeeCalculated
    {
        public long IdMove { get; set; }
        public string Name { get; set; } = "";
        public int Role { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateMove { get; set; }
        public string DateMoveSTR { get; set; } = "";
        public decimal SalaryBase { get; set; }
        public decimal SalaryPerMonth { get; set; }
        public decimal Deliver { get; set; }
        public decimal DeliverBonus { get; set; }
        public decimal HourBonus { get; set; }
        public decimal VoucherBonus { get; set; }
        public decimal ISR { get; set; }
        public decimal Total { get; set; }
    }
}
