using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DddInPractice.Logic.Management
{
    public class HeadOfficeInstance
    {
        private const long HeadOfficeId = 1L;

        public static HeadOffice Instance { get; private set; }

        public static void Init()
        {
            if (Instance == null)
            {
                var repository = new HeadOfficeRepository();
                Instance = repository.GetById(HeadOfficeId);
            }
        }
    }
}
