using Core;
namespace Core.Tests;

public class VendingMachineTests
{
    readonly VendingMachine fullVendingMachine = GetStockedVendingMachine();
    readonly VendingMachine emptyVendingMachine = GetEmptyVendingMachine();

    const string COKE = "cola";
    const string CHIPS = "chips";
    const string CANDY = "candy";

    private static VendingMachine GetEmptyVendingMachine()
    {
        VendingMachine vendingMachine = new();
        vendingMachine.AddProduct(COKE, 1.00);
        vendingMachine.AddProduct(CHIPS, 0.50);
        vendingMachine.AddProduct(CANDY, 0.65);
        vendingMachine.SetCoinStock("quarter", 100);
        vendingMachine.SetCoinStock("dime", 100);
        vendingMachine.SetCoinStock("nickel", 100);

        return vendingMachine;
    }

    private static VendingMachine GetStockedVendingMachine()
    {
        VendingMachine vendingMachine = GetEmptyVendingMachine();
        // Add stock here
        vendingMachine.SetProductStock(COKE, 10);
        vendingMachine.SetProductStock(CHIPS, 10);
        vendingMachine.SetProductStock(CANDY, 10);
        return vendingMachine;
    }

    const string INSERTCOINMESSAGE = "INSERT COIN";
    const string PRICEMESSAGE = "PRICE";
    const string THANKYOUMESSAGE = "THANK YOU";
    const string SOLDOUTMESSAGE = "SOLD OUT";
    const string EXACTCHANGEONLYMESSAGE = "EXACT CHANGE ONLY";

    [Fact]
    public void NewVendingMachineWillShowINSERTCOIN()
    {
        Assert.Equal(INSERTCOINMESSAGE, fullVendingMachine.GetDisplay());
    }

    [Theory]
    [InlineData("nickel", "$0.05")]
    [InlineData("dime", "$0.10")]
    [InlineData("quarter", "$0.25")]
    public void InsertValidCointWillDisplayAmount(string coin, string expectedDisplay)
    {
        // When
        fullVendingMachine.InsertCoin(coin);

        // Then
        Assert.Equal(expectedDisplay, fullVendingMachine.GetDisplay());
    }

    [Fact]
    public void InsertInvalidCoinWillNotChangeMessage()
    {
        // When
        string startMessage = fullVendingMachine.GetDisplay();
        fullVendingMachine.InsertCoin("klingondollar");

        // Then
        Assert.Equal(startMessage, fullVendingMachine.GetDisplay());
    }

    [Fact]
    public void InsertMultipleCoinsWillDisplayTotalAmount()
    {
        // When
        fullVendingMachine.InsertCoin("nickel");
        fullVendingMachine.InsertCoin("dime");
        fullVendingMachine.InsertCoin("quarter");

        // Then
        Assert.Equal("$0.40", fullVendingMachine.GetDisplay());
    }

    [Fact]
    public void SelectProductWithNotEnoughMoneyWillDisplayPriceMessage()
    {
        // When
        fullVendingMachine.BuyProduct(COKE);

        // Then
        Assert.Equal(PRICEMESSAGE, fullVendingMachine.GetDisplay());
    }

    [Fact]
    public void SelectProductWithEnoughMoneyWillDisplayThankYouAndThenReset()
    {
        // Given
        fullVendingMachine.InsertCoin("quarter");
        fullVendingMachine.InsertCoin("quarter");

        // When
        fullVendingMachine.BuyProduct(CHIPS);

        // Then
        Assert.Equal(THANKYOUMESSAGE, fullVendingMachine.GetDisplay());
        Assert.Equal(INSERTCOINMESSAGE, fullVendingMachine.GetDisplay());
    }

    [Fact]
    public void PressingReturnChangeWillReturnCorrectAmountAndUpdateDisplay()
    {
        // Arrange
        fullVendingMachine.InsertCoin("quarter");
        double expectedAmountback = 0.25;

        // Act
        double actualAmountReturned = fullVendingMachine.ReturnCoins();

        // Assert
        Assert.Equal(expectedAmountback, actualAmountReturned);
        Assert.Equal(INSERTCOINMESSAGE, fullVendingMachine.GetDisplay());
    }

    [Fact]
    public void SelectingOutOfStockProductWillDisplaySoldOut()
    {
        // Given
        emptyVendingMachine.InsertCoin("quarter");
        emptyVendingMachine.InsertCoin("quarter");

        // When
        emptyVendingMachine.BuyProduct(COKE);

        // Then
        Assert.Equal(SOLDOUTMESSAGE, emptyVendingMachine.GetDisplay());
        Assert.Equal("$0.50", emptyVendingMachine.GetDisplay());
    }

    [Fact]
    public void VendingMachineWithoutCoinsWillDisplayExactChangeOnly()
    {
        // Given
        VendingMachine vendingMachine = new();

        // When
        string expectedDisplay = vendingMachine.GetDisplay();

        // Then
        Assert.Equal(EXACTCHANGEONLYMESSAGE, expectedDisplay);
    }
}