using System;
using System.Collections.Generic;
using SemiconductorTestFramework.Core;

namespace SemiconductorTestFramework.TestSuites
{
    public static class PowerTests
    {
        public static List<TestCase> GetPowerTestSuite()
        {
            var tests = new List<TestCase>();

            var test1 = new TestCase("POWER_001", "Power-On Sequence", "Power");
            test1.TestExecutionLogic = (simulator) =>
            {
                simulator.SetPower(false);
                System.Threading.Thread.Sleep(100);
                simulator.SetPower(true);
                if (!simulator.PowerEnabled)
                    throw new Exception("Power failed to enable");
                if (simulator.VoltageVolts == 0)
                    throw new Exception("Voltage not established after power-on");
            };
            tests.Add(test1);

            var test2 = new TestCase("POWER_002", "Power-Off Sequence", "Power");
            test2.TestExecutionLogic = (simulator) =>
            {
                simulator.SetPower(true);
                simulator.SetVoltage(3.3);
                simulator.SetCurrent(1.5);
                System.Threading.Thread.Sleep(100);
                simulator.SetPower(false);
                if (simulator.PowerEnabled)
                    throw new Exception("Power failed to disable");
            };
            tests.Add(test2);

            var test3 = new TestCase("POWER_003", "Current Draw Under Load", "Power");
            test3.TestExecutionLogic = (simulator) =>
            {
                simulator.SetPower(true);
                simulator.SetVoltage(3.3);
                for (double current = 0; current <= 5.0; current += 0.5)
                {
                    simulator.SetCurrent(current);
                    simulator.SimulatePowerDissipation();
                    System.Threading.Thread.Sleep(50);
                }
            };
            tests.Add(test3);

            var test4 = new TestCase("POWER_004", "Power Cycling Test", "Power");
            test4.TestExecutionLogic = (simulator) =>
            {
                for (int i = 0; i < 10; i++)
                {
                    simulator.SetPower(true);
                    System.Threading.Thread.Sleep(50);
                    simulator.SetPower(false);
                    System.Threading.Thread.Sleep(50);
                }
                simulator.SetPower(true);
                if (!simulator.PowerEnabled)
                    throw new Exception("Device failed to recover after power cycling");
            };
            tests.Add(test4);

            return tests;
        }
    }
}