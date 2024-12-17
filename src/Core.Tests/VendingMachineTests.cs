using Core;
namespace Core.Tests;

public class VendingMachineTests
{
    readonly VendingMachine vendingMachine = GetStockedVendingMachine();

    const string COKE = "cola";
    const string CHIPS = "chips";
    const string CANDY = "candy";

    private static VendingMachine GetStockedVendingMachine()
    {
        VendingMachine vendingMachine = new();
        vendingMachine.AddProduct(COKE, 1.00);
        vendingMachine.AddProduct(CHIPS, 0.50);
        vendingMachine.AddProduct(CANDY, 0.65);
        return vendingMachine;
    }

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
        vendingMachine.SelectProduct(COKE);

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
        vendingMachine.SelectProduct(CHIPS);

        // Then
        Assert.Equal(THANKYOUMESSAGE, vendingMachine.GetDisplay());
        Assert.Equal(INSERTCOINMESSAGE, vendingMachine.GetDisplay());
    }

    [Fact]
    public void PressingReturnChangeWillReturnCorrectAmountAndUpdateDisplay()
    {
        // Arrange
        vendingMachine.InsertCoin("quarter");
        double expectedAmountback = 0.25;

        // Act
        double actualAmountReturned = vendingMachine.ReturnCoins();

        // Assert
        Assert.Equal(expectedAmountback, actualAmountReturned);
        Assert.Equal(INSERTCOINMESSAGE, vendingMachine.GetDisplay());
    }

    [Fact]
    public void SelectingOutOfStockProductWillDisplaySoldOut()
    {
        // Given
        vendingMachine.InsertCoin("quarter");
        vendingMachine.InsertCoin("quarter");

        // When
        vendingMachine.SelectProduct(COKE);

        // Then
        Assert.Equal("SOLD OUT", vendingMachine.GetDisplay());
        Assert.Equal("$0.50", vendingMachine.GetDisplay());
    }
}