using COM.Messaging;
using COM.Utilities;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM.Serial
{
    public class SerialPortViewModel : BaseViewModel
    {
        private string _connectedPort;

        public string ConnectedPort
        {
            get => _connectedPort;
            set => RaisePropertyChanged(ref _connectedPort, value);
        }

        public SerialPort Port { get; set; }

        private bool _isConnected;

        public bool IsConnected 
        { 
            get => _isConnected;
            set => RaisePropertyChanged(ref _isConnected, value);
        }

        public void CloseAll()
        {
            Disconnect();
            Receiver.StopThreadLoop();
        }

        public Command AutoConnectDisconnectCommand { get; }

        public Command ClearBuffersCommand { get; }

        public PortSettingsViewModel Settings { get; set; }

        public MessagesViewModel Messages { get; set; }

        public MessageReceiver Receiver { get; set; }

        public MessageSender Sender { get; set; }


        public SerialPortViewModel()
        {
            Port = new SerialPort();
            Port.ReadTimeout = 1000;
            Port.WriteTimeout = 1000;
            Settings = new PortSettingsViewModel();

            AutoConnectDisconnectCommand = new Command(AutoConnectDisconnect);
            ClearBuffersCommand = new Command(ClearBuffers);
        }


        private void AutoConnectDisconnect()
        {
            if (Port.IsOpen)
            {
                Disconnect();
            } else
            {
                Connect();
            }
        }

        public void Connect()
        {
            IsConnected = Port.IsOpen;
            if (IsConnected)
            {
                Messages.AddMessage("Port is already open.");
                return;
            }

            // system com port so it can't be used
            if (Settings.SelectedComPort == "COM1")
            {
                Messages.AddMessage("Cannot use COM1.");
                return;
            }

            if (string.IsNullOrEmpty(Settings.SelectedComPort))
            {
                Messages.AddMessage("Error with the COM port");
            }

            Port.PortName = Settings.SelectedComPort;

            try
            {
                Port.Open();
            } catch (Exception e)
            {
                Messages.AddMessage("Error opening port: " + e.StackTrace);
            }

            ConnectedPort = Settings.SelectedComPort;
            Messages.AddMessage($"Connected to: {ConnectedPort}.");

            Receiver.CanReceive = true;
            IsConnected = Port.IsOpen;

        }

        public void Disconnect()
        {

            IsConnected = Port.IsOpen;
            if (!IsConnected)
            {
                Messages.AddMessage("Port is already closed.");
                return;
            }

            try
            {
                Port.Close();
            }
            catch (Exception e)
            {
                Messages.AddMessage("Error closing port: " + e.StackTrace);
            }


            Messages.AddMessage($"Disconnected from: {ConnectedPort}."); 
            ConnectedPort = "(None)";
            
            Receiver.CanReceive = false;
        }

        private void ClearBuffers()
        {
            if (!Port.IsOpen)
            {
                Messages.AddMessage("You need to be connected to clear the buffers.");
                return;
            }

            Port.DiscardInBuffer();
            Port.DiscardOutBuffer();
        }
    }
}
