namespace TPC.Api.Model.ManyToManyRelations
{
    public class UserProject
    {
        public long Id { set; get; }
        public long UserId { set; get; }
        public long ProjectId { set; get; }
        public bool Active { set; get; }
        public long RoleId { set; get; }
    }
}
