namespace EmployeeDemo.Services
{
    public class Stripe : IPaymentServices

    {

        public string payment(double amount)
        {
            return $"Payment of {amount} processed successfully via Stripe.";
        }
    }
    
}
