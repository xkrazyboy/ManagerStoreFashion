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

        private ObservableCollection<Model.Object> _Object;
        public ObservableCollection<Model.Object> Object { get => _Object; set { _Object = value; OnPropertyChanged(); } }

        private ObservableCollection<Model.Input> _Input;
        public ObservableCollection<Model.Input> Input { get => _Input; set { _Input = value; OnPropertyChanged(); } }

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
                    SelectedObject = SelectedItem.Object;
                    SelectedInput = SelectedItem.Input;
                    Count = SelectedItem.Count;
                    Status = SelectedItem.Status;
                    InputPrice = SelectedItem.InputPrice;
                    OutputPrice = SelectedItem.OutputPrice;
                }
            }
        }

        private Model.Object _SelectedObject;
        public Model.Object SelectedObject
        {
            get => _SelectedObject;
            set
            {
                _SelectedObject = value;
                OnPropertyChanged();
            }
        }

        private Model.Input _SelectedInput;
        public Model.Input SelectedInput
        {
            get => _SelectedInput;
            set
            {
                _SelectedInput = value;
                OnPropertyChanged();
            }
        }

        //private Model.Input _DateInput;
        //public Model.Input DateInput { get => _DateInput; set { _DateInput = value; OnPropertyChanged(); } }

        private int? _Count;
        public int? Count { get => _Count; set { _Count = value; OnPropertyChanged(); } }              

        private double? _InputPrice;
        public double? InputPrice { get => _InputPrice; set { _InputPrice = value; OnPropertyChanged(); } }

        private double? _OutputPrice;
        public double? OutputPrice { get => _OutputPrice; set { _OutputPrice = value; OnPropertyChanged(); } }

        private string _Status;
        public string Status { get => _Status; set { _Status = value; OnPropertyChanged(); } }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public InputInfoViewModel()
        {
            List = new ObservableCollection<Model.InputInfo>(DataProvider.Ins.DB.InputInfoes);
            Object = new ObservableCollection<Model.Object>(DataProvider.Ins.DB.Objects);
            Input = new ObservableCollection<Model.Input>(DataProvider.Ins.DB.Inputs);

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedObject == null || SelectedInput == null)
                    return false;
                return true;

            }, (p) =>

            {
                var NewInput = new Model.Input() { Id = Guid.NewGuid().ToString(), DateInput = SelectedInput.DateInput };
                var InputInfo = new Model.InputInfo() { IdObject = SelectedObject.Id, IdInput = NewInput.Id, Count = Count, InputPrice = InputPrice, OutputPrice = OutputPrice, Status = Status, Id = Guid.NewGuid().ToString() };
                
                DataProvider.Ins.DB.Inputs.Add(NewInput);
                DataProvider.Ins.DB.InputInfoes.Add(InputInfo);
                DataProvider.Ins.DB.SaveChanges();

                List.Add(InputInfo);
            });

            EditCommand = new RelayCommand<InputInfo>((p) =>
            {
                if (SelectedItem == null || SelectedObject == null || SelectedInput == null)
                    return false;

                var displayList = DataProvider.Ins.DB.InputInfoes.Where(x => x.Id == SelectedItem.Id);
                if (displayList != null && displayList.Count() != 0)
                    return true;
                return false;

            }, (p) =>
            {
                var InputInfo = DataProvider.Ins.DB.InputInfoes.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                InputInfo.IdObject = SelectedObject.Id;
                InputInfo.IdInput = SelectedInput.Id;

                //InputInfo.IdInput = Input.Id;
                InputInfo.Count = Count;
                InputInfo.InputPrice = InputPrice;
                InputInfo.OutputPrice = OutputPrice;
                InputInfo.Status = Status;
                DataProvider.Ins.DB.SaveChanges();              
            });
        }
    }
}
