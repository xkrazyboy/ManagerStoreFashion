using QuanLyCuaHang.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanLyCuaHang.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<Inventory> _InventoryList;
        public ObservableCollection<Inventory> InventoryList { get => _InventoryList; set { _InventoryList = value; OnPropertyChanged(); } }

        private ThongKe _ThongKe;
        public ThongKe ThongKe { get => _ThongKe; set { _ThongKe = value; OnPropertyChanged(); } }

        private DateTime? _DateBeginInventory;
        public DateTime? DateBeginInventory { get => _DateBeginInventory; set { _DateBeginInventory = value; OnPropertyChanged(); } }

        private DateTime? _DateEndInventory;
        public DateTime? DateEndInventory { get => _DateEndInventory; set { _DateEndInventory = value; OnPropertyChanged(); } }

        private DateTime? _DateBegin;
        public DateTime? DateBegin { get => _DateBegin; set { _DateBegin = value; OnPropertyChanged(); } }

        private DateTime? _DateEnd;
        public DateTime? DateEnd { get => _DateEnd; set { _DateEnd = value; OnPropertyChanged(); } }

        public bool Isloaded = false;
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand UnitCommand { get; set; }
        public ICommand SuplierCommand { get; set; }
        public ICommand CustomerCommand { get; set; }
        public ICommand ObjectCommand { get; set; }
        public ICommand UserCommand { get; set; }
        public ICommand InputInfoCommand { get; set; }
        public ICommand OutputInfoCommand { get; set; }
        public ICommand OutputCommand { get; set; }
        public ICommand Inventory { get; set; }
        public ICommand InventoryCommand { get; set; }

        public MainViewModel()
        {
            LoadedWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                Isloaded = true;
                if (p == null)
                    return;
                p.Hide();
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();

                if (loginWindow.DataContext == null)
                    return;
                var loginVM = loginWindow.DataContext as LoginViewModel;

                if (loginVM.IsLogin)
                {
                    p.Show();
                    LoadInventoryData();
                }
                else
                {
                    p.Close();
                }
            }
            );

            UnitCommand = new RelayCommand<object>((p) => { return true; }, (p) => { UnitWindow wd = new UnitWindow(); wd.ShowDialog(); });
            SuplierCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SuplierWindow wd = new SuplierWindow(); wd.ShowDialog(); });
            CustomerCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CustomerWindow wd = new CustomerWindow(); wd.ShowDialog(); });
            ObjectCommand = new RelayCommand<object>((p) => { return true; }, (p) => { ObjectWindow wd = new ObjectWindow(); wd.ShowDialog(); });
            UserCommand = new RelayCommand<object>((p) => { return true; }, (p) => { UserWindow wd = new UserWindow(); wd.ShowDialog(); });
            InputInfoCommand = new RelayCommand<object>((p) => { return true; }, (p) => { InputInfoWindow wd = new InputInfoWindow(); wd.ShowDialog(); });
            OutputInfoCommand = new RelayCommand<object>((p) => { return true; }, (p) => { OutputInfoWindow wd = new OutputInfoWindow(); wd.ShowDialog(); });
            OutputCommand = new RelayCommand<object>((p) => { return true; }, (p) => { OutputWindow wd = new OutputWindow(); wd.ShowDialog(); });

            InventoryCommand = new RelayCommand<object>((x) =>
            {
                if (DateBeginInventory == null || DateEndInventory == null)
                    return false;
                return true;

            }, (x) =>

            {
                InventoryList.Clear();
                LoadInventoryData();

            });

            //MessageBox.Show(DataProvider.Ins.DB.Users.First().DisplayName);
        }

        void LoadInventoryData()
        {
            InventoryList = new ObservableCollection<Inventory>();
            ThongKe = new ThongKe();
            var objectList = DataProvider.Ins.DB.Object;

            int luongNhap = 0;
            int luongXuat = 0;
            double tongTienNhap = 0;
            double tongTienXuat = 0;
            double tongTienTon = 0;
            double tongTienLai = 0;

            int i = 1;
            foreach (var item in objectList)
            {
                var inputList = DataProvider.Ins.DB.InputInfo.Where(p => p.IdObject == item.Id);
                var outputList = DataProvider.Ins.DB.OutputInfo.Where(p => p.IdObject == item.Id);

                int sumInput = 0;
                int sumOutput = 0;
                double tienNhap = 0;
                double tienXuat = 0;

                if (inputList != null && inputList.Count() > 0)
                {
                    sumInput = (int)inputList.Sum(p => p.Count);
                    tienNhap = (double)inputList.Sum(p => p.InputPrice);
                    tienXuat = (double)inputList.Sum(p => p.OutputPrice);
                    luongNhap += sumInput;
                }

                if (outputList != null && outputList.Count() > 0)
                {
                    sumOutput = (int)outputList.Sum(p => p.Count);
                    luongXuat += sumOutput;
                }

                tongTienTon += (sumInput - sumOutput) * tienNhap;
                tongTienLai += sumOutput * (tienXuat - tienNhap);
                tongTienNhap += sumInput * tienNhap;
                tongTienXuat += sumOutput * tienXuat;

                Inventory inventory = new Inventory();
                inventory.STT = i;
                inventory.CountInput = sumInput;
                inventory.CountOutput = sumOutput;
                inventory.CountInventory = sumInput - sumOutput;
                inventory.MoneyInput = sumInput * tienNhap;
                inventory.MoneyOutput = sumOutput * tienXuat;
                inventory.MoneyInventory = (sumInput - sumOutput) * tienNhap;
                inventory.MoneyIncome = sumOutput * (tienXuat - tienNhap);
                inventory.Object = item;

                InventoryList.Add(inventory);

                i++;
            }

            ThongKe.LuongNhap = luongNhap;
            ThongKe.LuongXuat = luongXuat;
            ThongKe.GiaNhap = tongTienNhap;
            ThongKe.GiaXuat = tongTienXuat;
            ThongKe.LuongTon = luongNhap - luongXuat;
            ThongKe.GiaTon = tongTienTon;
            ThongKe.GiaLai = tongTienLai;
        }


       
    }
}