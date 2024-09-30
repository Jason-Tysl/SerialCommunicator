using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace COM.Messaging
{
    /// <summary>
    /// A class for receiving messages on another thread
    /// </summary>
    public class MessageReceiver
    {
        private Thread ReceiverThread { get; set; }

        public bool CanReceive { get; set; }
        public bool ShouldShutDownPermanently { get; set; }

        public SerialPort Port { get; set; }

        // Easier to hold a reference of the MessagesViewModel instead of creating a few callback methods
        public MessagesViewModel Messages { get; set; }

        public MessageReceiver()
        {
            CanReceive = true;
            ShouldShutDownPermanently = false;

            ReceiverThread = new Thread(ReceiveLoop);
            ReceiverThread.Start();
        }

        /// <summary>
        /// Constantly runs through the entire duration of the program
        /// </summary>
        private void ReceiveLoop()
        {
            string message = "";
            char read;

            while (true)
            {
                if (ShouldShutDownPermanently)
                {
                    // stop loop
                    return;
                }

                // used for pausing/resuming the receiver loop
                if (CanReceive)
                {
                    if (Port != null && Port.IsOpen)
                    {
                        while(Port.BytesToRead > 0)
                        {
                            read = (char)Port.ReadChar();
                            switch (read)
                            {
                                case '\r':
                                    break;
                                case '\n':
                                    // newline means new message
                                    Messages.AddReceivedMessage(message);
                                    message = "";
                                    break;
                                default:
                                    // add the char to the message
                                    message += read;
                                    break;
                            }
                        }

                        // Sleeps to slow down CPU usage
                        Thread.Sleep(1);
                    }
                }

                // Sleeps to slow down CPU usage
                Thread.Sleep(1);
            }
        }

        public void StopThreadLoop()
        {
            CanReceive = false;
            ShouldShutDownPermanently = true;
            ReceiverThread.Abort();
        }
    }
}
