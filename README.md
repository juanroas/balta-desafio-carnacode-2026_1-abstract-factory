
## Problema
Uma plataforma de e-commerce precisa integrar com múltiplos gateways de pagamento (PagSeguro, MercadoPago, Stripe) e cada gateway tem componentes específicos (Processador, Validador, Logger).
O código atual está muito acoplado e dificulta a adição de novos gateways.

## Arquitetura do Sistema de Pagamentos

```mermaid
classDiagram
    class PaymentCreator {
        +ProcessPayment(amount: decimal, cardNumber: string)
        #CreateValidator() ICardValidator
        #CreateProcessor() ITransactionProcessor
        #CreateLogger() ILogger
    }

    class PagSeguroPaymentCreator {
        +CreateValidator() ICardValidator
        +CreateProcessor() ITransactionProcessor
        +CreateLogger() ILogger
    }

    class MercadoPagoPaymentCreator {
        +CreateValidator() ICardValidator
        +CreateProcessor() ITransactionProcessor
        +CreateLogger() ILogger
    }

    class StripePaymentCreator {
        +CreateValidator() ICardValidator
        +CreateProcessor() ITransactionProcessor
        +CreateLogger() ILogger
    }

    class ICardValidator {
        <<interface>>
        +ValidateCard(cardNumber: string) bool
    }

    class ITransactionProcessor {
        <<interface>>
        +ProcessTransaction(amount: decimal, cardNumber: string) string
    }

    class ILogger {
        <<interface>>
        +Log(message: string)
    }

    PaymentCreator <|-- PagSeguroPaymentCreator
    PaymentCreator <|-- MercadoPagoPaymentCreator
    PaymentCreator <|-- StripePaymentCreator

    ICardValidator <|.. PagSeguroValidator
    ITransactionProcessor <|.. PagSeguroProcessor
    ILogger <|.. PagSeguroLogger

    ICardValidator <|.. MercadoPagoValidator
    ITransactionProcessor <|.. MercadoPagoProcessor
    ILogger <|.. MercadoPagoLogger

    ICardValidator <|.. StripeValidator
    ITransactionProcessor <|.. StripeProcessor
    ILogger <|.. StripeLogger
