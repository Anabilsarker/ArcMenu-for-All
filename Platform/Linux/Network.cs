using System.Collections.Generic;
using System.Diagnostics;

namespace ArcMenu_for_All.Platform.Linux;

public class Network
{
    // Enable Wi-Fi
    public static bool EnableWiFi()
    {
        return RunCommand("nmcli radio wifi on");
    }

    // Disable Wi-Fi
    public static bool DisableWiFi()
    {
        return RunCommand("nmcli radio wifi off");
    }
    
    // Connect to Wi-Fi
    public static bool ConnectToWiFi(string ssid, string password)
    {
        string command = $"nmcli dev wifi connect \"{ssid}\" password \"{password}\"";
        return RunCommand(command);
    }
    
    // Disconnect from Wi-Fi
    public static bool DisconnectWiFi()
    {
        string command = "nmcli connection down id \"$(nmcli -t -f NAME connection show --active | head -n1)\"";
        return RunCommand(command);
    }
    
    // Disconnect from Wi-Fi by connection name
    public static bool DisconnectWiFi(string connectionName)
    {
        string command = $"nmcli connection down id \"{connectionName}\"";
        return RunCommand(command);
    }

    // Get available Wi-Fi networks
    public static List<string> GetAvailableWiFiNetworks()
    {
        var output = RunCommandWithOutput("nmcli -t -f SSID dev wifi");
        var networks = new List<string>();
        foreach (var line in output.Split('\n'))
        {
            var ssid = line.Trim();
            if (!string.IsNullOrEmpty(ssid))
                networks.Add(ssid);
        }
        return networks;
    }

    // Enable Bluetooth
    public static bool EnableBluetooth()
    {
        return RunCommand("rfkill unblock bluetooth");
    }
    
    // Disable Bluetooth
    public static bool DisableBluetooth()
    {
        return RunCommand("rfkill block bluetooth");
    }
    
    // Connect to Bluetooth device
    public static bool ConnectBluetooth(string macAddress)
    {
        string command = $"echo -e \"connect {macAddress}\\nquit\" | bluetoothctl";
        return RunCommand(command);
    }
    
    // Disconnect from Bluetooth device
    public static bool DisconnectBluetooth(string macAddress)
    {
        string command = $"echo -e \"disconnect {macAddress}\\nquit\" | bluetoothctl";
        return RunCommand(command);
    }

    // Get available Bluetooth devices
    public static List<string> GetAvailableBluetoothDevices()
    {
        var output = RunCommandWithOutput("bluetoothctl devices");
        var devices = new List<string>();
        foreach (var line in output.Split('\n'))
        {
            // Example line: Device XX:XX:XX:XX:XX:XX DeviceName
            if (line.StartsWith("Device "))
            {
                var parts = line.Split(' ', 3);
                if (parts.Length == 3)
                    devices.Add($"{parts[1]}: {parts[2]}");
            }
        }
        return devices;
    }

    // Helper to run shell command, returns true if exit code is 0
    private static bool RunCommand(string command)
    {
        try
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = command.Split(' ')[0],
                    Arguments = command.Split(' ', 2)[1] ,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            proc.WaitForExit();
            return proc.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }

    // Helper to run shell command and get output
    private static string RunCommandWithOutput(string command)
    {
        try
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{command}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            string output = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
            return output;
        }
        catch
        {
            return string.Empty;
        }
    } 
}