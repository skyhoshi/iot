using System;
using System.Device.Gpio;
using System.Threading;
using Iot.Device.ShiftRegister;

namespace shift_register
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var controller = new GpioController();
            var sr = new Sn74hc595(Sn74hc595.PinMapping.Standard, controller,true,1);

            var cancellationSource = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cancellationSource.Cancel();
                sr.Clear();
            };



            sr.Clear();

            // write values and latch
            for (int i = 0; i < sr.Length; i++)
            {
                sr.Shift(i);
            }
            sr.Latch();
            Console.WriteLine("Write values");
            Console.ReadLine();
            sr.Clear();

            Console.WriteLine("Write more values");
            sr.ShiftAndLatch(23);
            Console.ReadLine();
            sr.Clear();

            Console.WriteLine("Write 255 to each register");
            for (int i = 0; i < sr.Length; i++)
            {
                sr.ShiftAndLatch(255);
            }
            Console.ReadLine();
            sr.Clear();


            sr.OutputDisable();
            Console.WriteLine("disable");
            Console.ReadLine();

            Console.WriteLine("enable");
            sr.OutputEnable();
            Console.ReadLine();

            if (cancellationSource.IsCancellationRequested)
            {
                return;
            }

            for (int i = 0; i < 8; i++)
            {
                sr.Shift(1);
                sr.Shift(0);
                sr.Latch();
                Thread.Sleep(500);
            }
            
            Thread.Sleep(1000);

            for (int i = 0; i < 255; i++)
            {
                Console.WriteLine($"Index: {i}");
                Thread.Sleep(500);
                sr.ShiftAndLatch((byte)i);
                Thread.Sleep(500);

                if (cancellationSource.IsCancellationRequested)
                {
                    Console.WriteLine("Cancelled Requested");
                    sr.Clear();
                    Console.WriteLine("Cancel acknowleged");
                    break;
                }
            }
            
            sr.Clear();
            Console.WriteLine("done");
            
/*

            while (!cancellationSource.IsCancellationRequested)
            {
                for (int i = 0; i < 8; i++)
                {
                    controller.Write(SER,PinValue.High);
                    controller.Write(SRCLK,PinValue.High);
                    controller.Write(SER,PinValue.Low);
                    controller.Write(SRCLK,PinValue.Low);

                    controller.Write(RCLK,PinValue.High);
                    controller.Write(RCLK,PinValue.Low);

                    Thread.Sleep(100);
                }
                Thread.Sleep(500);
            }

            for (int i = 0; i < 8; i++)
            {
                controller.Write(SER,PinValue.Low);
                controller.Write(SRCLK,PinValue.High);
                controller.Write(SRCLK,PinValue.Low);
            }

            controller.Write(RCLK,PinValue.High);
            */
        }
    }
}
