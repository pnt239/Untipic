namespace Untipic.Business.Action
{
    /// <summary>
    /// Server will send to the client a id in ReceiverId
    /// </summary>
    public class IdentifyAction : IAction
    {
        public IdentifyAction(Client client)
        {
            _client = client;
            Visible = false;
            RePaint = false;
            IsToAll = false;
        }

        public int ReceiverId { get; set; }
        public int SenderId { get; set; }
        public bool Visible { get; set; }

        public bool RePaint { get; set; }

        public bool IsToAll { get; set; }

        public ActionType GetActionType()
        {
            return ActionType.IdentifyUser;
        }

        public void Execute()
        {
            _client.Id = ReceiverId;
        }

        private Client _client;
    }
}
