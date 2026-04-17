using System;
using System.Collections.Generic;
using SemiconductorTestFramework.Core;

namespace SemiconductorTestFramework.TestSuites
{
    public static class SystemTests
    {
        public static List<TestCase> GetSystemTestSuite()
        {
            var tests = new List<TestCase>();

            var test1 = new TestCase("SYS_001", "Register Write/Read Test", "System");
            test1.TestExecutionLogic = (simulator) =>
            {
                simulator.SetPower(true);
                simulator.WriteRegister("CONFIG", 0xABCD);
                var value = simulator.ReadRegister("CONFIG");
                if ((int)value != 0xABCD)
                    throw new Exception("Register read/write verification failed");
            };
            tests.Add(test1);

            var test2 = new TestCase("SYS_002", "Device Status Monitoring", "System");
            test2.TestExecutionLogic = (simulator) =>
            {
                simulator.SetPower(true);
                simulator.SetVoltage(3.3);
                simulator.SetTemperature(50);
                simulator.SetFrequency(100);
                string status = simulator.GetStatus();
                if (string.IsNullOrEmpty(status))
                    throw new Exception("Status retrieval failed");
            };
            tests.Add(test2);

            var test3 = new TestCase("SYS_003", "Continuous Operation Stress Test", "System");
            test3.TimeoutMs = 10000;
            test3.TestExecutionLogic = (simulator) =>
            {
                simulator.SetPower(true);
                for (int i = 0; i < 100; i++)
                {
                    simulator.SetVoltage(3.3);
                    simulator.SetCurrent(1.0);
                    simulator.SetFrequency(100);
                    simulator.SimulatePowerDissipation();
                    simulator.SimulateSignalIntegrity();
                    System.Threading.Thread.Sleep(10);
                }
            };
            tests.Add(test3);

            var test4 = new TestCase("SYS_004", "Multi-Register Operations", "System");
            test4.TestExecutionLogic = (simulator) =>
            {
                simulator.SetPower(true);
                for (int i = 0; i < 20; i++)
                {
                    simulator.WriteRegister($"REG_{i}", i * 0x1000);
                }
                for (int i = 0; i < 20; i++)
                {
                    var value = simulator.ReadRegister($"REG_{i}");
                    if ((int)value != i * 0x1000)
                        throw new Exception($"Register {i} verification failed");
                }
            };
            tests.Add(test4);

            return tests;
        }
    }
}