using Reports.Common.DataTransferObjects;

namespace Reports.DataAccessLayer.Entities
{
    public class DbComment
    {
        public int Id { get; set; }

        public string Commentary { get; set; }

        public static DbComment FromDto(CommentDto commentary)
        {
            return new DbComment
            {
                Id = commentary.Id,
                Commentary = commentary.Commentary
            };
        }

        public CommentDto ToDto()
        {
            return new CommentDto
            {
                Id = Id,
                Commentary = Commentary
            };
        }
    }
}