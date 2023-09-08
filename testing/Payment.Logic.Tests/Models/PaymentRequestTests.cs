using System.ComponentModel.DataAnnotations;
using Payment.Logic.Models;

public class PaymentRequestTests
{
    [Fact]
    public void PaymentRequest_AllEmpty_Invalid()
    {
        var request = new PaymentRequest();

        var (isValid, results) = ValidateModel(request);
        
        var sortedResults = results.OrderBy(x => x.ErrorMessage).ToList();

        Assert.False(isValid);
        Assert.Equal(9, results.Count);
        Assert.Equal("The CCV field is required.", sortedResults[0].ErrorMessage);
        Assert.Equal("The Currency field is required.", sortedResults[1].ErrorMessage);
        Assert.Equal("The field Month must be between 1 and 12.", sortedResults[2].ErrorMessage);
        Assert.Equal("The field Year must be between 1970 and 2200.", sortedResults[3].ErrorMessage);
        Assert.Equal("The MerchantID field is required.", sortedResults[4].ErrorMessage);
        Assert.Equal("The Name field is required.", sortedResults[5].ErrorMessage);
        Assert.Equal("The Number field is required.", sortedResults[6].ErrorMessage);
        Assert.Equal("The Referance field is required.", sortedResults[7].ErrorMessage);
        Assert.Equal("The Type field is required.", sortedResults[8].ErrorMessage);
    }

    [Fact]
    public void PaymentRequest_InvalidValues_Invalid()
    {
        var request = new PaymentRequest{
            Currency = "USD",
            MerchantID = "123",
            Referance = "123",
            CardDetails = new PaymentRequestCardDetails{
                Type = "DeathCard",
                Number = "gdsfuhgshgudsfg",
                CCV = "154848s",
                Name = "Ld Vador",
                Expiry = new PaymentRequestExpiry {
                    Month = 42,
                    Year = 42
                }
            }
        };

        var (isValid, results) = ValidateModel(request);
        
        var sortedResults = results.OrderBy(x => x.ErrorMessage).ToList();

        Assert.False(isValid);
        Assert.Equal(4, results.Count);
        Assert.Equal("Invalid Card Number.", sortedResults[0].ErrorMessage);
        Assert.Equal("Invalid CVV.", sortedResults[1].ErrorMessage);
        Assert.Equal("The field Month must be between 1 and 12.", sortedResults[2].ErrorMessage);
        Assert.Equal("The field Year must be between 1970 and 2200.", sortedResults[3].ErrorMessage);
    }

    [Fact]
    public void PaymentRequest_Valid()
    {
                var request = new PaymentRequest{
            Currency = "USD",
            MerchantID = "123",
            Referance = "123",
            CardDetails = new PaymentRequestCardDetails{
                Type = "DeathCard",
                Number = "4463742019252517",
                CCV = "123",
                Name = "Ld Vador",
                Expiry = new PaymentRequestExpiry {
                    Month = 1,
                    Year = 2000
                }
            }
        };

        var (isValid, results) = ValidateModel(request);

        Assert.True(isValid);
        Assert.Equal(0, results.Count);
    }

    private Tuple<bool, IList<ValidationResult>> ValidateModel(PaymentRequest model)
    {
        var validationResults = new List<ValidationResult>();
        var validator = new DataAnnotationsValidator.DataAnnotationsValidator();
        var result = validator.TryValidateObjectRecursive<PaymentRequest>(model, validationResults);
        return new(result, validationResults);
    }
}


