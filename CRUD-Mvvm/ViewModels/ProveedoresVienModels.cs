using CRUD_MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CRUD_MVVM.ViewModels
{
    internal class ProveedoresVienModels : INotifyPropertyChanged
    {
        private DatabaseService _databaseService;
        private Proveedor _nuevoProveedor = new Proveedor();

        public ObservableCollection<Proveedor> Proveedores { get; set; }
        public Proveedor NuevoProveedor
        {
            get => _nuevoProveedor;
            set
            {
                _nuevoProveedor = value;
                OnPropertyChanged();
            }
        }

        public ICommand AgregarProveedorCommand { get; }
        public ICommand EliminarProveedorCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ProveedoresViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            Proveedores = new ObservableCollection<Proveedor>();
            AgregarProveedorCommand = new Command(async () => await AgregarProveedor());
            EliminarProveedorCommand = new Command<Proveedor>(async (proveedor) => await EliminarProveedor(proveedor));
            CargarProveedores();
        }

        private async void CargarProveedores()
        {
            var lista = await _databaseService.GetProveedoresAsync();
            foreach (var proveedor in lista)
                Proveedores.Add(proveedor);
        }

        private async Task AgregarProveedor()
        {
            if (string.IsNullOrWhiteSpace(NuevoProveedor.Nombre) ||
                string.IsNullOrWhiteSpace(NuevoProveedor.Direccion) ||
                string.IsNullOrWhiteSpace(NuevoProveedor.Telefono))
            {
                
                return;
            }

            await _databaseService.SaveProveedorAsync(NuevoProveedor);
            Proveedores.Add(NuevoProveedor);
            NuevoProveedor = new Proveedor(); 
        }

        private async Task EliminarProveedor(Proveedor proveedor)
        {
            await _databaseService.DeleteProveedorAsync(proveedor);
            Proveedores.Remove(proveedor);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

