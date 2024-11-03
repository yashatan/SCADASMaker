using SCADACreator.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SCADACreator
{
    public class SCADADataProvider
    {
        public List<TagInfo> TagInfos; 
        public List<ConnectDevice> ConnectDevices; 
        private static SCADADataProvider instance;

        // Private constructor to prevent instantiation
        private SCADADataProvider()
        {
            TagInfos = new List<TagInfo>();
            ConnectDevices = new List<ConnectDevice>();
        }

        // Public static method to get the single instance of the class
        public static SCADADataProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SCADADataProvider();
                }
                return instance;
            }
        }
    }
}
