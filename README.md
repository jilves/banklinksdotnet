# BanklinksDotNet (Beta)
.NET library for integrating Estonian bank link services.

Currently supports the Estcard (estcard.ee) and IPizza (Swedbank, Seb, Danske, Nordea, Lhv, Krediidipank) protocols. Support for more protocols might be added in the future.

The work is still ongoing and there is much to improve. 

In order to get started just install the nuget packages for banks you want to integrate in your web app:

```
PM> Install-Package BanklinksDotNet
PM> Install-Package BanklinksDotNet.EstcardProvider
PM> Install-Package BanklinksDotNet.IPizzaProvider
```


The BanklinksDotNet library defines a generic interface and the providers add concrete implementations, classes and method signatures using extension methods.

## Quick introduction

You can use the [Pangalink.net](http://pangalink.net/) application to test the library.

### Setup
```
_banklinkApi = new BanklinkApi();
_banklinkApi.Configure()
  .AddIPizzaBankProvider()
  .AddEstcardProvider()
  .AddIPizzaBankConfigurations(new IPizzaConfiguration
  {
      // The BankId has to correspond with what is defined by the specific bank you are integrating.
      // For production environments, these are currently (14.09.2015) "KREP" for Krediidipank, "LHV" for Lhv,
      // "HP" for Swedbank, "SAMPOBANK" for Danske, "EYP" for Seb and "NORDEA" for Nordea.
      BankId = "GENIPIZZA",
      MerchantId = "uid100010",
      PaymentServiceUrl = "http://localhost:8080/banklink/ipizzapayment",
      AuthServiceUrl = "http://localhost:8080/banklink/ipizzaauth",
      BanksPublicCertificate = () => new X509Certificate2("Certs/IPizza/bank_cert.pem"),
      MerchantsPrivateKey = () => new X509Certificate2("Certs/IPizza/merchant.pfx"),
  }, ... more IPizza bank configurations)
  .AddEstcardConfiguration(new EstcardConfiguration
  {
      MerchantId = "uid100049",
      ServiceUrl = "http://localhost:8080/banklink/ec",
      BanksPublicCertificate = () => new X509Certificate2("Certs/Estcard/bank_cert.pem"),
      MerchantsPrivateKey = () => new X509Certificate2("Certs/Estcard/merchant.pfx"),
  });
```
### Create post parameters for payment

```
// Estcard
BankRequest bankRequest = _banklinkApi.CreateEstcardPaymentRequest(new EstcardPaymentRequestParams
{
    AmountInCents = 1336,
    ReturnUrl = "http://localhost:8080/project/mJltvgDF1boyOPpL?payment_action=success",
    TransactionNr = 1392644629
});

// Any IPizza bank
BankRequest bankRequest = _banklinkApi.CreateIPizzaPaymentRequest(new IPizzaPaymentRequestParams
{
  ErrorReturnUrl = "http://localhost:8080/project/6rGPnXJ7cvstStKx?payment_action=cancel",
  Amount = 150M,
  PaymentMessage = "Torso Tiger",
  SuccessReturnUrl = "http://localhost:8080/project/6rGPnXJ7cvstStKx?payment_action=success",
  Stamp = "12345",
  Currency = "EUR",
  PaymentReferenceNumber = "1234561",
  BankId = "GENIPIZZA"
});
```

### Create post parameters for authentication

```
BankRequest bankRequest = _banklinkApi.CreateIPizzaAuthRequest(new IPizzaAuthRequestParams
{
  BankId = "GENIPIZZA",
  ExpectedReturnCode = "3012",
  RequestId = "1428175410690",
  ReturnUrl = "http://localhost:8080/project/6rGPnXJ7cvstStKx?auth_action=success"
});

BankRequest bankRequest2 = _banklinkApi.CreateIPizzaAuthRequest(new IPizzaAuthRequestParams
{
  BankId = "GENIPIZZA",
  RequestId = "1428175410690",
  ReturnUrl = "http://localhost:30535/Home/AcceptAuth",
  Nonce = "9a3d6bd2-36d4-49b0-ae44-680c0281f39f"
});
```

### Post the parameters in an ASP.NET MVC app

```
@using BanklinksDotNet.ProviderBase
@model BanklinksDotNet.ProviderBase.BankRequest
@{
    Layout = null;
}
<html>
    <head>
        <title></title>
    </head>

    <body onload="submitform()">
        <form id="bankForm" name="bankForm" action="@Model.RequestUrl" method="post">
            @foreach (BankMessageField field in Model.PostParameters)
            {
                <input type="hidden" name="@field.FieldName" value="@field.Value" />
            }

            <input type="submit" value="Liigu edasi panka .." />
        </form>
        
        <script type="text/javascript">
            function submitform() {
                document.bankForm.submit();
            }
        </script>
    </body>
</html>
```

### Parse the response from the bank

```
NameValueCollection parameters = System.Web.HttpContext.Current.Request.Params;
AbstractBankResponse abstractBankResponse = _bankLinkApi.ParseBankResponse(parameters);
if (abstractBankResponse is EstcardPaymentResponse) { ... }
if (abstractBankResponse is IPizzaPaymentResponse) { ... }
if (abstractBankResponse is IPizzaAuthResponse) { ... }

// Or if you know which response you are accepting at the current route:

IPizzaPaymentResponse response = _bankLinkApi.ParseIPizzaPaymentResponse(parameters);
IPizzaAuthResponse response2 = _bankLinkApi.ParseIPizzaAuthResponse(parameters);
EstcardPaymentResponse response3 = _bankLinkApi.ParseEstcardPaymentResponse(parameters);

```

If the response Mac is invalid, an `InvalidMacException` is thrown. 

See `BanklinksDotNet.IntegrationTests` and `BankLinksDotNet.Demo` for more code samples though tests and the demo project are currently lacking.

### References:

http://pangaliit.ee/images/files/Pangalingi_tehniline_spetsifikatsioon_2014_FINAL.pdf
http://www.estcard.ee/publicweb/html/est/e-commerce.html#tehnilineinfo

### Changelog:

* 19/11/2015 - Released new versions of nuget packages.
* [19/11/2015 - Fixed an issue where nuget downloaded the oldest 1.0.0 BanklinksDotNet library as a dependency for IPizza and Estcard providers. This resulted in cryptic runtime errors](https://github.com/jilves/banklinksdotnet/commit/a15030af60f685d3dd70df1fe0d70c8a6b74c87f)
* [28/10/2015 - AddBankConfiguration, AddBankConfigurations Func overloads for configurations that might change during runtime.](https://github.com/jilves/banklinksdotnet/commit/a01403e35ba633043ee2f92eb132a7c6bfb9c6be)

# Licensed under MIT
