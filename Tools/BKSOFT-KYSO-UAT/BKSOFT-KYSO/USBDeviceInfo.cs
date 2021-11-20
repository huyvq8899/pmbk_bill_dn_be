using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BKSOFT_KYSO
{
    public class USBDeviceInfo
    {
        public string DeviceID { get; private set; }
        public string PnpDeviceID { get; private set; }
        public string Description { get; private set; }

        public USBDeviceInfo(string deviceID, string pnpDeviceID, string description)
        {
            this.DeviceID = deviceID;
            this.PnpDeviceID = pnpDeviceID;
            this.Description = description;
        }
    }
}
