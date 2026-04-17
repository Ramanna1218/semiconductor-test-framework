using System;
using System.Collections.Generic;

namespace SemiconductorTestFramework.Core
{
    public class HardwareSimulator
    {
        public double TemperatureCelsius { get; private set; }
        public double VoltageVolts { get; private set; }
        public double CurrentAmps { get; private set; }
        public double FrequencyMHz { get; private set; }
        public bool PowerEnabled { get; private set; }
        public Dictionary<string, object> Registers { get; private set; }

        private Random _random = new Random();
        private bool _initialized = false;

        public HardwareSimulator()
        {
            Registers = new Dictionary<string, object>();
            ResetState();
        }

        public void Initialize()
        {
            if (_initialized) return;
            TemperatureCelsius = 25.0;
            VoltageVolts = 3.3;
            CurrentAmps = 0.0;
            FrequencyMHz = 100.0;
            PowerEnabled = true;
            _initialized = true;
        }

        public void ResetState()
        {
            TemperatureCelsius = 25.0;
            VoltageVolts = 0.0;
            CurrentAmps = 0.0;
            FrequencyMHz = 0.0;
            PowerEnabled = false;
            Registers.Clear();
        }

        public void SetPower(bool enabled)
        {
            PowerEnabled = enabled;
            if (enabled)
            {
                VoltageVolts = 3.3;
                FrequencyMHz = 100.0;
            }
            else
            {
                ResetState();
            }
        }

        public void SetTemperature(double celsius)
        {
            if (celsius < -40 || celsius > 125)
                throw new InvalidOperationException("Temperature out of valid range (-40°C to 125°C)");
            TemperatureCelsius = celsius;
        }

        public void SetVoltage(double volts)
        {
            if (volts < 0 || volts > 5.0)
                throw new InvalidOperationException("Voltage out of valid range (0V to 5V)");
            VoltageVolts = volts;
        }

        public void SetFrequency(double mhz)
        {
            if (mhz < 0 || mhz > 1000)
                throw new InvalidOperationException("Frequency out of valid range (0MHz to 1000MHz)");
            FrequencyMHz = mhz;
        }

        public void SetCurrent(double amps)
        {
            if (amps < 0 || amps > 10.0)
                throw new InvalidOperationException("Current out of valid range (0A to 10A)");
            CurrentAmps = amps;
        }

        public void WriteRegister(string registerName, object value)
        {
            Registers[registerName] = value;
        }

        public object ReadRegister(string registerName)
        {
            return Registers.ContainsKey(registerName) ? Registers[registerName] : null;
        }

        public void SimulateSignalIntegrity()
        {
            double jitter = (_random.NextDouble() - 0.5) * 0.1;
            FrequencyMHz += jitter;
            System.Threading.Thread.Sleep(_random.Next(10, 50));
        }

        public void SimulatePowerDissipation()
        {
            double powerConsumption = VoltageVolts * CurrentAmps;
            TemperatureCelsius += powerConsumption * 0.5;
        }

        public string GetStatus()
        {
            return $"Status: Temp={TemperatureCelsius:F1}°C, Voltage={VoltageVolts:F2}V, " +
                   $"Current={CurrentAmps:F2}A, Freq={FrequencyMHz:F1}MHz, Power={PowerEnabled}";
        }
    }
}