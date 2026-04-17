using System;
using System.Collections.Generic;
using SemiconductorTestFramework.Core;

namespace SemiconductorTestFramework.TestSuites
{
    public static class TemperatureTests
    {
        public static List<TestCase> GetTemperatureTestSuite()
        {
            var tests = new List<TestCase>();

            var test1 = new TestCase("TEMP_001", "Low Temperature Operation (-40°C)", "Temperature");
            test1.TestExecutionLogic = (simulator) =>
            {
                simulator.SetPower(true);
                simulator.SetTemperature(-40);
                simulator.SimulatePowerDissipation();
                if (simulator.TemperatureCelsius < -45 || simulator.TemperatureCelsius > -35)
                    throw new Exception("Temperature regulation failed at low temperature");
            };
            tests.Add(test1);

            var test2 = new TestCase("TEMP_002", "Room Temperature Operation (25°C)", "Temperature");
            test2.TestExecutionLogic = (simulator) =>
            {
                simulator.SetPower(true);
                simulator.SetTemperature(25);
                simulator.SimulatePowerDissipation();
                if (simulator.TemperatureCelsius < 20 || simulator.TemperatureCelsius > 30)
                    throw new Exception("Temperature regulation failed at room temperature");
            };
            tests.Add(test2);

            var test3 = new TestCase("TEMP_003", "High Temperature Operation (125°C)", "Temperature");
            test3.TestExecutionLogic = (simulator) =>
            {
                simulator.SetPower(true);
                simulator.SetTemperature(125);
                simulator.SimulatePowerDissipation();
                if (simulator.TemperatureCelsius < 120 || simulator.TemperatureCelsius > 130)
                    throw new Exception("Temperature regulation failed at high temperature");
            };
            tests.Add(test3);

            var test4 = new TestCase("TEMP_004", "Temperature Cycling Test", "Temperature");
            test4.TestExecutionLogic = (simulator) =>
            {
                simulator.SetPower(true);
                for (int i = 0; i < 5; i++)
                {
                    simulator.SetTemperature(-40);
                    System.Threading.Thread.Sleep(100);
                    simulator.SetTemperature(125);
                    System.Threading.Thread.Sleep(100);
                }
            };
            tests.Add(test4);

            return tests;
        }
    }
}