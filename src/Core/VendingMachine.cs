

using System.Globalization;

namespace Core;

public class VendingMachine
{
    private double amount = 0;
    const string INSERTCOINMESSAGE = "INSERT COIN";
    const string THANKYOUMESSAGE = "THANK YOU";
    const string INSUFFICIENTFUNDSMESSAGE = "PRICE";

    private SelectProductState selectProductState = SelectProductState.NotYetSelected;

    private Dictionary<Product, double> products = new()
    {
        { Product.Cola, 1.0 },
        { Product.Chips, 0.5 },
        { Product.Candy, 0.65 }
    };

    private Dictionary<string, double> coins = new()
    {
        { "nickel", 0.05 },
        { "dime", 0.10 },
        { "quarter", 0.25 }
    };

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
        if (!coins.ContainsKey(coin))
        {
            return;
        }

        amount += coins[coin];
    }

    public void SelectProduct(Product product)
    {
        if (products.TryGetValue(product, out double price))
        {
            if (amount < price)
            {
                selectProductState = SelectProductState.InsufficientAmount;
                return;
            }

            selectProductState = SelectProductState.Success;
            amount -= price;

            //Call some hardware method to dispense the product here
        }
        else
        {
            throw new ArgumentException("Product not found!");
        }
    }
}

public enum Product
{
    Cola,
    Chips,
    Candy
}

public enum SelectProductState
{
    NotYetSelected,
    InsufficientAmount,
    Success
}
