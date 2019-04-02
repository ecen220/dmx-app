using System;
using LibUsbDotNet;
using LibUsbDotNet.Main;

namespace dmxapp
{
    public class DMX : IDisposable
    {
        /// Manufacturer string in USB device
        public static string MANUFACTURER = "www.anyma.ch";

        /// Product string in device
        public static string PRODUCT = "uDMX";

        //private UsbDevice _device;
        public static int vID = 0x16c0;
        public static int pID = 0x05dc;

        public static UsbDevice _device;

        enum Command : byte
        {
            SetSingleChannel = 1,
            SetChannelRange = 2,
            StartBootloader = 0x32
        }

        public DMX()
        {
            // Find device by vID and pID
            UsbDeviceFinder finder = new UsbDeviceFinder(vID, pID);
            _device = UsbDevice.OpenUsbDevice(finder);

            if (_device != null)
            {
                // Verify if it's really uDMX
                if (!(_device.Info.ManufacturerString == MANUFACTURER &&
                  _device.Info.ProductString == PRODUCT))
                {
                    _device = null;
                }
            }
        }

        public static bool ScanForDMX()
        {
            UsbDeviceFinder finder = new UsbDeviceFinder(DMX.vID, DMX.pID);

            var reg = UsbDevice.AllDevices.Find(finder);
            return (reg != null);

        }

        /// <summary>
        /// Gets a value indicating whether device is open.
        /// </summary>
        /// <value><c>true</c> if device is open; otherwise, <c>false</c>.</value>
        public bool IsOpen
        {
            get
            {
                return (_device != null && _device.IsOpen);
            }
        }

        /// <summary>
        /// Sets the single channel.
        /// </summary>
        /// <returns><c>true</c>, if single channel was set, <c>false</c> otherwise.</returns>
        /// <param name="channel">Channel.</param>
        /// <param name="value">Value.</param>
        public bool SetSingleChannel(short channel, byte value)
        {
            return SendCommand(Command.SetSingleChannel, value, channel, null);
        }

        /// <summary>
        /// Sets the channel range.
        /// </summary>
        /// <returns><c>true</c>, if channel range was set, <c>false</c> otherwise.</returns>
        /// <param name="channel">Starting channel</param>
        /// <param name="data">Channels values</param>
        public bool SetChannelRange(short channel, byte[] values)
        {
            return SendCommand(Command.SetChannelRange, (short)values.Length, channel, values);
        }

        /// <summary>
        /// Starts the bootloader.
        /// </summary>
        /// <returns><c>true</c>, if bootloader was started, <c>false</c> otherwise.</returns>
        public bool StartBootloader()
        {
            return SendCommand(Command.StartBootloader, 0, 0, null);
        }

        /// <summary>
        /// Releases all resource used by the <see cref="UsbTest.uDMX"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="uDMX"/>. The <see cref="Dispose"/>
        /// method leaves the <see cref="uDMX"/> in an unusable state. After calling <see cref="Dispose"/>, you must
        /// release all references to the <see cref="UsbTest.uDMX"/> so the garbage collector can reclaim the memory that the
        /// <see cref="uDMX"/> was occupying.</remarks>
        public void Dispose()
        {
            if (_device != null)
                _device.Close();

            UsbDevice.Exit();
        }

        /// <summary>
        /// Sends command to uDMX
        /// </summary>
        /// <returns><c>true</c>, if command was sent, <c>false</c> otherwise.</returns>
        /// <param name="command">Command.</param>
        /// <param name="cvalue">Cvalue.</param>
        /// <param name="cindex">Cindex.</param>
        /// <param name="buffer">Buffer.</param>
        private bool SendCommand(Command command, short cvalue, short cindex, byte[] buffer)
        {
            bool result = false;
            int transfered;

            UsbSetupPacket packet = new UsbSetupPacket();
            // This is alegedly ignored by the uDMX, but let's play nice
            packet.RequestType = (byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointOut;
            packet.Request = (byte)command;
            packet.Value = cvalue;
            packet.Index = cindex;
            packet.Length = cvalue;

            // create empty buffer if the buffer is null
            if (buffer == null)
                buffer = new byte[0];

            if (_device != null)
            {
                // Send data and get the result
                if (_device.ControlTransfer(ref packet, buffer, buffer.Length, out transfered))
                {
                    result = true;
                }
            }


            return result;
        }



    }
}
