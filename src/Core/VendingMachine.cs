namespace Core;

public class VendingMachine
{
    private double amount = 0;
    const string INSERTCOINMESSAGE = "INSERT COIN";
    const string THANKYOUMESSAGE = "THANK YOU";
    const string INSUFFICIENTFUNDSMESSAGE = "PRICE";
    const string OUTOFSTOCKMESSAGE = "SOLD OUT";
    const string EXACTCHANGEONLYMESSAGE = "EXACT CHANGE ONLY";

    private SelectProductState selectProductState = SelectProductState.NotYetBought;

    private Dictionary<Product, int> products = new();
    private Dictionary<Coin, int> coins = new();

    public VendingMachine()
    {
        coins.Add(new Coin("nickel", 0.05), 0);
        coins.Add(new Coin("dime", 0.10), 0);
        coins.Add(new Coin("quarter", 0.25), 0);
    }

    public void SetCoinStock(string coinName, int stock)
    {
        Coin coin = GetCoin(coinName);
        coins[coin] = stock;
    }

    public void AddProduct(string product, double price)
    {
        products.Add(new Product(product, price), 0);
    }

    public void SetProductStock(string productName, int stock)
    {
        Product product = GetProduct(productName);
        products[product] = stock;
    }

    public string GetDisplay()
    {
        string displayMessage;

        switch (selectProductState)
        {
            case SelectProductState.ProductOutOfStock:
                displayMessage = OUTOFSTOCKMESSAGE;
                break;
            case SelectProductState.InsufficientAmount:
                displayMessage = INSUFFICIENTFUNDSMESSAGE;
                break;
            case SelectProductState.Success:
                displayMessage = THANKYOUMESSAGE;
                break;
            case SelectProductState.NotYetBought:
            default:
                if (amount == 0)
                {
                    displayMessage = ExactChangeOnly() ? EXACTCHANGEONLYMESSAGE : INSERTCOINMESSAGE;
                }
                else
                {
                    displayMessage = $"${amount}";
                }
                break;
        }

        selectProductState = SelectProductState.NotYetBought;
        return displayMessage;
    }

    public void InsertCoin(string coinName)
    {
        var coinEntry = coins.FirstOrDefault(x => x.Key.Name == coinName);
        if (coinEntry.Key == null)
        {
            //If the coin is not recognized, we should do nothing
            return;
        }

        amount += coinEntry.Key.Value;
    }

    public void BuyProduct(string productName)
    {
        Product product = GetProduct(productName);

        if (!ProductIsAvailable(product))
        {
            selectProductState = SelectProductState.ProductOutOfStock;
            return;
        }

        if (amount < product.Price)
        {
            selectProductState = SelectProductState.InsufficientAmount;
            return;
        }

        amount -= product.Price;
        selectProductState = SelectProductState.Success;

        //Call some hardware method to dispense the product here
    }

    public double ReturnCoins()
    {
        double amountToBeReturned = amount;
        amount = 0;
        return amountToBeReturned;
    }

    private Product GetProduct(string productName)
    {
        var productEntry = products.FirstOrDefault(x => x.Key.Name == productName);
        if (productEntry.Key == null)
        {
            throw new ArgumentException("Product not found! This should not happen. Please configure the machine correctly.");
        }

        return productEntry.Key;
    }

    private Coin GetCoin(string coinName)
    {
        var coinEntry = coins.FirstOrDefault(x => x.Key.Name == coinName);
        if (coinEntry.Key == null)
        {
            throw new ArgumentException("Coin not found! This should not happen. Please configure the machine correctly.");
        }

        return coinEntry.Key;
    }

    private bool ProductIsAvailable(Product product)
    {
        return products[product] > 0;
    }

    private bool ExactChangeOnly()
    {
        // This could be more complex, but for now, we will just check if there is at 
        // least one coin of each type in the machine to pretend it can give change
        return !coins.Values.All(x => x > 0);
    }
}

public record Product(string Name, double Price);
public record Coin(string Name, double Value);

public enum SelectProductState
{
    NotYetBought,
    InsufficientAmount,
    ProductOutOfStock,
    Success
}
