using System;

namespace Geofy.Infrastructure.ServiceBus.Interfaces
{
    public interface ICommandMetadata
    {
        /// <summary>
        /// Unique Id of Command
        /// </summary>
        string CommandId { get; set; }

        /// <summary>
        /// User Id of user who initiate this command
        /// </summary>
        string UserId { get; set; }

        /// <summary>
        /// Assembly qualified CLR Type name of Command Type
        /// </summary>
        string TypeName { get; set; }

        /// <summary>
        /// Time when command was created
        /// </summary>
        DateTime CreatedDate { get; set; }
    }
}