using System;
using System.Collections.Generic;
using SemiconductorTestFramework.Core;

namespace SemiconductorTestFramework.TestSuites
{
    public static class VoltageTests
    {
        public static List<TestCase> GetVoltageTestSuite()
        {
            var tests = new List<TestCase>();

            var test1 = new TestCase("VOLT_001", "Minimum Supply Voltage (2.7V)", "Voltage");
            test1.TestExecutionLogic = (simulator) =>
            {
                simulator.SetPower(true);
                simulator.SetVoltage(2.7);
                if (simulator.VoltageVolts < 2.6 || simulator.VoltageVolts > 2.8)
                    throw new Exception("Voltage out of specification at minimum");
            };
            tests.Add(test1);

            var test2 = new TestCase("VOLT_002", "Nominal Supply Voltage (3.3V)", "Voltage");
            test2.TestExecutionLogic = (simulator) =>
            {
                simulator.SetPower(true);
                simulator.SetVoltage(3.3);
                if (simulator.VoltageVolts < 3.2 || simulator.VoltageVolts > 3.4)
                    throw new Exception("Voltage out of specification at nominal");
            };
            tests.Add(test2);

            var test3 = new TestCase("VOLT_003", "Maximum Supply Voltage (5.0V)", "Voltage");
            test3.TestExecutionLogic = (simulator) =>
            {
                simulator.SetPower(true);
                simulator.SetVoltage(5.0);
                if (simulator.VoltageVolts < 4.9 || simulator.VoltageVolts > 5.1)
                    throw new Exception("Voltage out of specification at maximum");
            };
            tests.Add(test3);

            var test4 = new TestCase("VOLT_004", "Voltage Ramp Test", "Voltage");
            test4.TestExecutionLogic = (simulator) =>
            {
                simulator.SetPower(true);
                for (double v = 2.7; v <= 5.0; v += 0.1)
                {
                    simulator.SetVoltage(v);
                    System.Threading.Thread.Sleep(20);
                }
            };
            tests.Add(test4);

            return tests;
        }
    }
}