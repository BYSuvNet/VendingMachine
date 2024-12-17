

using System.Globalization;
using System.Runtime.InteropServices;

namespace Core;

public class VendingMachine
{
    private double amount = 0;
    const string INSERTCOINMESSAGE = "INSERT COIN";
    const string THANKYOUMESSAGE = "THANK YOU";
    const string INSUFFICIENTFUNDSMESSAGE = "PRICE";

    private SelectProductState selectProductState = SelectProductState.NotYetSelected;

    private Dictionary<Product, int> products = new();

    private Dictionary<string, double> coins = new()
    {
        { "nickel", 0.05 },
        { "dime", 0.10 },
        { "quarter", 0.25 }
    };

    public void AddProduct(string product, double price)
    {
        products.Add(new Product(product, price), 0);
    }

    public void SetProductStock(string product, int stock)
    {
        var productEntry = products.FirstOrDefault(x => x.Key.Name == product);
        if (productEntry.Key == null)
        {
            throw new ArgumentException("Product not found! This should not happen. Please configure the machine correctly.");
        }

        products[productEntry.Key] = stock;
    }

    public string GetDisplay()
    {
        string displayMessage = string.Empty;

        switch (selectProductState)
        {
            case SelectProductState.InsufficientAmount:
                displayMessage = INSUFFICIENTFUNDSMESSAGE;
                break;
            case SelectProductState.Success:
                displayMessage = THANKYOUMESSAGE;
                break;
            case SelectProductState.NotYetSelected:
                displayMessage = amount == 0 ? INSERTCOINMESSAGE : $"${amount.ToString("0.00", CultureInfo.InvariantCulture)}";
                break;
            default:
                break;
        }

        selectProductState = SelectProductState.NotYetSelected;
        return displayMessage;
    }

    public void InsertCoin(string coin)
    {
        if (coins.TryGetValue(coin, out double coinValue))
        {
            amount += coinValue;
        }
    }

    public void SelectProduct(string productName)
    {
        var productEntry = products.FirstOrDefault(x => x.Key.Name == productName);
        if (productEntry.Key == null)
        {
            throw new ArgumentException("Product not found! This should not happen. Please configure the machine correctly.");
        }

        Product product = productEntry.Key;
        double price = productEntry.Key.Price;

        if (amount < price)
        {
            selectProductState = SelectProductState.InsufficientAmount;
            return;
        }

        amount -= price;
        selectProductState = SelectProductState.Success;

        //Call some hardware method to dispense the product here
    }

    public double ReturnCoins()
    {
        double amountToBeReturned = amount;
        amount = 0;
        return amountToBeReturned;
    }
}

public record Product(string Name, double Price);

public enum SelectProductState
{
    NotYetSelected,
    InsufficientAmount,
    Success
}
