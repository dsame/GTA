using System;
using EG.Sherpany.Boardroom.Interfaces;

namespace EG.Sherpany.Boardroom.Tests.Fakes
{
    class FakeDateTimeProvider :IDateTimeProvider
    {
        private readonly DateTime _currentTime;

        public FakeDateTimeProvider(DateTime currentTime)
        {
            _currentTime = currentTime;
        }

        public bool ElapsedLessThan(DateTime dateTime, TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }

        public bool ElapsedMoreThan(DateTime dateTime, TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }

        public DateTime GetCurrentDateTime() => _currentTime;
    }
}
