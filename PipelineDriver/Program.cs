using IViewNet.Common;
using IViewNet.Common.Models;
using IViewNet.Pipes;
using System;

namespace PipelineDriver
{
    class Program
    {
        private static PipeConfig PipeCinfig;
        private static IViewPipeServer ServerPipe;
        static void Main(string[] args)
        {
            PipeCinfig = new PipeConfig(1024 * 100, 1);
            ServerPipe = new IViewPipeServer(PipeCinfig);
            ServerPipe.PipeConnectedEvent += SetOnPipeConnected;
            ServerPipe.PipeReceivedEvent += SetOnPipeReceived;
            ServerPipe.PipeSentEvent += SetOnPipeSent;
            ServerPipe.PipeClosedEvent += SetOnPipeClosed;
            ServerPipe.PipeExceptionEvent += SetOnPipeException;
            ServerPipe.PacketManager = CreatePacketManager();
            StartPipeline();
            Console.ReadKey();
        }
        private static PacketManager CreatePacketManager()
        {
            PacketManager PacketManager = new PacketManager();
            PacketManager.AddPacket(new Packet(1111, "SetDetectionType", null));
            PacketManager.AddPacket(new Packet(1112, "SetOrientation", null));
            PacketManager.AddPacket(new Packet(1113, "GetDetectedFrame", null));
            PacketManager.AddPacket(new Packet(1114, "SetDetectedFrame", null));
            PacketManager.AddPacket(new Packet(1115, "EndOfFrame", null));
            return PacketManager;
        }
        private static void SetOnPipeConnected()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("End Pipe Connected Successfully");
            ServerPipe.SendMessage(new Packet(1113, "GetDetectedFrame", new byte[1024 * 2]));
        }
        private static void SetOnPipeReceived(Packet Message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(string.Format("Pipe Received From PyExecutor: {0}", Message.Name));
            ServerPipe.SendMessage(new Packet(1115, "EndOfFrame", new byte[1024 * 2]));
        }
        private static void SetOnPipeSent(Packet Message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(string.Format("Pipe Sent To PyExecutor: {0}", Message.Name));
        }
        private static void SetOnPipeClosed()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("End Pipe Closed Successfully");
        }
        private static void SetOnPipeException(Exception Error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("Pipe Exception: {0}", Error.Message));
        }
        public static void StartPipeline()
        {
            if (ServerPipe.IsPipeConnected == false)
            {
                ServerPipe.StartPipeServer();
            }
        }
        public static void StopPipeline()
        {
            if (ServerPipe.IsPipeShutdown == false)
            {
                ServerPipe.ClosePipeServer();
            }
        }
    }
}