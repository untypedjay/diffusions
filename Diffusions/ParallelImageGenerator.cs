using System.Collections.Generic;
using System.Threading.Tasks;

namespace Diffusions
{
    public class ParallelImageGenerator : ImageGenerator
    {
        protected override void UpdateMatrix(Area area)
        {
            var tasks = new List<Task>();

            for (int x = 0; x < area.Width; x++)
            {
                for (int y = 0; y < area.Height; y++)
                {
                    tasks.Add(Task.Factory.StartNew(delegate { CalculateDiffusion(ref area, x, y); }));
                }
            }

            Task.WaitAll(tasks.ToArray());
        }

        private void CalculateDiffusion(ref Area area, int x, int y)
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

            //area.Matrix[x, y] = sum / usedNeighbours;
        }
    }
}
