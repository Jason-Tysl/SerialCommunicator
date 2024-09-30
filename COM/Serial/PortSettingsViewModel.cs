using COM.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM.Serial
{
    /// <summary>
    /// Could be used in the future to bind the serial port's settings to the view. (Buad rate, dataBits, etc.)
    /// </summary>
    public class PortSettingsViewModel : BaseViewModel
    {
        private string _selectedComPort;

        public string SelectedComPort
        {
            get => _selectedComPort;
            set => RaisePropertyChanged(ref _selectedComPort, value);
        }

        public ObservableCollection<string> AvailablePorts { get; set; }

        public int BaudRate = 9600;

        public int DataBits = 0;

        public StopBits StopBits = StopBits.None;

        public Parity Parity = Parity.None;

        public Handshake Handshake = Handshake.None;

        public Command RefreshPortsCommand { get; }

        public PortSettingsViewModel() 
        {
            AvailablePorts = new ObservableCollection<string>();

            RefreshPortsCommand = new Command(RefreshPorts);
        }

        private void RefreshPorts()
        {
            AvailablePorts.Clear();
            foreach(string port in SerialPort.GetPortNames())
            {
                AvailablePorts.Add(port);
            }
        }
    }
}
