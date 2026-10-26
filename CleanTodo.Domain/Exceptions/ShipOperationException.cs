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

            ShipErrorCode.AlreadySailing =>
                "Le navire est déjà en navigation.",

            ShipErrorCode.BrokerRejected =>
                "Le broker a refusé l'envoi du navire.",

            ShipErrorCode.DeliveryUnknown =>
                "La livraison n'a pas pu être confirmée. Le navire reste en navigation pour éviter un double envoi.",

            _ => "Une erreur est survenue."
        };
    }
}
