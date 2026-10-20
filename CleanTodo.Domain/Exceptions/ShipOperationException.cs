namespace CleanTodo.Domain.Exceptions;

public class ShipOperationException : Exception
{
    public ShipErrorCode Code { get; }

    public ShipOperationException(ShipErrorCode code) : base(GetMessage(code))
    {
        Code = code;
    }

    private static string GetMessage(ShipErrorCode code)
    {
        return code switch
        {
            ShipErrorCode.NotFound =>
                "Bateau introuvable.",

            ShipErrorCode.PortFull =>
                "Le port possède déjà huit bateaux.",

            ShipErrorCode.InvalidDestination =>
                "La destination est invalide.",

            ShipErrorCode.BrokerUnavailable =>
                "Le Broker est indisponible.",

            _ => "Une erreur est survenue."
        };
    }
}