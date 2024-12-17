using Core;
namespace Core.Tests;

public class VendingMachineTests
{
    readonly VendingMachine vendingMachine = new();
    const string INSERTCOINMESSAGE = "INSERT COIN";
    const string PRICEMESSAGE = "PRICE";
    const string THANKYOUMESSAGE = "THANK YOU";

    [Fact]
    public void NewVendingMachineWillShowINSERTCOIN()
    {
        Assert.Equal(INSERTCOINMESSAGE, vendingMachine.GetDisplay());
    }

    [Theory]
    [InlineData("nickel", "$0.05")]
    [InlineData("dime", "$0.10")]
    [InlineData("quarter", "$0.25")]
    public void InsertValidCointWillDisplayAmount(string coin, string expectedDisplay)
    {
        // When
        vendingMachine.InsertCoin(coin);

        // Then
        Assert.Equal(expectedDisplay, vendingMachine.GetDisplay());
    }

    [Fact]
    public void InsertInvalidCoinWillNotChangeMessage()
    {
        // When
        string startMessage = vendingMachine.GetDisplay();
        vendingMachine.InsertCoin("klingondollar");

        // Then
        Assert.Equal(startMessage, vendingMachine.GetDisplay());
    }

    [Fact]
    public void InsertMultipleCoinsWillDisplayTotalAmount()
    {
        // When
        vendingMachine.InsertCoin("nickel");
        vendingMachine.InsertCoin("dime");
        vendingMachine.InsertCoin("quarter");

        // Then
        Assert.Equal("$0.40", vendingMachine.GetDisplay());
    }

    [Fact]
    public void SelectProductWithNotEnoughMoneyWillDisplayPriceMessage()
    {
        // When
        vendingMachine.SelectProduct(Product.Cola);

        // Then
        Assert.Equal(PRICEMESSAGE, vendingMachine.GetDisplay());
    }

    [Fact]
    public void SelectProductWithEnoughMoneyWillDisplayThankYouAndThenReset()
    {
        // Given
        vendingMachine.InsertCoin("quarter");
        vendingMachine.InsertCoin("quarter");

        // When
        vendingMachine.SelectProduct(Product.Chips);

        // Then
        Assert.Equal(THANKYOUMESSAGE, vendingMachine.GetDisplay());
        Assert.Equal(INSERTCOINMESSAGE, vendingMachine.GetDisplay());
    }
}