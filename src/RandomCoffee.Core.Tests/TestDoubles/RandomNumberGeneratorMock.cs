using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace RandomCoffee.Core.Tests.TestDoubles
{
    public class RandomNumberGeneratorMock : RandomNumberGenerator
    {
        private readonly List<double> _numbers = new ();
        private int _timesCalled;

        public void Add(double number) =>
            _numbers.Add(number);

        public override void GetBytes(byte[] data)
        {
            _timesCalled++;
            var bytes = BitConverter.GetBytes(_numbers[_timesCalled - 1]);

            for (int i = 0; i < data.Length; i++)
                data[i] = bytes[i];
        }
    }
}
