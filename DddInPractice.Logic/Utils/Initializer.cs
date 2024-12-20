using DddInPractice.Logic.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DddInPractice.Logic
{
    public static class Initializer
    {
        public static void Init(string connectionString)
        {
            SessionFactory.Init(connectionString);
            HeadOfficeInstance.Init();
        }
    }
}
