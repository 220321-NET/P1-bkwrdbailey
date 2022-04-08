using Xunit;
using Models;
using System.ComponentModel.DataAnnotations;

namespace Tests;

public class UnitTests
{
    [Fact]
    public void UserShouldSetValidUsernames()
    {
        User user = new User();
        user.UserName = "Bailey";

        Assert.Equal("Bailey", user.UserName);
    }

    [Fact]
    public void QuantityProperlyUpdated()
    {
        Product product = new Product {
            Name = "Lollipop",
            Quantity = 12
        };

        product.Quantity -= 10;

        Assert.Equal(2, product.Quantity);
    }
}