using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows;
using Canteen.Dto;
using Canteen.Management.Views;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Canteen.Management.ViewModels;

public class AddItemViewModel : ObservableValidator
{
    private string _name;
    private string _price;
    private CategoryDto _category;
    private string _path;

    [Required]
    [MinLength(1)]
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value, true);
    }

    [Required]
    [CustomValidation(typeof(AddItemViewModel), nameof(ValidatePrice))]
    public string Price
    {
        get => _price;
        set => TrySetProperty(ref _price, value, out _);
    }

    [Required]
    public CategoryDto Category
    {
        get => _category;
        set => SetProperty(ref _category, value, true);
    }

    [Required]
    [CustomValidation(typeof(AddItemViewModel), nameof(ValidateFileName))]
    public string Path
    {
        get => _path;
        set => SetProperty(ref _path, value, true);
    }

    public ObservableCollection<CategoryDto> Categories { get; } = new();
    public IRelayCommand AddCommand { get; }
    public IRelayCommand CloseCommand { get; }
    public ItemDto Item { get; private set; }

    public AddItemViewModel()
    {
        AddCommand = new RelayCommand(CreateItem, () => !HasErrors);
        CloseCommand = new RelayCommand(() =>
        {
            foreach (Window window in App.Current.Windows)
            {
                if (window.GetType() == typeof(AddItemWindow))
                    window.Close();
            }
        });
        
        ErrorsChanged += (_, _) => AddCommand.NotifyCanExecuteChanged();
        
        ValidateAllProperties();
    }

    private void CreateItem()
    {
        Item = new ItemDto
        {
            Name = Name,
            Price = decimal.Parse(Price),
            Image = File.ReadAllBytes(Path),
            CategoryId = Category.CategoryId,
            Category = Category // Required because the controller only accepts an ItemDto, which has the Category property
        };

        CloseCommand.Execute(null);
    }
    
    public static ValidationResult ValidatePrice(string price, ValidationContext context)
    {
        return decimal.TryParse(price, out _) ? ValidationResult.Success : new ValidationResult("Cannot be parsed to a decimal");
    }
    
    public static ValidationResult ValidateFileName(string fileName, ValidationContext context)
    {
        return File.Exists(fileName) ? ValidationResult.Success : new ValidationResult("File does not exist");
    }
}