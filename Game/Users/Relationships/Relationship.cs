﻿namespace Wibbo.Game.Users.Relationships
{
    public class Relationship
    {
        public int UserId { get; private set; }
        public int Type { get; set; }

        public Relationship(int User, int Type)
        {
            this.UserId = User;
            this.Type = Type;
        }
    }
}
