using System;
using System.Collections.Generic;

namespace Diffusions
{
    public class SequentialImageGenerator : ImageGenerator
    {
        protected override void UpdateMatrix(Area area)
        {
            for (int x = 0; x < area.Width; x++)
            {
                for (int y = 0; y < area.Height; y++)
                {
                    area.Matrix[x, y] = CalculateDiffusion(area, x, y);
                }
            }
        }

        private double CalculateDiffusion(Area area, int x, int y)
        {
            int usedNeighbours = 0;
            double sum = 0;

            if (x > 0)
            {
                if (y > 0)
                {
                    sum = sum + area.Matrix[x - 1, y - 1];
                    usedNeighbours++;
                }

                sum = sum + area.Matrix[x - 1, y];
                usedNeighbours++;

                if (y < area.Height - 1)
                {
                    sum = sum + area.Matrix[x - 1, y + 1];
                    usedNeighbours++;
                }
            }

            if (y > 0)
            {
                sum = sum + area.Matrix[x, y - 1];
                usedNeighbours++;
            }

            if (y < area.Height - 1)
            {
                sum = sum + area.Matrix[x, y + 1];
                usedNeighbours++;
            }


            if (x < area.Width - 1)
            {
                if (y > 0)
                {
                    sum = sum + area.Matrix[x + 1, y - 1];
                    usedNeighbours++;
                }

                sum = sum + area.Matrix[x + 1, y];
                usedNeighbours++;

                if (y < area.Height - 1)
                {
                    sum = sum + area.Matrix[x + 1, y + 1];
                    usedNeighbours++;
                }
            }

            return sum / usedNeighbours;
        }
    }
}
