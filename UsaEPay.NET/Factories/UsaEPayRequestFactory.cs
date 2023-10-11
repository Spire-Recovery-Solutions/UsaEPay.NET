using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsaEPay.NET.Models.Classes;

namespace UsaEPay.NET.Factories
{
    public static class UsaEPayRequestFactory
    {
        public static UsaEPayRequest CreditCardSaleRequest(decimal amount, string firstName, string lastName, string address, string address2, string city, string state, string zip, string country, string phone, string creditCardNumber, string expiration, int cvc, string invoice, string clientIP, Dictionary<string, string> customFields) {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cc:sale",
                Amount = amount,
                CreditCard = new CreditCard
                {
                    Number = creditCardNumber,
                    Cvc = cvc,
                    Expiration = expiration,
                    CardHolder = $"{firstName} {lastName}"
                },
                BillingAddress = new Address
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Street = address,
                    Street2 = address2,
                    City = city,
                    State = state,
                    PostalCode = zip,
                    Country = country,
                    Phone = phone,
                },
                Invoice = invoice,
                ClientIP = clientIP,
                CustomFields = customFields
            }; 
        }

        public static UsaEPayRequest CreditCardSaleRequest(decimal amount, string firstName, string lastName, string address, string address2, string city, string state, string zip, string creditCardNumber, string expiration, int cvc)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cc:sale",
                Amount = amount,
                CreditCard = new CreditCard
                {
                    Number = creditCardNumber,
                    Cvc = cvc,
                    Expiration = expiration,
                    CardHolder = $"{firstName} {lastName}"
                },
                BillingAddress = new Address
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Street = address,
                    Street2 = address2,
                    City = city,
                    State = state,
                    PostalCode = zip
                }
            };
        }

        public static UsaEPayRequest CheckSaleRequest(decimal amount, string firstName, string lastName, string address, string address2, string city, string state, string zip, string country, string phone, string routing, string account, string accountType, string checkNumber, string invoice, string clientIP, Dictionary<string, string> customFields)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "check:sale",
                Amount = amount,
                Check = new Check
                {
                    Number = checkNumber,
                    AccountType = accountType,
                    Account = account,
                    Routing = routing,
                },
                BillingAddress = new Address
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Street = address,
                    Street2 = address2,
                    City = city,
                    State = state,
                    PostalCode = zip,
                    Country = country,
                    Phone = phone,
                },
                Invoice = invoice,
                ClientIP = clientIP,
                CustomFields = customFields
            };
        }

        public static UsaEPayRequest CheckSaleRequest(decimal amount, string firstName, string lastName, string address, string address2, string city, string state, string zip, string routing, string account, string accountType, string checkNumber) {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "check:sale",
                Amount = amount,
                Check = new Check
                {
                    Number = checkNumber,
                    AccountType = accountType,
                    Account = account,
                    Routing = routing,
                },
                BillingAddress = new Address
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Street = address,
                    Street2 = address2,
                    City = city,
                    State = state,
                    PostalCode = zip
                }
            };
        }
        public static UsaEPayRequest TokenSaleRequest(decimal amount, string firstName, string lastName, string address, string address2, string city, string state, string zip, string country, string phone, string token, int cvc, string invoice, string clientIP, Dictionary<string, string> customFields)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cc:sale",
                Amount = amount,
                CreditCard = new CreditCard
                {
                    Number = token,
                    Cvc = cvc
                },
                BillingAddress = new Address
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Street = address,
                    Street2 = address2,
                    City = city,
                    State = state,
                    PostalCode = zip,
                    Country = country,
                    Phone = phone,
                },
                Invoice = invoice,
                ClientIP = clientIP,
                CustomFields = customFields
            };
        }

        public static UsaEPayRequest TokenSaleRequest(decimal amount, string firstName, string lastName, string address, string address2, string city, string state, string zip, string token, int cvc)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cc:sale",
                Amount = amount,
                CreditCard = new CreditCard
                {
                    Number = token,
                    Cvc = cvc
                },
                BillingAddress = new Address
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Street = address,
                    Street2 = address2,
                    City = city,
                    State = state,
                    PostalCode = zip
                }
            };
        }
        public static UsaEPayRequest PaymentKeySaleRequest(decimal amount, string paymentKey) { 
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "sale",
                Amount = amount,
                PaymentKey = paymentKey
            }; 
        }
        public static UsaEPayRequest CashSaleRequest(decimal amount) {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cash:sale",
                Amount = amount
            };
        }
        public static UsaEPayRequest QuickSaleRequest(decimal amount, string transactionKey) {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                TransactionKey = transactionKey,
                Command = "quicksale",
                Amount = amount
            };
        }
        public static UsaEPayRequest AuthOnlySaleRequest(decimal amount, string cardHolder, string cardNumber, string expiration, int cvc) {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "authonly",
                Amount = amount,
                CreditCard = new CreditCard
                {
                    CardHolder = cardHolder,
                    Number = cardNumber,
                    Expiration = expiration,
                    Cvc = cvc
                }
            };
        }
        public static UsaEPayRequest CreditCardRefundRequest(decimal amount, string cardHolder, string cardNumber, string expiration, int cvc)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cc:credit",
                Amount = amount,
                CreditCard = new CreditCard
                {
                    CardHolder = cardHolder,
                    Number = cardNumber,
                    Expiration = expiration,
                    Cvc = cvc
                }
            };
        }

        public static UsaEPayRequest CheckRefundRequest(decimal amount, string accountHolder, string accountNumber, string routingNumber, string accountType, string checkNumber)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "check:credit",
                Amount = amount,
                Check = new Check
                {
                    AccountHolder = accountHolder,
                    Number = checkNumber,
                    Account = accountNumber,
                    AccountType = accountType,
                    Routing = routingNumber
                }
            };
        }

        public static UsaEPayRequest CashRefundRequest(decimal amount)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cash:refund",
                Amount = amount
            };
        }

        public static UsaEPayRequest QuickRefundRequest(decimal amount, string tranKey)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "quickrefund",
                Amount = amount,
                TransactionKey = tranKey
            };
        }

        public static UsaEPayRequest CreditVoidRequest(string tranKey)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "creditvoid",
                TransactionKey = tranKey
            };
        }

        public static UsaEPayRequest PostPaymentRequest(decimal amount, string authCode, string cardHolder, string cardNumber, string expiration, int cvc)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cc:postauth",
                Amount = amount,
                CreditCard = new CreditCard
                {
                    Number = cardNumber,
                    Expiration = expiration,
                    Cvc = cvc,
                    CardHolder = cardHolder
                }
            };
        }

        public static UsaEPayRequest VoidPaymentRequest (string transactionKey)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "void",
                TransactionKey = transactionKey
            };
        }

        public static UsaEPayRequest ReleaseFundsRequest (string transactionKey)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cc:void:release",
                TransactionKey = transactionKey
            };
        }

        public static UsaEPayRequest UnvoidRequest(string transactionKey)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "unvoid",
                TransactionKey = transactionKey
            };
        }

        public static UsaEPayRequest AdjustPaymentRequest(string tranKey)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cc:adjust",
                TransactionKey = tranKey
            };
        }

        public static UsaEPayRequest AdjustPaymentRefundRequest(string transKey, decimal amount)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cc:refund:adjust",
                TransactionKey = transKey,
                Amount = amount
            };
        }

        public static UsaEPayRequest TokenizeCardRequest(string cardHolder, string creditCardNumber, string expiration, int cvc)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cc:save",

                CreditCard = new CreditCard
                {
                    Number = creditCardNumber,
                    Cvc = cvc,
                    Expiration = expiration,
                    CardHolder = cardHolder
                },
            };
        }

        public static UsaEPayGetRequest RetrieveTransactionDetailsRequest(string transactionId)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"transactions/{transactionId}"
            };
        }

        public static UsaEPayGetRequest RetrieveTokenDetailsRequest(string tokenId)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"tokens/{tokenId}"
            };
        }
    }
}
