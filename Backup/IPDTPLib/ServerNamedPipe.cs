using System;
using System.Threading;
using System.IO;
using IPDTP.IPC;
using IPDTP.COM;

namespace IPDTPLib
{
    public class PacketReceivedArgs
    {
        public IPDTPacket Packet;
        public string PacketCapsule;
        public PacketReceivedArgs(IPDTPacket p, string pcap)
        {
            Packet = p;
            PacketCapsule = pcap;
        }
    }
    public delegate void PacketReceivedHandler(PacketReceivedArgs e);
	public sealed class ServerNamedPipe : IDisposable {
		internal Thread PipeThread;
		internal ServerPipeConnection PipeConnection;
		internal bool Listen = true;
		internal DateTime LastAction;
		private bool disposed = false;
        public event PacketReceivedHandler PacketReceived;
        void OnPR(PacketReceivedArgs e)
        {
            if (PacketReceived != null)
                PacketReceived(e);
        }
		private void PipeListener() {
			CheckIfDisposed();
			try {
				Listen = IPDTPApplication.PipeManager.Listen;
                IPDTPApplication.ChatHistory.Add("Pipe " + this.PipeConnection.NativeHandle.ToString() + ": new pipe started");
				while (Listen) {
					LastAction = DateTime.Now;
					string request = PipeConnection.Read();
					LastAction = DateTime.Now;
					if (request.Trim() != "") {
                        PacketReceivedArgs es = new PacketReceivedArgs(PacketBuilder.Open(request), request);
                        OnPR(es);
                        IPDTPApplication.ChatHistory.Add("Pipe " + this.PipeConnection.NativeHandle.ToString() + ": request handled");
					}
					else {
						PipeConnection.Write("ERROR 51 BAD REQUEST");
					}
					LastAction = DateTime.Now;
					PipeConnection.Disconnect();
					if (Listen) {
						Connect();
					}
                    IPDTPApplication.PipeManager.WakeUp();
				}
			}
            catch (System.Threading.ThreadAbortException ex) { Log.Write(ex); }
            catch (System.Threading.ThreadStateException ex) { Log.Write(ex); }
			catch (Exception ex) { 
				// Log exception
            
			}
			finally {
				this.Close();
			}
		}
		internal void Connect() {
			CheckIfDisposed();
			PipeConnection.Connect();
		}
		internal void Close() {
			CheckIfDisposed();
			this.Listen = false;
            IPDTPApplication.PipeManager.RemoveServerChannel(this.PipeConnection.NativeHandle);
			this.Dispose();
		}
		internal void Start() {
			CheckIfDisposed();
			PipeThread.Start();
		}
		private void CheckIfDisposed() {
			if(this.disposed) {
				throw new ObjectDisposedException("ServerNamedPipe");
			}
		}
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void Dispose(bool disposing) {
			if(!this.disposed) {
				PipeConnection.Dispose();
				if (PipeThread != null) {
					try {
						PipeThread.Abort();
					} 
					catch (System.Threading.ThreadAbortException ex) { }
					catch (System.Threading.ThreadStateException ex) { }
					catch (Exception ex) {
						// Log exception
					}
				}
			}
			disposed = true;         
		}
		~ServerNamedPipe() {
			Dispose(false);
		}
		internal ServerNamedPipe(string name, uint outBuffer, uint inBuffer, int maxReadBytes, bool secure) {
			PipeConnection = new ServerPipeConnection(name, outBuffer, inBuffer, maxReadBytes, secure);
			PipeThread = new Thread(new ThreadStart(PipeListener));
			PipeThread.IsBackground = true;
			PipeThread.Name = "Pipe Thread " + this.PipeConnection.NativeHandle.ToString();
			LastAction = DateTime.Now;
		}
	}
}