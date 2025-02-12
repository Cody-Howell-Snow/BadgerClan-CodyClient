using System.Runtime.Serialization;
using System.ServiceModel;

namespace BadgerClan_CodyClient.Library;

[DataContract]
public class StringRequest {
    [DataMember(Order = 1)]
    public string NewStrat { get; set; }
}

[DataContract]
public class StringResponse {
    [DataMember(Order = 1)]
    public string NewStrat { get; set; }
    [DataMember(Order = 2)]
    public bool stratChanged { get; set; }
}

[ServiceContract]
public interface IStrategyChanger {
    [OperationContract]
    public Task<StringResponse> StrategyChange(StringRequest request);
}
