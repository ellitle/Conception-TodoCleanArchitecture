namespace CleanTodo.Infrastructure.Configuration;
public class BrokerConfiguration
{
    public string BaseUrl { get; set; } = "https://pwa-broker-pirates-2bc1349418b0.herokuapp.com/api/";
    public string AppToken { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string CreatorId { get; set; } = string.Empty;
    public string ClientIdHeader { get; set; } = string.Empty;
    public bool ProtocolConfirmed { get; set; }
    public string PortsArrayProperty { get; set; } = string.Empty;
    public string PortIdProperty { get; set; } = "id";
    public string PortNameProperty { get; set; } = "name";
    public int[] ConfirmedStatusCodes { get; set; } = [];
    public int[] RejectedStatusCodes { get; set; } = [];
}
