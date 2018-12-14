using QuanLyCuaHang.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLyCuaHang.ViewModel
{
    class InputInfoViewModel : BaseViewModel
    {
        private ObservableCollection<Model.InputInfo> _List;
        public ObservableCollection<Model.InputInfo> List { get => _List; set { _List = value; OnPropertyChanged(); } }        

        private Model.InputInfo _SelectedItem;
        public Model.InputInfo SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    Object = SelectedItem.Object;
                    Input = SelectedItem.Input;
                    Count = SelectedItem.Count;
                    Status = SelectedItem.Status;
                    InputPrice = (int)SelectedItem.InputPrice;
                    OutputPrice = (int)SelectedItem.OutputPrice;
                }
            }
        }

        private int? _Count;
        public int? Count { get => _Count; set { _Count = value; OnPropertyChanged(); } }

        private Model.Object _Object;
        public Model.Object Object { get => _Object; set { _Object = value; OnPropertyChanged(); } }

        private Model.Input _Input;
        public Model.Input Input { get => _Input; set { _Input = value; OnPropertyChanged(); } }        

        private int? _InputPrice;
        public int? InputPrice { get => _InputPrice; set { _InputPrice = value; OnPropertyChanged(); } }


        private int? _OutputPrice;
        public int? OutputPrice { get => _OutputPrice; set { _OutputPrice = value; OnPropertyChanged(); } }

        private string _Status;
        public string Status { get => _Status; set { _Status = value; OnPropertyChanged(); } }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public InputInfoViewModel()
        {
            List = new ObservableCollection<Model.InputInfo>(DataProvider.Ins.DB.InputInfoes);


            AddCommand = new RelayCommand<object>((p) =>
            {
                if (Object == null || Input == null)
                    return false;
                return true;

            }, (p) =>

            {
                var InputInfo = new Model.InputInfo() { IdObject = Object.Id, IdInput = Input.Id, Count = Count, InputPrice = InputPrice, OutputPrice = OutputPrice, Status = Status };

                DataProvider.Ins.DB.InputInfoes.Add(InputInfo);
                DataProvider.Ins.DB.SaveChanges();

                List.Add(InputInfo);
            });

            EditCommand = new RelayCommand<InputInfo>((p) =>
            {
                if (SelectedItem == null || Object == null || Input == null)
                    return false;

                var displayList = DataProvider.Ins.DB.InputInfoes.Where(x => x.Id == SelectedItem.Id);
                if (displayList != null && displayList.Count() != 0)
                    return true;
                return false;

            }, (p) =>
            {
                var InputInfo = DataProvider.Ins.DB.InputInfoes.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                InputInfo.Object = Object;
                InputInfo.Input = Input;
                InputInfo.Count = Count;
                InputInfo.InputPrice = InputPrice;
                InputInfo.OutputPrice = OutputPrice;
                DataProvider.Ins.DB.SaveChanges();              
            });
        }
    }
}
