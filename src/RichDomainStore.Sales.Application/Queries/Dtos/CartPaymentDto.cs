namespace RichDomainStore.Sales.Application.Queries.Dtos
{
    public class CartPaymentDto
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string CardExpiration { get; set; }
        public string CardSecurityCode { get; set; }
    }
}