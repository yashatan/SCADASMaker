﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADACreator.Model
{
    public class DataProvider
    {
        private static DataProvider instance;
        public static DataProvider Instance { get { if (instance == null) instance = new DataProvider(); return instance; } set { instance = value; } }

        public SCADACreator_DBEntities DB { get; set; }
        public DataProvider() { 
            DB = new SCADACreator_DBEntities();
        }

    }
}
