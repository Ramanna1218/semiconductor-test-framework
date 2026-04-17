using System;
using System.Collections.Generic;
using SemiconductorTestFramework.Core;

namespace SemiconductorTestFramework.TestSuites
{
    public static class SignalIntegrityTests
    {
        public static List<TestCase> GetSignalIntegrityTestSuite()
        {
            var tests = new List<TestCase>();

            var test1 = new TestCase("SIGNAL_001", "Low Frequency Stability (50MHz)", "Signal Integrity");
            test1.TestExecutionLogic = (simulator) =>
            {
                simulator.SetPower(true);
                simulator.SetFrequency(50);
                for (int i = 0; i < 10; i++)
                {
                    simulator.SimulateSignalIntegrity();
                    if (simulator.FrequencyMHz < 45 || simulator.FrequencyMHz > 55)
                        throw new Exception("Frequency jitter exceeded tolerance at 50MHz");
                }
            };
            tests.Add(test1);

            var test2 = new TestCase("SIGNAL_002", "Mid Frequency Stability (200MHz)", "Signal Integrity");
            test2.TestExecutionLogic = (simulator) =>
            {
                simulator.SetPower(true);
                simulator.SetFrequency(200);
                for (int i = 0; i < 10; i++)
                {
                    simulator.SimulateSignalIntegrity();
                    if (simulator.FrequencyMHz < 195 || simulator.FrequencyMHz > 205)
                        throw new Exception("Frequency jitter exceeded tolerance at 200MHz");
                }
            };
            tests.Add(test2);

            var test3 = new TestCase("SIGNAL_003", "High Frequency Stability (500MHz)", "Signal Integrity");
            test3.TestExecutionLogic = (simulator) =>
            {
                simulator.SetPower(true);
                simulator.SetFrequency(500);
                for (int i = 0; i < 10; i++)
                {
                    simulator.SimulateSignalIntegrity();
                    if (simulator.FrequencyMHz < 490 || simulator.FrequencyMHz > 510)
                        throw new Exception("Frequency jitter exceeded tolerance at 500MHz");
                }
            };
            tests.Add(test3);

            return tests;
        }
    }
}