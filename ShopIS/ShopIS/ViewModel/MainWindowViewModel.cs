using System;
using DataAccessLayer;
using System.Collections.Generic;
using System.ComponentModel;
using Prism.Mvvm;
using Prism.Commands;
using ShopIS.URI;
using System.Net.Http;
using System.Text;
using ShopIS;

namespace ShopIS.ViewModel
{
    class MainWindowViewModel: BindableBase, INotifyPropertyChanged
    {
        #region Properties

        private List<Product> _products;
        public List<Product> Products
        {
            get { return _products; }
            set { SetProperty(ref _products, value); }
        }

        private Product _selectedProduct;
        public Product SelectedProduct {
            get { return _selectedProduct; }
            set { SetProperty(ref _selectedProduct, value);
            }
        }

        private Product _createProduct;
        public Product CreateProduct
        {
            get { return _createProduct; }
            set { SetProperty(ref _createProduct, value); }
        }

        private bool _isLoadData;
        public bool IsLoadData
        {
            get { return _isLoadData; }
            set { SetProperty(ref _isLoadData, value); }
        }

        private string _responseMessage = "Добро пожаловать в магазин товаров";

        public string ResponseMessage
        {
            get { return _responseMessage; }
            set { SetProperty(ref _responseMessage, value); }
        }        
        
        #region [Create Product Properties]
        private string _productName;
        public string ProductName
        {
            get { return _productName; }
            set { SetProperty(ref _productName, value); }
        }

        private string _price;
        public string Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }

        private string _category;
        public string Category
        {
            get { return _category; }
            set { SetProperty(ref _category, value); }
        }

        private string _count;
        public string Count
        {
            get { return _count; }
            set { SetProperty(ref _count, value); }
        }
        
        #endregion
        private bool _isShowForm;
        public bool IsShowForm
        {
            get { return _isShowForm; }
            set { SetProperty(ref _isShowForm, value); }
        }
        private string _showPostMessage = "";
        public string ShowPostMessage
        {
            get { return _showPostMessage; }
            set { SetProperty(ref _showPostMessage, value); }
        }
        #endregion

        #region ICommands  
        public DelegateCommand GetButtonClicked { get; set; }
        public DelegateCommand ShowRegistrationForm { get; set; }
        public DelegateCommand PostButtonClick { get; set; }
        public DelegateCommand<Product> PutButtonClicked { get; set; }
        public DelegateCommand<Product> DeleteButtonClicked { get; set; }
        #endregion

        #region Constructor
        public MainWindowViewModel()
        {
            GetButtonClicked = new DelegateCommand(GetProductDetails);
            PutButtonClicked = new DelegateCommand<Product>(UpdateProductDetails);
            DeleteButtonClicked = new DelegateCommand<Product>(DeleteProductDetails);
            PostButtonClick = new DelegateCommand(CreateNewProduct);
            ShowRegistrationForm = new DelegateCommand(RegisterProduct);
        }
        #endregion

        #region CRUD
        private void RegisterProduct()
        {
            IsShowForm = true;
        }

        private void GetProductDetails()
        {
            var productDetails = WebAPI.GetCall(API_URI.products);
            
            if (productDetails.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Products = productDetails.Result.Content.ReadAsAsync<List<Product>>().Result;
                IsLoadData = true;
            }
        }

        private void CreateNewProduct()
        {
            
            Product newProduct = new Product()
            {
                ProductName = ProductName,
                Category = Category,
                Price = Double.Parse(Price),
                Count = Int32.Parse(Count)
            };
            var productDetails = WebAPI.PostCall(API_URI.products, newProduct);
            if (productDetails.Result.StatusCode == System.Net.HttpStatusCode.Created)
            {
                ShowPostMessage = newProduct.ProductName + " Успешно добавлен";
            }
            else
            {
                ShowPostMessage = "Не удалось добавить " + newProduct.ProductName ;
            }
        }

        private void UpdateProductDetails(Product product)
        {
            var employeeDetails = WebAPI.PutCall(API_URI.products + "?id=" + product.IDProduct, product);
            if (employeeDetails.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ShowPostMessage = product.ProductName + "успешно изменен";
                GetProductDetails();
            }
            else ShowPostMessage = "Не удалось изменить " + product.ProductName;
           
        }

        private void DeleteProductDetails(Product product)
        {
            var employeeDetails = WebAPI.DeleteCall(API_URI.products + "?id=" + product.IDProduct);
            if (employeeDetails.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ShowPostMessage = product.ProductName + " успешно удален";
                GetProductDetails();
            }
            else ShowPostMessage = "Не удалось удалить " + product.ProductName;           
        }
        #endregion
    }
}
