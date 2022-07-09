﻿namespace Wibbo.Core.FigureData.Types
{
    public class Palette
    {
        public int Id;

        private Dictionary<int, Color> _colors;

        public Palette(int id)
        {
            this.Id = id;
            this._colors = new Dictionary<int, Color>();
        }

        public Dictionary<int, Color> Colors
        {
            get => this._colors;
            set => this._colors = value;
        }
    }
}
