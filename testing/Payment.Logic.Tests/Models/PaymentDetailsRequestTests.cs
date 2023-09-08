using System.ComponentModel.DataAnnotations;
using Payment.Logic.Models;

public class PaymentDetailsRequestTests
{
    [Fact]
    public void PaymentDetailsRequest_AllEmpty_Invalid()
    {
        var request = new PaymentDetailsRequest();

        var (isValid, results) = ValidateModel(request);

        Assert.False(isValid);
        Assert.Equal(1, results.Count);
        Assert.Equal("The MerchantID field is required.", results[0].ErrorMessage);
    }

    [Fact]
    public void PaymentDetailsRequest_Valid()
    {
        var request = new PaymentDetailsRequest
        {
            PaymentID = Guid.NewGuid(),
            MerchantID = "123"
        };

        var (isValid, results) = ValidateModel(request);

        Assert.True(isValid);
        Assert.Equal(0, results.Count);
    }

    private Tuple<bool, IList<ValidationResult>> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);
        var result = Validator.TryValidateObject(model, ctx, validationResults, true);
        return new(result, validationResults);
    }
}


