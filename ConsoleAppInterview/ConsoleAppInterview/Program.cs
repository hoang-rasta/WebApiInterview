using System;

public class Program
{
    //Interface for Payment
    public interface IPaymentMethod
    {
        void ProcessPayment(decimal amount);
    }

    //Concrete Implementations
    public class CreditCard : IPaymentMethod
    {
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing credit card payment of {amount}");
        }
    }
    public class PayPal : IPaymentMethod
    {
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing PayPal payment of {amount}");
        }

        public void ProcessPayment2(decimal amount)
        {
            Console.WriteLine($"Processing PayPal 2 payment of {amount}");
        }
    }
    //Our PaymentProcessor class will now depend on the abstraction
    public class PaymentProcessor
    {
        private readonly IPaymentMethod _paymentMethod;
        public PaymentProcessor(IPaymentMethod paymentMethod)
        {
            _paymentMethod = paymentMethod;
        }
        public void ExecutePayment(decimal amount)
        {

            if (_paymentMethod is PayPal payPal)
            {
                payPal.ProcessPayment2(amount); 
            }
            else
            {
                _paymentMethod.ProcessPayment(amount);

            }
        }
    }



    public static void Main()
    {
        var creditCardPayment = new CreditCard();
        var paymentProcessor1 = new PaymentProcessor(creditCardPayment);
        paymentProcessor1.ExecutePayment(100m);
        var paypalPayment = new PayPal();
        var paymentProcessor2 = new PaymentProcessor(paypalPayment);
        paymentProcessor2.ExecutePayment(100m);

        Console.ReadKey();
    }

}
