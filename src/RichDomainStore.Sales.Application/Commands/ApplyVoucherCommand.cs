using System;
using RichDomainStore.Core.Messages;
using RichDomainStore.Sales.Application.Validators;

namespace RichDomainStore.Sales.Application.Commands
{
    public class ApplyVoucherCommand : Command
    {
        public Guid CustomerId { get; private set; }
        public string VoucherCode { get; private set; }

        public ApplyVoucherCommand(Guid customerId, string voucherCode)
        {
            CustomerId = customerId;
            VoucherCode = voucherCode;
        }

        public override bool IsValid()
        {
            ValidationResult = new ApplyVoucherValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}