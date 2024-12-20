using DddInPractice.Logic.ATM;
using DddInPractice.Logic.Common;
using System;

namespace DddInPractice.Logic.Management
{
    public class BalanceChangedEventHandler : IHandler<BalanceChangedEvent>
    {
        public void Handle(BalanceChangedEvent domainEvent)
        {
            var repository = new HeadOfficeRepository();
            var headOffice = HeadOfficeInstance.Instance;
            headOffice.ChangeBalance(domainEvent.Delta);
            repository.Save(headOffice);
        }
    }
}
