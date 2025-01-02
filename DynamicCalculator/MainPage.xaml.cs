using Microsoft.Maui.Controls;
//Kalkulator - poprawnie przedstawia wyniki
namespace DynamicCalculator;

public partial class MainPage : ContentPage
{
    private readonly Dictionary<string, Func<double, double, double>> _operations;

    public MainPage()
    {
        InitializeComponent();

        // Initialize the operations dictionary
        _operations = new Dictionary<string, Func<double, double, double>>
        {
            { "Add", (a, b) => a + b },
            { "Subtract", (a, b) => a - b },
            { "Multiply", (a, b) => a * b },
            { "Divide", (a, b) => b != 0 ? a / b : double.NaN },
            { "Power", (a, b) => a % b }, // Incorrect logic here
            { "Modulus", (a, b) => b != 0 ? b % a : double.NaN } // Incorrect logic here
        };

        // Populate the Picker with operations
        OperationPicker.ItemsSource = _operations.Keys.ToList();
    }

    private void OnCalculateClicked(object sender, EventArgs e)
    {
        // Validate input
        if (!double.TryParse(Number1Entry.Text, out double number1) ||
            !double.TryParse(Number2Entry.Text, out double number2))
        {
            ResultLabel.Text = "Please enter valid numeric values.";
            return;
        }

        if (OperationPicker.SelectedItem == null)
        {
            ResultLabel.Text = "Please select an operation.";
            return;
        }

        // Get the selected operation
        string operation = OperationPicker.SelectedItem.ToString();

        // Perform the calculation
        if (_operations.TryGetValue(operation, out var operationFunc))
        {
            double result = operationFunc(number1, number2);

            // Handle NaN cases (e.g., division by zero)
            if (double.IsNaN(result))
            {
                ResultLabel.Text = "Error: Division by zero or invalid operation.";
            }
            else
            {
                ResultLabel.Text = $"Result: {result:F2}";
            }
        }
        else
        {
            ResultLabel.Text = "Invalid operation selected.";
        }
    }
}
