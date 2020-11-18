using System;
using ViewModel.ServiceEnum;

namespace ViewModel.ServiceModel
{
    public class BaseServiceModel
    {
        public string Insertusername { get; set; }
        public DateTime? InsertTransacDate { get; set; }
        public TransacType? InsertTransacType { get; set; }
    }
}
