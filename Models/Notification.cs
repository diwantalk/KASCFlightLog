﻿namespace KASCFlightLog.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public string TargetUserId { get; set; }

        public virtual ApplicationUser TargetUser { get; set; }
    }
}