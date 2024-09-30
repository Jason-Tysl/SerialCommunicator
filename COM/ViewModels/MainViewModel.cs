using COM.Messaging;
using COM.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using COM.Serial;

namespace COM.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MessagesViewModel Messages { get; set;}

        //To go in SerialPortViewModel
        public MessageReceiver Receiver { get; set; }
        public MessageSender Sender { get; set; }

        public SerialPortViewModel SerialPort { get; set; }

        public MainViewModel()
        {
            SerialPort = new SerialPortViewModel();
            Receiver = new MessageReceiver();
            Sender = new MessageSender();
            Messages = new MessagesViewModel(Sender);

            Receiver.Messages = Messages;
            Messages.Sender = Sender;
            Sender.Messages = Messages;


            SerialPort.Receiver = Receiver;
            SerialPort.Sender = Sender;
            SerialPort.Messages = Messages;

            Receiver.Port = SerialPort.Port;
            Sender.Port = SerialPort.Port;
        }
    }
}
