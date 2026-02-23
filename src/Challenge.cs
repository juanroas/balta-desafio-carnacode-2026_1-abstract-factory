// DESAFIO: Sistema de Pagamentos Multi-Gateway
// PROBLEMA: Uma plataforma de e-commerce precisa integrar com múltiplos gateways de pagamento
// (PagSeguro, MercadoPago, Stripe) e cada gateway tem componentes específicos (Processador, Validador, Logger)
// O código atual está muito acoplado e dificulta a adição de novos gateways

using System;

namespace DesignPatternChallenge
{
    // Contexto: Sistema de pagamentos que precisa trabalhar com diferentes gateways
    // Cada gateway tem sua própria forma de processar, validar e logar transações

    // Estrutura do Padrão
    // :Produto - Interface comum para os gateways de pagamento
    // :Produtos Concretos - Implementações específicas para cada gateway (PagSeguro, MercadoPago, Stripe)
    // :Creator - Classe que utiliza os gateways para processar pagamentos (PaymentService)
    // :Concrete Creators - Implementações específicas para cada gateway dentro do PaymentService (switch case)


    //Produto
    public interface ICardValidator
    {
        bool ValidateCard(string cardNumber);
    }

    public interface ITransactionProcessor
    {
        string ProcessTransaction(decimal amount, string cardNumber);
    }

    public interface ILogger
    {
        void Log(string message);
    }

    //Concreto Produto
    // Cada gateway tem suas próprias implementações de validação, processamento e logging    
    #region Produtos Concretos
    // Componentes do PagSeguro
    public class PagSeguroValidator : ICardValidator
    {
        public bool ValidateCard(string cardNumber)
        {
            Console.WriteLine("PagSeguro: Validando cartão...");
            return cardNumber.Length == 16;
        }
    }

    public class PagSeguroProcessor : ITransactionProcessor
    {
        public string ProcessTransaction(decimal amount, string cardNumber)
        {
            Console.WriteLine($"PagSeguro: Processando R$ {amount}...");
            return $"PAGSEG-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }
    }

    public class PagSeguroLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"[PagSeguro Log] {DateTime.Now}: {message}");
        }
    }

    // Componentes do MercadoPago
    public class MercadoPagoValidator : ICardValidator
    {
        public bool ValidateCard(string cardNumber)
        {
            Console.WriteLine("MercadoPago: Validando cartão...");
            return cardNumber.Length == 16 && cardNumber.StartsWith("5");
        }
    }

    public class MercadoPagoProcessor : ITransactionProcessor
    {
        public string ProcessTransaction(decimal amount, string cardNumber)
        {
            Console.WriteLine($"MercadoPago: Processando R$ {amount}...");
            return $"MP-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }
    }

    public class MercadoPagoLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"[MercadoPago Log] {DateTime.Now}: {message}");
        }
    }

    // Componentes do Stripe
    public class StripeValidator : ICardValidator
    {
        public bool ValidateCard(string cardNumber)
        {
            Console.WriteLine("Stripe: Validando cartão...");
            return cardNumber.Length == 16 && cardNumber.StartsWith("4");
        }
    }

    public class StripeProcessor : ITransactionProcessor
    {
        public string ProcessTransaction(decimal amount, string cardNumber)
        {
            Console.WriteLine($"Stripe: Processando ${amount}...");
            return $"STRIPE-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }
    }

    public class StripeLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"[Stripe Log] {DateTime.Now}: {message}");
        }
    }
    #endregion



    #region Criador
    public abstract class PaymentCreator
    {
        protected abstract ICardValidator CreatValidator();
        protected abstract ITransactionProcessor CreateProcessor();
        protected abstract ILogger CreateLogger();

        public void ProcessPayment(decimal amount, string cardNumber)
        {
            var _cardValidator = CreatValidator();
            var _transactionProcessor = CreateProcessor();
            var _logger = CreateLogger();

            if (!_cardValidator.ValidateCard(cardNumber))
            {
                Console.WriteLine("Cartão inválido");
                return;
            }
            var result = _transactionProcessor.ProcessTransaction(amount, cardNumber);
            _logger.Log($"Transação processada: {result}");
        }
    }

    #endregion

    #region Concrete Creators
    public class PagSeguroPaymentCreator : PaymentCreator
    {
        public override ICardValidator CreatValidator()
           => new PagSeguroValidator();

        public override ITransactionProcessor CreateProcessor()
           => new PagSeguroProcessor();

        public override ILogger CreateLogger()
           => new PagSeguroLogger();
    }

    public class MercadoPagoPaymentCreator : PaymentCreator
    {
        public override ICardValidator CreatValidator()
           => new MercadoPagoValidator();
        public override ITransactionProcessor CreateProcessor()
           => new MercadoPagoProcessor();
        public override ILogger CreateLogger()
           => new MercadoPagoLogger();
    }

    public class StripePaymentCreator : PaymentCreator
    {
        public override ICardValidator CreatValidator()
           => new StripeValidator();
        public override ITransactionProcessor CreateProcessor()
           => new StripeProcessor();
        public override ILogger CreateLogger()
           => new StripeLogger();
    }

    #endregion

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Sistema de Pagamentos ===\n");
            var pagSeguroPayment = new PagSeguroPaymentCreator();
            pagSeguroPayment.ProcessPayment(150.00m, "1234567890123456");
            Console.WriteLine();
            var mercadoPagoPayment = new MercadoPagoPaymentCreator();
            mercadoPagoPayment.ProcessPayment(200.00m, "5234567890123456");
            Console.WriteLine();
            var stripePayment = new StripePaymentCreator();
            stripePayment.ProcessPayment(250.00m, "4234567890123456");
        }
    }
}