using QuanLyCuaHang.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace QuanLyCuaHang.ViewModel
{
    class OutputViewModel : BaseViewModel
    {
        private ObservableCollection<Model.Output> _ListOutput;
        public ObservableCollection<Model.Output> ListOutput { get => _ListOutput; set { _ListOutput = value; OnPropertyChanged(); } }

        private ObservableCollection<Model.OutputInfo> _ListOutputInfo;
        public ObservableCollection<Model.OutputInfo> ListOutputInfo { get => _ListOutputInfo; set { _ListOutputInfo = value; OnPropertyChanged(); } }

        private ObservableCollection<Model.Customer> _Customer;
        public ObservableCollection<Model.Customer> Customer { get => _Customer; set { _Customer = value; OnPropertyChanged(); } }

        private ObservableCollection<Model.Object> _Object;
        public ObservableCollection<Model.Object> Object { get => _Object; set { _Object = value; OnPropertyChanged(); } }

        private ObservableCollection<Model.Users> _Users;
        public ObservableCollection<Model.Users> Users { get => _Users; set { _Users = value; OnPropertyChanged(); } }

        private Model.Output _SelectedItem;
        public Model.Output SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    SelectedCustomer = SelectedItem.Customer;
                    DateOutput = SelectedItem.DateOutput;
                    SelectedUsers = SelectedItem.Users;
                    //Count = SelectedItem.Count;
                    //SelectedInputInfo = SelectedItem.InputInfo;

                    Id = SelectedItem.Id;
                    //Status = SelectedItem.Status;

                }
            }
        }

        private Model.OutputInfo _SelectedOutputInfo;
        public Model.OutputInfo SelectedOutputInfo
        {
            get => _SelectedOutputInfo; set
            {
                _SelectedOutputInfo = value;
                OnPropertyChanged();
                if (SelectedOutputInfo != null)
                {
                    SelectedObject = SelectedOutputInfo.Object;
                    Count = SelectedOutputInfo.Count;
                    Status = SelectedOutputInfo.Status;
                    //SelectedInputInfo = SelectedItem.InputInfo;
                }
            }
        }

        private Model.Object _SelectedObject;
        public Model.Object SelectedObject { get => _SelectedObject; set { _SelectedObject = value; OnPropertyChanged(); } }

        private Model.InputInfo _SelectedInputInfo;
        public Model.InputInfo SelectedInputInfo { get => _SelectedInputInfo; set { _SelectedInputInfo = value; OnPropertyChanged(); } }

        private Model.Customer _SelectedCustomer;
        public Model.Customer SelectedCustomer { get => _SelectedCustomer; set { _SelectedCustomer = value; OnPropertyChanged(); } }

        private Model.Users _SelectedUsers;
        public Model.Users SelectedUsers { get => _SelectedUsers; set { _SelectedUsers = value; OnPropertyChanged(); } }

        private Model.Promotion _SelectedPromotion;
        public Model.Promotion SelectedPromotion { get => _SelectedPromotion; set { _SelectedPromotion = value; OnPropertyChanged(); } }

        private DateTime? _DateOutput;
        public DateTime? DateOutput { get => _DateOutput; set { _DateOutput = value; OnPropertyChanged(); } }

        private string _Id;
        public string Id { get => _Id; set { _Id = value; OnPropertyChanged(); } }

        private int? _Count;
        public int? Count { get => _Count; set { _Count = value; OnPropertyChanged(); } }

        private double? _Sum;
        public double? Sum { get => _Sum; set { _Sum = value; OnPropertyChanged(); } }

        private string _Status;
        public string Status { get => _Status; set { _Status = value; OnPropertyChanged(); } }

        public ICommand SelectedItemListViewChangedCommand { get; set; }
        public ICommand SelectedOutputInfoListViewChangedCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand AddOuputInfoCommand { get; set; }        
        public ICommand EditOuputInfoCommand { get; set; }
        public ICommand DeleteOuputInfoCommand { get; set; }

        public OutputViewModel()
        {
            ListOutput = new ObservableCollection<Model.Output>(DataProvider.Ins.DB.Output);
            ListOutputInfo = new ObservableCollection<OutputInfo>(DataProvider.Ins.DB.OutputInfo);
            Users = new ObservableCollection<Model.Users>(DataProvider.Ins.DB.Users);
            Customer = new ObservableCollection<Model.Customer>(DataProvider.Ins.DB.Customer);
            Object = new ObservableCollection<Model.Object>(DataProvider.Ins.DB.Object);

            // Nhấn vào danh sách hóa đơn => chi tiết hóa đơn thay đổi theo
            SelectedItemListViewChangedCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                ListOutputInfo.Clear();
                if (SelectedItem != null)
                {
                    var collection = DataProvider.Ins.DB.OutputInfo.Where(x => x.IdOutput == SelectedItem.Id).ToList();
                    foreach (var item in collection)
                    {
                        ListOutputInfo.Add(item);
                    }
                }
            });

            //SelectedOutputInfoListViewChangedCommand = new RelayCommand<object>((p) => true, (p) =>
            //{
            //    //SelectedObject = SelectedOutputInfo.Object;
            //    Count = SelectedOutputInfo.Count;
            //    Status = SelectedOutputInfo.Status;
            //});

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedUsers == null || SelectedItem == null)
                    return false;
                return true;

            }, (p) =>

            {
                var Customer = DataProvider.Ins.DB.Customer.Where(x => x.Id == SelectedItem.IdCustomer).SingleOrDefault();
                if (Customer == null)
                {
                    Customer = new Model.Customer() { DisplayName = SelectedCustomer.DisplayName, Address = SelectedCustomer.Address, Phone = SelectedCustomer.Phone };
                }
                //, IdPromotion = SelectedPromotion.Id, Status = Status
                var Output = new Model.Output() { IdCustomer = Customer.Id, IdUser = SelectedUsers.Id, DateOutput = DateOutput, Id = Guid.NewGuid().ToString() };

                DataProvider.Ins.DB.Output.Add(Output);
                DataProvider.Ins.DB.SaveChanges();

                ListOutput.Add(Output);
            });

            EditCommand = new RelayCommand<Output>((p) =>
            {
                if (SelectedUsers == null || SelectedCustomer == null || SelectedItem == null)
                    return false;

                var displayList = DataProvider.Ins.DB.Output.Where(x => x.Id == SelectedItem.Id);
                if (displayList != null && displayList.Count() != 0)
                    return true;
                return false;

            }, (p) =>
            {
                var Output = DataProvider.Ins.DB.Output.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                Output.IdCustomer = SelectedCustomer.Id;
                Output.IdUser = SelectedUsers.Id;
                //Output.IdPromotion = SelectedPromotion.Id;
                DateOutput = DateOutput;
                Status = Status;
                DataProvider.Ins.DB.SaveChanges();

                ICollectionView view = CollectionViewSource.GetDefaultView(ListOutput);
                view.Refresh();
            });

            DeleteCommand = new RelayCommand<Output>((p) =>
            {
                if (SelectedUsers == null || SelectedCustomer == null || SelectedItem == null)
                    return false;

                var displayList = DataProvider.Ins.DB.Output.Where(x => x.Id == SelectedItem.Id);
                if (displayList != null && displayList.Count() != 0)
                    return true;
                return false;

            }, (p) =>
            {
                var Output = DataProvider.Ins.DB.Output.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                var collection = DataProvider.Ins.DB.OutputInfo.Where(x => x.IdOutput == SelectedItem.Id).ToList();

                foreach (var item in collection)
                {
                    DataProvider.Ins.DB.OutputInfo.Remove(item);

                }
                ListOutputInfo.Clear();

                DataProvider.Ins.DB.Output.Remove(Output);
                ListOutput.Remove(Output);
                DataProvider.Ins.DB.SaveChanges();
                ICollectionView view = CollectionViewSource.GetDefaultView(ListOutputInfo);
                view.Refresh();
            });

            AddOuputInfoCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedObject == null || Count == null)
                    return false;
                return true;

            }, (p) =>

            {
                var InputInfo = DataProvider.Ins.DB.InputInfo.Where(x => x.Id == SelectedObject.Id).SingleOrDefault();
                if (InputInfo == null)
                {
                    MessageBox.Show("Hàng trong kho đã hết");
                }

                else
                {
                    var OutputInfo = new Model.OutputInfo()
                    {
                        IdOutput = SelectedItem.Id,
                        IdObject = SelectedObject.Id,
                        IdInputInfo = InputInfo.Id,
                        Count = Count,
                        Status = Status,
                        //Status = SelectedOutputInfo.Status,
                        SumPrice = Count * InputInfo.OutputPrice,
                        Id = Guid.NewGuid().ToString()
                    };

                    DataProvider.Ins.DB.OutputInfo.Add(OutputInfo);
                    DataProvider.Ins.DB.SaveChanges();

                    ListOutputInfo.Add(OutputInfo);
                }
            });

            EditOuputInfoCommand = new RelayCommand<Output>((p) =>
            {
                if (SelectedObject == null || SelectedCustomer == null || SelectedOutputInfo == null)
                    return false;

                var displayList = DataProvider.Ins.DB.Output.Where(x => x.Id == SelectedItem.Id);
                if (displayList != null && displayList.Count() != 0)
                    return true;
                return false;

            }, (p) =>
            {
                var OutputInfo = DataProvider.Ins.DB.OutputInfo.Where(x => x.Id == SelectedOutputInfo.Id).SingleOrDefault();

                //MessageBox.Show(OutputInfo.Id);
                //var Output = DataProvider.Ins.DB.Output.Where(x => x.Id == OutputInfo.IdOutput).SingleOrDefault();


                var InputInfo = DataProvider.Ins.DB.InputInfo.Where(x => x.Id == SelectedObject.Id).SingleOrDefault();
                if (InputInfo == null)
                {
                    MessageBox.Show("Hàng trong kho đã hết");
                }

                else
                {
                    OutputInfo.IdObject = SelectedObject.Id;
                    OutputInfo.Count = Count;
                    OutputInfo.Status = Status;
                    OutputInfo.SumPrice = Count * InputInfo.OutputPrice;
                    //Output.IdPromotion = SelectedPromotion.Id;
                    DataProvider.Ins.DB.SaveChanges();

                    ICollectionView view = CollectionViewSource.GetDefaultView(ListOutputInfo);
                    view.Refresh();
                }

            });


            DeleteOuputInfoCommand = new RelayCommand<Output>((p) =>
            {
                if (SelectedObject == null || SelectedCustomer == null || SelectedOutputInfo == null)
                    return false;

                var displayList = DataProvider.Ins.DB.Output.Where(x => x.Id == SelectedItem.Id);
                if (displayList != null && displayList.Count() != 0)
                    return true;
                return false;

            }, (p) =>
            {
                var OutputInfo = DataProvider.Ins.DB.OutputInfo.Where(x => x.Id == SelectedOutputInfo.Id).SingleOrDefault();               
                
                ListOutputInfo.Remove(OutputInfo);
                DataProvider.Ins.DB.SaveChanges();

                ICollectionView view = CollectionViewSource.GetDefaultView(ListOutputInfo);
                view.Refresh();
                
            });

        }
    }
}
