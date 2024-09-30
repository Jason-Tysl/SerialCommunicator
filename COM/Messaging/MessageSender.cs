using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM.Messaging
{
    public class MessageSender
    {
        public bool CanSend { get; set; }
        public SerialPort Port { get; set; }
        public MessagesViewModel Messages { get; set; }

        public MessageSender()
        {
            CanSend = true;
        }
        /// <summary>
        /// Sends a message through the serial port. 
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message, bool shouldSendNewLine = true)
        {
            // acts as if statement; if shouldSendNewLine==true, add a newline, otherwise add nothing
            string newMessage = message + (shouldSendNewLine ? "\n" : "");

            byte[] buffer = Port.Encoding.GetBytes(newMessage);

            for (int i = 0; i < buffer.Length; i++)
            {
                Port.BaseStream.WriteByte(buffer[i]);
            }
        }
    }
}
