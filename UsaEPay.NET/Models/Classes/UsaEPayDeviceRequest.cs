using System.Text.Json.Serialization;
using RestSharp;

namespace UsaEPay.NET.Models.Classes
{
    public class UsaEPayDeviceRequest : IUsaEPayRequest
    {
        [JsonIgnore]
        public string Endpoint { get; set; }
        [JsonIgnore]
        public Method RequestType { get; set; }

        /// <summary>
        /// Type of terminal being registered, currently the only option is 'standalone'.
        /// </summary>
        [JsonPropertyName("terminal_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string TerminalType { get; set; }

        /// <summary>
        /// A name associated with the terminal. Device name can contain letters, numbers, spaces, and dashes.
        /// </summary>
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Name { get; set; }

        /// <summary>
        /// Device settings.
        /// </summary>
        [JsonPropertyName("settings")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public DeviceSettings Settings { get; set; }

        /// <summary>
        /// Terminal configuration.
        /// </summary>
        [JsonPropertyName("terminal_config")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TerminalConfig TerminalConfig { get; set; }
    }

    public class DeviceSettings
    {
        /// <summary>
        /// Transaction timeout, how long to wait for transaction authorization to complete.
        /// </summary>
        [JsonPropertyName("timeout")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Timeout { get; set; }

        /// <summary>
        /// If true, this allows the payment device to be used by other merchants.
        /// </summary>
        [JsonPropertyName("share_device")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ShareDevice { get; set; }

        /// <summary>
        /// Allows transactions to be initiated from terminal (if supported).
        /// </summary>
        [JsonPropertyName("enable_standalone")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableStandalone { get; set; }

        /// <summary>
        /// If true, device will be notified on all future updates.
        /// </summary>
        [JsonPropertyName("notify_update")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? NotifyUpdate { get; set; }

        /// <summary>
        /// If true, device will be notified only on the next update. After notification, this is automatically set back to false.
        /// </summary>
        [JsonPropertyName("notify_update_next")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? NotifyUpdateNext { get; set; }

        /// <summary>
        /// Amount of inactive time (in minutes) before the device enters full sleep on battery. Set to 0 to never sleep.
        /// </summary>
        [JsonPropertyName("sleep_battery_device")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? SleepBatteryDevice { get; set; }

        /// <summary>
        /// Amount of inactive time (in minutes) before the device enters display sleep on battery. Set to 0 to never sleep.
        /// </summary>
        [JsonPropertyName("sleep_battery_display")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? SleepBatteryDisplay { get; set; }

        /// <summary>
        /// Amount of inactive time (in minutes) before the device enters full sleep when plugged into power. Set to 0 to never sleep.
        /// </summary>
        [JsonPropertyName("sleep_powered_device")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? SleepPoweredDevice { get; set; }

        /// <summary>
        /// Amount of inactive time (in minutes) before the device enters display sleep when plugged into power. Set to 0 to never sleep.
        /// </summary>
        [JsonPropertyName("sleep_powered_display")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? SleepPoweredDisplay { get; set; }
    }

    public class TerminalConfig
    {
        /// <summary>
        /// Enables EMV processing.
        /// </summary>
        [JsonPropertyName("enable_emv")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableEmv { get; set; }

        /// <summary>
        /// Enables PIN debit for swiped transactions.
        /// </summary>
        [JsonPropertyName("enable_debit_msr")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableDebitMsr { get; set; }

        /// <summary>
        /// Allows EMV transaction amounts to be adjusted after authorization (to add tip). Disables PIN authentication.
        /// </summary>
        [JsonPropertyName("enable_tip_adjust")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableTipAdjust { get; set; }

        /// <summary>
        /// Enables NFC reader.
        /// </summary>
        [JsonPropertyName("enable_contactless")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableContactless { get; set; }
    }

    public class TerminalInfo
    {
        /// <summary>
        /// Terminal manufacturer.
        /// </summary>
        [JsonPropertyName("make")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Make { get; set; }

        /// <summary>
        /// Terminal model.
        /// </summary>
        [JsonPropertyName("model")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Model { get; set; }

        /// <summary>
        /// Terminal firmware revision.
        /// </summary>
        [JsonPropertyName("revision")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Revision { get; set; }

        /// <summary>
        /// Terminal serial number.
        /// </summary>
        [JsonPropertyName("serial")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Serial { get; set; }

        /// <summary>
        /// PIN encryption key identifier.
        /// </summary>
        [JsonPropertyName("key_pin")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string KeyPin { get; set; }

        /// <summary>
        /// PAN encryption key identifier.
        /// </summary>
        [JsonPropertyName("key_pan")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string KeyPan { get; set; }
    }
}
