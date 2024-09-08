﻿namespace Infrastructure.TockenTractker
{
    public class AssignedTokenInfo<TUserIdType>
    {
        public TUserIdType UserId { get; set; } = default!;
        public DateTime DateTime { get; set; }
    }
}