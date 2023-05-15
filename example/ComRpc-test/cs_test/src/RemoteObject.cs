using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace ComRpc
{
	public partial class RemoteObject
	{
        const int buff_size=100;
        const int read_wait = 100;
        private Stream stream;
        Queue<byte[]> lines = new Queue<byte[]>();
        List<byte> cLine=new List<byte>();
        byte[] buffer = new byte[buff_size];
        bool isWaiting = false;
        Int64 timeout = 10 * TimeSpan.TicksPerMillisecond * 1000;

        public RemoteObject(Stream stream, int timeout)
        {
            this.stream = stream;
            this.timeout = timeout * TimeSpan.TicksPerMillisecond * 1000;
        }
        
        public void BytesReceived(IAsyncResult ar)
        {
            try
            {
                int actualLength = stream.EndRead(ar);
                for (int i = 0; i < actualLength; i++)
                {
                    switch (buffer[i])
                    {
                        case 10:
                        case 13:
                            if (cLine.Count > 0)
                            {
                                string foo = Encoding.Default.GetString(cLine.ToArray());
                                lines.Enqueue(cLine.ToArray());
                                cLine.Clear();
                            }
                            break;
                        default:
                            cLine.Add(buffer[i]);
                            break; 
                    }
                }
                isWaiting = false;
            }
            catch (IOException exc)
            {
               
            }
        }
        
        private byte[] ReadLineBytes()
        {
            Int64 t0 = DateTime.Now.Ticks;
            while (0 == lines.Count)
            {
                if (DateTime.Now.Ticks - t0 > timeout) throw new Exception(this.GetType().ToString() + "::ReadLineBytes:timeout");
                if (!isWaiting)
                {
                    isWaiting = true;
                    stream.BeginRead(buffer, 0, buffer.Length, BytesReceived, 0);
                }
//                Application.DoEvents();
            }
            return lines.Dequeue();
        }

        private void FlushInputBuffer() {
            if (isWaiting)
            {
                lines.Clear();
                cLine.Clear();
            }
            else
            {
                isWaiting = true;
                stream.BeginRead(buffer, 0, buffer.Length, BytesReceived, 0);
                Thread.Sleep(read_wait);
                FlushInputBuffer();
            }
        }

    }
}